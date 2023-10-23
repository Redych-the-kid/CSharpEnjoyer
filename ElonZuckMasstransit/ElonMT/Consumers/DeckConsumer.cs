using Contracts;
using MassTransit;
using Strategies;

namespace ElonMT.Consumers;

public class DeckConsumer : IConsumer<DeckMessage>
{
    public Task Consume(ConsumeContext<DeckMessage> context)
    {
        var cards = context.Message.Deck;
        var deck = cards.Split(';').Select(n => Convert.ToInt32(n)).ToArray();
        IStrategy strategy = new ElonStrategy();
        strategy.Cards = deck;
        ElonStats.Cards = deck;
        var decision = strategy.Decide();
        context.Publish(new NumberMessage
        {
            Number = decision,
            Signature = "elon"
        });
        return Task.CompletedTask;
    }
}