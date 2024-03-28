using System.Globalization;
using Google.Apis.Calendar.v3.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace GoogleCalendarWebApp.Controllers;

public class CalendarEventController : Controller
{
    public const string googleEventUrl = "https://www.googleapis.com/calendar/v3/calendars/calendarId/events";

    // [HttpPost]
    public ActionResult CreateEvent(Event calendarEvent)
    {
        string tokenFile = "/Users/edrisym/Desktop/webApp/File/token.json";
        var tokens = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

        //TODO
        var restClient = new RestClient();
        var request = new RestRequest();
        // "yyyy-MM-dd'T'HH:mm:ss.fffK"
        calendarEvent.Start.DateTime = DateTime.Parse(calendarEvent.Start.DateTime.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None);
        calendarEvent.End.DateTime = DateTime.Parse(calendarEvent.Start.DateTime.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None);

        var model = JsonConvert.SerializeObject(calendarEvent, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });

        request.AddQueryParameter("key", "AIzaSyDqfzOuygFWIarznCEjw1AE_Kzw40rJh3s");
        request.AddHeader("Authorization", "Bearer" + tokens["access_token"]);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Content-Type", "application/json");
        request.AddParameter("application/json", model, ParameterType.RequestBody);

        restClient = new RestClient(googleEventUrl);

        var response = restClient.Post(request);
        System.Console.WriteLine("Event was successfully created!");

        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            return RedirectToAction("Index", "Home", new { status = "success" });
        }
        return View("Error");
    }
}
