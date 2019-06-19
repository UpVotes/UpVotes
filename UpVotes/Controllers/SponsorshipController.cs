using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.BusinessEntities.Helper;
using UpVotes.Models;

namespace UpVotes.Controllers
{
    public class SponsorshipController : Controller
    {
        public ActionResult GetSponsorShipForm()
        {
            if (Session["UserDashboardInfo"] != null)
            {
                DashboardViewModel dashboardObj = new DashboardViewModel();
                dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                if (dashboardObj.IsAdmin && ((UserEntity)Session["UserObj"]).UserType == 4)
                {
                    return PartialView("~/Views/Authenticated/Center/UserSponsershipForm.cshtml", dashboardObj);
                }
                else
                {
                    return PartialView("~/Views/Error/PageNotFound.cshtml");
                }
            }
            else
            {
                return PartialView("~/Views/Error/PageNotFound.cshtml");
            }
        }

        public ActionResult GetSchedulerForm()
        {
            if (Session["UserDashboardInfo"] != null)
            {
                DashboardViewModel dashboardObj = new DashboardViewModel();
                dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                if (dashboardObj.IsAdmin && ((UserEntity)Session["UserObj"]).UserType == 4)
                {
                    SponsorshipRequestEntity sponsorshipReqObj = new SponsorshipRequestEntity();
                    sponsorshipReqObj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    List<SponsorshipExpiredListEntity> ExpiredListObj = new SponsorshipService().GetExpiredSponsorshipList(sponsorshipReqObj);
                    return PartialView("~/Views/Authenticated/Center/SchedulerForm.cshtml", ExpiredListObj);
                }
                else
                {
                    return PartialView("~/Views/Error/PageNotFound.cshtml");
                }
            }
            else
            {
                return PartialView("~/Views/Error/PageNotFound.cshtml");
            }
        }

        [HttpPost]
        public ActionResult ApplySponsorship(SponsorshipRequestEntity sponsorshipReqObj)
        {
            dynamic jsonData = default(dynamic);
            try
            {
                sponsorshipReqObj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                bool isApplied = new SponsorshipService().ApplySponsorship(sponsorshipReqObj);

                if (isApplied)
                {
                    if (Utility.CacheHandler.Exists(sponsorshipReqObj.CompanyOrSoftwareName.ToLower().Replace(" ", "-")))
                    {
                        UpVotes.Utility.CacheHandler.Clear(sponsorshipReqObj.CompanyOrSoftwareName.ToLower().Replace(" ", "-"));
                    }

                    jsonData = new
                    {
                        IsSuccess = true,
                    };
                }
                else
                {
                    jsonData = new
                    {
                        IsSuccess = false,
                    };
                }

            }
            catch (Exception)
            {
                jsonData = new
                {
                    IsSuccess = false
                };
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SchedulerToDeactivateSponsor()
        {
            dynamic jsonData = default(dynamic);
            try
            {
                SponsorshipRequestEntity sponsorshipReqObj = new SponsorshipRequestEntity();
                sponsorshipReqObj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                List<SponsorshipExpiredListEntity> ExpiredListObj = new SponsorshipService().GetExpiredSponsorshipList(sponsorshipReqObj);
                bool isDeactivated = new SponsorshipService().SchedulerToDeactivateSponsor(sponsorshipReqObj);
                if (isDeactivated)
                {                    
                    foreach (var item in ExpiredListObj)
                    {
                        if (Utility.CacheHandler.Exists(item.Name.ToLower().Replace(" ", "-")))
                        {
                            UpVotes.Utility.CacheHandler.Clear(item.Name.ToLower().Replace(" ", "-"));
                        }
                    }                    

                    jsonData = new
                    {
                        IsSuccess = true,
                    };
                }
                else
                {
                    jsonData = new
                    {
                        IsSuccess = false,
                    };
                }

            }
            catch (Exception)
            {
                jsonData = new
                {
                    IsSuccess = false
                };
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}