using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.Models;

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
            if (urlFocusAreaName != string.Empty)
            {
                Session["FocusAreaName"] = urlFocusAreaName.ToString();
                focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);
            }


            CompanyService companyService = new CompanyService();
            if (string.IsNullOrEmpty(id))
            {
                id = "0";
            }
            else
            {
                id = id.Replace("-", "space");
            }
            CompanyViewModel companyViewModel = companyService.GetCompany("0", 0, 0, 0, 0, "ASC", focusAreaID, id, Convert.ToInt32(Session["UserID"]));
            companyViewModel.WebBaseURL = _webBaseURL;
            GetCategoryHeadLine(urlFocusAreaName, companyViewModel, id.Replace("space", " "));
            companyViewModel.PageCount = 0;
            companyViewModel.PageNumber = 1;
            companyViewModel.PageIndex = 1;
            companyViewModel.TotalNoOfCompanies = companyViewModel.CompanyList.Select(a => a.TotalCount).FirstOrDefault();
            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.AverageUserRating = 4;
                companyViewModel.TotalNoOfUsers = 10;
                companyViewModel.PageCount = (companyViewModel.CompanyList[0].TotalCount + 10 - 1) / 10;
            }

            Session["CompanyNames"] = companyViewModel.CompanyFocusData;

            return View(companyViewModel);
        }

        [HttpPost]
        public ActionResult CompanyList(string companyID, decimal minRate, decimal maxRate, int minEmployee, int maxEmployee, string sortby, string location, int PageNo, int PageSize, int FirstPage, int LastPage)
        {
            CompanyService companyService = new CompanyService();
            string urlFocusAreaName = Convert.ToString(Session["FocusAreaName"]);
            if (location != "0")
            {
                location = location.Replace("-", "space");
            }
            int focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);
            CompanyViewModel companyViewModel = companyService.GetCompany(companyID, minRate, maxRate, minEmployee, maxEmployee, sortby, focusAreaID, location, Convert.ToInt32(Session["UserID"]), PageNo, PageSize);
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
            Session["CompanyNames"] = companyViewModel.CompanyFocusData;
            return PartialView("_CompList", companyViewModel);
        }

        public PartialViewResult CompanyAdd()
        {
            return PartialView("_CompanyAddEdit");
        }

        private void GetCategoryHeadLine(string urlFocusAreaName, CompanyViewModel companyViewModel, string Country)
        {
            Country = Country == "0" ? "globe" : Country;
            Country = Country.ToUpper() == "UNITED STATES" ? "USA" : Country;
            int year = DateTime.Now.Year;

            string headLine = Country == "globe" ? "Top " : "Top 10+ ";

            switch (urlFocusAreaName.Trim())
            {
                case "mobile-application-developers":
                    companyViewModel.CategoryHeadLine = "Mobile App Development Companies in " + Country;
                    companyViewModel.Title = headLine + "Mobile App Development Companies in " + Country + "- " + year + " | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("mobile-application-developers", Country);
                    break;

                case "seo-companies":
                    companyViewModel.CategoryHeadLine = "SEO Companies in " + Country;
                    companyViewModel.Title = headLine + "SEO Companies and Services in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("seo-companies", Country);
                    break;

                case "digital-marketing-companies":
                    companyViewModel.CategoryHeadLine = "Digital Marketing Companies and Agencies in " + Country;
                    companyViewModel.Title = headLine + "Digital Marketing Companies & Agencies in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("digital-marketing-companies", Country);
                    break;

                case "web-design-companies":
                    companyViewModel.CategoryHeadLine = "Web Design Companies in " + Country;
                    companyViewModel.Title = headLine + "Web Design Companies & Agencies in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("web-design-companies", Country);
                    break;

                case "software-development-companies":
                    companyViewModel.CategoryHeadLine = "Custom Software Development Companies in " + Country;
                    companyViewModel.Title = headLine + "Software Development Companies & Agencies in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("software-development-companies", Country);
                    break;

                case "web-development-companies":
                    companyViewModel.CategoryHeadLine = "Web Development Companies in " + Country;
                    companyViewModel.Title = headLine + "Web Development Companies & Agencies in " + Country + "- " + year + " | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("web-development-companies", Country);
                    break;
                case "ui-ux-agencies":
                    companyViewModel.CategoryHeadLine = "UI/UX Design Companies in " + Country;
                    companyViewModel.Title = headLine + "UI/UX Design Companies & Agencies in " + Country + "- " + year + " | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("ui-ux-agencies", Country);
                    break;
                default:
                    companyViewModel.CategoryHeadLine = "Mobile App Development Companies in " + Country;
                    companyViewModel.Title = headLine + "Mobile App Development Companies in " + Country + "- " + year + " | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("mobile-application-developers", Country);
                    break;
            }
        }

        private string CategoryMetaTags(string category, string Country)
        {
            StringBuilder MetaStr = new StringBuilder();
            string CategoryName = string.Empty;
            int year = DateTime.Now.Year;
            MetaStr.Append("<meta property='og:url' content='{WebsiteUrl}' />");
            MetaStr.Append("<meta property='og:type' content='website' />");
            MetaStr.Append("<meta property='og:title' content='Top 10 {Category} Companies in {Country} - " + year + "' />");
            MetaStr.Append("<meta property='og:image' content='' />");
            MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
            MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
            MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
            //MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} companies in {Country} "+ year +" (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
            MetaStr.Append("<meta name='twitter:title' content='Top 10 {Category} Companies in {Country} - " + year + "' />");
            MetaStr.Append("<meta name='twitter:image' content='' />");
            MetaStr.Append("<link rel='canonical' href='{WebsiteUrl}' />");
            MetaStr.Append("<link rel='publisher' href='' />");

            switch (category)
            {
                case "mobile-application-developers":
                    CategoryName = "mobile app development companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} companies in {Country} " + year + " (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} companies in {Country} " + year + " (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} companies in {Country} " + year + " (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
                    break;
                case "seo-companies":
                    CategoryName = "SEO companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} in {Country} " + year + " with user votes. Select best SEO agencies from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} in {Country} " + year + " with user votes. Select best SEO agencies from the {Country}.' />");
                    MetaStr.Append("<meta name = 'twitter:description' content = 'Here is a top 10 {Category} in {Country} " + year + " with user votes. Select best SEO agencies from the {Country}.' />");
                    break;
                case "digital-marketing-companies":
                    CategoryName = "digital marketing companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} " + year + " with user votes. Select best marketing firms from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} " + year + " with user votes. Select best marketing firms from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} " + year + " with user votes. Select best marketing firms from the {Country}.' />");
                    break;
                case "web-design-companies":
                    CategoryName = "web design companies";
                    MetaStr.Append("<meta property='og:description' content='Find out top 10 {Category} in {Country} " + year + " with user votes. Select best web designers from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Find out top 10 {Category} in {Country} " + year + " with user votes. Select best web designers from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Find out top 10 {Category} in {Country} " + year + " with user votes. Select best web designers from the {Country}.' />");
                    break;
                case "software-development-companies":
                    CategoryName = "software development companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    break;
                case "web-development-companies":
                    CategoryName = "web development Companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    break;
                case "ui-ux-agencies":
                    CategoryName = "UI/UX Design Companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} and Agencies " + year + " with user votes. Find best {Category} from the {Country}.' />");
                    break;
            }
            return MetaStr.Replace("{Category}", CategoryName).Replace("{Country}", Country).Replace("{WebsiteUrl}", Request.Url.ToString()).ToString();
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

        public ActionResult GetUserReviews()
        {
            string companyNames = Convert.ToString(Request.Params["companyName"]);
            if (companyNames != string.Empty)
            {
                CompanyViewModel companyViewModel = new CompanyService().GetUserReviews(companyNames);
                if (companyViewModel != null && companyViewModel.CompanyList[0].CompanyReviews.Count > 0)
                {
                    return PartialView("~/Views/Company/_CompanyReviews.cshtml", companyViewModel.CompanyList[0].CompanyReviews);
                }
                else
                {
                    return Json("No reviews found.", JsonRequestBehavior.AllowGet);
                }
            }

            return PartialView("~/Views/Company/_CompanyReviews.cshtml", new List<UpVotes.BusinessEntities.Entities.CompanyReviewsEntity>());
        }

        public ActionResult GetCompanyNames()
        {
            return PartialView("_UsersReviewsList", Convert.ToString(Session["CompanyNames"]));
        }

        public ActionResult GetOverViewPage()
        {
            string focusAreaname = Session["FocusAreaName"].ToString();
            string url = "";
            switch (focusAreaname)
            {
                case "mobile-application-developers":
                    url = "~/Views/Overview/_mobileAppOverview.cshtml";
                    break;
                case "seo-companies":
                    url = "~/Views/Overview/_seoOverview.cshtml";
                    break;
                case "digital-marketing-companies":
                    url = "~/Views/Overview/_digitalMarketingOverview.cshtml";
                    break;
                case "web-design-companies":
                    url = "~/Views/Overview/_webDesignOverview.cshtml";
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
            }
            return PartialView(url);
        }
    }
}