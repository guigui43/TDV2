using System;
using System.ServiceModel;
using TDV.Client.Data;
using TDV.Client.Data.Interfaces;

namespace TDV.Client.Server
{
    public class WcfServer : IDisposable
    {
        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class ConnectionListener : IRemoteSubscriptionService
        {
            private readonly ChannelFactory<IRemotePublishingService> _channelFutureFactory;
            private readonly IRemotePublishingService _futureProxy;

            private readonly ChannelFactory<IRemotePublishingService> _channelBondFactory;
            private readonly IRemotePublishingService _bondFuture;

            private readonly ChannelFactory<IRemotePublishingService> _channelPlayerFactory;
            private readonly IRemotePublishingService _playerProxy;

            public ConnectionListener()
            {
                _channelFutureFactory = new ChannelFactory<IRemotePublishingService>(
                        new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                        "net.pipe://TDV/Future/PublishingService");
                _futureProxy = _channelFutureFactory.CreateChannel();

                _channelBondFactory = new ChannelFactory<IRemotePublishingService>(
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                    "net.pipe://TDV/Bond/PublishingService");
                _bondFuture = _channelBondFactory.CreateChannel();

                _channelPlayerFactory = new ChannelFactory<IRemotePublishingService>(
                    new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                    "net.pipe://TDV/Player/PublishingService");
                _playerProxy = _channelPlayerFactory.CreateChannel();
            }

            #region IRemoteSubscriptionService Members

            public void GetData(RequestRecord requestRecord)
            {
                Console.WriteLine("GetData called");
                switch (requestRecord.AssetType)
                {
                    case AssetType.Bond:
                        BondServer.GetData(_bondFuture);
                        break;
                    case AssetType.Future:
                        FutureServer.GetData(_futureProxy);
                        break;
                    case AssetType.Player:
                        PlayerServer.GetData(_playerProxy);
                        break;
                    default:
                        break;
                }
            }

            #endregion
        }

        private readonly ServiceHost _host;
        public WcfServer()
        {
            _host = new ServiceHost(new ConnectionListener());
            _host.AddServiceEndpoint(typeof(IRemoteSubscriptionService),
                new NetNamedPipeBinding(NetNamedPipeSecurityMode.None) { MaxReceivedMessageSize = 5000000, MaxBufferSize = 5000000 },
                "net.pipe://TDV/SubscriptionService");
            _host.Open();
        }

        #region IDisposable Members

        public void Dispose()
        {
            _host.Close();
        }

        #endregion
    }
}
