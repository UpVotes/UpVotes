﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.Models;
using UpVotes.Utility;

namespace UpVotes.Controllers
{
    public class OverViewAndNewsController : Controller
    {
        private string _webBaseURL = ConfigurationManager.AppSettings["WebBaseURL"].ToString();

        public ActionResult GetOverViewNewsForService(CompanyViewModel companymodel)
        {
            try
            {
                int focusAreaID = 0;
                if (Session["FocusAreaName"] != null)
                {
                    focusAreaID = new FocusAreaService().GetFocusAreaID(Session["FocusAreaName"].ToString());
                }

                OverviewNewsEntity NewsFilter = new OverviewNewsEntity{
                        CategoryID = focusAreaID,
                        IsCompanySoftware = 1,
                        location = companymodel.Location == "0" ? "" : companymodel.Location,
                        SubcategoryID = companymodel.SubFocusArea,
                        CompanySoftwareID = 0
                };

                OverviewAndNewsService newsService = new OverviewAndNewsService();
                OverviewNewsViewModel newsViewModel = newsService.GetCompanySoftwareNews(NewsFilter);

                return PartialView("~/Views/CompanyList/_CompanyNewsList.cshtml", newsViewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult DetailNews(string id)
        {
            try
            {
                Session["calledPage"] = "N";
                OverviewNewsEntity NewsFilter = new OverviewNewsEntity
                {
                    CategoryID = 0,
                    IsCompanySoftware = 0,
                    location = "",
                    SubcategoryID = "",
                    CompanySoftwareID = 0,
                    Title = id
                };

                OverviewAndNewsService newsService = new OverviewAndNewsService();
                OverviewNewsViewModel newsViewModel = newsService.GetCompanySoftwareNews(NewsFilter);
                if (newsViewModel != null && (newsViewModel.OverviewNewsData != null && newsViewModel.OverviewNewsData.Count > 0))
                {
                    newsViewModel.Title = id.Replace("-", " ");
                    //newsViewModel.Title = id.FirstCharToUpper().Replace("-", " ");
                    return View("~/Views/News/NewsDetail.cshtml", newsViewModel);
                }
                else
                {
                    return View("~/Views/Error/PageNotFound.cshtml");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetOverViewNewsForSoftware(SoftwareViewModel softwaremodel)
        {
            try
            {
                int SoftwareCategoryID = 0;
                if (Session["SoftwareCategory"] != null)
                {
                    SoftwareCategoryID = new FocusAreaService().GetSoftwareCategoryID(Session["SoftwareCategory"].ToString());
                }

                OverviewNewsEntity NewsFilter = new OverviewNewsEntity
                {
                    CategoryID = SoftwareCategoryID,
                    IsCompanySoftware = 2,
                    location = "",
                    SubcategoryID ="",
                    CompanySoftwareID = 0
                };

                OverviewAndNewsService newsService = new OverviewAndNewsService();
                OverviewNewsViewModel newsViewModel = newsService.GetCompanySoftwareNews(NewsFilter);

                return PartialView("~/Views/SoftwareList/_SoftwareNewsList.cshtml", newsViewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult CompanyNewsByName(string id)
        {
            try
            {
                Session["calledPage"] = "N";
                OverviewNewsEntity NewsFilter = new OverviewNewsEntity
                {
                    CategoryID = 0,
                    IsCompanySoftware = 1,
                    location = "",
                    SubcategoryID = "",
                    CompanySoftwareID = 0,
                    CompanySoftwareName = id
                };
                ViewBag.IsService = true;
                OverviewAndNewsService newsService = new OverviewAndNewsService();
                OverviewNewsViewModel newsViewModel = newsService.GetCompanySoftwareNewsByName(NewsFilter);
                if (newsViewModel != null && (newsViewModel.OverviewNewsData != null && newsViewModel.OverviewNewsData.Count > 0))
                {
                    newsViewModel.CompanySoftwareName = id;
                    newsViewModel.Title = id.Replace("-", " ");
                    return View("~/Views/AllListPages/AllNewsList.cshtml", newsViewModel);
                }
                else
                {
                    return View("~/Views/Error/PageNotFound.cshtml");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult IsNewsExists(string Title, string WebsiteUrl)
        {
            try
            {
                OverviewNewsEntity NewsFilter = new OverviewNewsEntity
                {
                    Title = Title,
                    WebsiteURL = WebsiteUrl
                };

                OverviewAndNewsService newsService = new OverviewAndNewsService();
                bool newsViewModel = newsService.IsNewsExists(NewsFilter);

                var jsonData = default(dynamic);
                jsonData = new
                {
                    IsSuccess = newsViewModel
                };
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetNewsForm()
        {
            try
            {
                Session["calledPage"] = "N";
                bool isAdmin = (Session["UserObj"] as UserEntity).UserType == 4 ? true : false;                
                if (isAdmin)
                {                    
                    return PartialView("~/Views/Authenticated/Center/AdminNews.cshtml");
                }
                else
                {
                    return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult GetUserNewsForm()
        {
            try
            {
                Session["calledPage"] = "N";
                if (Session["UserDashboardInfo"] != null)
                {
                    DashboardViewModel dashboardObj = new DashboardViewModel();
                    dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                    if (Convert.ToBoolean(dashboardObj.IsUserApproved) && Convert.ToBoolean(dashboardObj.IsAdminApproved))
                    {
                        return PartialView("~/Views/Authenticated/Center/UserNews.cshtml", dashboardObj);
                    }
                    else
                    {
                        return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new EmptyResult(), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            try
            {
                System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() => codeBlock());
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveFileToServer()
        {
            try
            {
                int ServiceRequest_ID = Convert.ToInt32(Request.Params["CompanyID"]);
                HttpPostedFileBase[] postedFile = new HttpPostedFileBase[Request.Files.Count];
                string AppPath = Request.ApplicationPath == "/" ? "" : Request.ApplicationPath;

                bool Completed = ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(300000), () =>
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        postedFile[i] = Request.Files[i];

                        if (postedFile[i] != null && ServiceRequest_ID > 0)
                        {
                            string fileName = postedFile[i].FileName;
                            if (fileName.Contains("\\"))
                            {
                                int lastIndex = fileName.LastIndexOf('\\') + 1;
                                int len = fileName.Length - lastIndex;
                                fileName = fileName.Substring(lastIndex, len);
                            }
                            string SMP = Server.MapPath(AppPath + "/images/News");
                            string Path = SMP + "/" + ServiceRequest_ID;
                            string fullPath = SMP + "/" + ServiceRequest_ID + "/" + fileName;
                            if (System.IO.Directory.Exists(Path))
                            {
                                postedFile[i].SaveAs(fullPath);
                            }
                            else
                            {
                                System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(Path);
                                postedFile[i].SaveAs(fullPath);
                            }
                        }
                        else if (postedFile != null && ServiceRequest_ID == 0)
                        {
                            int UserID = Convert.ToInt32(Session["UserID"]);
                            string TempLoc = "Temp_" + UserID;
                            string fileName = postedFile[i].FileName;

                            if (fileName.Contains("\\"))
                            {
                                int lastIndex = fileName.LastIndexOf('\\') + 1;
                                int len = fileName.Length - lastIndex;
                                fileName = fileName.Substring(lastIndex, len);
                            }

                            if (fileName.Contains("\\"))
                            {
                                int lastIndex = fileName.LastIndexOf('\\') + 1;
                                int len = fileName.Length - lastIndex;
                                fileName = fileName.Substring(lastIndex, len);
                            }
                            string SMP = Server.MapPath(AppPath + "/images/News");
                            string Path = SMP + "/" + TempLoc;
                            string fullPath = SMP + "/" + TempLoc + "/" + fileName;
                            if (System.IO.Directory.Exists(Path))
                            {
                                postedFile[i].SaveAs(fullPath);
                            }
                            else
                            {
                                System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(Path);
                                postedFile[i].SaveAs(fullPath);
                            }
                        }
                    }
                });
                if (Completed)
                    Response.Write("<script>var x=window.open('','_self','','');window.opener = null;x.close();</script>");
            }
            catch (Exception ex)
            {

            }
        }

        [ValidateInput(false)]
        public JsonResult SaveNews()
        {
            var jsonData = default(dynamic);
            try
            {
                if (Request.Params["NewsData"] != null)
                {
                    string AppPath = string.Empty; string fileName = string.Empty; string extension = string.Empty;
                    OverviewNewsEntity news = JObject.Parse(Request.Params["NewsData"].ToString()).ToObject<OverviewNewsEntity>();
                    news.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    
                    if (Request.Files.Count > 0 && Request.Files[0].FileName != string.Empty)
                    {
                        fileName = Request.Files[0].FileName;
                        AppPath = Request.ApplicationPath == "/" ? "" : Request.ApplicationPath;
                        if (fileName.Contains("\\"))
                        {
                            int lastIndex = fileName.LastIndexOf('\\') + 1;
                            int len = fileName.Length - lastIndex;
                            fileName = fileName.Substring(lastIndex, len);
                            fileName = fileName.Replace(" ", "");
                        }

                        extension = System.IO.Path.GetExtension(fileName);
                        news.ImageName = news.Title.RemoveSpecialCharacters() + extension;
                    }

                    int NewsID = new OverviewAndNewsService().SaveNews(news);

                    if (NewsID != 0 && Request.Files.Count > 0 && Request.Files[0].FileName != string.Empty)
                    {
                        //bool isFileUploaded = Helper.FTPFileUpload.UploadFile(Request.Files[0]);
                        string SMP = Server.MapPath(AppPath + "/images/News");
                        string fullPath = SMP + "/" + news.ImageName.Replace(" ", "");
                        if (System.IO.Directory.Exists(SMP))
                        {
                            Request.Files[0].SaveAs(fullPath);
                        }
                        else
                        {
                            System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(SMP);
                            Request.Files[0].SaveAs(fullPath);
                        }                        
                    }

                    if (NewsID > 0)
                    {
                        if (CacheHandler.Exists(news.CompanySoftwareName.Replace(" ", "-").ToLower()))
                        {
                            CacheHandler.Clear(news.CompanySoftwareName.Replace(" ", "-").ToLower());
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
            }
            catch (Exception ex)
            {
                jsonData = new
                {
                    IsSuccess = false
                };
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
                
        [ValidateInput(false)]
        public JsonResult SaveUserNews()
        {
            var jsonData = default(dynamic);
            try
            {
                if (Request.Params["NewsData"] != null)
                {
                    string AppPath = string.Empty; string fileName = string.Empty; string extension = string.Empty;
                    OverviewNewsEntity news = JObject.Parse(Request.Params["NewsData"].ToString()).ToObject<OverviewNewsEntity>();
                    news.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    news.Description = Utility.Utility.ExtractText(news.Description);
                    if (Request.Files.Count > 0 && Request.Files[0].FileName != string.Empty)
                    {
                        fileName = Request.Files[0].FileName;
                        AppPath = Request.ApplicationPath == "/" ? "" : Request.ApplicationPath;
                        if (fileName.Contains("\\"))
                        {
                            int lastIndex = fileName.LastIndexOf('\\') + 1;
                            int len = fileName.Length - lastIndex;
                            fileName = fileName.Substring(lastIndex, len);
                            fileName = fileName.Replace(" ", "");
                        }

                        extension = System.IO.Path.GetExtension(fileName);
                        news.ImageName = news.Title.RemoveSpecialCharacters() + extension;
                    }

                    int NewsID = new OverviewAndNewsService().SaveUserNews(news);

                    if (NewsID != 0 && Request.Files.Count > 0 && Request.Files[0].FileName != string.Empty)
                    {
                        //bool isFileUploaded = Helper.FTPFileUpload.UploadFile(Request.Files[0]);
                        string SMP = Server.MapPath(AppPath + "/images/News");
                        string fullPath = SMP + "/" + news.ImageName.Replace(" ", "");
                        if (System.IO.Directory.Exists(SMP))
                        {
                            Request.Files[0].SaveAs(fullPath);
                        }
                        else
                        {
                            System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(SMP);
                            Request.Files[0].SaveAs(fullPath);
                        }
                        if (CacheHandler.Exists(news.CompanySoftwareName.Replace(" ", "-").ToLower()))
                        {
                            CacheHandler.Clear(news.CompanySoftwareName.Replace(" ", "-").ToLower());
                        }
                    }

                    if (NewsID > 0)
                    {
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
            }
            catch (Exception ex)
            {
                jsonData = new
                {
                    IsSuccess = false
                };
            }

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SoftwareNewsByName(string id)
        {
            try
            {
                Session["calledPage"] = "N";
                OverviewNewsEntity NewsFilter = new OverviewNewsEntity
                {
                    CategoryID = 0,
                    IsCompanySoftware = 2,
                    location = "",
                    SubcategoryID = "",
                    CompanySoftwareID = 0,
                    CompanySoftwareName = id
                };
                ViewBag.IsService = false;
                OverviewAndNewsService newsService = new OverviewAndNewsService();
                OverviewNewsViewModel newsViewModel = newsService.GetCompanySoftwareNewsByName(NewsFilter);
                if (newsViewModel != null && (newsViewModel.OverviewNewsData != null && newsViewModel.OverviewNewsData.Count > 0))
                {
                    newsViewModel.CompanySoftwareName = id;
                    newsViewModel.Title = id.Replace("-", " ");
                    return View("~/Views/AllListPages/AllNewsList.cshtml", newsViewModel);
                }
                else
                {
                    return View("~/Views/Error/PageNotFound.cshtml");
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}