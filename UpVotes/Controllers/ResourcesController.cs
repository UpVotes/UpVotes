using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpVotes.Models;

namespace UpVotes.Controllers
{
    public class ResourcesController : Controller
    {
        // GET: Resouces
        public ActionResult Resource()
        {
            return View("~/Views/UnderConstruction/_underConstruction.cshtml");
        }
        public ActionResult ResourceChild(string id)
        {
            string url = "~/Views/UnderConstruction/_underConstruction.cshtml";
            ViewBag.isPublicQuote = false;
            if (id == "how-much-cost-to-make-an-mobile-app")
            {
                url = "~/Views/Quotation/_Quotation.cshtml";
                ViewBag.Title = "How Much Does it Cost to Make an Mobile App in " + DateTime.Now.Year +"?| App Cost Calculator | Upvotes.co";
                ViewBag.isPublicQuote = true;
            }
            return View(url);
        }
        public ActionResult GetQuotation()
        {            
            ViewBag.isPublicQuote = true;
            return PartialView("~/Views/Quotation/_Quotation.cshtml");            
        }

        [HttpPost]
        public ActionResult GetQuote(string platform, string Theme, string LoginSecurity, string Profile, string Security, string ReviewRate, string Service, string Database, string featuresstring, string EmailId, string Name, string CompanyName)
        {
            string features = string.Empty;
            ViewBag.isPublicQuote = true;
            var quotationresponse = new QuotationResponse();
            try
            {
                var quotationrequest = new QuotationRequest()
                {
                    platform = platform,
                    Theme = Theme,
                    LoginSecurity = LoginSecurity,
                    Profile = Profile,
                    Security = Security,
                    ReviewRate = ReviewRate,
                    Service = Service,
                    Database = Database,
                    featuresstring = featuresstring,
                    EmailId = EmailId,
                    Name = Name,
                    CompanyName = CompanyName
                };
                quotationresponse = new Business.QuotationService().GetQuoteForMobileApp(quotationrequest);
            }
            catch (Exception ex)
            {

            }
            return PartialView("~/Views/Quotation/_QuotationBreakdown.cshtml", quotationresponse);            
        }
    }
}