using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Utility;

namespace UpVotes.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult HomePage()
        {
            Session["calledPage"] = "H";
            List<CompanyEntity> TopVotedCompanyList = new List<CompanyEntity>();
            if (CacheHandler.Exists("TopVoteCompaniesList"))
            {
                CacheHandler.Get("TopVoteCompaniesList", out TopVotedCompanyList);
            }
            else
            {
                TopVotedCompanyList = new CompanyService().GetTopVoteCompanies();
                CacheHandler.Add(TopVotedCompanyList, "TopVoteCompaniesList");
            }
            return View(TopVotedCompanyList);
        }

        public ActionResult GetListedPage()
        {
            Session["calledPage"] = "G";
            List<CompanyEntity> TopVotedCompanyList = new List<CompanyEntity>();
            if (CacheHandler.Exists("TopVoteCompaniesList"))
            {
                CacheHandler.Get("TopVoteCompaniesList", out TopVotedCompanyList);
            }
            else
            {
                TopVotedCompanyList = new CompanyService().GetTopVoteCompanies();
                CacheHandler.Add(TopVotedCompanyList, "TopVoteCompaniesList");
            }
            return View("~/Views/Login/GetListed.cshtml", TopVotedCompanyList);
        }

        public ActionResult PrivatePolicy()
        {
            if (Session["calledPage"] == null)
            {
                Session["calledPage"] = "P";
            }
            return View("~/Views/Policy/_PrivatePolicy.cshtml");
        }
        public ActionResult Sponsorship()
        {
            if (Session["calledPage"] == null)
            {
                Session["calledPage"] = "P";
            }
            return View("~/Views/Sponsorship/_PublicPricingPlan.cshtml");
        }
    }
}