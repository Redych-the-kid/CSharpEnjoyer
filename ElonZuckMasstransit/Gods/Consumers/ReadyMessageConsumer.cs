using Contracts;
using MassTransit;

namespace Gods.Consumers;

public class ReadyMessageConsumer : IConsumer<ReadyMessage>
{
    private static int _counter;
    public async Task Consume(ConsumeContext<ReadyMessage> context)
    {
        _counter++;
        if (_counter == 2)
        {
            using (HttpClient client = new HttpClient())
            {
                using HttpResponseMessage responce1 = await client.GetAsync("http://127.0.0.1:5001/getcolor");
                using HttpResponseMessage responce2 = await client.GetAsync("http://127.0.0.1:5002/getcolor");
                responce1.EnsureSuccessStatusCode();
                responce2.EnsureSuccessStatusCode();
                int responceBody1 = Convert.ToInt32(await responce1.Content.ReadAsStringAsync());
                int responceBody2 = Convert.ToInt32(await responce2.Content.ReadAsStringAsync());
                ExperimentsResults.Count++;
                Console.WriteLine(ExperimentsResults.Count);
                if (responceBody1 == responceBody2)
                {
                    ExperimentsResults.SucessCount++;
                }
                _counter = 0;
            }
        }
    }
}