﻿using MassTransit;

namespace RabbitConsumer2;

[MessageUrn("dadospub")]
[EntityName("dadospub")]
public class DadosPubData
{
    public string Text { get; set; }
}