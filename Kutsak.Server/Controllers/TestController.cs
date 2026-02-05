using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("{*ignored}")]
    public IActionResult Get() {
        return Ok("Test answer");
    }
}
