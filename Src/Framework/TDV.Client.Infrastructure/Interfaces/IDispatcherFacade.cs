using System;

namespace TDV.Client.Infrastructure.Interfaces
{
    public interface IDispatcherFacade
    {
        void AddToDispatcherQueue(Delegate workItem);
    }
}
