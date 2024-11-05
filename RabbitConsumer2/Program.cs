using MassTransit;

namespace RabbitConsumer2
{
    public class MessageConsumer : IConsumer<PortalTranspMessage>
    {
        public async Task Consume(ConsumeContext<PortalTranspMessage> context)
        {
            var messageText = context.Message.Text;
            Console.WriteLine($" [x] Received 'msg PORTALTRANSP':'{messageText}'");
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