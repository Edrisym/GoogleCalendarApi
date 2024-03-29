using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GoogleCalendarWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult OauthRedirect()
        {

            var credentialsFile = "/Users/edrisym/Desktop/webApp/File/Credentials.json";

            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));

            var client_id = credentials["client_id"];
            var redirectUri =
                                "https://accounts.google.com/o/oauth2/v2/auth?" +
                                "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                                "access_type=offline&" +
                                "include_granted_scopes=true&" +
                                "response_type=code&" +
                                "state=successful&" +
                                "redirect_uri=https://localhost:7130/oauth/callback&" +
                                "client_id=" + client_id;
            System.Console.WriteLine("redirectUrl was successfull");
            return Redirect(redirectUri);
        }
    }
}