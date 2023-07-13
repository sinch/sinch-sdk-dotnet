using Microsoft.AspNetCore.Mvc;
using Sinch;
using Sinch.Numbers.Regions;

namespace WebApiExamples.Controllers;

[ApiController]
[Route("[controller]")]
public class Numbers : ControllerBase
{
    private readonly ISinch _sinch;

    public Numbers(ISinch sinch)
    {
        _sinch = sinch;
    }

    [HttpGet(Name = "AvailableRegions")]
    public async Task<IEnumerable<Region>> Get()
    {
        var regions = await _sinch.Numbers.Regions.List(default);
        return regions;
    }
}
