using System;
using System.ComponentModel.Composition;
using System.Threading;
using TDV.Client.Data;
using TDV.Client.Data.ResourceManager;
using TDV.Client.Infrastructure;
using TDV.Client.Infrastructure.Interfaces;
using TDV.Client.Infrastructure.Messaging;

namespace PlayerService.Implementation
{
    [Export("MOCK", typeof(IService))]
    public class PlayerService : IService
    {
        private readonly Random _rand = new Random();

        #region IService Members
        [RegisterInterest(Topic.PlayerServiceGetData, TaskType.Background)]
        public void GetData()
        {
            {
                for (; ; )
                {
                    Thread.Sleep(1);
                    var key = "PLAYER" + _rand.Next(1, 20);
                    int prop = _rand.Next(1, 57);
                    string propName = string.Empty;
                    object propValue = null;
                    switch (prop)
                    {
                        case 1:
                            propName = "BidPrice";
                            propValue = _rand.NextDouble();
                            break;
                        case 2:
                            propName = "AskPrice";
                            propValue = _rand.NextDouble();
                            break;
                        
                    }
                    EventHandler<EventArgs<DataRecord>> handler = DataReceived;
                    if (handler != null)
                    {
                        handler(this, new EventArgs<DataRecord>(new DataRecord(key, propName, propValue)));
                    }
                }
            }
        }

        public event EventHandler<EventArgs<DataRecord>> DataReceived;

        public AssetType AssetType { get { return AssetType.Player; } }
        #endregion


    }
}