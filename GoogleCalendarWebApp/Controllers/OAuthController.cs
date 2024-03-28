using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GoogleCalendarWebApp.Controllers
{
    public class OAuthController : Controller
    {
        private const string googleApiToken = "https://oauth2.googleapis.com/token";

        public void Callback(string code, string error, string state)
        {
            if (string.IsNullOrWhiteSpace(error))
                this.GetTokens(code);
        }

        public ActionResult GetTokens(string code)
        {
            string CredentialsFile = "/Users/edrisym/Desktop/webApp/File/Credentials.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(CredentialsFile));

            string tokenFile = "/Users/edrisym/Desktop/webApp/File/token.json";

            //TODO
            var restClient = new RestSharp.RestClient();
            var request = new RestSharp.RestRequest();

            request.AddQueryParameter("client_id", credentials["client_id"].ToString());
            request.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            request.AddQueryParameter("code", code);
            request.AddQueryParameter("grant_type", "authorization_code");
            request.AddQueryParameter("redirect_uri", "https://localhost:7130/oauth/callback");

            restClient = new RestClient(googleApiToken);

            var response = restClient.Post(request);
            System.Console.WriteLine("request was successfully sent!");


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.Console.WriteLine("StatusCode is OK!");
                System.IO.File.WriteAllText(tokenFile, response.Content);

                return RedirectToAction("Index", "Home");
            }
            return View("Error");
        }

    }


}
