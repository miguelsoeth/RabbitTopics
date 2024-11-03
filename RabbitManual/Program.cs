using MassTransit;
using RabbitConsumer;

namespace RabbitManual
{
    public class Publisher
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
                
                cfg.Publish<DadosPubData>(x =>
                {
                    x.ExchangeType = "topic";
                    x.BindAlternateExchangeQueue("dadospub_consumer");
                });
                
                cfg.Publish<PortalTranspData>(x =>
                {
                    x.ExchangeType = "topic";
                    x.BindAlternateExchangeQueue("portaltransp_consumer");
                });
            });

            await busControl.StartAsync();

            while (true)
            {
                var message = new DadosPubData
                {
                    Text = "Hello World!"
                };
                await busControl.Publish(message);
                Console.WriteLine($" [x] Sent 'msg DADOSPUB'");
                await Task.Delay(1000);
                
                var message2 = new PortalTranspData
                {
                    Text = "Hello World!"
                };
                await busControl.Publish(message2);
                Console.WriteLine($" [x] Sent 'msg PORTALTRANSP'");
                await Task.Delay(1000);
            }
        }
    }
}