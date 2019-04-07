using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KafkaClient
{
    public class Producer<TKey,TValue> 
        where TKey: class
        where TValue : class
    {
        private ProducerConfig _producerConfig;

        public Producer(ProducerConfig producerConfig)
        {
            _producerConfig = producerConfig;
        }

        public async Task<DeliveryResult<TKey, TValue>> ProduceAsync(string topic, TKey key, TValue value)
        {
            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using (var producer = new ProducerBuilder<TKey, TValue>(_producerConfig).SetValueSerializer(new ValueJsonSerializer<TValue>()).Build())
            {
                try
                {
                    var message = new Message<TKey, TValue>
                    {
                        Key = key,
                        Value = value
                    };

                    var result = await producer.ProduceAsync(topic, message);

                    return result;
                }
                catch (ProduceException<TKey, TValue> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                    throw;
                }
            }
        }
    }
}
