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
    public class PricingController : Controller
    {
        
        public ActionResult DashboardPricing()
        {
            return PartialView("~/Views/Authenticated/Center/PricingPlan.cshtml");
        }

        
    }
}