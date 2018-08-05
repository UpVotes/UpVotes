﻿using Newtonsoft.Json;
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
        HttpClient _httpClient = null;

        internal CompanyViewModel GetUserCompanyies(int userID, string companyName)
        {
            using (_httpClient = new HttpClient())
            {
                companyName = companyName == string.Empty ? "0" : companyName;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetUserCompanies";
                string completeURL = WebAPIURL + apiMethod + '/' + userID + '/' + companyName;
                var response = _httpClient.GetStringAsync(completeURL).Result;
                CompanyViewModel companyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response);
                return companyViewModel;
            }
        }

        internal CompanyViewModel GetCompany(CompanyFilterEntity companyFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetCompany";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyFilter), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
                CompanyViewModel companyViewModel = new CompanyViewModel();
                if (response.IsSuccessStatusCode)
                {
                    companyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response.Content.ReadAsStringAsync().Result);
                    if (companyViewModel != null && companyViewModel.CompanyList.Count > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (CompanyEntity company in companyViewModel.CompanyList)
                        {
                            sb.Append(company.CompanyName + ",");
                        }

                        companyViewModel.CompanyFocusData = sb.ToString().TrimEnd(new char[] { ',' });

                        return companyViewModel;
                    }
                    else
                    {
                        foreach (CompanyEntity company in companyViewModel.CompanyList)
                        {
                            if (companyFilter.CompanyName != "0")
                            {
                                if (company.CompanyFocus != null && company.CompanyFocus.Count > 0)
                                {
                                    companyViewModel.PrimaryCompanyFocus = company.CompanyFocus;
                                }

                                if (company.IndustialCompanyFocus != null && company.IndustialCompanyFocus.Count > 0)
                                {
                                    companyViewModel.IndustialCompanyFocus = company.IndustialCompanyFocus;
                                }

                                if (company.CompanyClientFocus != null && company.CompanyClientFocus.Count > 0)
                                {
                                    companyViewModel.CompanyClientFocus = company.CompanyClientFocus;
                                }

                                if (company.SubfocusNames != null && company.SubfocusNames.Count > 0)
                                {
                                    companyViewModel.SubfocusNames = company.SubfocusNames;
                                    companyViewModel.CompanySubFocus = company.CompanySubFocus;
                                }
                            }
                        }

                        return companyViewModel;
                    }
                }
                else
                {
                    return companyViewModel;
                }
            }
        }

        private bool AddCompanyReview(CompanyReviewsEntity companyReviewsEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveCompanyReview";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyReviewsEntity), Encoding.UTF8, "application/json");

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

        internal CategoryMetaTagsDetails GetCategoryMetaTags(string FocusAreaName, string SubFocusAreaName)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetCategoryMetaTags";
                if (string.IsNullOrEmpty(SubFocusAreaName))
                    SubFocusAreaName = "0";
                string apiParameter = FocusAreaName + "/" + SubFocusAreaName;
                string completeURL = WebAPIURL + apiMethod + '/' + apiParameter;
                var response = _httpClient.GetStringAsync(completeURL).Result;
                CategoryMetaTagsDetails metaTagsTitle = JsonConvert.DeserializeObject<CategoryMetaTagsDetails>(response);
                return metaTagsTitle;
            }  
        }

        internal bool AddReview(CompanyReviewViewModel companyReviewModel)
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

        internal string VoteForCompany(int companyID, int userID)
        {
            using (_httpClient = new HttpClient())
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

        internal string ThanksNoteForReview(int companyID, int companyReviewID, int userID)
        {
            using (_httpClient = new HttpClient())
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

        internal List<string> GetDataForAutoComplete(int type, int focusAreaID, string searchTerm)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetDataForAutoComplete";
                string completeURL = WebAPIURL + apiMethod + '/' + type + '/' + focusAreaID + '/' + searchTerm;
                var response = _httpClient.GetStringAsync(completeURL).Result;
                List<string> myAutoCompleteList = JsonConvert.DeserializeObject<List<string>>(response);
                return myAutoCompleteList;
            }
        }

        internal CompanyViewModel GetUserReviews(string companyName)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetUserReviews";
                string completeURL = WebAPIURL + apiMethod + '/' + companyName;

                var response = _httpClient.GetStringAsync(completeURL).Result;
                CompanyViewModel companyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response);
                return companyViewModel;
            }
        }
        internal int SaveCompany(CompanyEntity companyEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveCompany";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyEntity), Encoding.UTF8, "application/json");

                var response = _httpClient.PostAsync(completeURL, httpContent).Result;
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

        internal List<CountryEntity> GetCountry()
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetCountry";
                string completeURL = WebAPIURL + apiMethod;

                var response = _httpClient.GetStringAsync(completeURL).Result;
                List<CountryEntity> countryList = JsonConvert.DeserializeObject<List<CountryEntity>>(response);
                return countryList;
            }
        }

        internal List<StateEntity> GetStates(int countryID)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetStates";
                string completeURL = WebAPIURL + apiMethod + '/' + countryID;

                var response = _httpClient.GetStringAsync(completeURL).Result;
                List<StateEntity> statesList = JsonConvert.DeserializeObject<List<StateEntity>>(response);
                return statesList;
            }
        }

        internal bool CompanyVerificationByUser(int uID, string cID, int compID)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "CompanyVerificationByUser";
                string completeURL = WebAPIURL + apiMethod + '/' + uID + '/' + cID + '/' + compID;

                var response = _httpClient.GetStringAsync(completeURL).Result;
                bool isUserVerifiedCompany = JsonConvert.DeserializeObject<bool>(response);
                return isUserVerifiedCompany;
            }
        }

        internal bool UpdateRejectionComments(CompanyRejectComments companyRejectComments)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "UpdateRejectionComments";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyRejectComments), Encoding.UTF8, "application/json");

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