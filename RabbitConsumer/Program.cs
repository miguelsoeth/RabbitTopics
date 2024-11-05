using MassTransit;
using System;
using System.Threading.Tasks;

namespace RabbitConsumer
{
    public class MessageConsumer : IConsumer<DadosPubMessage>
    {
        public async Task Consume(ConsumeContext<DadosPubMessage> context)
        {
            var messageText = context.Message.Text;
            Console.WriteLine($" [x] Received 'msg DADOSPUB':'{messageText}'");
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
                
                cfg.ReceiveEndpoint( "dadospub",e =>
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