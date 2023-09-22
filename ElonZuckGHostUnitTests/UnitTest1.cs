using NUnit.Framework;
using ElonZuckGHost;
using Strategies;

namespace TestProject1;

public class Tests
{
    private int[] _cards = null!;
    [SetUp]
    public void Setup()
    {
        _cards = new int[36];
        CollisiumExperimentWorker.Fill(_cards);
    }

    [Test]
    public void DeckTest()
    {
        int redCounter = 0;
        int blackCounter = 0;
        foreach (var a in _cards)
        {
            if (a == 1)
            {
                redCounter++;
            }
            else
            {
                blackCounter++;
            }
        }
        Assert.AreEqual(redCounter, blackCounter);
        Assert.Pass();
    }
    [Test]
    public void DeckShuffleTest()
    {
        IDeckShuffler shuffler = new DeckShuffler();
        int[] copy = _cards;
        _cards = shuffler.Shuffle(_cards);
        Assert.AreNotEqual(_cards, copy);
        Assert.Pass();
    }

    [Test]
    public void ElonStrategyTest()
    {
        ElonStrategy strategy = new ElonStrategy();
        int[] cards = { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1 };
        strategy.Cards = cards;
        Assert.AreEqual(12, strategy.Decide());
        Assert.Pass();
    }

    [Test]
    public void ZuckStrategyTest()
    {
        ZuckStrategy strategy = new ZuckStrategy();
        int[] cards = { 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 1, 1, 1 };
        strategy.Cards = cards;
        Assert.AreEqual(12, strategy.Decide());
        Assert.Pass();
    }

    [Test]
    public void ExperimentTest()
    {
        //TODO: MOOQ
        ZuckStrategy strategy1 = new ZuckStrategy();
        ElonStrategy strategy2 = new ElonStrategy();
        CollisiumSandbox sandbox = new CollisiumSandbox(strategy1, strategy2);
        int[] cards =
        {
            0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
        };
        sandbox.AddCards(cards);
        Assert.AreEqual(false, sandbox.RunControllableSandboxIteration());
        Assert.Pass();
    }
}