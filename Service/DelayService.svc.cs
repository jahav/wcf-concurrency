using System.ServiceModel;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode =InstanceContextMode.Single)]
    public class CalculatorService : IDelayService
    {
        public async Task<int> Delay(int value)
        {
            await Task.Delay(5000);
            return value;
        }
    }
}
