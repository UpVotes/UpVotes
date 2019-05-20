using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;
using System;

namespace UpVotes.Business
{
    public class SoftwareService
    {
        HttpClient _httpClient = null;
        readonly string _webApiurl = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();

        internal SoftwareViewModel GetSoftware(SoftwareFilterEntity softwareFilter)
        {
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(softwareFilter), Encoding.UTF8, "application/json");
            using (_httpClient = new HttpClient())
            {                
                const string apiMethod = "GetSoftware";
                var completeUrl = _webApiurl + apiMethod + '/';

                var response = _httpClient.PostAsync(completeUrl, httpContent).Result;
                var softwareViewModel = JsonConvert.DeserializeObject<SoftwareViewModel>(response.Content.ReadAsStringAsync().Result);
                return softwareViewModel;
            }
        }
        internal SoftwareViewModel GetAllSoftwareReviews(SoftwareFilterEntity softwareReviewsFilter)
        {
            using (_httpClient = new HttpClient())
            {                
                string apiMethod = "GetUserReviewsForSoftware";
                string completeURL = _webApiurl + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(softwareReviewsFilter), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                SoftwareViewModel softwareReviewsViewModel = new Models.SoftwareViewModel();
                if (response.IsSuccessStatusCode)
                {
                    softwareReviewsViewModel = JsonConvert.DeserializeObject<SoftwareViewModel>(response.Content.ReadAsStringAsync().Result);
                    return softwareReviewsViewModel;
                }
                else
                {
                    return softwareReviewsViewModel;
                }                
                
            }
        }

        internal SoftwareViewModel GetUserSoftwares(int userId, bool isAdmin)
        {            
            using (_httpClient = new HttpClient())
            {                
                const string apiMethodName = "GetUserSoftwares";
                var completeUrl = _webApiurl + apiMethodName + '/' + userId + '/' + isAdmin;
                var response = _httpClient.GetStringAsync(completeUrl).Result;
                return JsonConvert.DeserializeObject<SoftwareViewModel>(response);                
            }
        }

        internal string ThanksNoteForReview(int softwareId, int softwareReviewId, int userId)
        {
            using (_httpClient = new HttpClient())
            {                
                const string apiMethod = "ThanksNoteForSoftwareReview";
                var completeUrl = _webApiurl + apiMethod + '/';

                SoftwareReviewThankNoteEntity softwareReviewThankNoteEntity = new SoftwareReviewThankNoteEntity
                {
                    SoftwareID = softwareId,
                    UserID = userId,
                    SoftwareReviewID = softwareReviewId,
                };


                var httpContent = new StringContent(JsonConvert.SerializeObject(softwareReviewThankNoteEntity), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeUrl, httpContent).Result;
                var message = response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : "Something error occured while giving thanks note. Please contact support.";

                return message;
            }
        }

        internal string VoteForSoftware(int softwareId, int userId)
        {
            var softwareVote = new SoftwareVoteEntity
            {
                SoftwareID = softwareId,
                UserID = userId
            };
            var httpContent = new StringContent(JsonConvert.SerializeObject(softwareVote), Encoding.UTF8, "application/json");
            using (_httpClient = new HttpClient())
            {
                const string apiMethod = "VoteForSoftware";
                var completeUrl = _webApiurl + apiMethod + '/';

                var response = _httpClient.PostAsync(completeUrl, httpContent).Result;
                var message = response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : "Something error occured while voting. Please contact support.";

                return message;
            }
        }

        internal string ClaimSoftwareListing(ClaimApproveRejectListingRequest claimlistingrequest)
        {
            const string apiMethod = "InsertVerifyClaimSoftwareListing";
            var completeUrl = _webApiurl + apiMethod + '/';
            var httpContent = new StringContent(JsonConvert.SerializeObject(claimlistingrequest), Encoding.UTF8, "application/json");
            using (_httpClient = new HttpClient())
            {
                HttpResponseMessage response;
                using (httpContent)
                {
                    response = _httpClient.PostAsync(completeUrl, httpContent).Result;
                }

                var message = response.IsSuccessStatusCode ? "Successfully Claimed" : "Something error occured while claiming. Please contact support.";
                return message;
            }
        }

        internal List<string> GetSoftwareForAutoComplete(int softwareCategory, string searchTerm)
        {
            using (_httpClient = new HttpClient())
            {                
                string apiMethod = "GetSoftwareForAutoComplete";
                string completeURL = _webApiurl + apiMethod + '/' + softwareCategory + '/' + searchTerm;
                var response = _httpClient.GetStringAsync(completeURL).Result;
                List<string> myAutoCompleteList = JsonConvert.DeserializeObject<List<string>>(response);
                return myAutoCompleteList;
            }
        }

        internal CategoryMetaTagsDetails GetSoftwareCategoryMetaTags(string urlSoftwareCategory)
        {
            using (_httpClient = new HttpClient())
            {                
                string apiMethod = "GetSoftwareCategoryMetaTags";                
                string apiParameter = urlSoftwareCategory;
                string completeURL = _webApiurl + apiMethod + '/' + apiParameter;
                var response = _httpClient.GetStringAsync(completeURL).Result;
                CategoryMetaTagsDetails metaTagsTitle = JsonConvert.DeserializeObject<CategoryMetaTagsDetails>(response);
                return metaTagsTitle;
            }
        }

        internal CompanySoftwareUserReviews GetUserReviewsForSoftwareListingPage(SoftwareFilterEntity userReviewObj)
        {
            using (_httpClient = new HttpClient())
            {                
                const string apiMethod = "GetSoftwareReviewsForListingPage";
                string completeUrl = _webApiurl + apiMethod;
                var httpContent = new StringContent(JsonConvert.SerializeObject(userReviewObj), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeUrl, httpContent).Result;
                var softwareReviews = new CompanySoftwareUserReviews();
                if (response.IsSuccessStatusCode)
                {
                    softwareReviews = JsonConvert.DeserializeObject<CompanySoftwareUserReviews>(response.Content.ReadAsStringAsync().Result);
                    return softwareReviews;
                }
                else
                {
                    return softwareReviews;
                }

            }
        }

        internal SoftwareViewModel GetUserSoftwareByName(string softwareName)
        {
            using (_httpClient = new HttpClient())
            {
                const string apiMethod = "GetUserSoftwareByName";
                var completeUrl = _webApiurl + apiMethod + '/' + softwareName;
                var response = _httpClient.GetStringAsync(completeUrl).Result;
                return JsonConvert.DeserializeObject<SoftwareViewModel>(response);
            }
        }

        public int SaveSoftwareDetails(SoftwareEntity software)
        {
            using (_httpClient = new HttpClient())
            {                
                const string apiMethod = "SaveSoftwareDetails";
                var completeUrl = _webApiurl + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(software), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeUrl, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool SoftwareVerificationByUser(int uId, string cId, int softId)
        {
            using (_httpClient = new HttpClient())
            {                
                const string apiMethod = "SoftwareVerificationByUser";
                string completeURL = _webApiurl + apiMethod + '/' + uId + '/' + cId + '/' + softId;

                var response = _httpClient.GetStringAsync(completeURL).Result;
                bool isUserVerifiedCompany = JsonConvert.DeserializeObject<bool>(response);
                return isUserVerifiedCompany;
            }
        }

        public bool UpdateSoftwareRejectionComments(SoftwareRejectComments softwareRejectComments)
        {
            using (_httpClient = new HttpClient())
            {                
                const string apiMethod = "UpdateSoftwareRejectionComments";
                string completeURL = _webApiurl + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(softwareRejectComments), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}