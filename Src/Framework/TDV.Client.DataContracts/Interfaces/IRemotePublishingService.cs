using System.ServiceModel;

namespace TDV.Client.Data.Interfaces
{
    [ServiceContract]
    public interface IRemotePublishingService
    {
        [OperationContract(IsOneWay = true)]
        void DataSetRecordsChanged(DataRecord data);
    }
}
