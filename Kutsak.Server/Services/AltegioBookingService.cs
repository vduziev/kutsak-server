using System.Globalization;
using Kutsak.Server.Altegio;
using Microsoft.AspNetCore.Mvc;

namespace Kutsak.Server.Services;

public class AltegioBookingService
{
    private readonly TelegramNotificationsService _notifications;
    private readonly GoogleMeetService _meet;
    
    private readonly Dictionary<long, AltegioBookingBody> _unpaidBookings = [];
    
    public static DateTimeFormatInfo Format = new() {
        DateSeparator = "-",
        TimeSeparator = ":",
        FullDateTimePattern = "yyyy-MM-dd HH:mm:ss",
    };
    
    public AltegioBookingService(IServiceProvider services) {
        _notifications = services.GetRequiredService<TelegramNotificationsService>();
        _meet = services.GetRequiredService<GoogleMeetService>();
    }

    public async Task ProcessBookingAsync(AltegioBookingBody booking) {
        if (!booking.Data.Id.HasValue) return;
        var id = booking.Data.Id.GetValueOrDefault();

        Console.WriteLine("Looking at");
        Console.WriteLine(booking.Status);
        Console.WriteLine(booking.Data.PaidFull.GetValueOrDefault());
        Console.WriteLine(booking.Data.Prepaid.GetValueOrDefault());
        Console.WriteLine(booking.Data.PrepaidConfirmed.GetValueOrDefault());
        Console.WriteLine(booking.Data.Date);
        Console.WriteLine(booking.Data.Client.Email);
        Console.WriteLine("-");

        switch (booking.Status) {
            case "create" when booking.Data.PaidFull.GetValueOrDefault() != 0:
                Console.WriteLine($"Received prepaid booking: {id}");
                await PostBookingAsync(booking);
                break;
            case "create":
                Console.WriteLine($"Received unpaid booking: {id}");
                _unpaidBookings.Add(id, booking);
                break;
            case "update" when booking.Data.PaidFull.GetValueOrDefault() != 0: {
                Console.WriteLine($"Removing...");
                if (_unpaidBookings.Remove(id)) {
                    Console.WriteLine($"Booking was paid for: {id}");
                    await PostBookingAsync(booking);
                }

                break;
            }
            case "delete":
                Console.WriteLine($"Booking was deleted: {id}");
                _unpaidBookings.Remove(id);
                await DeleteBookingAsync(id);
                break;
        }
        Console.WriteLine("----------------");
    }

    public async Task ConfirmBookingAsync(long bookingId) {
        Console.WriteLine("Confirming booking...");
        if (!_unpaidBookings.TryGetValue(bookingId, out var booking)) return;
        Console.WriteLine($"Booking {booking.Data.Id} found");
        
        await PostBookingAsync(booking);
    }
    
    public async Task PostBookingAsync(AltegioBookingBody booking) {
        var meetingEvent = await _meet.CreateEventAsync(booking);
        
        var time = DateTime.Parse(booking.Data.Date, Format);
        
        await _notifications.NotifyAllAsync(
            $"""
             <i>{booking.Data.Id} | {booking.Status}</i>
             <b>Отримано новий запис!</b>

             <b>Ім'я:</b> {booking.Data.Client.Name}
             <b>Е. Пошта:</b> {booking.Data.Client.Email}
             <b>Телефон:</b> <code>{booking.Data.Client.Phone}</code>

             <b>Дата:</b> {time}
             <b>Google Meet:</b> {meetingEvent.HangoutLink}
             <b>Google Calendar:</b> {meetingEvent.HtmlLink}

             <b>Повідомлення:</b> {(string.IsNullOrWhiteSpace(booking.Data.Comment) ? "<i>немає</i>" : "")}
             {booking.Data.Comment}
             """
        );
    }

    public async Task DeleteBookingAsync(long bookingId) {
        await _meet.DeleteEventAsync(bookingId);
    }
}
