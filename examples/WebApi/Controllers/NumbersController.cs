using Microsoft.AspNetCore.Mvc;
using Sinch;
using Sinch.Numbers;
using Sinch.Numbers.Regions;

namespace WebApiExamples.Controllers;

[ApiController]
[Route("[controller]")]
public class NumbersController : ControllerBase
{
    private readonly ISinchClient _sinch;

    public NumbersController(ISinchClient sinch)
    {
        _sinch = sinch;
    }

    [HttpGet]
    [Route("regions")]
    public async Task<IEnumerable<Region>> GetRegions()
    {
        var regions = await _sinch.Numbers.Regions.List(new List<Types>());
        return regions;
    }
}
