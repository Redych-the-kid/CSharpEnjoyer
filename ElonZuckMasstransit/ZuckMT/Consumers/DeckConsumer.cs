using Contracts;
using MassTransit;
using Strategies;

namespace ZuckMT.Consumers;

public class DeckConsumer : IConsumer<DeckMessage>
{
    internal static int Number { get; set; }
    internal static int[]? Cards { get; set; }
    public Task Consume(ConsumeContext<DeckMessage> context)
    {
        var cards = context.Message.Deck;
        var deck = cards.Split(';').Select(n => Convert.ToInt32(n)).ToArray();
        IStrategy strategy = new ZuckStrategy();
        strategy.Cards = deck;
        ZuckStats.Cards = deck;
        var decision = strategy.Decide();
        context.Publish(new NumberMessage
        {
            Number = decision,
            Signature = "zuck"
        });
        return Task.CompletedTask;
    }
}