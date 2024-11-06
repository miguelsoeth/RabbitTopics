using MassTransit;

namespace RabbitManual;

[MessageUrn("portaltransp")]
[EntityName("portaltransp")]
public class PortalTranspMessage
{
    public string Text { get; set; }
    
    public PortalTranspData? Data { get; set; }
}