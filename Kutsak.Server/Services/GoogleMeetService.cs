using System.Globalization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Kutsak.Server.Altegio;

namespace Kutsak.Server.Services;

public class GoogleMeetService
{
    private const string ApplicationName = "Kutsak Serverrr";
    private const string CalendarId = "primary";

    private readonly CalendarService _calendar;
    
    public GoogleMeetService() {
        _calendar = CreateCalendar();
    }

    private CalendarService CreateCalendar() {
        var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? throw new Exception("GOOGLE_CLIENT_ID is not set");
        var clientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? throw new Exception("GOOGLE_CLIENT_SECRET is not set");
        var refreshToken = Environment.GetEnvironmentVariable("GOOGLE_REFRESH_TOKEN") ?? throw new Exception("GOOGLE_REFRESH_TOKEN is not set");
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            Scopes = [ CalendarService.Scope.CalendarEvents ]
        });
        var token = new TokenResponse { RefreshToken = refreshToken };
        var credential = new UserCredential(flow, "user", token);

        return new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });
    }

    public async Task<Event> CreateEventAsync(AltegioBookingBody payload) {
        var length = payload.Data.SeanceLength.GetValueOrDefault();
        if (length == 0) {
            throw new Exception("Invalid seance length");
        }
        
        var start = DateTime.Parse(payload.Data.Date, AltegioBookingService.Format);
        var end = start.AddSeconds(length);
        
        var meetingEmail = Environment.GetEnvironmentVariable("MEETING_EMAIL");

        var newEvent = new Event {
            Summary = "Консультація з Адвокатом",
            Start = new EventDateTime() { DateTimeDateTimeOffset = start, TimeZone = "Europe/Kyiv" },
            End = new EventDateTime() { DateTimeDateTimeOffset = end, TimeZone = "Europe/Kyiv" },
            ExtendedProperties = new Event.ExtendedPropertiesData()
            {
                Private__ = new Dictionary<string, string>()
                {
                    { "AltegioBookingId", payload.Data.Id.GetValueOrDefault().ToString() }
                }
            },
            ConferenceData = new ConferenceData()
            {
                CreateRequest = new CreateConferenceRequest()
                {
                    RequestId = Guid.NewGuid().ToString(), 
                    ConferenceSolutionKey = new ConferenceSolutionKey() { Type = "hangoutsMeet" }
                }
            },
            Attendees = new List<EventAttendee>()
        };

        if (!string.IsNullOrWhiteSpace(payload.Data.Client.Email)) {
            newEvent.Attendees.Add(new EventAttendee { Email = payload.Data.Client.Email });
        }
        if (!string.IsNullOrWhiteSpace(meetingEmail) && meetingEmail != payload.Data.Client.Email) {
            newEvent.Attendees.Add(new EventAttendee { Email = meetingEmail });
        }

        var request = _calendar.Events.Insert(newEvent, CalendarId);
        request.ConferenceDataVersion = 1; 
        request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All; 

        return await request.ExecuteAsync();
    }
    
    public async Task DeleteEventAsync(long altegioBookingId) {
        var listRequest = _calendar.Events.List(CalendarId);
        listRequest.PrivateExtendedProperty = $"AltegioBookingId={altegioBookingId}";
    
        var events = await listRequest.ExecuteAsync();
        if (events.Items.FirstOrDefault() is not { } googleEvent) return;

        var deleteRequest = _calendar.Events.Delete(CalendarId, googleEvent.Id);
        deleteRequest.SendUpdates = EventsResource.DeleteRequest.SendUpdatesEnum.All;

        await deleteRequest.ExecuteAsync();
        Console.WriteLine($"Canceled Google Meeting for Altegio booking {altegioBookingId}");
    }
}
