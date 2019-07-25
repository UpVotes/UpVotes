using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;
using UpVotes.Utility;

namespace UpVotes.Controllers
{
    public class SoftwareListController : Controller
    {
        private readonly string _webBaseUrl = ConfigurationManager.AppSettings["WebBaseURL"].ToString();

        // GET: CompanyList
        public ActionResult SoftwareList(string id)
        {
            Session["calledPage"] = "S";
            var urlSoftwareCategory = Convert.ToString(Request.Url.Segments[1]);
            var softwareService = new SoftwareService();
            var softwareCategoryId = 0;
            if (Request.Url.Segments.Length > 2)
            {
                urlSoftwareCategory = urlSoftwareCategory.Replace("/", "");
            }
            if (urlSoftwareCategory != string.Empty)
            {
                Session["SoftwareCategory"] = urlSoftwareCategory.ToString();
                softwareCategoryId = new FocusAreaService().GetSoftwareCategoryID(urlSoftwareCategory);
            }
            SoftwareFilterEntity softwareFilter = new SoftwareFilterEntity
            {
                SoftwareName = "",
                SoftwareCategoryId= softwareCategoryId,
                PageNo=1,
                PageSize=10,
                SortBy = "DESC",
                UserID = Convert.ToInt32(Session["UserID"]),
                OrderColumn = 1,
            };

            SoftwareViewModel softwareViewModel = softwareService.GetSoftware(softwareFilter);
            softwareViewModel.WebBaseUrl = _webBaseUrl;
            GetSoftwareCategoryHeadLine(urlSoftwareCategory, softwareViewModel);
            softwareViewModel.SoftwareCategoryId = softwareCategoryId.ToString();
            softwareViewModel.PageCount = 0;
            softwareViewModel.PageNumber = 1;
            softwareViewModel.PageIndex = 1;
            softwareViewModel.TotalNoOfSoftwares = softwareViewModel.SoftwareList.Select(a => a.TotalCount).FirstOrDefault();
            if(softwareViewModel.SoftwareList.Count>0)
            {
                softwareViewModel.PageCount = (softwareViewModel.SoftwareList[0].TotalCount + 10 - 1) / 10;
            }
            return View(softwareViewModel);
        }
        private void GetSoftwareCategoryHeadLine(string urlSoftwareCategory, SoftwareViewModel softwareViewModel)
        {
            
            int year = DateTime.Now.Year;
            

            string cachename = urlSoftwareCategory.Trim();
            CategoryMetaTagsDetails metaTagObj;
            if (CacheHandler.Exists(cachename))
            {
                metaTagObj = new CategoryMetaTagsDetails();
                CacheHandler.Get(cachename, out metaTagObj);
            }
            else
            {
                metaTagObj = new CategoryMetaTagsDetails();
                metaTagObj = new SoftwareService().GetSoftwareCategoryMetaTags(urlSoftwareCategory.Trim());
                CacheHandler.Add(metaTagObj, cachename);
            }
            softwareViewModel.CategoryHeadLine = metaTagObj.Title.ToUpper();
            softwareViewModel.Title = metaTagObj.TwitterTitle.Replace("{Year}", year.ToString());
            softwareViewModel.MetaTag = SoftwareCategoryMetaTags(metaTagObj);
        }
        public JsonResult GetDataForAutoComplete()
        {
            try
            {
                var jsonResult = default(dynamic);
                string searchTerm = Convert.ToString(Request.Params["starts_with"]);
                int SoftwareCategory = new FocusAreaService().GetSoftwareCategoryID(Convert.ToString(Session["SoftwareCategory"]));
                List<string> myAutoCompleteList = new SoftwareService().GetSoftwareForAutoComplete(SoftwareCategory, searchTerm);
                jsonResult = from a in myAutoCompleteList
                             select new
                             {
                                 Name = a
                             };
                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetSoftwareUserReviews()
        {
            int SoftwareCategoryID = 0;
            if (Session["SoftwareCategory"] != null)
            {
                SoftwareCategoryID = new FocusAreaService().GetSoftwareCategoryID(Session["SoftwareCategory"].ToString());                
            }
            SoftwareFilterEntity UserReviewObj = new SoftwareFilterEntity
            {
                SoftwareCategoryId = SoftwareCategoryID,
                SoftwareName = "",
                PageNo = 1,
                PageSize = 10
            };

            CompanySoftwareUserReviews companyReViewModel = new CompanySoftwareUserReviews();
            companyReViewModel = new SoftwareService().GetUserReviewsForSoftwareListingPage(UserReviewObj);

            return PartialView("~/Views/SoftwareList/_SoftwareUsersReviewsList.cshtml", companyReViewModel);

        }

        [HttpPost]
        public ActionResult GetSoftwareUserReviews(string softwarename, int PageSize)
        {
            int SoftwareCategoryID = 0;
            if (Session["SoftwareCategory"] != null)
            {
                SoftwareCategoryID = new FocusAreaService().GetSoftwareCategoryID(Session["SoftwareCategory"].ToString());
            }
            SoftwareFilterEntity UserReviewObj = new SoftwareFilterEntity
            {
                SoftwareCategoryId = SoftwareCategoryID,
                SoftwareName = softwarename,
                PageNo = 1,
                PageSize = PageSize
            };

            CompanySoftwareUserReviews companyReViewModel = new CompanySoftwareUserReviews();
            companyReViewModel = new SoftwareService().GetUserReviewsForSoftwareListingPage(UserReviewObj);

            return PartialView("~/Views/SoftwareList/_SoftwareUsersReviewsList.cshtml", companyReViewModel);

        }

        public string ThanksNoteForReview(int softwareID, int softwareReviewID, string softwareName)
        {
            if (Convert.ToInt32(Session["UserID"]) != 0)
            {
                if (CacheHandler.Exists(softwareName))
                {
                    CacheHandler.Clear(softwareName);
                }
                string message = new SoftwareService().ThanksNoteForReview(softwareID, softwareReviewID, Convert.ToInt32(Session["UserID"]));
                return message;
            }
            else
            {
                return "Please login to provide thanks note.";
            }
        }

        private string SoftwareCategoryMetaTags(CategoryMetaTagsDetails metaTag)
        {
            StringBuilder MetaStr = new StringBuilder();
            int year = DateTime.Now.Year;
            MetaStr.Append("<meta property='og:url' content='{WebsiteUrl}' />");
            MetaStr.Append("<meta property='og:type' content='website' />");           
            MetaStr.Append("<meta property='og:title' content='" + metaTag.TwitterTitle.Replace("{Year}", year.ToString()) + "' />");
            MetaStr.Append("<meta name='twitter:title' content='" + metaTag.TwitterTitle.Replace("{Year}", year.ToString()) + "' />");
            MetaStr.Append("<meta property='og:image' content='' />");
            MetaStr.Append("<meta name='twitter:card' content='summary_large_image' />");
            MetaStr.Append("<meta name='twitter:site' content='@upvotes_co'>");
            MetaStr.Append("<meta name='twitter:creator' content='@upvotes_co'>");
            MetaStr.Append("<meta name='twitter:image' content='' />");
            MetaStr.Append("<link rel='canonical' href='{WebsiteUrl}' />");
            MetaStr.Append("<link rel='publisher' href='#' />");
            MetaStr.Append("<meta property='og:description' content='" + metaTag.Descriptions + "' />");
            MetaStr.Append("<meta name='description' content='" + metaTag.Descriptions + "' />");
            MetaStr.Append("<meta name='twitter:description' content='" + metaTag.Descriptions + "' />");
            return MetaStr.Replace("{WebsiteUrl}", Request.Url.ToString()).ToString();
        }

        [HttpPost]
        public ActionResult SoftwareList(string SoftwareName, int SoftwareCategoryID, string sortby, int PageNo, int PageSize, int FirstPage, int LastPage, int OrderColumn)
        {
            SoftwareService softwareService = new SoftwareService();

            SoftwareFilterEntity softwareFilter = new SoftwareFilterEntity
            {
                SoftwareName = SoftwareName,
                SoftwareCategoryId = SoftwareCategoryID,
                PageNo = PageNo,
                PageSize = PageSize,
                SortBy = sortby,
                UserID = Convert.ToInt32(Session["UserID"]),
                OrderColumn = OrderColumn
            };

            SoftwareViewModel softwareViewModel = softwareService.GetSoftware(softwareFilter);
            softwareViewModel.WebBaseUrl = _webBaseUrl;
            softwareViewModel.PageCount = 0;
            softwareViewModel.PageNumber = PageNo;
            softwareViewModel.PageIndex = 1;
            if (softwareViewModel.SoftwareList.Count > 0)
            {
                softwareViewModel.PageCount = (softwareViewModel.SoftwareList[0].TotalCount + PageSize - 1) / PageSize;
            }
            if ((PageNo == FirstPage || PageNo == LastPage) && LastPage >= 5)
            {
                if (PageNo == FirstPage && PageNo != 1)
                {
                    softwareViewModel.PageIndex = FirstPage - 1;
                }
                else if (PageNo == LastPage)
                {
                    if (PageNo == softwareViewModel.PageCount)
                        softwareViewModel.PageIndex = (PageNo - 5) + 1;
                    else
                        softwareViewModel.PageIndex = FirstPage + 1;
                }
            }
            else if (PageNo > LastPage && LastPage >= 5)
            {
                softwareViewModel.PageIndex = (PageNo - 5) + 1;
            }
            else if (PageNo >= FirstPage && PageNo <= LastPage)
            {
                softwareViewModel.PageIndex = FirstPage;
            }

            return PartialView("_SoftwareList", softwareViewModel);
        }
    }
}