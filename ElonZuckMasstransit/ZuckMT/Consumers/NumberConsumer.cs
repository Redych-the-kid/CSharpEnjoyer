using Contracts;
using MassTransit;

namespace ZuckMT.Consumers;

public class NumberConsumer : IConsumer<NumberMessage>
{
    public Task Consume(ConsumeContext<NumberMessage> context)
    {
        if (context.Message.Signature != "zuck")
        {
            //Console.WriteLine($"This is the message from {context.Message.Signature}. The number is {context.Message.Number}");
            ZuckStats.color = ZuckStats.Cards[context.Message.Number];
            ResourceLock.ResourceAvailable();
        }

        return Task.CompletedTask;
    }
}