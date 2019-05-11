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

        internal SoftwareViewModel GetSoftware(SoftwareFilterEntity softwareFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSoftware";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(softwareFilter), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                SoftwareViewModel softwareViewModel = new SoftwareViewModel();
                softwareViewModel = JsonConvert.DeserializeObject<SoftwareViewModel>(response.Content.ReadAsStringAsync().Result);
                return softwareViewModel;
            }
        }
        internal SoftwareViewModel GetAllSoftwareReviews(SoftwareFilterEntity softwareReviewsFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetUserReviewsForSoftware";
                string completeURL = WebAPIURL + apiMethod;
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

        internal string ThanksNoteForReview(int softwareID, int softwareReviewID, int userID)
        {
            using (_httpClient = new HttpClient())
            {
                string message = string.Empty;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "ThanksNoteForSoftwareReview";
                string completeURL = WebAPIURL + apiMethod + '/';

                SoftwareReviewThankNoteEntity softwareReviewThankNoteEntity = new SoftwareReviewThankNoteEntity
                {
                    SoftwareID = softwareID,
                    UserID = userID,
                    SoftwareReviewID = softwareReviewID,
                };


                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(softwareReviewThankNoteEntity), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    message = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    message = "Something error occured while giving thanks note. Please contact support.";
                }

                return message;
            }
        }

        internal string VoteForSoftware(int softwareID, int userID)
        {
            using (_httpClient = new HttpClient())
            {
                string message = string.Empty;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "VoteForSoftware";
                string completeURL = WebAPIURL + apiMethod + '/';

                SoftwareVoteEntity softwareVote = new SoftwareVoteEntity
                {
                    SoftwareID = softwareID,
                    UserID = userID
                };
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(softwareVote), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    message = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    message = "Something error occured while voting. Please contact support.";
                }

                return message;
            }
        }

        internal string ClaimSoftwareListing(ClaimApproveRejectListingRequest claimlistingrequest)
        {

            using (_httpClient = new HttpClient())
            {
                string message = string.Empty;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "InsertVerifyClaimSoftwareListing";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(claimlistingrequest), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    message = "Successfully Claimed";
                }
                else
                {
                    message = "Something error occured while claiming. Please contact support.";
                }
                return message;
            }
        }

        internal List<string> GetSoftwareForAutoComplete(int SoftwareCategory, string searchTerm)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSoftwareForAutoComplete";
                string completeURL = WebAPIURL + apiMethod + '/' + SoftwareCategory + '/' + searchTerm;
                var response = _httpClient.GetStringAsync(completeURL).Result;
                List<string> myAutoCompleteList = JsonConvert.DeserializeObject<List<string>>(response);
                return myAutoCompleteList;
            }
        }

        internal CategoryMetaTagsDetails GetSoftwareCategoryMetaTags(string urlSoftwareCategory)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSoftwareCategoryMetaTags";                
                string apiParameter = urlSoftwareCategory;
                string completeURL = WebAPIURL + apiMethod + '/' + apiParameter;
                var response = _httpClient.GetStringAsync(completeURL).Result;
                CategoryMetaTagsDetails metaTagsTitle = JsonConvert.DeserializeObject<CategoryMetaTagsDetails>(response);
                return metaTagsTitle;
            }
        }

        internal CompanySoftwareUserReviews GetUserReviewsForSoftwareListingPage(SoftwareFilterEntity UserReviewObj)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSoftwareReviewsForListingPage";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(UserReviewObj), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                CompanySoftwareUserReviews softwareReviews = new CompanySoftwareUserReviews();
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
    }
}