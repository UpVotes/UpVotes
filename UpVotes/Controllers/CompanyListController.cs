using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;
using UpVotes.Utility;

namespace UpVotes.Controllers
{
    public class CompanyListController : Controller
    {
        private string _webBaseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();

        // GET: CompanyList
        public ActionResult CompanyList(string id)
        {

            Session["calledPage"] = "L";
            string urlFocusAreaName = Convert.ToString(Request.Url.Segments[1]);
            int focusAreaID = 0;
            if (Request.Url.Segments.Length > 2)
            {
                urlFocusAreaName = urlFocusAreaName.Replace("/", "");
            }
            string actualFocusAreaName = string.Empty;
            Session["SubFocusAreaName"] ="";
            if (urlFocusAreaName != string.Empty)
            {
                Session["FocusAreaName"] = urlFocusAreaName.ToString();
                focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);
            }


            CompanyService companyService = new CompanyService();
            if (string.IsNullOrEmpty(id))
            {
                id = "0";
                Session["Country"] = "";
            }
            else
            {
                Session["Country"] =id;
                id = id.Replace("-", "space");
            }

            CompanyFilterEntity companyFilter = new CompanyFilterEntity
            {
                CompanyName = "0",
                MinRate = 0,
                MaxRate = 0,
                MinEmployee = 0,
                MaxEmployee = 0,
                SortBy = "DESC",
                FocusAreaID = focusAreaID,
                Location = id,
                SubFocusArea = "0",
                UserID = Convert.ToInt32(Session["UserID"]),
                PageNo = 1,
                PageSize = 25,
                OrderColumn= 1,
            };
            
                                    
            CompanyViewModel companyViewModel = companyService.GetCompany(companyFilter);
            companyViewModel.WebBaseURL = _webBaseURL;
            GetCategoryHeadLine(urlFocusAreaName, companyViewModel, id.Replace("space", " "),"0");
            companyViewModel.PageCount = 0;
            companyViewModel.PageNumber = 1;
            companyViewModel.PageIndex = 1;
            companyViewModel.SubFocusArea = "0";
            companyViewModel.Location = id;
            companyViewModel.TotalNoOfCompanies = companyViewModel.CompanyList.Select(a => a.TotalCount).FirstOrDefault();
            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.AverageUserRating = 4;
                companyViewModel.TotalNoOfUsers = 10;
                companyViewModel.PageCount = (companyViewModel.CompanyList[0].TotalCount + 25 - 1) / 25;
            }
            LoadJson(companyViewModel);
            Session["CompanyNames"] = companyViewModel.CompanyFocusData;

