using System;
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
            CompanyViewModel companyViewModel = companyService.GetCompany("0", 0, 0, 0, 0, "ASC", focusAreaID,id, Convert.ToInt32(Session["UserID"]));
            companyViewModel.WebBaseURL = _webBaseURL;
            GetCategoryHeadLine(urlFocusAreaName, companyViewModel);
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
            
            int focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);
            CompanyViewModel companyViewModel = companyService.GetCompany("0", minRate, maxRate, minEmployee, maxEmployee, sortby, focusAreaID, location, Convert.ToInt32(Session["UserID"]));
            companyViewModel.WebBaseURL = _webBaseURL;

            return PartialView("_CompList", companyViewModel);
        }

        public PartialViewResult CompanyAdd()
        {
            return PartialView("_CompanyAddEdit");
        }

        private void GetCategoryHeadLine(string urlFocusAreaName, CompanyViewModel companyViewModel)
        {
            switch (urlFocusAreaName.Trim())
            {
                case "mobile-application-developers":
                    companyViewModel.CategoryHeadLine = "Mobile App Development Companies";
                    companyViewModel.Title = "Top Mobile App Development Companies - 2017 | upvotes.co";
                    companyViewModel.MetaTag=CategoryMetaTags("mobile-application-developers");
                    break;

                case "seo-companies":
                    companyViewModel.CategoryHeadLine = "SEO Companies";
                    companyViewModel.Title = " Top SEO Companies and Services -2017 | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("seo-companies");
                    break;

                case "digital-marketing-companies":
                    companyViewModel.CategoryHeadLine = "Digital Marketing Companies and Agencies";
                    companyViewModel.Title = " Top Digital Marketing Companies & Agencies - 2017 | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("digital-marketing-companies");
                    break;

                case "web-design-companies":
                    companyViewModel.CategoryHeadLine = "Web Design Companies";
                    companyViewModel.Title = " Top Web Design Companies & Agencies - 2017 | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("web-design-companies");
                    break;

                case "software-development-companies":
                    companyViewModel.CategoryHeadLine = "Custom Software Development Companies";
                    companyViewModel.Title = " Top Software Development Companies & Agencies - 2017 | upvotes.co ";
                    companyViewModel.MetaTag = CategoryMetaTags("software-development-companies");
                    break;

                case "web-development-companies":
                    companyViewModel.CategoryHeadLine = "Web Development Companies";
                    companyViewModel.Title = "Top Web Development Companies & Agencies - 2017 | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("web-development-companies");
                    break;

                default:
                    companyViewModel.CategoryHeadLine = "Mobile App Development Companies";
                    companyViewModel.Title = "Top Mobile App Development Companies - 2017 | upvotes.co";
                    companyViewModel.MetaTag = CategoryMetaTags("mobile-application-developers");
                    break;
            }
        }

        private string CategoryMetaTags(string category)
        {
            StringBuilder MetaStr = new StringBuilder();
            switch (category)
            {
                case "mobile-application-developers":
                    MetaStr.Append("<meta name = 'description' content = 'Here is a list of top mobile app development companies 2017 (iOS, iPhone and Android) with user votes. Select top mobile app developers from the globe.' />");
                    MetaStr.Append("<meta property = 'og:url' content = 'https://www.upvotes.co/mobile-application-developers' />");
                    MetaStr.Append("<meta property = 'og:type' content = 'website' />");
                    MetaStr.Append("<meta property = 'og:title' content = 'Top Mobile App Development Companies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta property = 'og:description'   content = 'Here is a list of top mobile app development companies 2017 (iOS, iPhone and Android) with user votes. Select top mobile app developers from the globe.' />");
                    MetaStr.Append("<meta property = 'og:image' content = '' />");
                    MetaStr.Append("<meta name = 'twitter:card' content = 'summary_large_image' />");
                    MetaStr.Append("<meta name = 'twitter:site' content = '@upvotes_co' >");
                    MetaStr.Append("<meta name = 'twitter:creator' content = '@upvotes_co' >");
                    MetaStr.Append("<meta name = 'twitter:description' content = 'Here is a list of top mobile app development companies 2017 (iOS, iPhone and Android) with user votes. Select top mobile app developers from the globe.' />");
                    MetaStr.Append("<meta name = 'twitter:title' content = 'Top Mobile App Development Companies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta name = 'twitter:image' content = '' />");
                    MetaStr.Append("<link rel = 'canonical' href = 'https://www.upvotes.co/mobile-application-developers' />");
                    MetaStr.Append("<link rel = 'publisher' href = '' /> ");
                    break;
                case "seo-companies":
                    MetaStr.Append("<meta name='description' content='Rankings and votes of the top SEO companies and Services 2017 with user votes. Select the best SEO agencies, SEO firms from the globe.'/>");
                    MetaStr.Append("<meta property='og:url' content='https://www.upvotes.co/seo-companies' />");
                    MetaStr.Append("<meta property='og:type' content='website' />");
                    MetaStr.Append("<meta property='og:title' content='Top SEO Companies and Services - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta property='og:description'   content='Rankings and votes of the top SEO companies and Services 2017 with user votes. Select the best SEO agencies, SEO firms from the globe.' />");
                    MetaStr.Append("<meta property='og:image' content='' />");
                    MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
                    MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:description' content='Rankings and votes of the top SEO companies and Services 2017 with user votes. Select the best SEO agencies, SEO firms from the globe.' />");
                    MetaStr.Append("<meta name='twitter:title' content='Top SEO Companies and Services - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta name='twitter:image' content='' />");
                    MetaStr.Append("<link rel='canonical' href='https://www.upvotes.co/seo-companies' />");
                    MetaStr.Append("<link rel='publisher' href='' />");
                    break;
                case "digital-marketing-companies":
                    MetaStr.Append("<meta name='description' content='Are you looking for top digital marketing companies? Here is the list of top digital agencies 2017 with user votes. Select best marketing firms from the globe.' />");
                    MetaStr.Append("<meta property='og:url' content='https://www.upvotes.co/digital-marketing-companies' />");
                    MetaStr.Append("<meta property='og:type' content='website' />");
                    MetaStr.Append("<meta property='og:title' content='Top SEO Companies and Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta property='og:description' content='Are you looking for top digital marketing companies? Here is the list of top digital agencies 2017 with user votes. Select best marketing firms from the globe.' />");
                    MetaStr.Append("<meta property='og:image' content='' />");
                    MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
                    MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:description' content='Are you looking for top digital marketing companies? Here is the list of top digital agencies 2017 with user votes. Select best marketing firms from the globe.' />");
                    MetaStr.Append("<meta name='twitter:title' content='Top Digital Marketing Companies & Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta name='twitter:image' content='' />");
                    MetaStr.Append("<link rel='canonical' href='https://www.upvotes.co/digital-marketing-companies' />");
                    MetaStr.Append("<link rel='publisher' href='' />");
                    break;
                case "web-design-companies":
                    MetaStr.Append("<meta name='description' content='Looking for the top web design companies? Here is the list of best web design agencies 2017 with rankings and ratings. Select best web design firms from the globe.' />");
                    MetaStr.Append("<meta property='og:url' content='https://www.upvotes.co/web-design-companies' />");
                    MetaStr.Append("<meta property='og:type' content='website' />");
                    MetaStr.Append("<meta property='og:title' content='Top Web Design Companies & Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta property='og:description' content='Looking for the top web design companies? Here is the list of best web design agencies 2017 with rankings and ratings. Select best web design firms from the globe.' />");
                    MetaStr.Append("<meta property='og:image' content='' />");
                    MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
                    MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:description' content='Looking for the top web design companies? Here is the list of best web design agencies 2017 with rankings and ratings. Select best web design firms from the globe.' />");
                    MetaStr.Append("<meta name='twitter:title' content='Top Web Design Companies & Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta name='twitter:image' content='' />");
                    MetaStr.Append("<link rel='canonical' href='https://www.upvotes.co/web-design-companies' />");
                    MetaStr.Append("<link rel='publisher' href='' />");
                    break;
                case "software-development-companies":
                    MetaStr.Append("<meta name='description' content='Here is a list of top software development companies and Agencies 2017 with rankings and ratings. Find best software development companies from the globe.' />");
                    MetaStr.Append("<meta property='og:url' content='https://www.upvotes.co/software-development-companies' />");
                    MetaStr.Append("<meta property='og:type' content='website' />");
                    MetaStr.Append("<meta property='og:title' content='Top software development Companies & Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta property='og:description' content='Here is a list of top software development companies and Agencies 2017 with rankings and ratings. Find best software development companies from the globe.' />");
                    MetaStr.Append("<meta property='og:image' content='' />");
                    MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
                    MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a list of top software development companies and Agencies 2017 with rankings and ratings. Find best software development companies from the globe.' />");
                    MetaStr.Append("<meta name='twitter:title' content='Top software development Companies & Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta name='twitter:image' content='' />");
                    MetaStr.Append("<link rel='canonical' href='https://www.upvotes.co/software-development-companies' />");
                    MetaStr.Append("<link rel='publisher' href='' />");
                    break;
                case "web-development-companies":
                    MetaStr.Append("<meta name='description' content='' />");
                    MetaStr.Append("<meta property='og:url' content='https://www.upvotes.co/web-development-companies' />");
                    MetaStr.Append("<meta property='og:type' content='website' />");
                    MetaStr.Append("<meta property='og:title' content='Top web development Companies & Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta property='og:description' content='Here is a list of top web development companies and Agencies 2017 with rankings and ratings. Find best web development companies from the globe.' />");
                    MetaStr.Append("<meta property='og:image' content='' />");
                    MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
                    MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
                    MetaStr.Append("<meta name='twitter:description' content='Here is a list of top web development companies and Agencies 2017 with rankings and ratings. Find best web development companies from the globe.' />");
                    MetaStr.Append("<meta name='twitter:title' content='Top web development Companies & Agencies - 2017 | upvotes.co' />");
                    MetaStr.Append("<meta name='twitter:image' content='' />");
                    MetaStr.Append("<link rel='canonical' href='https://www.upvotes.co/web-development-companies' />");
                    MetaStr.Append("<link rel='publisher' href='' />");
                    break;
            }
            


            return MetaStr.ToString();
        }
    }
}