using System.Text;
using ForecastsRabbitMQProcessor;

internal class Program
{
    private static void Main(string[] args)
    {
        var rabbitMQDispatcher = new RabbitMQDispatcher("localhost");
        rabbitMQDispatcher.Received += (_, message) => Console.WriteLine($" [x] {message}");
        rabbitMQDispatcher.ConsumeMessage();

        Console.Read();
    }
}