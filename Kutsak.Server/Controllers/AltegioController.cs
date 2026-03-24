using System.Globalization;
using System.Text.Json;
using Kutsak.Server.Altegio;
using Kutsak.Server.Controllers.Base;
using Kutsak.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AltegioController : KutsakControllerBase
{
    private readonly AltegioBookingService _booking;

    public AltegioController(IServiceProvider services) {
        _booking = services.GetRequiredService<AltegioBookingService>();
    }

    [HttpPost("Test")]
    public async Task<IActionResult> Post([FromBody] JsonElement body) {
        Console.WriteLine("- - - body:");
        Console.WriteLine(body.GetRawText());
        Console.WriteLine("- - -");

        switch (body.GetProperty("resource").GetString()) {
            case "record": {
                Console.WriteLine("Got booking");
                var booking = JsonSerializer.Deserialize<AltegioBookingBody>(body.GetRawText());
                if (booking is null) {
                    Console.WriteLine("Cannot deserialize");
                    return BadRequest("Cannot deserialize to a webhook payload");
                }
        
                await _booking.ProcessBookingAsync(booking);
                break;
            }
            case "finances_operation": {
                Console.WriteLine("Got payment");
            
                var payment = JsonSerializer.Deserialize<AltegioTransactionPayload>(body.GetRawText());
                if (payment is null) {
                    Console.WriteLine("Cannot deserialize");
                    return BadRequest("Cannot deserialize to a webhook payload");
                }
            
                await _booking.ConfirmBookingAsync(payment.Data.RecordId.GetValueOrDefault());
                break;
            }
        }

        return Ok();
    }
}
