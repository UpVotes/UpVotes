using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UpVotes.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult HomePage()
        {
            Session["calledPage"] = "H";
            return View();
        }
    }
}