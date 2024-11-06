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
            
            var dadosPubClient = busControl.CreateRequestClient<DadosPubMessage>();
            var portalTranspClient = busControl.CreateRequestClient<PortalTranspMessage>();

            while (true)
            {
                var message = new DadosPubMessage
                {
                    Text = "Hello World!"
                };
                var dadosResponse = await dadosPubClient.GetResponse<DadosPubMessage>(message);
                Console.WriteLine($" [x] Sent 'msg DADOSPUB' and received {dadosResponse.Message.Data.Result}");
                await Task.Delay(500);
                
                var message2 = new PortalTranspMessage
                {
                    Text = "Hello World 2!"
                };
                var portalResponse = await portalTranspClient.GetResponse<PortalTranspMessage>(message2);
                Console.WriteLine($" [x] Sent 'msg PORTALTRANSP' and received {portalResponse.Message.Data.Result}");
                await Task.Delay(500);
            }
        }
    }
}