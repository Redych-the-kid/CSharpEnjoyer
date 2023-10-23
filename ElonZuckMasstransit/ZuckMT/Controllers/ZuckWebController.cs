using Microsoft.AspNetCore.Mvc;

namespace ZuckMT.Controllers;

[ApiController]
public class ZuckWebController : ControllerBase
{
    [Route("getcolor")]
    [HttpGet]
    public async Task<int> GetColor()
    {
        await ResourceLock.WaitForResourceAsync();
        return await Task.FromResult(ZuckStats.color);
    }
}