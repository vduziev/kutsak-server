using System.Globalization;
using System.Text.Json;
using Kutsak.Server.Controllers.Base;
using Kutsak.Server.Services;
using Microsoft.AspNetCore.Mvc;
using YourApp.Models.Altegio;

namespace Kutsak.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AltegioController : KutsakControllerBase
{
    private readonly TelegramNotificationsService _notifications;
    private readonly GoogleMeetService _meet;

    public AltegioController(TelegramNotificationsService notifications) {
        _notifications = notifications;
        _meet = new GoogleMeetService();
    }

    [HttpPost("Test")]
    public async Task<IActionResult> Post([FromBody] JsonElement body) {
        //Console.WriteLine(body.GetRawText());
        
        var payload = JsonSerializer.Deserialize<AltegioWebhookPayload>(body.GetRawText());
        if (payload is null) {
            Console.WriteLine("Cannot deserialize");
            return BadRequest("Cannot deserialize to a webhook payload");
        }

        switch (payload.Status) {
            case "update":
                return Ok();
            case "delete":
                await _meet.DeleteEventByAltegioIdAsync(payload.Data.Id.GetValueOrDefault());
                return Ok();
            case "create": {
                var time = DateTime.Parse(payload.Data.Date, new DateTimeFormatInfo() {
                    DateSeparator = "-",
                    TimeSeparator = ":",
                    FullDateTimePattern = "yyyy-MM-dd HH:mm:ss",
                });
        
                var meetLink = await _meet.CreateMeetLinkAsync(payload);

                await _notifications.NotifyAllAsync(
                    $"""
                     <i>{payload.Data.Id} | {payload.Status}</i>
                     <b>Отримано новий запис!</b>

                     <b>Ім'я:</b> {payload.Data.Client.Name}
                     <b>Е. Пошта:</b> {payload.Data.Client.Email}
                     <b>Телефон:</b> <code>{payload.Data.Client.Phone}</code>

                     <b>Дата:</b> {time}
                     <b>Google Meet:</b> {meetLink}

                     <b>Повідомлення:</b> {(string.IsNullOrWhiteSpace(payload.Data.Comment) ? "<i>немає</i>" : "")}
                     {payload.Data.Comment}
                     """
                );
        
                return Ok(meetLink);
            }
        }
        
        return BadRequest("Unknown status");
    }
}
