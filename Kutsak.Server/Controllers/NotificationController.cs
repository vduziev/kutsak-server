using Kutsak.Server.Controllers.Base;
using Kutsak.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class NotificationController : KutsakControllerBase
{
    private readonly TelegramNotificationsService _notifications;

    public NotificationController(IServiceProvider services) {
        _notifications = services.GetRequiredService<TelegramNotificationsService>();
    }
    
    [HttpGet("Send/{message}")]
    public async Task<IActionResult> Get(string message = "Test message") {
        Verify("pass", Environment.GetEnvironmentVariable("PASS") ?? "pass");
        
        await _notifications.NotifyAllAsync(message);
        
        return Ok($"Test answer");
    }
}
