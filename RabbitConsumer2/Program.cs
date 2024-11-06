using MassTransit;

namespace RabbitConsumer2
{
    public class MessageConsumer : IConsumer<PortalTranspMessage>
    {
        public async Task Consume(ConsumeContext<PortalTranspMessage> context)
        {
            PortalTranspMessage portalTranspMessage = context.Message;
            Console.WriteLine($" [x] Received 'msg PORTALTRANSP':'{portalTranspMessage.Text}'");
            portalTranspMessage.Data = new PortalTranspData("RESPONSE PORTAL TRANSP");
            await context.RespondAsync(portalTranspMessage);
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
                    e.ConfigureConsumeTopology = false;
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