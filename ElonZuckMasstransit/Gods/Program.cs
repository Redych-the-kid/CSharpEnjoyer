using Contracts;
using MassTransit;

namespace Gods;

public class Program
{
    private static int sucessCount;

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
        });
        bus.Start();
        var sendPoint1 = await bus.GetSendEndpoint(new Uri("rabbitmq://localhost/new_queue"));
        var sendPoint2 = await bus.GetSendEndpoint(new Uri("rabbitmq://localhost/new_queue_2"));
        int experimentsCount = 100;
        for (int i = 0; i < experimentsCount; i++)
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
            var elonColor = await getColor(5001);
            var zuckColor = await getColor(5002);
            if (elonColor == zuckColor)
            {
                Console.WriteLine($"{i}: Sucess!");
                sucessCount++;
            }
            else
            {
                Console.WriteLine($"{i}: Failure!");
            }
        }
        var result = (float)sucessCount / experimentsCount * 100;
        Console.WriteLine(result + " %");
        bus.Stop();
    }

    private static async Task<int> getColor(int port)
    {
        using HttpClient client = new HttpClient();
        using HttpResponseMessage responce1 = await client.GetAsync($"http://127.0.0.1:{port}/getcolor");
        responce1.EnsureSuccessStatusCode();
        int responceBody1 = Convert.ToInt32(await responce1.Content.ReadAsStringAsync());
        return responceBody1;
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