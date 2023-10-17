using Contracts;
using MassTransit;

namespace ZuckMT.Consumers;

public class NumberConsumer : IConsumer<NumberMessage>
{
    public async Task Consume(ConsumeContext<NumberMessage> context)
    {
        if (context.Message.Signature != "zuck")
        {
            //Console.WriteLine($"This is the message from {context.Message.Signature}. The number is {context.Message.Number}");
            ZuckStats.Number = context.Message.Number;
            await context.Publish(new ReadyMessage()
            {
                Ready = true
            });
        }
    }
}