﻿using System;
using System.Configuration;
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
            if(Request.Url.Segments.Length >2)
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
            if(string.IsNullOrEmpty(id))
            {
                id = "0";
            }
            else
            {
                id = id.Replace("-", "space");
            }          
            CompanyViewModel companyViewModel = companyService.GetCompany("0", 0, 0, 0, 0, "ASC", focusAreaID,id, Convert.ToInt32(Session["UserID"]));
            companyViewModel.WebBaseURL = _webBaseURL;
            GetCategoryHeadLine(urlFocusAreaName, companyViewModel, id.Replace("space", " "));
            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.AverageUserRating = 4;
                companyViewModel.TotalNoOfUsers = 10;
            }

            return View(companyViewModel);
        }

        [HttpPost]
        public ActionResult CompanyList(string companyID, decimal minRate, decimal maxRate, int minEmployee, int maxEmployee, string sortby,string location)
        {
            CompanyService companyService = new CompanyService();
            string urlFocusAreaName = Convert.ToString(Request.Url.Segments[1]);            
            if(location != "0")
            {
                location = location.Replace("-", "space");
            }
            int focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);
            CompanyViewModel companyViewModel = companyService.GetCompany("0", minRate, maxRate, minEmployee, maxEmployee, sortby, focusAreaID, location, Convert.ToInt32(Session["UserID"]));
            companyViewModel.WebBaseURL = _webBaseURL;

            return PartialView("_CompList", companyViewModel);
        }

        public PartialViewResult CompanyAdd()
        {
            return PartialView("_CompanyAddEdit");
        }

        private void GetCategoryHeadLine(string urlFocusAreaName, CompanyViewModel companyViewModel,string Country)
        {
            Country = Country == "0" ? "globe" : Country;
            Country = Country.ToUpper() == "UNITED STATES" ? "USA" : Country;
            int year = DateTime.Now.Year;
            switch (urlFocusAreaName.Trim())
            {
                case "mobile-application-developers":
                    companyViewModel.CategoryHeadLine = "Mobile App Development Companies in "+ (Country == "globe"?"" : Country);
                    companyViewModel.Title = "Top Mobile App Development Companies in "+ Country + "- "+ year + " | upvotes.co";
                    companyViewModel.MetaTag=CategoryMetaTags("mobile-application-developers", Country);
                    break;

                case "seo-companies":
                    companyViewModel.CategoryHeadLine = "SEO Companies in " + (Country == "globe" ? "" : Country);
                    companyViewModel.Title = " Top SEO Companies and Services in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("seo-companies", Country);
                    break;

                case "digital-marketing-companies":
                    companyViewModel.CategoryHeadLine = "Digital Marketing Companies and Agencies in " + (Country == "globe" ? "" : Country);
                    companyViewModel.Title = " Top Digital Marketing Companies & Agencies in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("digital-marketing-companies", Country);
                    break;

                case "web-design-companies":
                    companyViewModel.CategoryHeadLine = "Web Design Companies in " + (Country == "globe" ? "" : Country);
                    companyViewModel.Title = " Top Web Design Companies & Agencies in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("web-design-companies", Country);
                    break;

                case "software-development-companies":
                    companyViewModel.CategoryHeadLine = "Custom Software Development Companies in " + (Country == "globe" ? "" : Country);
                    companyViewModel.Title = " Top Software Development Companies & Agencies in " + Country + "- " + year + " | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("software-development-companies", Country);
                    break;

                case "web-development-companies":
                    companyViewModel.CategoryHeadLine = "Web Development Companies in " + (Country == "globe" ? "" : Country);
                    companyViewModel.Title = "Top Web Development Companies & Agencies in " + Country + "- " + year + " | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("web-development-companies", Country);
                    break;

                default:
                    companyViewModel.CategoryHeadLine = "Mobile App Development Companies in " + (Country == "globe" ? "" : Country);
                    companyViewModel.Title = "Top Mobile App Development Companies in " + Country + "- " + year + " | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("mobile-application-developers",Country);
                    break;
            }
        }

        private string CategoryMetaTags(string category,string Country)
        {
            StringBuilder MetaStr = new StringBuilder();
            string CategoryName = string.Empty;
            int year = DateTime.Now.Year;            
            MetaStr.Append("<meta property='og:url' content='{WebsiteUrl}' />");
            MetaStr.Append("<meta property='og:type' content='website' />");
            MetaStr.Append("<meta property='og:title' content='Top 10 {Category} Companies in {Country} - "+ year +"' />");            
            MetaStr.Append("<meta property='og:image' content='' />");
            MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
            MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
            MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
            //MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} companies in {Country} "+ year +" (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
            MetaStr.Append("<meta name='twitter:title' content='Top 10 {Category} Companies in {Country} - "+ year +"' />");
            MetaStr.Append("<meta name='twitter:image' content='' />");
            MetaStr.Append("<link rel='canonical' href='{WebsiteUrl}' />");
            MetaStr.Append("<link rel='publisher' href='' />");

            switch (category)
            {
                case "mobile-application-developers":
                    CategoryName = "mobile app development companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} companies in {Country} "+ year +" (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} companies in {Country} "+ year +" (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} companies in {Country} "+ year +" (iOS and Android) with user votes. Select best mobile app developers from the {Country}.' />");
                    break;
                case "seo-companies":
                    CategoryName = "SEO companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} in {Country} "+ year +" with user votes. Select best SEO agencies from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} in {Country} "+ year +" with user votes. Select best SEO agencies from the {Country}.' />");
                    MetaStr.Append("<meta name = 'twitter:description' content = 'Here is a top 10 {Category} in {Country} "+ year +" with user votes. Select best SEO agencies from the {Country}.' />");
                    break;
                case "digital-marketing-companies":
                    CategoryName = "digital marketing companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} "+ year +" with user votes. Select best marketing firms from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} "+ year +" with user votes. Select best marketing firms from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} "+ year +" with user votes. Select best marketing firms from the {Country}.' />");
                    break;
                case "web-design-companies":
                    CategoryName = "web design companies";
                    MetaStr.Append("<meta property='og:description' content='Find out top 10 {Category} in {Country} "+ year +" with user votes. Select best web designers from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Find out top 10 {Category} in {Country} "+ year +" with user votes. Select best web designers from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Find out top 10 {Category} in {Country} "+ year +" with user votes. Select best web designers from the {Country}.' />");
                    break;
                case "software-development-companies":   
                    CategoryName = "software development companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} and Agencies "+ year +" with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} and Agencies "+ year +" with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} and Agencies "+ year +" with user votes. Find best {Category} from the {Country}.' />");
                    break;
                case "web-development-companies":
                    CategoryName = "web development Companies";
                    MetaStr.Append("<meta property='og:description' content='Here is a top 10 {Category} and Agencies "+ year +" with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='description' content='Here is a top 10 {Category} and Agencies "+ year +" with user votes. Find best {Category} from the {Country}.' />");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a top 10 {Category} and Agencies "+ year +" with user votes. Find best {Category} from the {Country}.' />");
                    break;
            }       
            return MetaStr.Replace("{Category}",CategoryName).Replace("{Country}", Country).Replace("{WebsiteUrl}",Request.Url.ToString()).ToString();
        }
    }
}