using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KafkaClient
{
    public class ValueJsonDeserializer<TValue> : IDeserializer<TValue> where TValue: class
    {
        public TValue Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            using (var stream = new MemoryStream(data.ToArray()))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    object value = JsonSerializer.Create().Deserialize(reader, typeof(TValue));
                    
                    return value as TValue;
                }
            }
        }
    }
}
