﻿using MassTransit;
using System;
using System.Threading.Tasks;
using SharedKernel;

namespace RabbitConsumer
{
    public class MessageConsumer : IConsumer<Message>
    {
        public async Task Consume(ConsumeContext<Message> context)
        {
            var messageText = context.Message.Text;
            Console.WriteLine($" [x] Received 'msg.abc':'{messageText}'");

            // Process the message (replace with your logic)
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
                    e.ConfigureConsumeTopology = false; // To handle custom routing key binding
                    e.Bind<Message>(exchange =>
                    {
                        exchange.RoutingKey = "*.xyz";
                        exchange.ExchangeType = "topic";
                    });

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