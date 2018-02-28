using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UpVotes.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult PageNotFound(int statusCode, Exception exception)
        {
            return View("~/Views/Error/PageNotFound.cshtml");
        }
    }
}