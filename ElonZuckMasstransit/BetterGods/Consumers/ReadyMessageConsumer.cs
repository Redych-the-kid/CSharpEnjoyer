using Contracts;
using MassTransit;

namespace BetterGods.Consumers;

public class ReadyMessageConsumer : IConsumer<ReadyMessage>
{
    private static int _counter;
    private static int _count;
    public async Task Consume(ConsumeContext<ReadyMessage> context)
    {
        _counter++;
        if (_counter == 2)
        {
            _count++;
            using (HttpClient client = new HttpClient())
            {
                using HttpResponseMessage responce1 = await client.GetAsync("http://127.0.0.1:5001/getcolor");
                using HttpResponseMessage responce2 = await client.GetAsync("http://127.0.0.1:5002/getcolor");
                responce1.EnsureSuccessStatusCode();
                responce2.EnsureSuccessStatusCode();
                int responceBody1 = Convert.ToInt32(await responce1.Content.ReadAsStringAsync());
                int responceBody2 = Convert.ToInt32(await responce2.Content.ReadAsStringAsync());
                if (responceBody1 == responceBody2)
                {
                    Console.WriteLine(_count + ": Sucess");
                }
                else
                {
                    Console.WriteLine(_count + ": Failed");
                }
                _counter = 0;
            }
        }
    }
}