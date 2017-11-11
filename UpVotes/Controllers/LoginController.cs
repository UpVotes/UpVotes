using RestSharp;
using System;
using System.Web.Mvc;
using TweetSharp;

namespace UpVotes.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier)
        {
            var requesttoken = new OAuthRequestToken { Token = oauth_token };
            string key = "AVh7505NzqBlYXTWQjF5x4PVM";
            string secret = "7wYQjSdLtJKqMfpO27FI1cOIX7czTjTTcnyggtm1y6XmjcqeHc";

            try
            {
                TwitterService service = new TwitterService(key, secret);
                OAuthAccessToken accesstoken = service.GetAccessToken(requesttoken, oauth_verifier);
                service.AuthenticateWith(accesstoken.Token, accesstoken.TokenSecret);
                VerifyCredentialsOptions option = new VerifyCredentialsOptions();
                TwitterUser user = service.VerifyCredentials(option);

                //AppendTwitterAuthCookie(accesstoken.ScreenName, accesstoken.Token, accesstoken.TokenSecret);
                Session["Name"] = user.Name;
                Session["userpic"] = user.ProfileImageUrl;
                return RedirectToAction("HomePage","Home");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult LinkedINcall()
        {
            //Need to install below library
            //Install-Package Restsharp
            return Redirect("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=81emosqfu7h5el&redirect_uri=http://localhost:57671/Login/LinkedINAuth&state=lkjlkxcxcx66&scope=r_basicprofile");
        }


        public ActionResult LinkedINAuth(string code, string state)
        {
            //This method path is your return URL
            string[] myArray = state.Split('-');
            try
            {
                var a = Request.Url;
                var client = new RestClient("https://www.linkedin.com/oauth/v2/accessToken");
                var request = new RestRequest(Method.POST);
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", code);
                request.AddParameter("redirect_uri", "http://localhost:57671/Login/LinkedINAuth");
                request.AddParameter("client_id", "81emosqfu7h5el");
                request.AddParameter("client_secret", "kjOQEHimgf15dEhr");
                var response = client.Execute<InventoryItem>(request);
                var content = response.Content;
                string access_token = response.Data.access_token;
                //client = new RestClient("https://api.linkedin.com/v1/people/~?oauth2_access_token=" + access_token + "&format=json");
                client = new RestClient("https://api.linkedin.com/v1/people/~:(id,first-name,last-name,certifications,date-of-birth,email-address,picture-url,summary,public-profile-url,positions,skills,location)?oauth2_access_token=" + access_token + "&format=json");
                request = new RestRequest(Method.GET);
                var response1 = client.Execute(request);
                content = response.Content;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (myArray[1] == "H")
            {
                return RedirectToAction("HomePage", "Home");
            }
            else if(myArray[1] == "L")
            {
                return Redirect("http://localhost:57671/CompanyList/CompanyList/web-developement");
            }
            else if(myArray[1] == "C")
            {
                return Redirect("http://localhost:57671/Company/Company/web-developement");
            }

            return null;
        }

        public class InventoryItem { public string access_token { get; set; } }

        public ActionResult TwitterAuth()
        {
            string key = "AVh7505NzqBlYXTWQjF5x4PVM";
            string secret = "7wYQjSdLtJKqMfpO27FI1cOIX7czTjTTcnyggtm1y6XmjcqeHc";

            TwitterService service = new TwitterService(key, secret);
            OAuthRequestToken requestToken = service.GetRequestToken("http://localhost:57671/Login/TwitterCallback?companyid=12");

            Uri uri = service.GetAuthenticationUrl(requestToken);
            //string script = "<html><head><script type='text/javascript'> var popupWindow =window.open('" + uri.ToString() + "','_blank','directories=no, status=no, menubar=no, scrollbars=yes, resizable=no,width=600, height=280,top=200,left=200');</script></head></html>";
            return Redirect(uri.ToString());
            //return Content(script);
        }

        [HttpPost]
        public ActionResult LinkedINcall(int companyid, char calledPage)
        {
            //Need to install below library
            //Install-Package Restsharp
            return Json("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=81emosqfu7h5el&redirect_uri=http://localhost:57671/Login/LinkedINAuth&state=" + companyid + "-" + calledPage + "&scope=r_basicprofile");
        }

        [HttpPost]
        public ActionResult TwitterCall(int companyid)
        {
            string key = "AVh7505NzqBlYXTWQjF5x4PVM";
            string secret = "7wYQjSdLtJKqMfpO27FI1cOIX7czTjTTcnyggtm1y6XmjcqeHc";

            TwitterService service = new TwitterService(key, secret);
            OAuthRequestToken requestToken = service.GetRequestToken("http://localhost:57671/Login/TwitterCallback?companyid="+companyid);

            Uri uri = service.GetAuthenticationUrl(requestToken);
            //string script = "<html><head><script type='text/javascript'> var popupWindow =window.open('" + uri.ToString() + "','_blank','directories=no, status=no, menubar=no, scrollbars=yes, resizable=no,width=600, height=280,top=200,left=200');</script></head></html>";
            return Json(uri.ToString());
        }
    }
}