using Strategies;

namespace CollisiumExperimentsWorkers;

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
        return _zuccCards[_elonStrategy.Decide()] == _elonCards[_zuckStrategy.Decide()];
    }
    public bool RunSandboxIteration(Experiment experiment)
    {
        if (_deckShuffler == null)
        {
            throw new ArgumentNullException();
        }
        _cards = _deckShuffler.Shuffle(_cards);
        experiment.Cards = string.Join(" ", _cards);
        _zuccCards = _cards.Take(_cards.Length / 2).ToArray();
        _elonCards = _cards.Skip(_cards.Length / 2).ToArray();
        _elonStrategy.Cards = _elonCards;
        _zuckStrategy.Cards = _zuccCards;
        return _zuccCards[_elonStrategy.Decide()] == _elonCards[_zuckStrategy.Decide()];
    }

    public bool RunSandboxIteration()
    {
        if (_deckShuffler == null)
        {
            throw new ArgumentNullException();
        }
        _cards = _deckShuffler.Shuffle(_cards);
        if (_cards.Length == 0)
        {
            return false;
        }
        _zuccCards = _cards.Take(_cards.Length / 2).ToArray();
        _elonCards = _cards.Skip(_cards.Length / 2).ToArray();
        _elonStrategy.Cards = _elonCards;
        _zuckStrategy.Cards = _zuccCards;
        Console.WriteLine(_elonCards.Length);
        Console.WriteLine(_zuccCards.Length);
        return _zuccCards[_elonStrategy.Decide()] == _elonCards[_zuckStrategy.Decide()];
    }
}