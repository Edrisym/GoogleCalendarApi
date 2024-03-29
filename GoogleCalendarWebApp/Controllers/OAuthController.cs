using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace GoogleCalendarWebApp.Controllers
{
    public class OAuthController : Controller
    {
        private const string refreshToken = "https://oauth2.googleapis.com/token";
        private const string revokeToken = "https://oauth2.googleapis.com/revoke";

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

            restClient = new RestClient(refreshToken);

            var response = restClient.Post(request);
            System.Console.WriteLine("request was successfully sent!");


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.Console.WriteLine("StatusCode is OK!");
                System.IO.File.WriteAllText(tokenFile, response.Content);

                return RedirectToAction("Index", "Home", new { status = "success" });
            }
            return View("Error");
        }


        public ActionResult RefreshToken()
        {
            string tokenFile = "/Users/edrisym/Desktop/webApp/File/token.json";
            string CredentialsFile = "/Users/edrisym/Desktop/webApp/File/Credentials.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(CredentialsFile));
            var token = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            var restClient = new RestSharp.RestClient();
            var request = new RestSharp.RestRequest();

            request.AddQueryParameter("client_id", credentials["client_id"].ToString());
            request.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            request.AddQueryParameter("grant_type", "refresh_token");
            request.AddQueryParameter("refresh_token", token["refresh_token"].ToString());

            restClient = new RestClient("https://oauth2.googleapis.com/token");


            var response = restClient.ExecutePost(request);
            // var newTokens = JObject.Parse(response.Content).ToString();

            // System.Console.WriteLine(newTokens);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.Console.WriteLine("request was successfully sent!");
                var newTokens = JObject.Parse(response.Content);
                newTokens["refresh_token"] = token["refresh_token"].ToString();
                System.IO.File.WriteAllText(tokenFile, newTokens.ToString());
                return RedirectToAction("Index", "Home", new { status = "success" });
            }
            return View("Error");
        }

        public ActionResult RevokeToken()
        {
            string tokenFile = "/Users/edrisym/Desktop/webApp/File/token.json";
            var token = JObject.Parse(System.IO.File.ReadAllText(tokenFile));

            var restClient = new RestSharp.RestClient();
            var request = new RestSharp.RestRequest();

            request.AddQueryParameter("token", token["access_token"].ToString());

            restClient = new RestClient("https://oauth2.googleapis.com/revoke");
            var response = restClient.ExecutePost(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var newtoken = JObject.Parse(System.IO.File.ReadAllText(tokenFile));
                System.Console.WriteLine($"successfully revoked the token = {0}", newtoken);
                return RedirectToAction("Index", "Home", new { status = "success" });
            }

            return View("Error");
        }
    }

}
