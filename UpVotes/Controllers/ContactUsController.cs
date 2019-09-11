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
    public class ContactUsController : Controller
    {
        public ActionResult ContactUsForm()
        {
            Session["calledPage"] = "";

            return View("~/Views/ContactUs/ContactUs.cshtml");
        }

        [HttpPost]
        public JsonResult SaveContactUsInfo(ContactUsInfoEntity contactInfo)
        {
            dynamic jsonData = default(dynamic);
            try
            {
                contactInfo.AddedBy = Convert.ToInt32(Session["UserID"]);
                int ContactUsID = new ContactUsService().SaveContactInfo(contactInfo);
                if(ContactUsID > 0)
                {
                    jsonData = new
                    {
                        IsSuccess = true
                    };
                }
                else
                {
                    jsonData = new
                    {
                        IsSuccess = false
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
        public JsonResult SaveSponsorerInfo(SponsorerInfoEntity sponsorInfo)
        {
            dynamic jsonData = default(dynamic);
            try
            {
                sponsorInfo.AddedBy = Convert.ToInt32(Session["UserID"]);
                int SponsorID = new ContactUsService().SaveSponsorInfo(sponsorInfo);
                if (SponsorID > 0)
                {
                    jsonData = new
                    {
                        IsSuccess = true
                    };
                }
                else
                {
                    jsonData = new
                    {
                        IsSuccess = false
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