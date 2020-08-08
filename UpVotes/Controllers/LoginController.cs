using RestSharp;
using System;
using System.Configuration;
using System.Web.Mvc;
using TweetSharp;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Helper;
using UpVotes.Models;
using UpVotes.Utility;

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

        public ActionResult TwitterCallback(string oauth_token, string oauth_verifier, string state)
        {
            var requesttoken = new OAuthRequestToken { Token = oauth_token };
            string key = ConfigurationManager.AppSettings["TwitterKey"].ToString();
            string secret = ConfigurationManager.AppSettings["TwitterSecret"].ToString();
            string[] myArray = state.Split('-');
            try
            {
                if (oauth_token != null)
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
                            if (myArray[1] == "C")
                            {
                                message = new Business.CompanyService().VoteForCompany(Convert.ToInt32(myArray[0]), userObj.UserID);
                                if (CacheHandler.Exists("TopVoteCompaniesList"))
                                {
                                    CacheHandler.Clear("TopVoteCompaniesList");
                                }
                                string compname = "";
                                if (!string.IsNullOrEmpty(Session["CompanyName"].ToString()))
                                {
                                    compname = Session["CompanyName"].ToString();
                                }
                                if (CacheHandler.Exists(compname))
                                {
                                    CacheHandler.Clear(compname);
                                }
                            }
                            else if (myArray[1] == "N")
                            {
                                message = new Business.SoftwareService().VoteForSoftware(Convert.ToInt32(myArray[0]), userObj.UserID);
                                string softwarename = "";
                                if (!string.IsNullOrEmpty(Session["SoftwareName"].ToString()))
                                {
                                    softwarename = Session["SoftwareName"].ToString();
                                }
                                if (CacheHandler.Exists(softwarename))
                                {
                                    CacheHandler.Clear(softwarename);
                                }
                            }

                        }
                    }
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
                else if (myArray[1] == "U")
                {
                    return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + "company/my-dashboard");
                }
                else if (myArray[1] == "S")
                {
                    return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + Convert.ToString(Session["SoftwareCategory"]));
                }
                else if (myArray[1] == "N")
                {
                    return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + "Software/" + Convert.ToString(Session["SoftwareName"]));
                }

                return null;

                // return RedirectToAction("HomePage", "Home");

            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
                throw;
            }
        }
        public string logout()
        {
            Session["UserObj"] = null;
            Session["UserID"] = null;
            Session["FocusAreaName"] = null;
            Session["CompanyName"] = null;
            Session["SoftwareName"] = null;
            Session["SoftwareCategory"] = null;
            Session["UserDashboardInfo"] = null;
            return ConfigurationManager.AppSettings["WebBaseURL"].ToString();
        }

        public ActionResult GetFooterSection()
        {
            return PartialView("~/Views/Shared/_FooterPartialView.cshtml");
        }

        public ActionResult LinkedINcall()
        {
            //Need to install below library
            //Install-Package Restsharp
            return Redirect("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=" + ConfigurationManager.AppSettings["LinkedInClientID"].ToString() + "&redirect_uri=" + _baseURL + "Login/LinkedINAuth&state=lkjlkxcxcx66&scope=r_basicprofile");
        }

        public ActionResult LoginRegisteredUser(int companyid, string workemail, string password, char calledPage)
        {
            try
            {
                RegisteredUser registeredUserObj = new RegisteredUser();
                registeredUserObj.WorkEmailID = workemail;
                registeredUserObj.Password = password;
                UserEntity userObj = new Business.UserService().LoginRegisteredUser(registeredUserObj);
                if (userObj != null && userObj.UserID > 0)
                {
                    Session["UserObj"] = userObj;
                    Session["UserID"] = userObj.UserID;
                    if (calledPage == 'U')
                    {
                        return Json(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + "company/my-dashboard");
                    }
                    else
                    {
                        return Json("success");
                    }
                }
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
            }

            return null;
        }

        public string ForgotPassword(string workemail)
        {
            string message = "";
            try
            {
                RegisteredUser ForgetObj = new RegisteredUser();
                ForgetObj.WorkEmailID = workemail;
                UserEntity userObj = new Business.UserService().ForgotPassword(ForgetObj);
                if (userObj != null && userObj.UserID > 0)
                {
                    message = "Your new password has been emailed to you.";
                }
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
            }

            return message;
        }
        public string ChangePassword(string CurrentPassword, string NewPassword)
        {
            string message = "";
            try
            {
                ChangePassword ChangeObj = new ChangePassword();
                ChangeObj.Password = CurrentPassword;
                ChangeObj.NewPassword = NewPassword;
                ChangeObj.UserID = Convert.ToInt32(Session["UserID"]);
                UserEntity userObj = new Business.UserService().ChangePassword(ChangeObj);
                if (userObj != null && userObj.UserID > 0)
                {
                    message = "Success! Your Password has been changed!";
                }
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
            }

            return message;
        }
        public ActionResult LinkedINAuth(string code, string state)
        {
            try
            {
                //This method path is your return URL
                string[] myArray = state.Split('-');
                if (code != null)
                {
                    //try
                    //{
                    var a = Request.Url;
                    var client = new RestClient("http://www.linkedin.com/oauth/v2/accessToken");
                    var request = new RestRequest(Method.POST);
                    request.AddParameter("grant_type", "authorization_code");
                    request.AddParameter("code", code);
                    request.AddParameter("redirect_uri", _baseURL + "Login/LinkedINAuth");
                    request.AddParameter("client_id", ConfigurationManager.AppSettings["LinkedInClientID"].ToString());
                    request.AddParameter("client_secret", ConfigurationManager.AppSettings["LinkedInCLientSecret"].ToString());
                    request.AddParameter("scope", "all");
                    var response = client.Execute<InventoryItem>(request);
                    var content = response.Content;
                    string access_token = response.Data.access_token;
                    //client = new RestClient("https://api.linkedin.com/v1/people/~:(id,first-name,last-name,certifications,date-of-birth,email-address,picture-url,summary,public-profile-url,positions,skills,location)?oauth2_access_token=" + access_token + "&format=json");
                    client = new RestClient("https://api.linkedin.com/v2/me?oauth2_access_token=" + access_token); //for user info
                    request = new RestRequest(Method.GET);
                    var response1 = client.Execute(request);
                    LinkedInInfo linkedInUserobj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<LinkedInInfo>(response1.Content.ToString());

                    //client = new RestClient("https://api.linkedin.com/v2/me?projection=(id,profilePicture(displayImage~:playableStreams))&oauth2_access_token=" + access_token);--for profileimage
                    //client = new RestClient("https://api.linkedin.com/v2/me?fields=id,firstName,lastName,educations,skills,positions&oauth2_access_token=" + access_token);
                    client = new RestClient("https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))&oauth2_access_token=" + access_token);

                    request = new RestRequest(Method.GET);

                    response1 = client.Execute(request);
                    LinkedInImageInfo linkedInImgObj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<LinkedInImageInfo>(response1.Content.Replace("~", "tilde").ToString());
                    string imageName = "";
                    if (linkedInImgObj.profilePicture != null && linkedInImgObj.profilePicture.displayImagetilde != null)
                    {
                        foreach (var item in linkedInImgObj.profilePicture.displayImagetilde.elements)
                        {
                            if (imageName == "")
                            {
                                foreach (var img in item.identifiers)
                                {
                                    if (img.identifier.Contains("_100_100"))
                                    {
                                        imageName = img.identifier;
                                        break;
                                    }

                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                    }

                    //TwitterLinkedInLoginModel obj = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<TwitterLinkedInLoginModel>(response1.Content.ToString());
                    TwitterLinkedInLoginModel obj = new TwitterLinkedInLoginModel();
                    obj.firstName = linkedInUserobj.localizedFirstName;
                    obj.id = linkedInUserobj.id;
                    obj.lastName = linkedInUserobj.localizedLastName;
                    obj.pictureUrl = imageName;
                    obj.publicProfileUrl = "";
                    obj.userType = 2; //LinkedIn
                    UserEntity userObj = new Business.UserService().AddOrUpdateUser(obj);
                    if (userObj != null)
                    {
                        Session["UserObj"] = userObj;
                        Session["UserID"] = userObj.UserID;
                        string message = string.Empty;
                        if (myArray[0] != "0")
                        {
                            if (myArray[1] == "C")
                            {
                                message = new Business.CompanyService().VoteForCompany(Convert.ToInt32(myArray[0]), userObj.UserID);
                                if (CacheHandler.Exists("TopVoteCompaniesList"))
                                {
                                    CacheHandler.Clear("TopVoteCompaniesList");
                                }
                                string compname = "";
                                if (!string.IsNullOrEmpty(Session["CompanyName"].ToString()))
                                {
                                    compname = Session["CompanyName"].ToString();
                                }
                                if (CacheHandler.Exists(compname))
                                {
                                    CacheHandler.Clear(compname);
                                }
                            }
                            else if (myArray[1] == "N")
                            {
                                message = new Business.SoftwareService().VoteForSoftware(Convert.ToInt32(myArray[0]), userObj.UserID);
                                string softwarename = "";
                                if (!string.IsNullOrEmpty(Session["SoftwareName"].ToString()))
                                {
                                    softwarename = Session["SoftwareName"].ToString();
                                }
                                if (CacheHandler.Exists(softwarename))
                                {
                                    CacheHandler.Clear(softwarename);
                                }
                            }
                        }
                    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
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
                else if (myArray[1] == "U")
                {
                    return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + "company/my-dashboard");
                }
                else if (myArray[1] == "S")
                {
                    return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + Convert.ToString(Session["SoftwareCategory"]));
                }
                else if (myArray[1] == "N")
                {
                    return Redirect(ConfigurationManager.AppSettings["WebBaseURL"].ToString() + "Software/" + Convert.ToString(Session["SoftwareName"]));
                }
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
                throw ex;
            }

            return null;
        }

        public class InventoryItem { public string access_token { get; set; } }

        public ActionResult TwitterAuth()
        {
            try
            {
                string key = ConfigurationManager.AppSettings["TwitterKey"].ToString();
                string secret = ConfigurationManager.AppSettings["TwitterSecret"].ToString();

                TwitterService service = new TwitterService(key, secret);
                OAuthRequestToken requestToken = service.GetRequestToken(_baseURL + "Login/TwitterCallback?companyid=12");

                Uri uri = service.GetAuthenticationUrl(requestToken);
                //string script = "<html><head><script type='text/javascript'> var popupWindow =window.open('" + uri.ToString() + "','_blank','directories=no, status=no, menubar=no, scrollbars=yes, resizable=no,width=600, height=280,top=200,left=200');</script></head></html>";
                return Redirect(uri.ToString());
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
                throw ex;
            }
            //return Content(script);
        }

        [HttpPost]
        public ActionResult LinkedINcall(int companyid, char calledPage)
        {
            try
            {
                //Need to install below library
                //Install-Package Restsharp
                return Json("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=" + ConfigurationManager.AppSettings["LinkedInClientID"].ToString() + "&redirect_uri=" + _baseURL + "Login/LinkedINAuth&state=" + companyid + "-" + calledPage + "&scope=r_liteprofile");
                //return Json("https://www.linkedin.com/oauth/v2/authorization?response_type=code&client_id=" + ConfigurationManager.AppSettings["LinkedInClientID"].ToString() + "&redirect_uri=" + _baseURL + "Login/LinkedINAuth&state=" + companyid + "-" + calledPage + "&scope=r_basicprofile");
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult TwitterCall(int companyid, char calledPage)
        {
            try
            {
                string key = ConfigurationManager.AppSettings["TwitterKey"].ToString();
                string secret = ConfigurationManager.AppSettings["TwitterSecret"].ToString();

                TwitterService service = new TwitterService(key, secret);
                OAuthRequestToken requestToken = service.GetRequestToken(_baseURL + "Login/TwitterCallback?state=" + companyid + "-" + calledPage);

                Uri uri = service.GetAuthenticationUrl(requestToken);
                //string script = "<html><head><script type='text/javascript'> var popupWindow =window.open('" + uri.ToString() + "','_blank','directories=no, status=no, menubar=no, scrollbars=yes, resizable=no,width=600, height=280,top=200,left=200');</script></head></html>";
                return Json(uri.ToString() + "&force_login=true");
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
                throw ex;
            }
        }

        public string VoteForCompany(int companyID)
        {
            try
            {

                int userID = Convert.ToInt32(Session["UserID"]);
                if (userID != 0)
                {
                    if (CacheHandler.Exists("TopVoteCompaniesList"))
                    {
                        CacheHandler.Clear("TopVoteCompaniesList");
                    }
                    return new Business.CompanyService().VoteForCompany(companyID, userID);
                }
                else
                {
                    return "0";
                }
            }
            catch(Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
                throw ex;
            }
        }

        public string VoteForSoftware(int softwareID)
        {
            try
            {
                int userID = Convert.ToInt32(Session["UserID"]);
                if (userID != 0)
                {
                    return new Business.SoftwareService().VoteForSoftware(softwareID, userID);
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                EmailHelper.SendErrorEmail(ex);
                throw ex;
            }
        }
    }
}