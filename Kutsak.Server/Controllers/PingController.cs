using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class PingController : ControllerBase
{
    [HttpGet("{*ignored}")]
    public IActionResult Get() {
        return Ok("Pong");
    }
}
