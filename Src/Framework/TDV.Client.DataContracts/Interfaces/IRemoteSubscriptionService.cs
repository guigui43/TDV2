using System.ServiceModel;

namespace TDV.Client.Data.Interfaces
{
    [ServiceContract]
    public interface IRemoteSubscriptionService
    {
        [OperationContract(IsOneWay = true)]
        void GetData(RequestRecord request);
    }
}
