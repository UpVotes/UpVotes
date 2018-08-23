using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;

namespace UpVotes.Business
{
    internal class UserService
    {
        private string _baseURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
        private HttpClient _httpClient = null;
        private string _apiMethod = string.Empty;
        private string _completeURL = string.Empty;

        internal UserEntity AddOrUpdateUser(TwitterLinkedInLoginModel linkedInObj = null)
        {
            UserEntity userObj = null;
            if (linkedInObj != null)
            {
                userObj = LinkedInUser(linkedInObj);
                using (_httpClient = new HttpClient())
                {
                    _apiMethod = "AddOrUpdateUser";
                    _completeURL = _baseURL + _apiMethod + "/";
                    StringContent httpContent = new StringContent(JsonConvert.SerializeObject(userObj), Encoding.UTF8, "application/json");

                    var response = _httpClient.PostAsync(_completeURL, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        userObj = new UserEntity();
                        userObj = JsonConvert.DeserializeObject<UserEntity>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        userObj = null;
                    }
                }
            }
            //else
            //{
            //    userObj = TwitterUser(twitterObj);
            //}

            return userObj;
        }
        internal UserEntity LoginRegisteredUser(RegisteredUser RegisteredUserObj = null)
        {
            UserEntity userObj = new UserEntity();
            if (RegisteredUserObj != null)
            {
                userObj.UserName = RegisteredUserObj.WorkEmailID;
                userObj.UserPassword = RegisteredUserObj.Password;

                using (_httpClient = new HttpClient())
                {
                    _apiMethod = "LoginRegisteredUser";
                    _completeURL = _baseURL + _apiMethod + "/";
                    StringContent httpContent = new StringContent(JsonConvert.SerializeObject(userObj), Encoding.UTF8, "application/json");
                    var response = _httpClient.PostAsync(_completeURL, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        userObj = new UserEntity();
                        userObj = JsonConvert.DeserializeObject<UserEntity>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        userObj = null;
                    }
                }
            }
            
            return userObj;
        }
        internal UserEntity ForgotPassword(RegisteredUser RegisteredUserObj = null)
        {
            UserEntity userObj = new UserEntity();
            if (RegisteredUserObj != null)
            {
                userObj.UserName = RegisteredUserObj.WorkEmailID;
                
                using (_httpClient = new HttpClient())
                {
                    _apiMethod = "ForgotPassword";
                    _completeURL = _baseURL + _apiMethod + "/";
                    StringContent httpContent = new StringContent(JsonConvert.SerializeObject(userObj), Encoding.UTF8, "application/json");
                    var response = _httpClient.PostAsync(_completeURL, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        userObj = new UserEntity();
                        userObj = JsonConvert.DeserializeObject<UserEntity>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        userObj = null;
                    }
                }
            }

            return userObj;
        }

        internal UserEntity ChangePassword(ChangePassword ChangePwdObj = null)
        {
            UserEntity userObj = new UserEntity();
            if (ChangePwdObj != null && ChangePwdObj.UserID > 0)
            {                
                using (_httpClient = new HttpClient())
                {
                    _apiMethod = "ChangePassword";
                    _completeURL = _baseURL + _apiMethod + "/";
                    StringContent httpContent = new StringContent(JsonConvert.SerializeObject(ChangePwdObj), Encoding.UTF8, "application/json");
                    var response = _httpClient.PostAsync(_completeURL, httpContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        userObj = new UserEntity();
                        userObj = JsonConvert.DeserializeObject<UserEntity>(response.Content.ReadAsStringAsync().Result);
                    }
                    else
                    {
                        userObj = null;
                    }
                }
            }

            return userObj;
        }

        private UserEntity TwitterUser(TwitterLoginModel twitterObj)
        {
            throw new NotImplementedException();
        }

        private UserEntity LinkedInUser(TwitterLinkedInLoginModel linkedInObj)
        {
            UserEntity userObj = new UserEntity();
            userObj.FirstName = linkedInObj.firstName;
            userObj.LastName = linkedInObj.lastName;
            userObj.ProfileID = linkedInObj.id;
            userObj.ProfilePictureURL = linkedInObj.pictureUrl;
            userObj.ProfileURL = linkedInObj.publicProfileUrl;
            userObj.UserType = linkedInObj.userType;
            return userObj;
        }
    }
}