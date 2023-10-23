using Microsoft.AspNetCore.Mvc;

namespace ElonMT.Controllers;

[ApiController]
public class ElonWebController : ControllerBase
{
    [Route("getcolor")]
    [HttpGet]
    public async Task<int> GetColor()
    {
        await ResourceLock.WaitForResourceAsync();
        return await Task.FromResult(ElonStats.Color);
    }   
}