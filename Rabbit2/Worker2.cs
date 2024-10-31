using MassTransit;
using System;
using System.Threading.Tasks;

namespace Rabbit2
{
    public class MessageConsumer : IConsumer<Message>
    {
        public async Task Consume(ConsumeContext<Message> context)
        {
            Console.WriteLine($" [x] Received: {context.Message.Text}");
        }
    }
    
    public class Worker2
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

                cfg.ReceiveEndpoint("queue", e =>
                {
                    e.Consumer<MessageConsumer>();
                    e.Bind<Message>(x => 
                    {
                        x.ExchangeType = "topic";
                        x.RoutingKey = "msg.#";
                    });
                });
            });

            await busControl.StartAsync();
            Console.WriteLine(" [*] Waiting for messages. Press [enter] to exit.");
            Console.ReadLine();

            await busControl.StopAsync();
        }
    }

    public class Message
    {
        public string Text { get; set; }
    }
}