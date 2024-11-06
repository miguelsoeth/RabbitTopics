using MassTransit;

namespace RabbitManual;

[MessageUrn("dadospub")]
[EntityName("dadospub")]
public class DadosPubMessage
{
    public string Text { get; set; }
    
    public DadosPubData? Data { get; set; }
}