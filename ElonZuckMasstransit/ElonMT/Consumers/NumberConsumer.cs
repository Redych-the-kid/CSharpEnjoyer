using Contracts;
using MassTransit;

namespace ElonMT.Consumers;

public class NumberConsumer : IConsumer<NumberMessage>
{
    public Task Consume(ConsumeContext<NumberMessage> context)
    {
        if (context.Message.Signature != "elon")
        {
            ElonStats.Color = ElonStats.Cards[context.Message.Number];
            ResourceLock.ResourceAvailable();
            // Console.WriteLine("Write to new queue 3");
        }
    
        return Task.CompletedTask;
    }
}