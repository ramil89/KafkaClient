using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient.Events
{
    public class MessageConsumedEventArgs<T> : EventArgs
    {
        public MessageConsumedEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

    }
}
