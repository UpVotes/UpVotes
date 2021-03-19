using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpVotes.Business;
//using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;

namespace UpVotes.Controllers
{
    public class BlogsController : Controller
    {
        
        public ActionResult Blog(string id)
        {
            Session["calledPage"] = "H";
            string url = "~/Views/UnderConstruction/_underConstruction.cshtml";
            ViewBag.isPublicQuote = false;
            if (!string.IsNullOrEmpty(id))
            {
                if(id.ToLower() == "how-much-cost-to-make-an-mobile-app")
                {
                    ViewBag.isPublicQuote = true;
                    ViewBag.Title = "How Much Does it Cost to Make an Mobile App in " + DateTime.Now.Year + "?| App Cost Calculator | Upvotes.co";
                    url = "~/Views/Quotation/_Quotation.cshtml";
                }
                else
                {
                    url="~/Views/Error/PageNotFound.cshtml";
                }
            }
            return View(url);
        }

        public ActionResult article(string id)
        {
            Session["calledPage"] = "H";
            string url = "~/Views/Error/PageNotFound.cshtml";
            string[] articleArray = { "how-much-cost-make-app", "app-store-optimization", "email-marketing-for-financial-advisors", "mobile-app-development-trends", "digital-marketing-trends-2020", "on-demand-app-services", "software-development-and-drive-success", "laravel-fortify", "tips-for-erp-implementation", "tips-for-web-design" };
            bool bol = Array.Exists(articleArray, E => E == id);                        
            if (bol)
            {
                url = "~/Views/Blog/"+id+".cshtml";                
            }            
            return View(url);
        }

        public ActionResult QuotationWidget()
        {
            ViewBag.isPublicQuote = true;
            return PartialView("~/Views/Quotation/_Quotation.cshtml");
        }
        //public ActionResult ResourceChild(string id)
        //{
        //    string url = "~/Views/UnderConstruction/_underConstruction.cshtml";
        //    ViewBag.isPublicQuote = false;
        //    if (id == "how-much-cost-to-make-an-mobile-app")
        //    {
        //        url = "~/Views/Quotation/_Quotation.cshtml";
        //        ViewBag.Title = "How Much Does it Cost to Make an Mobile App in " + DateTime.Now.Year +"?| App Cost Calculator | Upvotes.co";
        //        ViewBag.isPublicQuote = true;
        //    }
        //    return View(url);
        //}
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

        public ActionResult AllCategories()
        {
            Session["calledPage"] = "";
            List<UpVotes.BusinessEntities.Entities.CategoryLinksEntity> LinksObj = new List<UpVotes.BusinessEntities.Entities.CategoryLinksEntity>();
            LinksObj = new CompanyService().GetServiceCategoryLinks(9);
            return View("~/Views/Navigation/AllCategories.cshtml", LinksObj);
        }

        [HttpPost]
        public ActionResult AllCategories(int focusAreaID)
        {
            List<UpVotes.BusinessEntities.Entities.CategoryLinksEntity> LinksObj = new List<UpVotes.BusinessEntities.Entities.CategoryLinksEntity>();
            LinksObj = new CompanyService().GetServiceCategoryLinks(focusAreaID);
            return PartialView("~/Views/Navigation/_ServiceCategories.cshtml", LinksObj);
        }


    }
}