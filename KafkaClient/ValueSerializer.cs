using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaClient
{
    public class ValueSerializer<TValue> : ISerializer<TValue> where TValue : class
    {
        public byte[] Serialize(TValue data, SerializationContext context)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            return Encoding.UTF8.GetBytes(serializedData);
        }
    }
}
