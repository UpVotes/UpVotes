﻿using Newtonsoft.Json.Linq;
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
    public class SoftwaresController : Controller
    {
        private readonly string _webBaseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();
       
        [ValidateInput(false)]
        public ActionResult Softwares()
        {
            try
            {
                Session["calledPage"] = "N";
                string softwareName = Convert.ToString(Request.Url.Segments[2]).Trim();
                SoftwareViewModel softwareViewModel = null;
                if (softwareName != string.Empty)
                {
                    Session["SoftwareName"] = softwareName;
                }
                if (CacheHandler.Exists(softwareName.ToLower()))
                {
                    softwareViewModel = new SoftwareViewModel();
                    CacheHandler.Get(softwareName.ToLower(), out softwareViewModel);
                }
                else
                {
                    SoftwareFilterEntity companyFilter = new SoftwareFilterEntity
                    {
                        SoftwareName = softwareName.Replace("-", " "),                        
                        SortBy = "DESC",
                        SoftwareCategoryId = 0,                        
                        UserID = Convert.ToInt32(Session["UserID"]),
                        PageNo = 1,
                        PageSize = 10, 
                        OrderColumn = 1                       
                    };

                    softwareViewModel = new SoftwareService().GetSoftware(companyFilter);
                    CacheHandler.Add(softwareViewModel, softwareName);
                }

                softwareViewModel.WebBaseUrl = _webBaseURL;
                return View(softwareViewModel);
            }
            catch (Exception)
            {
                return null;
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

        public ActionResult CompanyAllTeamMembersByName(string id)
        {
            Session["calledPage"] = "P";            
            var teamMembersViewModel = new TeamMembersService().GetAllTeamMembers(id, false);
            if (teamMembersViewModel.Count > 0)
            {
                ViewBag.ServiceSoftwareName = id;
                ViewBag.IsService = false;
                return View("~/Views/AllListPages/AllTeamMembersList.cshtml", teamMembersViewModel);
            }
            else
            {
                return View("~/Views/Error/PageNotFound.cshtml");
            }
        }

        public string VoteForSoftware(int softwareID)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                string softname = "";
                if (!string.IsNullOrEmpty(Session["SoftwareName"].ToString()))
                {
                    softname = Session["SoftwareName"].ToString();
                }
                if (CacheHandler.Exists(softname))
                {
                    CacheHandler.Clear(softname);
                }                
                return new SoftwareService().VoteForSoftware(softwareID, Convert.ToInt32(Session["UserID"]));
            }
            else
            {
                return "0";
            }
        }

        public ActionResult SoftwareAllReviewsByName(string id)
        {
            try
            {
                Session["calledPage"] = "R";

                SoftwareViewModel softwareViewModel = null;

                SoftwareFilterEntity softwarereviewsFilter = new SoftwareFilterEntity
                {
                    SoftwareName = id.Replace("-", " "),
                    Rows = 0
                };

                softwareViewModel = new SoftwareService().GetAllSoftwareReviews(softwarereviewsFilter);
                if (softwareViewModel != null && (softwareViewModel.SoftwareList != null && softwareViewModel.SoftwareList.Count > 0))
                {
                    softwareViewModel.WebBaseUrl = _webBaseURL;
                    return View("~/Views/AllListPages/AllSoftwareReviewsList.cshtml", softwareViewModel);
                }
                else
                {
                    return View("~/Views/Error/PageNotFound.cshtml");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult SubmitSoftwareReview(int softwareID, string softwareName)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                TempData["SubmitReviewInformation"] = new SubmitReviewViewModel() { SoftwareOrCompanyId = softwareID, SoftwareOrCompanyName = softwareName, TabIndex = 1 };
                return Json("/submit/submit-review", JsonRequestBehavior.AllowGet);
                //return new CompanyService().ThanksNoteForReview(companyID, companyReviewID, Convert.ToInt32(Session["UserID"]));
            }
            else
            {
                return Json("Please login to submit the review.", JsonRequestBehavior.AllowGet);
            }
        }

        public string ThanksNoteForReview(int softwareID, int softwareReviewID)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                string softwarename = "";
                if (!string.IsNullOrEmpty(Session["SoftwareName"].ToString()))
                {
                    softwarename = Session["SoftwareName"].ToString();
                }
                if (CacheHandler.Exists(softwarename))
                {
                    CacheHandler.Clear(softwarename);
                }
                return new SoftwareService().ThanksNoteForReview(softwareID, softwareReviewID, Convert.ToInt32(Session["UserID"]));
            }
            else
            {
                return "Please login to provide thanks note.";
            }
        }

        public string ClaimSoftwareListing(int SoftwareID, string Email, string Domain)
        {
            int UserID = 0;
            string message = string.Empty;
            string softwarename = string.Empty;
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                UserID = Convert.ToInt32(Session["UserID"]);
            }
            if (!string.IsNullOrEmpty(Session["SoftwareName"].ToString()))
            {
                softwarename = Session["SoftwareName"].ToString();
            }
            
            try
            {
                var claimSoftwareListingRequest = new ClaimApproveRejectListingRequest
                {
                    ClaimListingID=0,
                    softwareID = SoftwareID,
                    SoftwareName = softwarename,
                    IsUserVerify = false,
                    Email = Email,
                    Domain = Domain,
                    userID = UserID
                };

                message = new SoftwareService().ClaimSoftwareListing(claimSoftwareListingRequest);
            }
            catch (Exception ex)
            {
            }
            return message;

        }

        public void RemoveSoftwareCache(string id)
        {
            if (CacheHandler.Exists(id))
            {
                CacheHandler.Clear(id);
            }

        }

        public ActionResult UpdateSoftwareRejectionComments()
        {
            if (Request.Params["softwareRejectComments"] != null)
            {
                JObject jobj = JObject.Parse(Request.Params["softwareRejectComments"]);
                SoftwareRejectComments softwareRejectComments = jobj.ToObject<SoftwareRejectComments>();
                UserEntity loggedInUser = (UserEntity)Session["UserObj"];
                softwareRejectComments.RejectedBy = loggedInUser.FirstName + " " + loggedInUser.LastName;
                bool isRejectCommentsUpdated = new SoftwareService().UpdateSoftwareRejectionComments(softwareRejectComments);
                return PartialView("~/Views/Authenticated/Center/UserSoftwareList.cshtml",
                    new SoftwareViewModel() { SoftwareList = new List<SoftwareEntity>() });
            }

            return null;
        }

    }
}