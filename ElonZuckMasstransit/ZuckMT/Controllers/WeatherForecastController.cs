using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ZuckMT.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase
{
    [Route("getcolor")]
    [HttpGet]
    public Task<int> GetColor()
    {
        return Task.FromResult(ZuckStats.Cards[ZuckStats.Number]);
    }
}