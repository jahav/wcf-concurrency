using Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static ChannelFactory<IDelayService> factory = new ChannelFactory<IDelayService>("httpDelayService");

        private static async Task<int> CallDelay(int value)
        {
            Console.WriteLine("Start: {0}", value);
            var channel = factory.CreateChannel();
            try
            {
                var result = await channel.DelayAsync(value);
                ((IClientChannel)channel).Close();
                Console.WriteLine("Done: {0}", value);
                return result;
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Timeout");
                ((IClientChannel)channel).Abort();
            }
            catch (CommunicationException ex)
            {
                var s = Info("Communication exception", ex);
                Console.WriteLine(s);
                ((IClientChannel)channel).Abort();
            }
            throw new Exception("Error " + value);
        }

        private static string Info(string headline, Exception ex)
        {
            var sb = new StringBuilder();
            sb.AppendLine(headline);
            sb.AppendLine("Messages:");
            var inner = ex;
            while (inner != null)
            {
                sb.AppendFormat("  {0:50} {1}\n", inner.GetType().FullName, inner.Message);
                inner = inner.InnerException;
            }
            sb.AppendLine("Stacktrace:");
            sb.AppendLine(ex.ToString());
            return sb.ToString();
        }

        static async Task Main(string[] args)
        {
            Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.InvariantCulture;
            IList<Task<int>> tasks = new List<Task<int>>();
            for (int i = 0; i < 3; i++)
            {
                tasks.Add(CallDelay(i));
            }

            await Task.WhenAll(tasks);

            Console.Write("Result: ");
            foreach (var task in tasks)
            {
                Console.Write("{0} ", task.IsFaulted ? -1 : task.Result);
            }
            //Console.ReadKey();
        }
    }
}
