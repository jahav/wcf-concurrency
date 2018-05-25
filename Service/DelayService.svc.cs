using Api;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode =InstanceContextMode.Single)]
    public class CalculatorService : IDelayService
    {
        public async Task<int> DelayAsync(int value)
        {
            await Task.Delay(5000);
            return value + 1;
        }
    }
}
