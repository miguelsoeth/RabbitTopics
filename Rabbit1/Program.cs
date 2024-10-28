using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Rabbit1
{
    class Program
    {
        public static void Main()
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
                // Declare a topic exchange
                string exchangeName = "topic_logs";
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

                // Declare a queue with a unique name and bind it to the exchange
                var queueName = channel.QueueDeclare().QueueName;
                string routingKeyPattern = "*.xyz";

                channel.QueueBind(queue: queueName,
                                  exchange: exchangeName,
                                  routingKey: routingKeyPattern);

                Console.WriteLine($" [*] Waiting for messages with topic matching '{routingKeyPattern}'.");

                // Create a consumer to listen for messages
                var consumer = new EventingBasicConsumer(channel);
                
                consumer.Received += (model, ea) =>
                {
                    // Get message body and convert to string
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received '{0}':'{1}'", ea.RoutingKey, message);

                    // Simulate work (e.g., processing a task)
                    int dots = message.Split('.').Length - 1;
                    System.Threading.Thread.Sleep(dots * 1000);

                    Console.WriteLine(" [x] Done");

                    // Acknowledge that the message has been processed
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                // Start consuming messages
                channel.BasicConsume(queue: queueName,
                                     autoAck: false, // Manual acknowledgment
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }
}
