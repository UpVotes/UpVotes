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
    public class SubmitReviewService
    {
        HttpClient _httpClient = null;

        private bool AddCompanyReview(CompanyReviewsEntity companyReviewsEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveCompanyReview";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyReviewsEntity), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;

                if (response != null)
                {
                    var isAdded = response.Content.ReadAsStringAsync().Result;
                    return Convert.ToBoolean(isAdded);
                }
                else
                {
                    return false;
                }                
            }
        }

        private bool AddSoftwareReview(SoftwareReviewsEntity softwareReviewsEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveSoftwareReview";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(softwareReviewsEntity), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;

                if (response != null)
                {
                    var isAdded = response.Content.ReadAsStringAsync().Result;
                    return Convert.ToBoolean(isAdded);
                }
                else
                {
                    return false;
                }
            }
        }

        internal AutocompleteResponseEntity GetDataForAutoComplete(AutocompleteRequestEntity AutocompleteObj)
        {
            using (_httpClient = new HttpClient())
            {
                AutocompleteResponseEntity AutocompleteResponseEntityObj = new AutocompleteResponseEntity();
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSoftwareCompanyAutoComplete";
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(AutocompleteObj), Encoding.UTF8, "application/json");
                string completeURL = WebAPIURL + apiMethod + '/';
                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    AutocompleteResponseEntityObj = JsonConvert.DeserializeObject<AutocompleteResponseEntity>(response.Content.ReadAsStringAsync().Result);
                }
                return AutocompleteResponseEntityObj;
            }
        }

        internal bool AddCompanyReview(CompanySoftwareReviewViewModel companyReviewModel)
        {
            CompanyReviewsEntity companyReviewEntity = new CompanyReviewsEntity();
            companyReviewEntity.CompanyID = companyReviewModel.CompanyID;
            companyReviewEntity.CompanyName = companyReviewModel.CompanyName;
            companyReviewEntity.FocusAreaID = companyReviewModel.FocusAreaID;
            companyReviewEntity.UserID = companyReviewModel.UserID;
            companyReviewEntity.ReviewerCompanyName = companyReviewModel.ReviewerCompanyName;
            companyReviewEntity.ReviewerDesignation = companyReviewModel.Designation;
            companyReviewEntity.ReviewerProjectName = companyReviewModel.ProjectName;
            companyReviewEntity.FeedBack = companyReviewModel.FeedBack;
            companyReviewEntity.Rating = companyReviewModel.Rating;
            companyReviewEntity.Email = companyReviewModel.Email;
            companyReviewEntity.Phone = companyReviewModel.PhoneNumber;
            companyReviewEntity.ReviewerFullName = companyReviewModel.UserName;

            bool isSuccess = AddCompanyReview(companyReviewEntity);
            return isSuccess;
        }

        internal bool AddSoftwareReview(CompanySoftwareReviewViewModel softwareReviewModel)
        {
            SoftwareReviewsEntity softwareReviewEntity = new SoftwareReviewsEntity();
            softwareReviewEntity.SoftwareName = softwareReviewModel.SoftwareName;
            softwareReviewEntity.SoftwareID = softwareReviewModel.SoftwareID;
            softwareReviewEntity.ServiceCategoryID = softwareReviewModel.ServiceCategoryID;
            softwareReviewEntity.UserID = softwareReviewModel.UserID;
            softwareReviewEntity.ReviewerCompanyName = softwareReviewModel.ReviewerCompanyName;
            softwareReviewEntity.ReviewerDesignation = softwareReviewModel.Designation;
            softwareReviewEntity.ReviewerProjectName = "";
            softwareReviewEntity.FeedBack = softwareReviewModel.FeedBack;
            softwareReviewEntity.Rating = softwareReviewModel.Rating;
            softwareReviewEntity.Email = softwareReviewModel.Email;
            softwareReviewEntity.Phone = softwareReviewModel.PhoneNumber;
            softwareReviewEntity.ReviewerFullName = softwareReviewModel.UserName;

            bool isSuccess = AddSoftwareReview(softwareReviewEntity);
            return isSuccess;
        }

        internal List<UserReviewsResponseEntity> GetUserReviewsListForApproval(UserReviewRequestEntity ReviewRequest)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetUserReviewsListForApproval";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(ReviewRequest), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                List<UserReviewsResponseEntity> reviewResponseObj = new List<UserReviewsResponseEntity>();
                if (response.IsSuccessStatusCode)
                {
                    reviewResponseObj = JsonConvert.DeserializeObject<List<UserReviewsResponseEntity>>(response.Content.ReadAsStringAsync().Result);
                    return reviewResponseObj;
                }
                else
                {
                    return reviewResponseObj;
                }

            }
        }

        internal UserReviewsResponseEntity ViewReviewByID(UserReviewRequestEntity ReviewRequest)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetUserReviewByReviewID";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(ReviewRequest), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                UserReviewsResponseEntity reviewResponseObj = new UserReviewsResponseEntity();
                if (response.IsSuccessStatusCode)
                {
                    reviewResponseObj = JsonConvert.DeserializeObject<UserReviewsResponseEntity>(response.Content.ReadAsStringAsync().Result);
                    return reviewResponseObj;
                }
                else
                {
                    return reviewResponseObj;
                }

            }
        }

        internal bool ApproveRejectUserReview(UserReviewRequestEntity ReviewRequest)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "ApproveRejectUserReview";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(ReviewRequest), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                bool isApproveReject = false;
                if (response.IsSuccessStatusCode)
                {
                    isApproveReject = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                    return isApproveReject;
                }
                else
                {
                    return isApproveReject;
                }

            }
        }
    }
}