using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitPublisher
{
    class Publisher
    {
        public static void Main(string[] args)
        {
            // Create a connection factory to connect to RabbitMQ
            var factory = new ConnectionFactory() 
            { 
                HostName = "localhost", 
                UserName = "rabbitmq", 
                Password = "rabbitmq" 
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declare the topic exchange
                string exchangeName = "topic_logs";
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                while (true)
                {
                    // Prompt user for routing key
                    Console.Write("Enter routing key (or type 'exit' to quit): ");
                    string routingKey = Console.ReadLine();
                    if (routingKey.ToLower() == "exit")
                    {
                        break;
                    }

                    // Prompt user for message
                    Console.Write("Enter message: ");
                    string message = Console.ReadLine();

                    var body = Encoding.UTF8.GetBytes(message);

                    // Publish the message to the exchange with the routing key
                    channel.BasicPublish(exchange: exchangeName,
                        routingKey: routingKey,
                        basicProperties: null,
                        body: body);

                    Console.WriteLine($" [x] Sent '{routingKey}':'{message}'");
                }
            }

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}