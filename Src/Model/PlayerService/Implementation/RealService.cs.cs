using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using TDV.Client.Data.Interfaces;
using TDV.Client.Data.ResourceManager;
using TDV.Client.Infrastructure;
using TDV.Client.Infrastructure.Interfaces;
using TDV.Client.Data;
using TDV.Client.Infrastructure.Messaging;

namespace PlayerService.Implementation
{
    [Export("REAL", typeof(IService))]
    public class RealService : IService
    {
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class ConnectionListener : IRemotePublishingService
        {
            private readonly RealService _parent;
            public ConnectionListener(RealService parent)
            {
                _parent = parent;
            }

            public void DataSetRecordsChanged(DataRecord data)
            {
                _parent.SendData(data);
            }
        }

        private readonly ChannelFactory<IRemoteSubscriptionService> _channelFactory;
        private readonly IRemoteSubscriptionService _proxy;

        public RealService()
        {
            // out channel
            _channelFactory = new ChannelFactory<IRemoteSubscriptionService>(
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                "net.pipe://TDV/SubscriptionService");
            _proxy = _channelFactory.CreateChannel();

            // in channel
            ServiceHost host = new ServiceHost(new ConnectionListener(this));
            host.AddServiceEndpoint(typeof(IRemotePublishingService),
                 new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                "net.pipe://TDV/Future/PublishingService");
            host.Open();
        }

        private void SendData(DataRecord data)
        {
            EventHandler<EventArgs<DataRecord>> handler = DataReceived;
            if (handler != null)
            {
                handler(this, new EventArgs<DataRecord>(data));
            }
        }

        #region IService Members
        [RegisterInterest(Topic.FutureServiceGetData, TaskType.Background)]
        public void GetData()
        {
            try
            {
                _proxy.GetData(new RequestRecord() { AssetType = AssetType });
            }
            catch (Exception ex)
            {
                throw new ApplicationException("WCF channel is closed", ex);
            }
        }

        public event EventHandler<EventArgs<DataRecord>> DataReceived;

        public AssetType AssetType { get { return AssetType.Player; } }

        #endregion
    }
}
