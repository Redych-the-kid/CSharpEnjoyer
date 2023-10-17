using Contracts;
using MassTransit;

namespace ElonMT.Consumers;

public class NumberConsumer : IConsumer<NumberMessage>
{
    public async Task Consume(ConsumeContext<NumberMessage> context)
    {
        if (context.Message.Signature != "elon")
        {
            Console.WriteLine($"This is the message from {context.Message.Signature}. The number is {context.Message.Number}");
            ElonStats.Number = context.Message.Number;
            await context.Publish(new ReadyMessage()
            {
                Ready = true
            });
            // Console.WriteLine("Write to new queue 3");
        }
    }
}