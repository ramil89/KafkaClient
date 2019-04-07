using Confluent.Kafka;
using KafkaClient.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KafkaClient.Tests
{
    [TestClass]
    public class ProducerConsumerTests
    {
        [TestMethod]
        public async Task ProduceConsumeTest()
        {
            #region Configuration 

            string topicName = "test_order1";

            string key = $"test_{DateTime.Now.Ticks}";

            Producer<string, TestOrder> producer = new Producer<string, TestOrder>(new ProducerConfig()
            {
                BootstrapServers = "localhost:29092",
                MessageTimeoutMs = 10000,
            });

            Consumer<string, TestOrder> consumer = new Consumer<string, TestOrder>(new ConsumerConfig()
            {
                BootstrapServers = "localhost:29092",
                GroupId = "group-F",
                EnableAutoOffsetStore = false,
                AutoOffsetReset = AutoOffsetReset.Earliest
            }, 
            4000);

            #endregion 

            var order = new TestOrder()
            {
                Id = 50,
                DateCreated = DateTime.Now,
                DeliveryPrice = 5.8,
                PaymentPrice = 3.55,
                Total = 50.85,
                OrderDetails = new List<string>()
                {
                    "Cola x 1",
                    "Pizza x 2",
                    "Salad x 1"
                }
            };

            var produceResult = await producer.ProduceAsync(topicName, key, order);

            consumer.StartConsume(topicName);

            TestOrder value = consumer.GetValue(key);

            Assert.IsNotNull(value);

            Assert.AreEqual(order.DateCreated, value.DateCreated);
            Assert.AreEqual(order.Id, value.Id);
        }
    }
}
