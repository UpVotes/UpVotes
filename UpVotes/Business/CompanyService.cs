using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;

namespace UpVotes.Business
{
    public class CompanyService
    {
        private HttpClient _httpClient = null;

        internal CompanyViewModel GetUserCompanyies(int userID, string companyName)
        {
            using (_httpClient = new HttpClient())
            {
                companyName = companyName == string.Empty ? "0" : companyName;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetUserCompanies";
                string completeURL = WebAPIURL + apiMethod + '/' + userID + '/' + companyName;
                string response = _httpClient.GetStringAsync(completeURL).Result;
                CompanyViewModel companyViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response);
                return companyViewModel;
            }
        }

        internal CompanyViewModel GetClaimListingsForApproval(int userID)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetClaimListingsForApproval";
                string completeURL = WebAPIURL + apiMethod + '/' + userID;
                string response = _httpClient.GetStringAsync(completeURL).Result;
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

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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

                                if(company.CompanyCompititors != null && company.CompanyCompititors.Count > 0)
                                {
                                    companyViewModel.CompanyCompititors = company.CompanyCompititors;
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

        internal CategoryMetaTagsDetails GetCategoryMetaTags(string FocusAreaName, string SubFocusAreaName)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetCategoryMetaTags";
                if (string.IsNullOrEmpty(SubFocusAreaName))
                {
                    SubFocusAreaName = "0";
                }

                string apiParameter = FocusAreaName + "/" + SubFocusAreaName;
                string completeURL = WebAPIURL + apiMethod + '/' + apiParameter;
                string response = _httpClient.GetStringAsync(completeURL).Result;
                CategoryMetaTagsDetails metaTagsTitle = JsonConvert.DeserializeObject<CategoryMetaTagsDetails>(response);
                return metaTagsTitle;
            }
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

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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
                string response = _httpClient.GetStringAsync(completeURL).Result;
                List<string> myAutoCompleteList = JsonConvert.DeserializeObject<List<string>>(response);
                return myAutoCompleteList;
            }
        }

        internal CompanyViewModel GetCompanyPortfolio(CompanyFilterEntity companyReviewsFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetAllCompanyPortfolio";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyReviewsFilter), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                CompanyViewModel companyReviewsViewModel = new Models.CompanyViewModel();
                if (response.IsSuccessStatusCode)
                {
                    companyReviewsViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response.Content.ReadAsStringAsync().Result);
                    return companyReviewsViewModel;
                }
                else
                {
                    return companyReviewsViewModel;
                }

            }
        }

        internal int DeletePortFolio(int portfolioID)
        {
            using (_httpClient = new HttpClient())
            {
                CompanyPortFolioEntity filter = new CompanyPortFolioEntity
                {
                    CompanyPortFolioID = portfolioID
                };
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "DeleteCompanyPortfolio";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(filter), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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

        internal List<CompanyPortFolioEntity> GetCompanyPortfolioByID(CompanyFilterEntity companyPortfolioFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetAllCompanyPortfolioByID";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyPortfolioFilter), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                List<CompanyPortFolioEntity> companyPortFolioObj = new List<CompanyPortFolioEntity>();
                if (response.IsSuccessStatusCode)
                {
                    companyPortFolioObj = JsonConvert.DeserializeObject<List<CompanyPortFolioEntity>>(response.Content.ReadAsStringAsync().Result);
                    return companyPortFolioObj;
                }
                else
                {
                    return companyPortFolioObj;
                }

            }
        }

        internal CompanyPortFolioEntity GetPortfolioInfoByID(CompanyPortFolioEntity PortFolioFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetPortfolioInfoByID";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(PortFolioFilter), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                CompanyPortFolioEntity PortFolioObj = new CompanyPortFolioEntity();
                if (response.IsSuccessStatusCode)
                {
                    PortFolioObj = JsonConvert.DeserializeObject<CompanyPortFolioEntity>(response.Content.ReadAsStringAsync().Result);
                    return PortFolioObj;
                }
                else
                {
                    return PortFolioObj;
                }
            }
        }

        internal CompanyViewModel GetUserReviews(CompanyFilterEntity companyReviewsFilter)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetUserReviews";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(companyReviewsFilter), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                CompanyViewModel companyReviewsViewModel = new Models.CompanyViewModel();
                if (response.IsSuccessStatusCode)
                {
                    companyReviewsViewModel = JsonConvert.DeserializeObject<CompanyViewModel>(response.Content.ReadAsStringAsync().Result);
                    return companyReviewsViewModel;
                }
                else
                {
                    return companyReviewsViewModel;
                }

            }
        }
        internal CompanySoftwareUserReviews GetUserReviewsForCompanyListingPage(CompanyFilterEntity UserReviewObj)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetCompanyReviewsForListingPage";
                string completeURL = WebAPIURL + apiMethod;
                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(UserReviewObj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                CompanySoftwareUserReviews companyReviews = new CompanySoftwareUserReviews();
                if (response.IsSuccessStatusCode)
                {
                    companyReviews = JsonConvert.DeserializeObject<CompanySoftwareUserReviews>(response.Content.ReadAsStringAsync().Result);
                    return companyReviews;
                }
                else
                {
                    return companyReviews;
                }

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

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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

        internal int SavePortFolio(CompanyPortFolioEntity portfolioEntity)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "SaveUpdateCompanyPortFolio";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(portfolioEntity), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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

                string response = _httpClient.GetStringAsync(completeURL).Result;
                List<CountryEntity> countryList = JsonConvert.DeserializeObject<List<CountryEntity>>(response);
                return countryList;
            }
        }

        internal List<CompanyEntity> GetTopVoteCompanies()
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetTopVoteCompanies";
                string completeURL = WebAPIURL + apiMethod;

                string response = _httpClient.GetStringAsync(completeURL).Result;
                List<CompanyEntity> TopVotedCompanyList = JsonConvert.DeserializeObject<List<CompanyEntity>>(response);
                return TopVotedCompanyList;
            }
        }

        internal List<CategoryLinksEntity> GetServiceCategoryLinks(int focusAreaID)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetServiceCategoryLinks";
                string completeURL = WebAPIURL + apiMethod + "/" + focusAreaID;

                string response = _httpClient.GetStringAsync(completeURL).Result;
                List<CategoryLinksEntity> LinksObj = JsonConvert.DeserializeObject<List<CategoryLinksEntity>>(response);
                return LinksObj;
            }
        }

        internal List<StateEntity> GetStates(int countryID)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetStates";
                string completeURL = WebAPIURL + apiMethod + '/' + countryID;

                string response = _httpClient.GetStringAsync(completeURL).Result;
                List<StateEntity> statesList = JsonConvert.DeserializeObject<List<StateEntity>>(response);
                return statesList;
            }
        }

        internal List<SubFocusAreaEntity> GetSubFocusAreaByFocusID(int focusAreaID)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "GetSubFocusAreaByFocusID";
                string completeURL = WebAPIURL + apiMethod + '/' + focusAreaID;

                string response = _httpClient.GetStringAsync(completeURL).Result;
                List<SubFocusAreaEntity> subfocusList = JsonConvert.DeserializeObject<List<SubFocusAreaEntity>>(response);
                return subfocusList;
            }
        }

        internal bool CompanyVerificationByUser(int uID, string cID, int compID)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "CompanyVerificationByUser";
                string completeURL = WebAPIURL + apiMethod + '/' + uID + '/' + cID + '/' + compID;

                string response = _httpClient.GetStringAsync(completeURL).Result;
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

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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

        internal string ClaimListing(ClaimApproveRejectListingRequest claimlistingrequest)
        {

            using (_httpClient = new HttpClient())
            {
                string message = string.Empty;
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "InsertVerifyClaimListing";
                string completeURL = WebAPIURL + apiMethod + '/';

                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(claimlistingrequest), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
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
        public string AdminApproveRejectForClaim(int userID, int claimlistingID, int companyID, bool isAdminApproved, string Rejectioncomment, string Email, string CompanyName, string Type)
        {
            string message = "";
            ClaimApproveRejectListingRequest claimRequestobj = new ClaimApproveRejectListingRequest
            {
                ClaimListingID = claimlistingID,
                userID = userID,
                companyID = companyID,
                IsAdminApproved = isAdminApproved,
                RejectionComment = Rejectioncomment,
                Email = Email,
                CompanyName = CompanyName,
                Type = Type
            };

            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "AdminApproveRejectForClaim";
                string completeURL = WebAPIURL + apiMethod + '/';


                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(claimRequestobj), Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync(completeURL, httpContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    message = response.Content.ReadAsStringAsync().Result;
                    if (message.Contains("claimed"))
                    {
                        message = "claimed";
                    }
                    else if (message.Contains("Rejected"))
                    {
                        message = "Rejected";
                    }
                    else
                    {
                        message = "error";
                    }
                }
                else
                {
                    message = "error";
                }
            }
            return message;
        }

        internal string DeleteCompany(int companyId)
        {
            using (_httpClient = new HttpClient())
            {
                string WebAPIURL = System.Configuration.ConfigurationManager.AppSettings["WebAPIURL"].ToString();
                string apiMethod = "DeleteCompany";
                string completeURL = WebAPIURL + apiMethod + '/' + companyId;
                return _httpClient.GetStringAsync(completeURL).Result;
            }
        }
    }
}