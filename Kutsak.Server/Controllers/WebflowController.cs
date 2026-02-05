using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Kutsak.Server.Controllers.Base;
using Kutsak.Server.Services;
using Kutsak.Server.Webflow;
using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WebflowController : KutsakControllerBase
{
    private readonly TelegramNotificationsService _notifications;

    private readonly string _wfToken;
    
    public WebflowController(TelegramNotificationsService notifications) {
        _notifications = notifications;
        
        _wfToken = Environment.GetEnvironmentVariable("WF_TOKEN") ?? throw new Exception("WF_TOKEN is not set");
    }

    private void VerifyWebflow(string body) {
        if (!Request.Headers.TryGetValue("x-webflow-timestamp", out var timestamp) ||
            !Request.Headers.TryGetValue("x-webflow-signature", out var providedSignature))
        {
            throw new UnauthorizedAccessException();
        }
        
        string calculatedSignature;
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_wfToken)))
        {
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{timestamp}:{body}"));
            calculatedSignature = Convert.ToHexString(hashBytes).ToLower();
        }
        
        if (!calculatedSignature.Equals(providedSignature.ToString(), StringComparison.CurrentCultureIgnoreCase))
        {
            throw new UnauthorizedAccessException();
        }
    }
    
    [HttpPost("Form")]
    public async Task<IActionResult> Post() {
        Console.WriteLine($"RECIEVED");
        
        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        
        VerifyWebflow(body);
        
        var submission = JsonSerializer.Deserialize<WebhookSubmission>(body);
        if (submission is null) return BadRequest("Cannot serialize to a webflow submission");
        if (submission.TriggerType != "form_submission") return BadRequest("Not a form submission");
        
        var form = JsonSerializer.Deserialize<FormSubmission>(submission.Payload)!;
        
        if (form.Name != "Contact Us") return BadRequest("Not a contact us form");
        
        Console.WriteLine($"Submission: {submission.TriggerType}");
        Console.WriteLine($"Form: {form.FormId}");
        
        await _notifications.NotifyAllAsync(
            $"""
            <b>Отримано нову форму!</b>
            
            <b>Ім'я:</b> {form.Data.CustomerName}
            <b>Е. Пошта:</b> {form.Data.CustomerEmail}
            <b>Телефон:</b> <code>{form.Data.PhoneNumber}</code>
            
            <b>Вид Послуги:</b> <i>{form.Data.Service}</i>
            
            <b>Повідомлення:</b> {(string.IsNullOrWhiteSpace(form.Data.CustomerMessage) ? "<i>no message</i>" : "")}
            {form.Data.CustomerMessage}
            """
        );
        
        return Ok();
    }
}
