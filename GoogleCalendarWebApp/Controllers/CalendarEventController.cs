using System.Globalization;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace GoogleCalendarWebApp.Controllers;

public class CalendarEventController : Controller
{
    public const string googleEventUrl = "https://www.googleapis.com/calendar/v3/calendars/primary/events";

    // [HttpPost]
    public ActionResult CreateEvent(Event calendarEvent)
    {
        string credentialFile = "/Users/edrisym/Desktop/webApp/File/Credentials.json";
        string tokenFile = "/Users/edrisym/Desktop/webApp/File/token.json";
        var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));
        var credential = JObject.Parse(System.IO.File.ReadAllText(credentialFile));
        //TODO
        // var restClient = new RestClient();
        // var restRequest = new RestRequest();

        // var timeNow = DateTime.UtcNow;ssssss

        // restClient = new RestClient(googleEventUrl);

        var newEvent = new Event()
        {
            Summary = "this is an invitation",
            Location = "herwe",
            Description = "edris.",
            Start = new EventDateTime()
            {

                DateTime = DateTime.Now,
                TimeZone = "Asia/Tehran",
            },
            End = new EventDateTime()
            {
                DateTime = DateTime.Now,
                TimeZone = "Asia/Tehran",
            },
            Attendees = new EventAttendee[] {
        new EventAttendee() { Email = "hafkhat.76@gmail.com" },
        },
            Reminders = new Event.RemindersData()
            {
                UseDefault = false,
                Overrides = new EventReminder[] {
            new EventReminder() { Method = "email", Minutes = 24 * 60 },
            new EventReminder() { Method = "sms", Minutes = 10 },
        }
            }
        };

        var client_id = credential["client_id"].ToString();
        var client_secret = credential["client_secret"].ToString();

        var secrets = new ClientSecrets()
        {
            ClientId = client_id,
            ClientSecret = client_secret
        };

        var token = new TokenResponse { RefreshToken = tokens["refresh_token"].ToString() };

        var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
            new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = secrets
            }),
            "user",
            token);

        var service = new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials,
            ApplicationName = "CalendarApi",
        });

        var calendarId = "primary";
        EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
        try
        {
            Event createdEvent = request.Execute();
            Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
        }
        catch (System.Exception ex)
        {

            throw ex;
        }

        return RedirectToAction("Index", "Home", new { status = "success" });
    }
}
