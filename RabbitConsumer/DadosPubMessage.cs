using MassTransit;

namespace RabbitConsumer;

[MessageUrn("dadospub")]
[EntityName("dadospub")]
public class DadosPubMessage
{
    public string Text { get; set; }
}