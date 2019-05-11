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
    public class SubmitReviewController : Controller
    {
        public ActionResult SubmitReview()
        {
            Session["calledPage"] = "H";
            SubmitReviewViewModel submitObj = new SubmitReviewViewModel();
            submitObj.SoftwareOrCompanyId = 0;
            submitObj.SoftwareOrCompanyName = "";
            submitObj.TabIndex = 0;
            if (TempData.ContainsKey("SubmitReviewInformation"))
            {
                var submit = TempData["SubmitReviewInformation"] as SubmitReviewViewModel;
                submitObj.SoftwareOrCompanyId = submit.SoftwareOrCompanyId;
                submitObj.SoftwareOrCompanyName = submit.SoftwareOrCompanyName;
                submitObj.TabIndex = submit.TabIndex;
            }                 
            return View("~/Views/Authenticated/SubmitReview/SubmitReview.cshtml", submitObj);            
        }

        [HttpPost]
        public ActionResult GetCompanySoftwareAutoComplete(int type,string search)
        {
            try
            {                
                AutocompleteRequestEntity AutocompleteObj = new AutocompleteRequestEntity{ Type = type, Search=search};
                var response = new SubmitReviewService().GetDataForAutoComplete(AutocompleteObj);
                return Json(response.companySoftwareAutocomplete, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string AddCompanyReview(CompanySoftwareReviewViewModel review)
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    review.UserID = Convert.ToInt32(Session["UserID"]);
                    bool isSuccess = new SubmitReviewService().AddCompanyReview(review);
                    string compname = "";
                    if (Session["CompanyName"] != null)
                    {
                        compname = Session["CompanyName"].ToString();
                    }
                    if (CacheHandler.Exists(compname + "reviews"))
                    {
                        CacheHandler.Clear(compname + "reviews");
                    }
                    if (CacheHandler.Exists(compname))
                    {
                        CacheHandler.Clear(compname);
                    }
                    if(!isSuccess)
                    {
                        return "Review is already added by this user.";
                    }
                    else
                    {
                        return "Success";
                    }
                }
                else
                {
                    return "Login to add reviews";
                }          
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string AddSoftwareReview(CompanySoftwareReviewViewModel review)
        {
            try
            {
                if (Session["UserID"] != null)
                {
                    review.UserID = Convert.ToInt32(Session["UserID"]);                    
                    bool isSuccess = new SubmitReviewService().AddSoftwareReview(review);
                    string softname = "";
                    if (Session["SoftwareName"] != null)
                    {
                        softname = Session["SoftwareName"].ToString();
                    }
                    if (CacheHandler.Exists(softname + "reviews"))
                    {
                        CacheHandler.Clear(softname + "reviews");
                    }
                    if (CacheHandler.Exists(softname))
                    {
                        CacheHandler.Clear(softname);
                    }
                    if (!isSuccess)
                    {
                        return "Review is already added by this user.";
                    }
                    else
                    {
                        return "Success";
                    }
                }
                else
                {
                    return "Login to add reviews";
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }

    }
}