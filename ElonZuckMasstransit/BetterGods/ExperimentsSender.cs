using Contracts;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace BetterGods;

public class ExperimentsSender : BackgroundService
{
    private readonly IBus _bus;

    public ExperimentsSender(IBus bus)
    {
        _bus = bus;
    }
    
    protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
    {
        var cards = new int[36];
        Fill(cards);
        IDeckShuffler shuffler = new DeckShuffler();
        cards = shuffler.Shuffle(cards);
        int[] zuccCards;
        int[] elonCards;
        var sendPoint1 = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/new_queue"));
        var sendPoint2 = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/new_queue_2"));
        for (int i = 0; i < 100; i++)
        {
            Fill(cards);
            shuffler = new DeckShuffler();
            cards = shuffler.Shuffle(cards);
            zuccCards = cards.Take(cards.Length / 2).ToArray();
            elonCards = cards.Skip(cards.Length / 2).ToArray();
            await sendPoint1.Send(new DeckMessage
            {
                Deck = string.Join(";", elonCards)
            });
            await sendPoint2.Send(new DeckMessage()
            {
                Deck = string.Join(";", zuccCards)
            });
            //Thread.Sleep(500);
        }
        return Task.CompletedTask;
    }
    private void Fill(int[] cards)
    {
        for(int i = 0; i < 18; ++i){
            cards[i] = 0;
        }
        for(int i = 18; i < 36; ++i){
            cards[i] = 1;
        }
    }
}