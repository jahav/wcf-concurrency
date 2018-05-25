using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Api
{
    [ServiceContract]
    public interface IDelayService
    {
        [OperationContract]
        Task<int> DelayAsync(int value);
    }
}