            return View(companyViewModel);
        }

        public ActionResult CompanySubCategoryList(string id)
        {

            Session["calledPage"] = "L";
            string urlFocusAreaName = Convert.ToString(Request.Url.Segments[1]);
            string urlSubFocusAreaName = Convert.ToString(Request.Url.Segments[2]);
            int focusAreaID = 0;
            if (Request.Url.Segments.Length > 2)
            {
                urlFocusAreaName = urlFocusAreaName.Replace("/", "");
                urlSubFocusAreaName = urlSubFocusAreaName.Replace("/", "");
                Session["FocusAreaName"] = urlFocusAreaName.ToString();
                Session["SubFocusAreaName"] = urlSubFocusAreaName.ToString();
                focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);
            }

            CompanyService companyService = new CompanyService();
            if (string.IsNullOrEmpty(id))
            {
                id = "0";
                Session["Country"] = "";
            }
            else
            {
                Session["Country"] = id;
                id = id.Replace("-", "space");
            }

            CompanyFilterEntity companyFilter = new CompanyFilterEntity
            {
                CompanyName = "0",
                MinRate = 0,
                MaxRate = 0,
                MinEmployee = 0,
                MaxEmployee = 0,
                SortBy = "DESC",
                FocusAreaID = focusAreaID,
                Location = id,
                SubFocusArea = urlSubFocusAreaName,
                UserID = Convert.ToInt32(Session["UserID"]),
                PageNo = 1,
                PageSize = 25,
                OrderColumn = 1
            };

            CompanyViewModel companyViewModel = companyService.GetCompany(companyFilter);            
            companyViewModel.WebBaseURL = _webBaseURL;
            GetCategoryHeadLine(urlFocusAreaName, companyViewModel, id.Replace("space", " "), urlSubFocusAreaName);
            companyViewModel.SubFocusArea = urlSubFocusAreaName;
            companyViewModel.Location = id;
            companyViewModel.PageCount = 0;
            companyViewModel.PageNumber = 1;
            companyViewModel.PageIndex = 1;
            companyViewModel.TotalNoOfCompanies = companyViewModel.CompanyList.Select(a => a.TotalCount).FirstOrDefault();
            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.AverageUserRating = 4;
                companyViewModel.TotalNoOfUsers = 10;
                companyViewModel.PageCount = (companyViewModel.CompanyList[0].TotalCount + 25 - 1) / 25;
            }

            Session["CompanyNames"] = companyViewModel.CompanyFocusData;

            return View("~/Views/CompanyList/CompanyList.cshtml", companyViewModel);
        }

        [HttpPost]
        public ActionResult CompanyList(string companyID, decimal minRate, decimal maxRate, int minEmployee, int maxEmployee, string sortby, string location,string subFocusArea, int PageNo, int PageSize, int FirstPage, int LastPage, int OrderColumn)
        {
            CompanyService companyService = new CompanyService();
            string urlFocusAreaName = Convert.ToString(Session["FocusAreaName"]);
            if (location != "0")
            {
                location = location.Replace("-", "space");
            }
            int focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);

            CompanyFilterEntity companyFilter = new CompanyFilterEntity
            {
                CompanyName = companyID,
                MinRate = minRate,
                MaxRate = maxRate,
                MinEmployee = minEmployee,
                MaxEmployee = maxEmployee,
                SortBy = sortby,
                FocusAreaID = focusAreaID,
                Location = location,
                SubFocusArea = subFocusArea,
                UserID = Convert.ToInt32(Session["UserID"]),
                PageNo = PageNo,
                PageSize = PageSize,
                OrderColumn = OrderColumn
            };

            CompanyViewModel companyViewModel = companyService.GetCompany(companyFilter);
            companyViewModel.WebBaseURL = _webBaseURL;
            companyViewModel.PageCount = 0;
            companyViewModel.PageNumber = PageNo;
            companyViewModel.PageIndex = 1;
            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.PageCount = (companyViewModel.CompanyList[0].TotalCount + PageSize - 1) / PageSize;
            }
            if ((PageNo == FirstPage || PageNo == LastPage) && LastPage >= 5)
            {
                if (PageNo == FirstPage && PageNo != 1)
                {
                    companyViewModel.PageIndex = FirstPage - 1;
                }
                else if (PageNo == LastPage)
                {
                    if (PageNo == companyViewModel.PageCount)
                        companyViewModel.PageIndex = (PageNo - 5) + 1;
                    else
                        companyViewModel.PageIndex = FirstPage + 1;
                }
            }
            else if (PageNo > LastPage && LastPage >= 5)
            {
                companyViewModel.PageIndex = (PageNo - 5) + 1;
            }
            else if(PageNo >= FirstPage && PageNo <= LastPage)
            {
                companyViewModel.PageIndex = FirstPage;
            }

            Session["CompanyNames"] = companyViewModel.CompanyFocusData;
            return PartialView("_CompList", companyViewModel);
        }

        private void GetCategoryHeadLine(string urlFocusAreaName, CompanyViewModel companyViewModel, string Country, string urlSubFocusAreaName)
        {
            Country = Country == "0" ? "globe" : Country;
            Country = Country.ToUpper() == "UNITED STATES" ? "USA" : Country;
            int year = DateTime.Now.Year;

            string headLine = Country == "globe" ? "Top " : "Top 10+ ";

            string cachename = urlFocusAreaName.Trim() + urlSubFocusAreaName.Trim();
            CategoryMetaTagsDetails metaTagObj;
            if (CacheHandler.Exists(cachename))
            {
                metaTagObj = new CategoryMetaTagsDetails();
                CacheHandler.Get(cachename, out metaTagObj);
            }
            else
            {
                metaTagObj = new CategoryMetaTagsDetails();
                metaTagObj = new CompanyService().GetCategoryMetaTags(urlFocusAreaName.Trim(), urlSubFocusAreaName.Trim());
                CacheHandler.Add(metaTagObj, cachename);
            }
            companyViewModel.CategoryHeadLine = metaTagObj.Title.ToUpper() + Country.ToUpper();
            companyViewModel.CategoryReviewHeadLine = metaTagObj.Title.Replace(" in ", " ").ToUpper();
            companyViewModel.Title = headLine + metaTagObj.Title + Country + "- " + year + " | upvotes.co";
            companyViewModel.MetaTag = CategoryMetaTags(metaTagObj, Country);
        }

        private string CategoryMetaTags(CategoryMetaTagsDetails metaTag, string Country)
        {
            StringBuilder MetaStr = new StringBuilder();
            int year = DateTime.Now.Year;
            MetaStr.Append("<meta property='og:url' content='{WebsiteUrl}' />");
            MetaStr.Append("<meta property='og:type' content='website' />");
            if (string.IsNullOrEmpty(metaTag.SubFocusAreaName))
            {
                MetaStr.Append("<meta property='og:title' content='" + metaTag.TwitterTitle + year + "' />");
                MetaStr.Append("<meta name='twitter:title' content='" + metaTag.TwitterTitle + year + "' />");
            }
            else
            {
                MetaStr.Append("<meta property='og:title' content='" + metaTag.TwitterTitle + "' />");
                MetaStr.Append("<meta name='twitter:title' content='" + metaTag.TwitterTitle + "' />");
            }

            MetaStr.Append("<meta property='og:image' content='"+ metaTag.OgImageURL+ "' />");
            MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
            MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
            MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
            MetaStr.Append("<meta name='twitter:image' content='' />");
            MetaStr.Append("<link rel='canonical' href='{WebsiteUrl}' />");
            MetaStr.Append("<link rel='publisher' href='#' />");
            MetaStr.Append("<meta property='og:description' content='" + metaTag.Descriptions + "' />");
            MetaStr.Append("<meta name='description' content='" + metaTag.Descriptions + "' />");
            MetaStr.Append("<meta name='twitter:description' content='" + metaTag.Descriptions + "' />");
            return MetaStr.Replace("{Year}", year.ToString()).Replace("{Country}", Country).Replace("{WebsiteUrl}", Request.Url.ToString()).ToString();
        }

        public JsonResult GetDataForAutoComplete(int type)
        {
            try
            {
                var jsonResult = default(dynamic);
                string searchTerm = Convert.ToString(Request.Params["starts_with"]);
                int focusAreaID = new FocusAreaService().GetFocusAreaID(Convert.ToString(Session["FocusAreaName"]));
                List<string> myAutoCompleteList = new CompanyService().GetDataForAutoComplete(type, focusAreaID, searchTerm);
                jsonResult = from a in myAutoCompleteList
                             select new
                             {
                                 Name = a
                             };
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string ThanksNoteForReview(int companyID, int companyReviewID, string compname)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {                
                if (CacheHandler.Exists(compname))
                {
                    CacheHandler.Clear(compname);
                }
                string message = new CompanyService().ThanksNoteForReview(companyID, companyReviewID, Convert.ToInt32(Session["UserID"]));
                return message;
            }
            else
            {
                return "Please login to provide thanks note.";
            }
        }

        public ActionResult GetUserReviews()
        {
            int focusAreaID = 0;
            if (Session["FocusAreaName"] != null)
            { 
                focusAreaID = new FocusAreaService().GetFocusAreaID(Session["FocusAreaName"].ToString());
            }
            CompanyFilterEntity UserReviewObj = new CompanyFilterEntity
            {
                FocusAreaID = focusAreaID,
                CompanyName = "",
                PageNo = 1,
                PageSize = 25
            };

            CompanySoftwareUserReviews companyReViewModel = new CompanySoftwareUserReviews();
            companyReViewModel = new CompanyService().GetUserReviewsForCompanyListingPage(UserReviewObj);

            return PartialView("~/Views/CompanyList/_UsersReviewsList.cshtml", companyReViewModel);
               
        }

        [HttpPost]
        public ActionResult GetUserReviews(string companyname, int PageSize)
        {
            int focusAreaID = 0;
            if (Session["FocusAreaName"] != null)
            {
                focusAreaID = new FocusAreaService().GetFocusAreaID(Session["FocusAreaName"].ToString());
            }
            CompanyFilterEntity UserReviewObj = new CompanyFilterEntity
            {
                FocusAreaID = focusAreaID,
                CompanyName = companyname,
                PageNo = 1,
                PageSize = PageSize
            };

            CompanySoftwareUserReviews companyReViewModel = new CompanySoftwareUserReviews();
            companyReViewModel = new CompanyService().GetUserReviewsForCompanyListingPage(UserReviewObj);

            return PartialView("~/Views/CompanyList/_UsersReviewsList.cshtml", companyReViewModel);

        }

        //public ActionResult GetCompanyNames()
        //{
        //    return PartialView("_UsersReviewsList", Convert.ToString(Session["CompanyNames"]));
        //}

        public ActionResult GetOverViewPage()
        {
            string focusAreaname = Session["FocusAreaName"].ToString();
            string url = string.Empty;
            string country = string.Empty;
            if (Request.Url.Segments.Length > 2)
            {
                if (!string.IsNullOrEmpty(Session["Country"].ToString()) && (focusAreaname == "mobile-application-developers" || focusAreaname == "web-design-companies" || focusAreaname == "seo-companies"))
                {
                    country = Session["Country"].ToString().ToLower();
                }
                else
                {
                    focusAreaname = "NotFound";
                }
                
            }

            if(Session["SubFocusAreaName"] != null && Session["SubFocusAreaName"].ToString() != "")
            {
                focusAreaname = "NotFound";
            }
            
            switch (focusAreaname)
            {
                case "mobile-application-developers":
                    url = "~/Views/Overview/MobileAppOverview/_mobileAppOverview.cshtml";
                    if (country != "")
                    {
                        switch(country)
                        {
                            case "austin":
                                url = "~/Views/Overview/MobileAppOverview/_AustinmobilepageOverview.cshtml";
                                break;
                            case "boston":
                                url = "~/Views/Overview/MobileAppOverview/_BostonmobilepageOverview.cshtml";
                                break;
                            case "chicago":
                                url = "~/Views/Overview/MobileAppOverview/_ChicagomobilepageOverview.cshtml";
                                break;
                            case "florida":
                                url = "~/Views/Overview/MobileAppOverview/_FloridamobilepageOverview.cshtml";
                                break;
                            case "illinois":
                                url = "~/Views/Overview/MobileAppOverview/_IllinoismobilepageOverview.cshtml";
                                break;
                            case "india":
                                url = "~/Views/Overview/MobileAppOverview/_IndiamobilepageOverview.cshtml";
                                break;
                            case "los-angeles":
                                url = "~/Views/Overview/MobileAppOverview/_LosAngelesmobilepageOverview.cshtml";
                                break;
                            case "new-york":
                                url = "~/Views/Overview/MobileAppOverview/_NewYorkmobilepageOverview.cshtml";
                                break;
                            case "seattle":
                                url = "~/Views/Overview/MobileAppOverview/_SeattlemobilepageOverview.cshtml";
                                break;
                            case "washington":
                                url = "~/Views/Overview/MobileAppOverview/_WashingtonmobilepageOverview.cshtml";
                                break;
                            case "bangalore":
                                url = "~/Views/Overview/MobileAppOverview/_BangaloremobilepageOverview.cshtml";
                                break;
                            case "ahmedabad":
                                url = "~/Views/Overview/MobileAppOverview/_AhmedabadmobilepageOverview.cshtml";
                                break;
                            case "california":
                                url = "~/Views/Overview/MobileAppOverview/_CaliforniamobilepageOverview.cshtml";
                                break;
                            case "massachusetts":
                                url = "~/Views/Overview/MobileAppOverview/_MassachusettsmobilepageOverview.cshtml";
                                break;
                            case "noida":
                                url = "~/Views/Overview/MobileAppOverview/_NoidamobilepageOverview.cshtml";
                                break;
                            case "texas":
                                url = "~/Views/Overview/MobileAppOverview/_TexasmobilepageOverview.cshtml";
                                break;
                            default:
                                url = "~/Views/UnderConstruction/_underConstruction.cshtml";
                                break;
                        }
                    }                    
                    break;
                case "seo-companies":
                    url = "~/Views/Overview/SEOOverview/_seoOverview.cshtml";
                    if (country != "")
                    {
                        switch (country)
                        {
                            case "boston":
                                url = "~/Views/Overview/SEOOverview/_BostonseopageOverview.cshtml";
                                break;
                            case "california":
                                url = "~/Views/Overview/SEOOverview/_CaliforniaseopageOverview.cshtml";
                                break;
                            case "chicago":
                                url = "~/Views/Overview/SEOOverview/_ChicagoseopageOverview.cshtml";
                                break;
                            case "florida":
                                url = "~/Views/Overview/SEOOverview/_FloridaseopageOverview.cshtml";
                                break;
                            case "illinois":
                                url = "~/Views/Overview/SEOOverview/_IllinoisseopageOverview.cshtml";
                                break;
                            case "jacksonville":
                                url = "~/Views/Overview/SEOOverview/_JacksonvilleseopageOverview.cshtml";
                                break;
                            case "los-angeles":
                                url = "~/Views/Overview/SEOOverview/_LosAngelesseopageOverview.cshtml";
                                break;
                            case "new-york":
                                url = "~/Views/Overview/SEOOverview/_NewYorkseopageOverview.cshtml";
                                break;
                            case "san-diego":
                                url = "~/Views/Overview/SEOOverview/_SanDiegoseopageOverview.cshtml";
                                break;
                            case "washington":
                                url = "~/Views/Overview/SEOOverview/_WashingtonseopageOverview.cshtml";
                                break;
                            default:
                                url = "~/Views/UnderConstruction/_underConstruction.cshtml";
                                break;
                        }
                    }

                    break;
                case "digital-marketing-companies":
                    url = "~/Views/Overview/_digitalMarketingOverview.cshtml";
                    break;
                case "web-design-companies":
                    url = "~/Views/Overview/WebDesignOverview/_webDesignOverview.cshtml";
                    if (country != "")
                    {
                        switch (country)
                        {
                            case "atlanta":
                                url = "~/Views/Overview/WebDesignOverview/_AtlantawebdesignpageOverview.cshtml";
                                break;
                            case "boston":
                                url = "~/Views/Overview/WebDesignOverview/_BostonwebdesignpageOverview.cshtml";
                                break;
                            case "california":
                                url = "~/Views/Overview/WebDesignOverview/_CaliforniawebdesignpageOverview.cshtml";
                                break;
                            case "chicago":
                                url = "~/Views/Overview/WebDesignOverview/_ChicagowebdesignpageOverview.cshtml";
                                break;
                            case "florida":
                                url = "~/Views/Overview/WebDesignOverview/_FloridawebdesignpageOverview.cshtml";
                                break;
                            case "georgia":
                                url = "~/Views/Overview/WebDesignOverview/_GeorgiawebdesignpageOverview.cshtml";
                                break;
                            case "illinois":
                                url = "~/Views/Overview/WebDesignOverview/_IllinoiswebdesignpageOverview.cshtml";
                                break;
                            case "india":
                                url = "~/Views/Overview/WebDesignOverview/_IndiawebdesignpageOverview.cshtml";
                                break;
                            case "los-angeles":
                                url = "~/Views/Overview/WebDesignOverview/_LosangeleswebdesignpageOverview.cshtml";
                                break;
                            case "massachusetts":
                                url = "~/Views/Overview/WebDesignOverview/_MassachusettswebdesignpageOverview.cshtml";
                                break;
                            case "minneapolis":
                                url = "~/Views/Overview/WebDesignOverview/_MinneapoliswebdesignpageOverview.cshtml";
                                break;
                            case "minnesota":
                                url = "~/Views/Overview/WebDesignOverview/_MinnesotawebdesignpageOverview.cshtml";
                                break;
                            case "new-york":
                                url = "~/Views/Overview/WebDesignOverview/_NewyorkwebdesignpageOverview.cshtml";
                                break;
                            case "san-diego":
                                url = "~/Views/Overview/WebDesignOverview/_SanDiegowebdesignpageOverview.cshtml";
                                break;
                            case "seattle":
                                url = "~/Views/Overview/WebDesignOverview/_SeattlewebdesignpageOverview.cshtml";
                                break;
                            case "texas":
                                url = "~/Views/Overview/WebDesignOverview/_texaswebdesignpageOverview.cshtml";
                                break;
                            case "washington":
                                url = "~/Views/Overview/WebDesignOverview/_WashingtonwebdesignpageOverview.cshtml";
                                break;
                            default:
                                url = "~/Views/UnderConstruction/_underConstruction.cshtml";
                                break;
                        }
                    }
                    break;
                case "software-development-companies":
                    url = "~/Views/Overview/_softwareDevelopmentOverview.cshtml";
                    break;
                case "web-development-companies":
                    url = "~/Views/Overview/_webDevelopmentOverview.cshtml";
                    break;
                case "ui-ux-agencies":
                    url = "~/Views/Overview/_uiUxOverview.cshtml";
                    break;
                case "wearable-application-developers":
                    url = "~/Views/Overview/_wearableApplicationOverview.cshtml";
                    break;
                case "ecommerce-developers":
                    url = "~/Views/Overview/_ecommerceOverview.cshtml";
                    break;
                case "social-media-marketing-companies":
                    url = "~/Views/Overview/_socialMediaMarketingOverview.cshtml";
                    break;
                case "ppc-companies":
                    url = "~/Views/Overview/_ppcOverview.cshtml";
                    break;
                case "content-marketing-companies":
                    url = "~/Views/Overview/_contentMarketingOverview.cshtml";
                    break;
                case "iot-application-developers":
                    url = "~/Views/Overview/_iotApplicationOverview.cshtml";
                    break;
                default:
                    url = "~/Views/UnderConstruction/_underConstruction.cshtml";
                    break;
            }
            return PartialView(url);
        }

        public void LoadJson(CompanyViewModel cvm)
        {
            string url = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            url= url+ @"\Json\FAQ_Slp.json";            
            using (StreamReader r = new StreamReader(url))
            {
                cvm.CompanylistFAQ = r.ReadToEnd();
                //JavaScriptSerializer jss = new JavaScriptSerializer();

                //faq = jss.Deserialize<FAQ>(json);
                //Dictionary<string, object> json_Dictionary = (new JavaScriptSerializer()).Deserialize<Dictionary<string, object>>(json);
                //foreach (var item in json_Dictionary)
                //{
                //    var a=item.Value;
                //}
                //var a = json_Dictionary.Select(i => i.Key == "MobileApp_FAQ_Json").ToList();
            }
        }
    }
}