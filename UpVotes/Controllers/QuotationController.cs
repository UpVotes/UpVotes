using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpVotes.Models;
using UpVotes.Business;
namespace UpVotes.Controllers
{
    public class QuotationController : Controller
    {
        // GET: Quotation

        [HttpPost]

        public ActionResult GetQuote(string platform, string Theme, string LoginSecurity, string Profile, string Security, string ReviewRate, string Service, string Database, string featuresstring, string EmailId, string Name, string CompanyName)
        {
            string features = string.Empty;

            //if(quotationdata.Features.Count >0)
            //{
            //    features = string.Join("|", quotationdata.Features);
            //}
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
            //return Json(null);
        }

        public ActionResult GetQuotation()
        {
            string focusAreaname = Session["FocusAreaName"].ToString();
            string url = string.Empty;
            switch (focusAreaname)
            {
                case "mobile-application-developers":
                    url = "~/Views/Quotation/_Quotation.cshtml";
                    break;
                default:
                    url = "~/Views/Overview/_iotApplicationOverview.cshtml";
                    break;
            }
            return PartialView(url);
        }
        
    }
   
}