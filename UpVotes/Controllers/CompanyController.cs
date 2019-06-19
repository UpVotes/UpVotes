using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;
using UpVotes.Utility;

namespace UpVotes.Controllers
{
    public class CompanyController : Controller
    {
        private readonly string _webBaseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();
        /// <summary>
        ///
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult Company()
        {
            try
            {
                Session["calledPage"] = "C";
                string companyName = Convert.ToString(Request.Url.Segments[2]).Trim();
                CompanyViewModel companyViewModel = null;
                if (companyName != string.Empty)
                {
                    Session["CompanyName"] = companyName;
                }
                if (CacheHandler.Exists(companyName.ToLower()))
                {
                    companyViewModel = new CompanyViewModel();
                    CacheHandler.Get(companyName.ToLower(), out companyViewModel);
                }
                else
                {
                    CompanyFilterEntity companyFilter = new CompanyFilterEntity
                    {
                        CompanyName = companyName.Replace("-", " "),
                        MinRate = 0,
                        MaxRate = 0,
                        MinEmployee = 0,
                        MaxEmployee = 0,
                        SortBy = "DESC",
                        FocusAreaID = 0,
                        Location = "0",
                        SubFocusArea = "0",
                        UserID = Convert.ToInt32(Session["UserID"]),
                        PageNo = 1,
                        PageSize = 10,
                        OrderColumn = 1
                    };

                    companyViewModel = new CompanyService().GetCompany(companyFilter);                    
                    CacheHandler.Add(companyViewModel, companyName);
                }

                companyViewModel.WebBaseURL = _webBaseURL;
                return View(companyViewModel);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ActionResult CompanyAllPortfolioByName(string id)
        {
            Session["calledPage"] = "P";
            CompanyViewModel companyViewModel = null;
            CompanyFilterEntity companyreviewsFilter = new CompanyFilterEntity
            {
                CompanyName = id.Replace("-", " "),
                Rows = 0
            };
            companyViewModel = new CompanyService().GetCompanyPortfolio(companyreviewsFilter);
            if (companyViewModel != null && (companyViewModel.CompanyList != null && companyViewModel.CompanyList.Count > 0))
            {
                companyViewModel.WebBaseURL = _webBaseURL;
                return View("~/Views/AllListPages/AllPortFolioList.cshtml", companyViewModel);
            }
            else
            {
                return View("~/Views/Error/PageNotFound.cshtml");
            }
        }

        public ActionResult CompanyAllTeamMembersByName(string id)
        {
            Session["calledPage"] = "P";            
            List<TeamMemebersEntity> teamMembersViewModel = new TeamMembersService().GetAllTeamMembers(id, true);
            if (teamMembersViewModel.Count > 0)
            {
                ViewBag.ServiceSoftwareName = id;
                return View("~/Views/AllListPages/AllTeamMembersList.cshtml", teamMembersViewModel);
            }
            else
            {
                return View("~/Views/Error/PageNotFound.cshtml");
            }
        }

        public ActionResult CompanyAllReviewsByName(string id)
        {
            try
            {
                Session["calledPage"] = "R";

                CompanyViewModel companyViewModel = null;

                CompanyFilterEntity companyreviewsFilter = new CompanyFilterEntity
                {
                    CompanyName = id.Replace("-", " "),
                    Rows = 0
                };

                companyViewModel = new CompanyService().GetUserReviews(companyreviewsFilter);
                if (companyViewModel != null && (companyViewModel.CompanyList != null && companyViewModel.CompanyList.Count > 0))
                {
                    companyViewModel.WebBaseURL = _webBaseURL;
                    return View("~/Views/AllListPages/AllCompanyReviewsList.cshtml", companyViewModel);
                }
                else
                {
                    return View("~/Views/Error/PageNotFound.cshtml");
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public JsonResult GetFocusArea()
        {
            dynamic jsonResult = default(dynamic);
            try
            {
                List<FocusAreaEntity> focusAreaList = new FocusAreaService().GetFocusAreas();
                jsonResult = new
                {
                    focusAreaList
                };
            }
            catch (Exception)
            {

            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }



        public string VoteForCompany(int companyID)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                string compname = "";
                if (!string.IsNullOrEmpty(Session["CompanyName"].ToString()))
                {
                    compname = Session["CompanyName"].ToString();
                }
                if (CacheHandler.Exists(compname))
                {
                    CacheHandler.Clear(compname);
                }
                if (CacheHandler.Exists("TopVoteCompaniesList"))
                {
                    CacheHandler.Clear("TopVoteCompaniesList");
                }
                return new CompanyService().VoteForCompany(companyID, Convert.ToInt32(Session["UserID"]));
            }
            else
            {
                return "0";
            }
        }

        public string ThanksNoteForReview(int companyID, int companyReviewID)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                string compname = "";
                if (Session["CompanyName"] != null)
                {
                    compname = Session["CompanyName"].ToString();
                }
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
        public ActionResult SubmitReview(int companyID, string companyName)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                TempData["SubmitReviewInformation"] = new SubmitReviewViewModel() { SoftwareOrCompanyId = companyID, SoftwareOrCompanyName = companyName, TabIndex = 0 };
                return Json("/submit/submit-review", JsonRequestBehavior.AllowGet);
                //return new CompanyService().ThanksNoteForReview(companyID, companyReviewID, Convert.ToInt32(Session["UserID"]));
            }
            else
            {
                return Json("Please login to submit the review.", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddNews()
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                return Json("/company/my-dashboard?section=news", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Please login to add the news.", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddTeamMember()
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                return Json("/company/my-dashboard?section=employees", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Please login to add the team members.", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddPortfolio()
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                return Json("/company/my-dashboard?section=portfolio", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Please login to add the portfolio.", JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult CompanyReview(string id)
        //{   
        //    return Json("Please login to submit the review.", JsonRequestBehavior.AllowGet);

        //}

        public string ClaimListing(int companyID, string Email, string Domain)
        {
            int UserID = 0;
            string message = string.Empty;
            string compname = string.Empty;
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }
            if (!string.IsNullOrEmpty(Session["CompanyName"].ToString()))
            {
                compname = Session["CompanyName"].ToString();
            }

            try
            {
                ClaimApproveRejectListingRequest claimListingRequest = new ClaimApproveRejectListingRequest
                {
                    ClaimListingID = 0,
                    companyID = companyID,
                    CompanyName = compname,
                    IsUserVerify = false,
                    Email = Email,
                    Domain = Domain,
                    userID = UserID
                };

                message = new CompanyService().ClaimListing(claimListingRequest);
            }
            catch (Exception)
            {
            }
            return message;

        }

        public void RemoveCompanyCache(string id)
        {
            if (CacheHandler.Exists(id))
            {
                CacheHandler.Clear(id);
            }

        }

    }
}