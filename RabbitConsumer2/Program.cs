using MassTransit;
using System;
using System.Threading.Tasks;

namespace RabbitConsumer
{
    public class MessageConsumer : IConsumer<DadosPubData>
    {
        public async Task Consume(ConsumeContext<DadosPubData> context)
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
                
                cfg.Message<MessageConsumer>(x =>
                {
                    x.SetEntityName("omg-we-got-one");
                });
                //
                // cfg.ReceiveEndpoint("dadospub", e =>
                // {
                //     e.ConfigureConsumeTopology = false; // To handle custom routing key binding
                //     e.Bind<DadosPubData>(exchange =>
                //     {
                //         exchange.RoutingKey = "*.dadospub";
                //         exchange.ExchangeType = "topic";
                //     });
                //
                //     e.Consumer<MessageConsumer>();
                // });
            });

            await busControl.StartAsync();

            Console.WriteLine("Consumer started. Press any key to exit");
            Console.ReadKey();

            await busControl.StopAsync();
        }
    }
}