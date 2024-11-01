using MassTransit;

namespace RabbitConsumer;

[MessageUrn("portaltransp")]
[EntityName("portaltransp")]
public class PortalTranspData
{
    public string Text { get; set; }
}