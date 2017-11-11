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
        private int _userID = 1;
        // GET: CompanyList
        public ActionResult CompanyList()
        {
            string focusAreaName = Convert.ToString(Request.Url.Segments[3]);
            int focusAreaID = 0;
            if(focusAreaName != string.Empty)
            {
                focusAreaName = focusAreaName.Replace("-", " ").Trim();
                focusAreaID = new FocusAreaService().GetFocusAreaID(focusAreaName);
            }
            CompanyService companyService = new CompanyService();
            CompanyViewModel companyViewModel = companyService.GetCompany("0", 0, 0, 0, 0, "DESC", focusAreaID, _userID);
            companyViewModel.WebBaseURL = _webBaseURL;
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
            CompanyViewModel companyViewModel = companyService.GetCompany("0", minRate, maxRate, minEmployee, maxEmployee, sortby, focusAreaID, _userID);
            companyViewModel.WebBaseURL = _webBaseURL;

            return PartialView("_CompList",companyViewModel);
        }

        public PartialViewResult CompanyAdd()
        {
            return PartialView("_CompanyAddEdit");
        }
    }
}