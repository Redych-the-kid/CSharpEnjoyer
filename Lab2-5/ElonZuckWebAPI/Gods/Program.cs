namespace Gods;

public static class Program
{
    private static readonly int _elonPort = 5001;
    private static readonly int _zuckPort = 5002;
    public static void Main(string[] args)
    {
        var cards = new int[36];
        Fill(cards);
        IDeckShuffler shuffler = new DeckShuffler();
        cards = shuffler.Shuffle(cards);
        var zuccCards = cards.Take(cards.Length / 2).ToArray();
        var elonCards = cards.Skip(cards.Length / 2).ToArray();
        var elon = SendDeck(elonCards, _elonPort).Result;
        var zuck = SendDeck(zuccCards, _zuckPort).Result;
        if (elonCards[zuck] == zuccCards[elon])
        {
            Console.WriteLine("They will be fighting on the arena!");
        }
        else
        {
            Console.WriteLine("Meh, they lost...");
        }
    }

    private static async Task<int> SendDeck(int[] cards, int port)
    {
        using (HttpClient client = new HttpClient())
        {
            using HttpResponseMessage responce = await client.GetAsync($"http://127.0.0.1:{port}/getchoice?cards={string.Join(";", cards)}");
            responce.EnsureSuccessStatusCode();
            int responceBody = Convert.ToInt32(await responce.Content.ReadAsStringAsync());
            return responceBody;
        }
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