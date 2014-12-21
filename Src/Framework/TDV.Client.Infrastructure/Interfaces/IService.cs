using System;
using TDV.Client.Data;

namespace TDV.Client.Infrastructure.Interfaces
{
    public interface IService
    {
        void GetData();
        event EventHandler<EventArgs<DataRecord>> DataReceived;
        AssetType AssetType { get; }
    }
}
