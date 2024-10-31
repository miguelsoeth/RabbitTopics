using MassTransit;
using MassTransit.Configuration;
using MassTransit.RabbitMqTransport.Configuration;
using MassTransit.Transports.Fabric;

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
                
                cfg.Publish<Message>(x =>
                {
                    x.ExchangeType = "topic";
                });
            });

            await busControl.StartAsync();

            while (true)
            {
                var message = new Message { Text = "DUMMY TEXT" };
                await busControl.Publish(message,context =>
                {
                    context.SetRoutingKey("msg.abc");
                });
                
                Console.WriteLine($" [x] Sent 'msg.abc':'DUMMY TEXT'");
                
                await Task.Delay(1000);
            }
        }
    }
    
    public class CustomPipeSpecification : IPipeSpecification<PublishContext>
    {
        public void Apply(IPipeBuilder<PublishContext> builder)
        {
            // Your implementation here
        }

        public IEnumerable<ValidationResult> Validate()
        {
            // Your validation logic here
            return Enumerable.Empty<ValidationResult>();
        }
    }

    public class Message
    {
        public string Text { get; set; }
    }
}