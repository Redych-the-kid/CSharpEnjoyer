using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Strategies;

namespace ElonZuckGHost;

class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
        {
            services.AddHostedService<CollisiumExperimentWorker>();
            services.AddScoped<CollisiumSandbox>();
            services.AddScoped<IDeckShuffler, DeckShuffler>();
            services.AddScoped<ElonStrategy>();
            services.AddScoped<ZuckStrategy>();
        });
    }
}

public interface IDeckShuffler
{
    public int[] Shuffle(int[] cards);
}

public class DeckShuffler : IDeckShuffler
{
    public int[] Shuffle(int[] cards)
    {
        return cards.OrderBy(_ => Guid.NewGuid()).ToArray();
    }
}


public class CollisiumExperimentWorker : IHostedService
{
    private readonly CollisiumSandbox _collisiumSandbox;
    private int _sucessCount;
    private const int ExperimentsCount = 1000000;

    public CollisiumExperimentWorker(CollisiumSandbox collisiumSandbox)
    {
        _collisiumSandbox = collisiumSandbox;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            try
            {
                int[] array = new int[36];
                Fill(array);
                _collisiumSandbox.AddCards(array);
                for (int i = 0; i < ExperimentsCount; ++i)
                {
                    if (_collisiumSandbox.RunSandboxIteration())
                    {
                        _sucessCount++;
                    }
                }
                Console.WriteLine(Convert.ToDouble(_sucessCount) / ExperimentsCount * 100 + "%");
                await Task.Delay(1000, cancellationToken);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }, cancellationToken);
        return Task.CompletedTask;
    }
    
    public static void Fill(int[] array){
        for(int i = 0; i < 18; ++i){
            array[i] = 0;
        }
        for(int i = 18; i < 36; ++i){
            array[i] = 1;
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

public class CollisiumSandbox 
{
    private int[] _cards = null!;
    private int[] _elonCards = null!;
    private int[] _zuccCards = null!;
    private readonly IDeckShuffler _deckShuffler;
    private readonly IStrategy _zuckStrategy;
    private readonly IStrategy _elonStrategy;
    public CollisiumSandbox(IDeckShuffler deckShuffler, ZuckStrategy zuckStrategy, ElonStrategy elonStrategy)
    {
        _deckShuffler = deckShuffler;
        _zuckStrategy = zuckStrategy;
        _elonStrategy = elonStrategy;
    }

    public CollisiumSandbox(IStrategy zuckStrategy, IStrategy elonStrategy)
    {
        _zuckStrategy = zuckStrategy;
        _elonStrategy = elonStrategy;
        _deckShuffler = null!;
    }
    public void AddCards(int[] cards)
    {
        _cards = cards;
    }

    public bool RunControllableSandboxIteration()
    {
        _zuccCards = _cards.Take(_cards.Length / 2).ToArray();
        _elonCards = _cards.Skip(_cards.Length / 2).ToArray();
        _elonStrategy.Cards = _elonCards;
        _zuckStrategy.Cards = _zuccCards;
        return _zuccCards[_zuckStrategy.Decide()] == _elonCards[_elonStrategy.Decide()];
    }
    public bool RunSandboxIteration()
    {
        if (_deckShuffler == null)
        {
            throw new ArgumentNullException();
        }
        _cards = _deckShuffler.Shuffle(_cards);
        _zuccCards = _cards.Take(_cards.Length / 2).ToArray();
        _elonCards = _cards.Skip(_cards.Length / 2).ToArray();
        _elonStrategy.Cards = _elonCards;
        _zuckStrategy.Cards = _zuccCards;
        return _zuccCards[_zuckStrategy.Decide()] == _elonCards[_elonStrategy.Decide()];
    }
}