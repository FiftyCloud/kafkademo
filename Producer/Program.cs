using Confluent.Kafka;
using System.Net;


var config = new ProducerConfig
{
    BootstrapServers = "localhost:9092"
};

using (var producer = new ProducerBuilder<Null, string>(config).Build())
{
    try
    {
        var message = new Message<Null, string>
        {
            Value = "Hello, Kafka!" // Replace with the message you want to send
        };

        var result = await producer.ProduceAsync("demo-topic", message);

        Console.WriteLine($"Delivered '{result.Value}' to '{result.TopicPartitionOffset}'");
    }
    catch (ProduceException<Null, string> e)
    {
        Console.WriteLine($"Delivery failed: {e.Error.Reason}");
    }
}