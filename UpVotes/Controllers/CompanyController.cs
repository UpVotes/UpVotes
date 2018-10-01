using Newtonsoft.Json.Linq;
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
        private string _webBaseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();
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
                if (CacheHandler.Exists(companyName))
                {
                    companyViewModel = new CompanyViewModel();
                    CacheHandler.Get(companyName, out companyViewModel);
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
                        SortBy = "ASC",
                        FocusAreaID = 0,
                        Location = "0",
                        SubFocusArea = "0",
                        UserID = Convert.ToInt32(Session["UserID"]),
                        PageNo = 1,
                        PageSize = 10,                        
                    };

                    companyViewModel = new CompanyService().GetCompany(companyFilter);
                    CacheHandler.Add(companyViewModel, companyName);
                }                
                
                companyViewModel.WebBaseURL = _webBaseURL;
                return View(companyViewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult GetFocusArea()
        {
            var jsonResult = default(dynamic);
            try
            {
                List<FocusAreaEntity> focusAreaList = new FocusAreaService().GetFocusAreas();
                jsonResult = new
                {
                    focusAreaList
                };
            }
            catch (Exception ex)
            {

            }

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }

        public bool AddReview()
        {
            try
            {
                if (Request.Params["companyReviewModel"] != null)
                {
                    JObject jobj = JObject.Parse(Request.Params["companyReviewModel"]);
                    CompanyReviewViewModel companyReviewModel = jobj.ToObject<CompanyReviewViewModel>();
                    companyReviewModel.UserID = Convert.ToInt32(Session["UserID"]);
                    bool isSuccess = new CompanyService().AddReview(companyReviewModel);
                    string compname = "";
                    if (!string.IsNullOrEmpty(Session["CompanyName"].ToString()))
                    {
                        compname = Session["CompanyName"].ToString();
                    }
                    if (CacheHandler.Exists(compname + "reviews"))
                    {                        
                        CacheHandler.Clear(compname + "reviews");
                    }
                    
                    return isSuccess;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
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
                return new CompanyService().ThanksNoteForReview(companyID, companyReviewID, Convert.ToInt32(Session["UserID"]));
            }
            else
            {
                return "Please login to provide thanks note.";
            }
        }

        public string ClaimListing(int companyID,string Email,string Domain)
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
                var claimListingRequest = new ClaimApproveRejectListingRequest
                {
                    ClaimListingID=0,
                    companyID = companyID,
                    CompanyName = compname,
                    IsUserVerify = false,
                    Email = Email,
                    Domain = Domain,
                    userID = UserID
                };

                message = new CompanyService().ClaimListing(claimListingRequest);
            }
            catch (Exception ex)
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