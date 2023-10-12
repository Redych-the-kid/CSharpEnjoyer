using Microsoft.AspNetCore.Mvc;
using Strategies;

namespace ZuckWeb.Controllers;

[ApiController]
public class StrategyController : ControllerBase
{
    [Route("getchoice")]
    [HttpGet]
    public async Task<int> Get(string cards)
    {
        Console.WriteLine(cards);
        var deck = cards.Split(';').Select(n => Convert.ToInt32(n)).ToArray();
        IStrategy strategy = new ZuckStrategy();
        strategy.Cards = deck;
        return strategy.Decide();
    }
}