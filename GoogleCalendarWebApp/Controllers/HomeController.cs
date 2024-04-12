using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Newtonsoft.Json.Linq;

namespace GoogleCalendarWebApp.Controllers
{
    // private IGoogleCalendarService _googleCalendarService;
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        // public ActionResult OauthRedirect()
        // {

        //     var credentialsFile = "/Users/edrisym/Desktop/webApp/File/Credentials.json";

        //     var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));

        //     var client_id = credentials["client_id"];
        //     var redirectUri =
        //                         "https://accounts.google.com/o/oauth2/v2/auth?" +
        //                         "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
        //                         "access_type=offline&" +
        //                         "include_granted_scopes=true&" +
        //                         "response_type=code&" +
        //                         "state=successful&" +
        //                         "redirect_uri=https://localhost:7130/oauth/callback&" +
        //                         "client_id=" + client_id;
        //     System.Console.WriteLine("redirectUrl was successfull");
        //     return Redirect(redirectUri);
        // }

        public ActionResult GoogleAuth()
        {
            return Redirect(GetAuthCode());
        }


        public string GetAuthCode()
        {
            var credentialsFile = "/Users/edrisym/Desktop/webApp/File/Credentials.json";

            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));
            try
            {
                string scopeURL1 = "https://accounts.google.com/o/oauth2/auth?redirect_uri={0}&state={1}&response_type={2}&client_id={3}&scope={4}&access_type={5}&include_granted_scopes={6}";
                var redirectURL = "https://localhost:7130/oauth/callback";
                string response_type = "code";
                var client_id = credentials["client_id"];
                string scope = "https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events";
                string access_type = "offline";
                var state = "successful";
                var include_granted_scopes = "true";
                string redirect_uri_encode = Method.UrlEncodeForGoogle(redirectURL);
                var mainURL = string.Format(scopeURL1, redirect_uri_encode, state, response_type, client_id, scope, access_type, include_granted_scopes);

                return mainURL;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }

    public static class Method
    {
        public static string UrlEncodeForGoogle(string url)
        {
            string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-.~";
            var result = new StringBuilder();
            foreach (char symbol in url)
            {
                if (unreservedChars.IndexOf(symbol) != -1)
                {
                    result.Append(symbol);
                }
                else
                {
                    result.Append("%" + ((int)symbol).ToString("X2"));
                }
            }

            return result.ToString();

        }
    }
}