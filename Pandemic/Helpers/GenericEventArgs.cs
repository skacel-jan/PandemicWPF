using System;

namespace Pandemic
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T EventData { get; private set; }

        public GenericEventArgs(T eventData)
        {
            EventData = eventData;
        }
    }
}