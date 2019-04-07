using KafkaClient.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KafkaClient.Tests
{
    [TestClass]
    public class ProducerTests
    { 
        [TestMethod]
        public async Task ProduceSingleMessageTestAsync()
        {
            var order = new TestOrder()
            {
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

            Producer<string, TestOrder> producer = new Producer<string, TestOrder>(new Confluent.Kafka.ProducerConfig()
            {
                BootstrapServers = "localhost:29092",
                MessageTimeoutMs = 10000,
            });

            var result = await producer.ProduceAsync("test_order", $"test_{DateTime.Now.Ticks}", order);
            
            Assert.AreEqual(Confluent.Kafka.PersistenceStatus.Persisted, result.Status);
        }
    }
}
