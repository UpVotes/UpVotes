using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;


namespace UpVotes.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult CustomSearch(string Query)
        {
            ViewBag.Query = Query;
            ViewBag.Title = "Search";
            return View("~/Views/Search/Search.cshtml");
            
        }
    }
}