using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaClient 
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string topicName = "order_create2";

            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:29092",
                MessageTimeoutMs = 10000,
            };

            await ProduceDataAsync(topicName, config);

            //var consumerConfig = new ConsumerConfig
            //{
            //    BootstrapServers = "localhost:29092",
            //    GroupId = "group-C",
            //    EnableAutoOffsetStore  = false,
            //    AutoOffsetReset = AutoOffsetReset.Earliest
            //};

            //ConsumeData(topicName, consumerConfig);
        }

        static async Task ProduceDataAsync(string topicName, ProducerConfig config)
        {
            Producer<string, Customer> producer = new Producer<string, Customer>(config);

            for (var i = 0; i < 10; i++)
            {
                var message = new Customer()
                {
                    FirstName = $"Mark {i}",
                    LastName = $"Johnson {i}"
                };

                await producer.ProduceAsync(topicName, DateTime.Now.Ticks.ToString(), message);
            }
        }

        static void ConsumeData(string topicName, ConsumerConfig config)
        {
            Consumer<Customer> consumer = new Consumer<Customer>(config);

            consumer.Consume(topicName);
        }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
