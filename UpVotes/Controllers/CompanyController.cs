using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;

namespace UpVotes.Controllers
{
    public class CompanyController : Controller
    {
        private string _webBaseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();

        private CompanyService _companyService = null;

        private int _userID = 1;
        /// <summary>
        ///
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public ActionResult Company()
        {
            try
            {
                string companyName = Convert.ToString(Request.Url.Segments[3]).Trim();
                _companyService = new CompanyService();
                CompanyViewModel companyViewModel = _companyService.GetCompany(companyName, 0, 0, 0, 0, "DESC", 0, 1);
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
                    _companyService = new CompanyService();

                    bool isSuccess = _companyService.AddReview(companyReviewModel);
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
            return new CompanyService().VoteForCompany(companyID, _userID);
        }

        public string ThanksNoteForReview(int companyID, int companyReviewID)
        {
            return new CompanyService().ThanksNoteForReview(companyID, companyReviewID, _userID);
        }
    }
}