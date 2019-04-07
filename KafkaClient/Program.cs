using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string topicName = "order_create3";

            //var config = new ProducerConfig
            //{
            //    BootstrapServers = "localhost:29092",
            //    MessageTimeoutMs = 10000,
            //};

            //await ProduceDataAsync(topicName, config);

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:29092",
                GroupId = "group-F",
                EnableAutoOffsetStore = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            ConsumeData(topicName, consumerConfig);


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
            Consumer<string, Customer> consumer = new Consumer<string, Customer>(config, 3000);
            consumer.StartConsume(topicName);

            var value = consumer.GetValue("636902520212945872");
        }
    }

    public class Customer
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
