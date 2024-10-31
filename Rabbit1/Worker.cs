using MassTransit;
using System;
using System.Threading.Tasks;

namespace Rabbit1
{
    public class Worker
    {
        public static async Task Main()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", h =>
                {
                    h.Username("rabbitmq");
                    h.Password("rabbitmq");
                });

                // Configure the topic exchange and queue
                cfg.ReceiveEndpoint("topic_logs_queue", e =>
                {
                    e.Bind("topic_logs", x =>
                    {
                        x.RoutingKey = "*.xyz"; // Set up topic matching
                        x.ExchangeType = "topic";
                    });

                    // Register the consumer
                    e.Consumer<MessageConsumer>();
                });
            });

            await busControl.StartAsync();
            Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }

    // Consumer class to handle messages
    public class MessageConsumer : IConsumer<Message>
    {
        public async Task Consume(ConsumeContext<Message> context)
        {
            var message = context.Message.Text;
            Console.WriteLine(" [x] Received '{0}':'{1}'", context.RoutingKey, message);

            // Simulate work based on message content
            int dots = message.Split('.').Length - 1;
            await Task.Delay(dots * 1000);

            Console.WriteLine(" [x] Done");
        }
    }

    // Define the message type
    public class Message
    {
        public string Text { get; set; }
    }
}