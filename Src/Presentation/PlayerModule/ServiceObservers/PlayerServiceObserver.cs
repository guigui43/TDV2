using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using TDV.Client.Data;
using TDV.Client.Data.Entities;
using TDV.Client.Data.ResourceManager;
using TDV.Client.Infrastructure;
using TDV.Client.Infrastructure.Interfaces;
using TDV.Client.Infrastructure.Messaging;

namespace PlayerModule.ServiceObservers
{
    [Export(typeof(BaseServiceObserver))]
    public class PlayerServiceObserver : BaseServiceObserver
    {
        public PlayerServiceObserver()
        {
            AddEventExploder(EventExploder);
        }

        public override void AddServicesToObserve(IEnumerable<IService> services)
        {
            //pass filter
            base.AddServicesToObserve(services.Where(s => s.AssetType == AssetType.Player));
        }

        public void EventExploder(IEnumerable<Entity> messages)
        {
            // broadcast news or updates using Mediator EventExploder
            Mediator.GetInstance.Broadcast(Topic.PlayerServiceDataReceived, messages);
        }
    }
}
