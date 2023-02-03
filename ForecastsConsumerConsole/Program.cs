using System.Text;
using ForecastsRabbitMQProcessor;

internal class Program
{
    private static void Main(string[] args)
    {
        var rabbitMQDispatcher = new RabbitMQDispatcher("localhost");
        rabbitMQDispatcher.ConsumeMessage();

        Console.Read();
    }
}