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