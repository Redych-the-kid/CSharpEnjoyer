using Contracts;
using Gods.Consumers;
using MassTransit;

namespace Gods;

public class Program
{
    public static async Task Main(string[] args)
    {
        var cards = new int[36];
        Fill(cards);
        IDeckShuffler shuffler = new DeckShuffler();
        cards = shuffler.Shuffle(cards);
        int[] zuccCards;
        int[] elonCards;
        
        var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
        {
            sbc.Host(new Uri("rabbitmq://localhost"), h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
            sbc.ReceiveEndpoint("new_queue_3", ep =>
            {
                ep.Consumer<ReadyMessageConsumer>();
            });
        });
        bus.Start();
        var sendPoint1 = await bus.GetSendEndpoint(new Uri("rabbitmq://localhost/new_queue"));
        var sendPoint2 = await bus.GetSendEndpoint(new Uri("rabbitmq://localhost/new_queue_2"));
        for (int i = 0; i < 20; i++)
        {
            Fill(cards);
            shuffler = new DeckShuffler();
            cards = shuffler.Shuffle(cards);
            zuccCards = cards.Take(cards.Length / 2).ToArray();
            elonCards = cards.Skip(cards.Length / 2).ToArray();
            var task1 = sendPoint1.Send(new DeckMessage
            {
                Deck = string.Join(";", elonCards)
            });
            var task2 = sendPoint2.Send(new DeckMessage()
            {
                Deck = string.Join(";", zuccCards)
            });
            Thread.Sleep(500);
        }
        Console.WriteLine(ExperimentsResults.Count);
        Console.WriteLine(ExperimentsResults.SucessCount);
        var result = (float)ExperimentsResults.SucessCount / ExperimentsResults.Count * 100;
        Console.WriteLine(result + " %");
        bus.Stop();
    }
    private static void Fill(int[] array)
    {
        for(int i = 0; i < 18; ++i){
            array[i] = 0;
        }
        for(int i = 18; i < 36; ++i){
            array[i] = 1;
        }
    }
}