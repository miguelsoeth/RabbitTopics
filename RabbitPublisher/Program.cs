using MassTransit;
using RabbitConsumer;

namespace RabbitPublisher
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
                
                cfg.Publish<DadosPubData>(x => x.ExchangeType = "topic");
                cfg.Publish<PortalTranspData>(x => x.ExchangeType = "topic");
            });

            await busControl.StartAsync();

            while (true)
            {
                var message = new DadosPubData { Text = "DUMMY TEXT" };
                await busControl.Publish(message);
            
                Console.WriteLine($" [x] Sent 'msg.dadospub':'DUMMY TEXT'");
            
                await Task.Delay(1000);
            
                var message2 = new PortalTranspData { Text = "DUMMY TEXT 2" };
                await busControl.Publish(message2);
            
                Console.WriteLine($" [x] Sent 'msg.portaltransp':'DUMMY TEXT 2'");
            
                await Task.Delay(1000);
            }
        }
    }
}