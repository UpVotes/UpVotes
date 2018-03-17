using RestSharp;
using System;
using System.Configuration;
using System.Web.Mvc;
using TweetSharp;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;

namespace UpVotes.Controllers
{
    public class LoginController : Controller
    {
        string _baseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier,string state)
        {
            var requesttoken = new OAuthRequestToken { Token = oauth_token };
            string key = ConfigurationManager.AppSettings["TwitterKey"].ToString();
            string secret = ConfigurationManager.AppSettings["TwitterSecret"].ToString();
            string[] myArray = state.Split('-');
            try
            {
                TwitterService service = new TwitterService(key, secret);
                OAuthAccessToken accesstoken = service.GetAccessToken(requesttoken, oauth_verifier);
                service.AuthenticateWith(accesstoken.Token, accesstoken.TokenSecret);
                VerifyCredentialsOptions option = new VerifyCredentialsOptions();
                TwitterUser user = service.VerifyCredentials(option);
                TwitterLinkedInLoginModel obj = new TwitterLinkedInLoginModel();
                if (user != null)
                {
                    string[] name = user.Name.Split(' ');
                    if (name.Length > 1)
                    {
                        obj.firstName = name[0].ToString();
                        obj.lastName = name[1].ToString();
                    }
                    else
                    {
                        obj.firstName = name[0].ToString();
                        obj.lastName = "";
                    }

                    obj.id = user.Id.ToString();
                    obj.pictureUrl = user.ProfileImageUrlHttps;
                    obj.publicProfileUrl = "https://twitter.com/" + user.ScreenName;
                    obj.userType = 3;


                    UserEntity userObj = new Business.UserService().AddOrUpdateUser(obj);
                    Session["UserObj"] = userObj;
                    Session["UserID"] = userObj.UserID;
                    string message = string.Empty;
                    if (myArray[0] != "0")
                    {
                        message = new Business.CompanyService().VoteForCompany(Convert.ToInt32(myArray[0]), userObj.UserID);
                    }

                    if (myArray[1] == "H")
                    {
                        return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString());
                    }
                    else if (myArray[1] == "L")
                    {
                        return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + Convert.ToString(Session["FocusAreaName"]));
                    }
                    else if (myArray[1] == "C")
                    {
                        return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + "Profile/" + Convert.ToString(Session["CompanyName"]));
                    }
                }
                return null;

                // return RedirectToAction("HomePage", "Home");

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public string logout()
        {
            Session["UserObj"] = null;
            Session["UserID"] = null;
            Session["FocusAreaName"] = null;
            Session["CompanyName"] = null;
            return ConfigurationManager.AppSettings["WebBaseURL"].ToString();
        }

        public ActionResult LinkedINcall()
        {
            //Need to install below library
            //Install-Package Restsharp
            return Redirect("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=" + ConfigurationManager.AppSettings["LinkedInClientID"].ToString() + "&redirect_uri=" + _baseURL + "Login/LinkedINAuth&state=lkjlkxcxcx66&scope=r_basicprofile");
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
                request.AddParameter("redirect_uri", _baseURL + "Login/LinkedINAuth");
                request.AddParameter("client_id", ConfigurationManager.AppSettings["LinkedInClientID"].ToString());
                request.AddParameter("client_secret", ConfigurationManager.AppSettings["LinkedInCLientSecret"].ToString());
                var response = client.Execute<InventoryItem>(request);
                var content = response.Content;
                string access_token = response.Data.access_token;
                client = new RestClient("https://api.linkedin.com/v1/people/~:(id,first-name,last-name,certifications,date-of-birth,email-address,picture-url,summary,public-profile-url,positions,skills,location)?oauth2_access_token=" + access_token + "&format=json");
                request = new RestRequest(Method.GET);

                var response1 = client.Execute(request);
                TwitterLinkedInLoginModel obj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<TwitterLinkedInLoginModel>(response1.Content.ToString());
                obj.userType = 2; //LinkedIn
                UserEntity userObj = new Business.UserService().AddOrUpdateUser(obj);
                Session["UserObj"] = userObj;
                Session["UserID"] = userObj.UserID;
                string message = string.Empty;
                if(myArray[0] != "0")
                {
                    message = new Business.CompanyService().VoteForCompany(Convert.ToInt32(myArray[0]) , userObj.UserID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (myArray[1] == "H")
            {
                return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString());
            }
            else if (myArray[1] == "L")
            {
                return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + Convert.ToString(Session["FocusAreaName"]));
            }
            else if (myArray[1] == "C")
            {
                return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + "Profile/"+ Convert.ToString(Session["CompanyName"]));
            }

            return null;
        }

        public class InventoryItem { public string access_token { get; set; } }

        public ActionResult TwitterAuth()
        {
            string key = ConfigurationManager.AppSettings["TwitterKey"].ToString();
            string secret = ConfigurationManager.AppSettings["TwitterSecret"].ToString();

            TwitterService service = new TwitterService(key, secret);
            OAuthRequestToken requestToken = service.GetRequestToken(_baseURL+"Login/TwitterCallback?companyid=12");

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
            return Json("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=" + ConfigurationManager.AppSettings["LinkedInClientID"].ToString() + "&redirect_uri=" + _baseURL + "Login/LinkedINAuth&state=" + companyid + "-" + calledPage + "&scope=r_basicprofile");
        }

        [HttpPost]
        public ActionResult TwitterCall(int companyid, char calledPage)
        {
            string key = ConfigurationManager.AppSettings["TwitterKey"].ToString();
            string secret = ConfigurationManager.AppSettings["TwitterSecret"].ToString();

            TwitterService service = new TwitterService(key, secret);
            OAuthRequestToken requestToken = service.GetRequestToken(_baseURL + "Login/TwitterCallback?state=" + companyid+ "-" + calledPage);

            Uri uri = service.GetAuthenticationUrl(requestToken);
            //string script = "<html><head><script type='text/javascript'> var popupWindow =window.open('" + uri.ToString() + "','_blank','directories=no, status=no, menubar=no, scrollbars=yes, resizable=no,width=600, height=280,top=200,left=200');</script></head></html>";
            return Json(uri.ToString() + "&force_login=true");
        }

        public string VoteForCompany(int companyID)
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            if (userID != 0)
            {
                return new Business.CompanyService().VoteForCompany(companyID, userID);
            }else
            {
                return "0";
            }
        }
    }
}