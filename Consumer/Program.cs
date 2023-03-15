using Confluent.Kafka;
using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092", // Replace with the appropriate bootstrap server(s)
            GroupId = "my-group", // Replace with the name of your consumer group
            AutoOffsetReset = AutoOffsetReset.Earliest // Replace with the desired offset reset policy
        };

        using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
        {
            consumer.Subscribe("demo-topic"); // Replace with the name of the Kafka topic you want to consume messages from

            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) => {
                e.Cancel = true; // Prevent the process from terminating.
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    var result = consumer.Consume(cts.Token);

                    Console.WriteLine($"Consumed message '{result.Message.Value}' at: '{result.TopicPartitionOffset}'.");
                }
            }
            catch (OperationCanceledException)
            {
                // The consumer was stopped.
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}