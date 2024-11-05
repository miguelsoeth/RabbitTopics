using MassTransit;

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
            });

            await busControl.StartAsync();

            while (true)
            {
                var message = new DadosPubMessage
                {
                    Text = "Hello World!"
                };
                await busControl.Publish(message);
                Console.WriteLine($" [x] Sent 'msg DADOSPUB'");
                await Task.Delay(500);
                
                var message2 = new PortalTranspMessage
                {
                    Text = "Hello World!"
                };
                await busControl.Publish(message2);
                Console.WriteLine($" [x] Sent 'msg PORTALTRANSP'");
                await Task.Delay(500);
            }
        }
    }
}