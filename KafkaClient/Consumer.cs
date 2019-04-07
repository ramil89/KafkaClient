using Confluent.Kafka;
using KafkaClient.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KafkaClient
{
    public class Consumer<TKey, TValue> where TValue : class
    {
        private readonly ConsumerConfig _config;

        private readonly ConcurrentDictionary<TKey, TValue> _messages;

        private bool _consuming;

        private readonly int _pollingTimeout;

        public Consumer(ConsumerConfig config, int pollingTimeout)
        {
            _config = config;
            _messages = new ConcurrentDictionary<TKey, TValue>();
            _consuming = false;
            _pollingTimeout = pollingTimeout;
        }

        public Task StartConsume(string topic)
        {
            if (_consuming)
            {
                throw new KafkaConsumeStartedException("Consuming already started.");
            }

            return Task.Factory.StartNew(() =>
            {
                _consuming = true;
                using (var builder = new ConsumerBuilder<TKey, TValue>(_config)
                        .SetValueDeserializer(new ValueJsonDeserializer<TValue>()).Build())
                {
                    builder.Subscribe(topic);

                    CancellationTokenSource cts = new CancellationTokenSource();
                    Console.CancelKeyPress += (_, e) =>
                    {
                        e.Cancel = true; // prevent the process from terminating.
                        cts.Cancel();
                    };

                    try
                    {
                        while (true)
                        {
                            try
                            {
                                var consumeResult = builder.Consume(cts.Token);
                                _messages.TryAdd(consumeResult.Key, consumeResult.Value);

                                Console.WriteLine($"Consumed message '{consumeResult.Key}' at: '{consumeResult.TopicPartitionOffset}'.");
                            }
                            catch (ConsumeException e)
                            {
                                Console.WriteLine($"Error occured: {e.Error.Reason}");
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Ensure the consumer leaves the group cleanly and final offsets are committed.
                        builder.Close();
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public TValue GetValue(TKey key)
        {
            TValue value = null;

            // waiting for 3 seconds
            int intervalCount = 0;
            int pollingInterval = 100;

            while (intervalCount < _pollingTimeout)
            {
                if (_messages.TryRemove(key, out value))
                    return value;

                intervalCount += pollingInterval;
                Thread.Sleep(pollingInterval);

                Console.WriteLine(intervalCount);
            }

            return value;
        }
    }
}