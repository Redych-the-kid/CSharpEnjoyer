using Microsoft.AspNetCore.Mvc;

namespace ElonMT.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase
{
    [Route("getcolor")]
    [HttpGet]
    public Task<int> GetColor()
    {
        return Task.FromResult(ElonStats.Cards[ElonStats.Number]);
    }
}