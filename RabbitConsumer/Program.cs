using MassTransit;
using System;
using System.Threading.Tasks;

namespace RabbitConsumer
{
    public class MessageConsumer : IConsumer<PortalTranspData>
    {
        public async Task Consume(ConsumeContext<PortalTranspData> context)
        {
            var messageText = context.Message.Text;
            Console.WriteLine($" [x] Received 'msg.abc':'{messageText}'");
            await Task.CompletedTask;
        }
    }

    public class ConsumerProgram
    {
        public static async Task Main(string[] args)
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", h =>
                {
                    h.Username("rabbitmq");
                    h.Password("rabbitmq");
                });

                cfg.ReceiveEndpoint("portaltransp", e =>
                {
                    e.ConfigureConsumeTopology = false; // Avoid auto-configuring
                    e.Bind("*.portaltransp"); // Bind to the specific exchange name
                    e.Consumer<MessageConsumer>();
                });
            });

            await busControl.StartAsync();

            Console.WriteLine("Consumer started. Press any key to exit");
            Console.ReadKey();

            await busControl.StopAsync();
        }
    }
}