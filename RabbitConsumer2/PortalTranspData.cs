using MassTransit;

namespace RabbitConsumer2;

[MessageUrn("portaltransp")]
[EntityName("portaltransp")]
public class PortalTranspData
{
    public string Text { get; set; }
}