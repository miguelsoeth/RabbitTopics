using MassTransit;

namespace RabbitConsumer;

[MessageUrn("dadospub")]
[EntityName("dadospub")]
public class DadosPubData
{
    public string Text { get; set; }
}