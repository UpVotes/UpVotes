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
    public class CompanyService
    {
        public CompanyViewModel GetCompany(string companyName)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetCompany"; string apiParameter = companyName;
                string completeURL = WebAPIURL + apiMethod + '/' + apiParameter;
                var response = httpClient.GetStringAsync(completeURL).Result;
                CompanyViewModel companyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response);

                foreach (CompanyEntity company in companyViewModel.CompanyList)
                {
                    if (companyName.Length > 0 && company.CompanyFocus.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var item in company.CompanyFocus)
                        {
                            sb.Append(item.FocusAreaName + ":");
                            sb.Append(item.FocusAreaPercentage.ToString() + ",");
                        }

                        companyViewModel.CompanyFocusData = sb.ToString().TrimEnd(new char[] { ',' });
                    }
                }

                return companyViewModel;
            }
        }
        public CompanyViewModel GetCompany(string companyName = "0", decimal minRate = 0, decimal maxRate = 0, int minEmployee = 0, int maxEmployee = 0, string sortby = "DESC", int focusAreaID = 0,string location="0", int userID = 0)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetCompany";
                if (string.IsNullOrEmpty(sortby))
                    sortby = "asc";
                string apiParameter = companyName + "/" + minRate + "/" + maxRate + "/" + minEmployee + "/" + maxEmployee + "/" + sortby + "/" + focusAreaID + "/" + location + "/"+ userID;
                string completeURL = WebAPIURL + apiMethod + '/' + apiParameter;
                var response = httpClient.GetStringAsync(completeURL).Result;
                CompanyViewModel companyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response);

                if (companyViewModel != null && companyViewModel.CompanyList.Count > 1)
                {
                    return companyViewModel;
                }
                else
                {
                    foreach (CompanyEntity company in companyViewModel.CompanyList)
                    {
                        if (companyName != "0" && company.CompanyFocus != null && company.CompanyFocus.Count > 0)
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var item in company.CompanyFocus)
                            {
                                sb.Append(item.FocusAreaName + ":");
                                sb.Append(item.FocusAreaPercentage.ToString() + ",");
                            }

                            companyViewModel.CompanyFocusData = sb.ToString().TrimEnd(new char[] { ',' });
                        }
                    }

                    return companyViewModel;
                }
            }
        }

        private bool AddCompanyReview(CompanyReviewsEntity companyReviewsEntity)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveCompanyReview";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyReviewsEntity), Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync(completeURL, httpContent).Result;
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

        public bool AddReview(CompanyReviewViewModel companyReviewModel)
        {
            CompanyReviewsEntity companyReviewEntity = new CompanyReviewsEntity();
            companyReviewEntity.CompanyID = companyReviewModel.CompanyID;
            companyReviewEntity.FocusAreaID = companyReviewModel.FocusAreaID;
            companyReviewEntity.UserID = companyReviewModel.UserID;
            companyReviewEntity.ReviewerCompanyName = companyReviewModel.ReviewerCompanyName;
            companyReviewEntity.ReviewerDesignation = companyReviewModel.Designation;
            companyReviewEntity.ReviewerProjectName = companyReviewModel.ProjectName;
            companyReviewEntity.FeedBack = companyReviewModel.FeedBack;
            companyReviewEntity.Rating = companyReviewModel.Rating;
            companyReviewEntity.Email = companyReviewModel.Email;
            companyReviewEntity.Phone = companyReviewModel.PhoneNumber;

            bool isSuccess = AddCompanyReview(companyReviewEntity);
            return isSuccess;
        }

        public string VoteForCompany(int companyID, int userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string message = string.Empty;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "VoteForCompany";
                string completeURL = WebAPIURL + apiMethod + '/';

                CompanyVoteEntity companyVote = new CompanyVoteEntity
                {
                    CompanyID = companyID,
                    UserID = userID
                };


                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyVote), Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync(completeURL, httpContent).Result;
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

        internal string ThanksNoteForReview(int companyID, int companyReviewID, int userID)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string message = string.Empty;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "ThanksNoteForReview";
                string completeURL = WebAPIURL + apiMethod + '/';

                CompanyReviewThankNoteEntity companyReviewThankNoteEntity = new CompanyReviewThankNoteEntity
                {
                    CompanyID = companyID,
                    UserID = userID,
                    CompanyReviewID = companyReviewID,
                };


                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyReviewThankNoteEntity), Encoding.UTF8, "application/json");

                var response = httpClient.PostAsync(completeURL, httpContent).Result;
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
    }
}