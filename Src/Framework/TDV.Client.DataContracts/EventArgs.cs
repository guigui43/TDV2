using System;

namespace TDV.Client.Data
{
    [Serializable]
    public sealed class EventArgs<TData> : EventArgs
    {
        private readonly TData _value;

        public EventArgs(TData value)
        {
            _value = value;
        }
        public TData Value
        {
            get { return _value; }
        }
    }
}
