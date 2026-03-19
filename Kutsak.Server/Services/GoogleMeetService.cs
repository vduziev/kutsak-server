using System.Globalization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using YourApp.Models.Altegio;

namespace Kutsak.Server.Services;

public class GoogleMeetService
{
    // Replace these with your actual strings from Google Cloud and the OAuth Playground
    private readonly string ClientId = "334544712513-abla8vqch3rpii5rqcfoadhg3fojb9sv.apps.googleusercontent.com";
    private readonly string ClientSecret = "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";
    private readonly string RefreshToken = "1//04WRWLAM8xcreCgYIARAAGAQSNwF-L9IrIb_YzF9pQ0plKCrrbcyM0dZrfxrni1zfgWNo1yXrZgR5SJ0vHQZm2-j0turmau7RQrw";

    private readonly string ApplicationName = "Kutsak Serverrr";
        
    // "primary" targets the default calendar of the account that generated the Refresh Token
    private readonly string CalendarId = "primary"; 

    // Add clientEmail to the parameters
    public async Task<string> CreateMeetLinkAsync(AltegioWebhookPayload payload) {
        var format = new DateTimeFormatInfo() {
            DateSeparator = "-",
            TimeSeparator = ":",
            FullDateTimePattern = "yyyy-MM-dd HH:mm:ss",
        };

        var length = payload.Data.SeanceLength.GetValueOrDefault();
        if (length == 0) {
            throw new Exception("Invalid seance length");
        }
        
        var start = DateTime.Parse(payload.Data.Date, format);
        var end = start.AddSeconds(length);
        
        var meetingEmail = Environment.GetEnvironmentVariable("MEETING_EMAIL");
        
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets {
                ClientId = ClientId,
                ClientSecret = ClientSecret
            },
            Scopes = [ CalendarService.Scope.CalendarEvents ]
        });
        var token = new TokenResponse { RefreshToken = RefreshToken };
        var credential = new UserCredential(flow, "user", token);

        var service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = ApplicationName,
        });

        var newEvent = new Event()
        {
            Summary = "Консультація з Адвокатом",
            Start = new EventDateTime() { DateTimeDateTimeOffset = start, TimeZone = "Europe/Kyiv" },
            End = new EventDateTime() { DateTimeDateTimeOffset = end, TimeZone = "Europe/Kyiv" },
            ExtendedProperties = new Event.ExtendedPropertiesData()
            {
                Private__ = new Dictionary<string, string>()
                {
                    { "AltegioBookingId", payload.Data.Id.ToString() }
                }
            },
            ConferenceData = new ConferenceData()
            {
                CreateRequest = new CreateConferenceRequest()
                {
                    RequestId = Guid.NewGuid().ToString(), 
                    ConferenceSolutionKey = new ConferenceSolutionKey() { Type = "hangoutsMeet" }
                }
            }
        };
        
        newEvent.Attendees = new List<EventAttendee>();

        if (!string.IsNullOrWhiteSpace(payload.Data.Client.Email)) {
            newEvent.Attendees.Add(new EventAttendee { Email = payload.Data.Client.Email });
        }
        if (!string.IsNullOrWhiteSpace(meetingEmail) && meetingEmail != payload.Data.Client.Email) {
            newEvent.Attendees.Add(new EventAttendee { Email = meetingEmail });
        }

        var request = service.Events.Insert(newEvent, CalendarId);
        request.ConferenceDataVersion = 1; 

        // NEW: 6. Tell Google to send the email invitation!
        if (!string.IsNullOrWhiteSpace(payload.Data.Client.Email))
        {
            // "All" means it will email anyone in the Attendees list
            request.SendUpdates = EventsResource.InsertRequest.SendUpdatesEnum.All; 
        }

        Event createdEvent = await request.ExecuteAsync();
        return createdEvent.HangoutLink; 
    }
    public async Task DeleteEventByAltegioIdAsync(long altegioBookingId)
    {
        try
        {
            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
                },
                Scopes = [ CalendarService.Scope.CalendarEvents ]
            });
            var token = new TokenResponse { RefreshToken = RefreshToken };
            var credential = new UserCredential(flow, "user", token);
            var service = new CalendarService(new BaseClientService.Initializer() { HttpClientInitializer = credential });

            var listRequest = service.Events.List(CalendarId);
            listRequest.PrivateExtendedProperty = $"AltegioBookingId={altegioBookingId}";
        
            var events = await listRequest.ExecuteAsync();

            if (events.Items is { Count: > 0 })
            {
                var googleEventId = events.Items[0].Id;
            
                var deleteRequest = service.Events.Delete(CalendarId, googleEventId);
                deleteRequest.SendUpdates = EventsResource.DeleteRequest.SendUpdatesEnum.All;
            
                await deleteRequest.ExecuteAsync();
                Console.WriteLine($"Successfully canceled Google Meeting for Altegio booking {altegioBookingId}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete event: {ex.Message}");
        }
    }
}
