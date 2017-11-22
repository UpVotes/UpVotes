using System;
using System.Configuration;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.Models;

namespace UpVotes.Controllers
{
    public class CompanyListController : Controller
    {
        private string _webBaseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();

        // GET: CompanyList
        public ActionResult CompanyList()
        {
            Session["calledPage"] = "L";
            string urlFocusAreaName = Convert.ToString(Request.Url.Segments[1]);
            int focusAreaID = 0;
            string actualFocusAreaName = string.Empty;
            if (urlFocusAreaName != string.Empty)
            {
                Session["FocusAreaName"] = urlFocusAreaName.ToString();
                focusAreaID = new FocusAreaService().GetFocusAreaID(urlFocusAreaName);
            }
            CompanyService companyService = new CompanyService();
            CompanyViewModel companyViewModel = companyService.GetCompany("0", 0, 0, 0, 0, "ASC", focusAreaID, Convert.ToInt32(Session["UserID"]));
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
        public ActionResult CompanyList(string companyID, decimal minRate, decimal maxRate, int minEmployee, int maxEmployee, string sortby, int focusAreaID)
        {
            CompanyService companyService = new CompanyService();
            CompanyViewModel companyViewModel = companyService.GetCompany("0", minRate, maxRate, minEmployee, maxEmployee, sortby, focusAreaID, Convert.ToInt32(Session["UserID"]));
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
                    break;

                case "seo-companies":
                    companyViewModel.CategoryHeadLine = "SEO Companies";
                    break;

                case "digital-marketing-companies":
                    companyViewModel.CategoryHeadLine = "Digital Marketing Companies and Agencies";
                    break;

                case "web-design-companies":
                    companyViewModel.CategoryHeadLine = "Web Design Companies";
                    break;

                case "software-development-companies":
                    companyViewModel.CategoryHeadLine = "Custom Software Development Companies";
                    break;

                case "web-development-companies":
                    companyViewModel.CategoryHeadLine = "Web Development Companies";
                    break;

                default:
                    companyViewModel.CategoryHeadLine = "Mobile App Development Companies";
                    break;
            }
        }
    }
}