using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UpVotes.Business;
using UpVotes.BusinessEntities.Entities;
using UpVotes.BusinessEntities.Helper;
using UpVotes.Models;

namespace UpVotes.Controllers
{
    public class UserCompanyListController : Controller
    {
        public ActionResult UserDashboard()
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            Session["calledPage"] = "U";
            if (userID > 0)
            {
                UserService userService = new UserService();
                UserEntity userobj = new UserEntity
                {
                    UserID = userID
                };
                DashboardViewModel dashboardObj = new DashboardViewModel();
                dashboardObj = userService.UserDashboardInfo(userobj);
                dashboardObj.section = "";
                if (Request.QueryString["section"] != null)
                {
                    dashboardObj.section = Request.QueryString["section"].ToString();
                }
                Session["UserDashboardInfo"] = dashboardObj;
                return View("~/Views/Authenticated/Dashboard/UserDashboard.cshtml", dashboardObj);
            }
            else
            {
                return RedirectToAction("HomePage", "Home");
            }
        }

        public ActionResult UserSoftware()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            bool isAdmin = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false;
            SoftwareViewModel softwareViewModel = new SoftwareService().GetUserSoftwares(userId, isAdmin);
            softwareViewModel.IsAdmin = isAdmin;

            if (softwareViewModel.SoftwareList.Any())
            {
                return PartialView("~/Views/Authenticated/Center/UserSoftwareList.cshtml", softwareViewModel);
            }
            else
            {
                return PartialView("~/Views/Authenticated/Center/UserSoftwareList.cshtml",
                    new SoftwareViewModel() { SoftwareList = new List<SoftwareEntity>() });
            }

        }

        public ActionResult GetUserPortfolioForm()
        {
            Session["calledPage"] = "N";
            if (Session["UserDashboardInfo"] != null)
            {
                DashboardViewModel dashboardObj = new DashboardViewModel();
                dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                if (Convert.ToBoolean(dashboardObj.IsUserApproved) && Convert.ToBoolean(dashboardObj.IsAdminApproved))
                {
                    CompanyFilterEntity companyportfolioFilter = new CompanyFilterEntity
                    {
                        CompanyID = Convert.ToInt32(dashboardObj.CompanySoftwareID),
                        Rows = 0
                    };
                    List<CompanyPortFolioEntity> portfolioObj = new CompanyService().GetCompanyPortfolioByID(companyportfolioFilter);
                    //if (portfolioObj.Count > 0)
                    //{
                    return PartialView("~/Views/Authenticated/Center/UserPortfolioList.cshtml", portfolioObj);
                    //}
                    //else
                    //{
                    //    portfolioObj = new List<CompanyPortFolioEntity>();
                    //    return PartialView("~/Views/Authenticated/Center/UserPortfolio.cshtml", portfolioObj);                        
                    //}


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

        public ActionResult GetAllTeamMembers()
        {
            Session["calledPage"] = "N";
            if (Session["UserDashboardInfo"] != null)
            {
                DashboardViewModel dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                if (dashboardObj != null && (Convert.ToBoolean(dashboardObj.IsUserApproved) && Convert.ToBoolean(dashboardObj.IsAdminApproved)))
                {
                    List<TeamMemebersEntity> teamMembersViewModel = new TeamMembersService().GetAllTeamMembers(dashboardObj.CompanySoftwareID, dashboardObj.IsService);

                    return PartialView("~/Views/Authenticated/Center/TeamMembersList.cshtml", teamMembersViewModel);
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
        
        public ActionResult TeamMemberForm()
        {
            if (Session["UserDashboardInfo"] != null)
            {
                int memberId = Convert.ToInt32(Request.Params["memberId"]);
                DashboardViewModel dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                if (dashboardObj != null && (Convert.ToBoolean(dashboardObj.IsUserApproved) && Convert.ToBoolean(dashboardObj.IsAdminApproved)))
                {
                    if (memberId > 0)
                    {
                        TeamMemebersEntity teamMember = new TeamMembersService().GetTeamMember(memberId, dashboardObj.IsService);
                        teamMember.CompanyId = dashboardObj.IsService ? dashboardObj.CompanySoftwareID : null;
                        teamMember.SoftwareId = dashboardObj.IsSoftware ? dashboardObj.CompanySoftwareID : null;
                        teamMember.CompanyOrSoftwareName = dashboardObj.CompanySoftwareName;
                        teamMember.IsService = dashboardObj.IsService;
                        return PartialView("~/Views/Authenticated/Center/TeamMember.cshtml", teamMember);
                    }
                    else
                    {
                        TeamMemebersEntity teamMemeber = new TeamMemebersEntity
                        {
                            MemberId = 0,
                            CompanyId = dashboardObj.IsService ? dashboardObj.CompanySoftwareID : null,
                            SoftwareId = dashboardObj.IsSoftware ? dashboardObj.CompanySoftwareID : null,
                            CompanyOrSoftwareName = dashboardObj.CompanySoftwareName,
                            IsService = dashboardObj.IsService,
                        };

                        return PartialView("~/Views/Authenticated/Center/TeamMember.cshtml", teamMemeber);
                    }
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

        public ActionResult SaveTeamMembers()
        {
            dynamic jsonData = default(dynamic);
            try
            {
                string appPath = string.Empty;
                if (Request.Params["TeamMemberData"] != null)
                {
                    string extension = string.Empty;
                    TeamMemebersEntity teamMembersInfo = JObject.Parse(Request.Params["TeamMemberData"].ToString()).ToObject<TeamMemebersEntity>();
                    DashboardViewModel dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                    if (dashboardObj != null)
                    {
                        teamMembersInfo.IsService = dashboardObj.IsService;
                        teamMembersInfo.CompanyId = dashboardObj.IsService ? dashboardObj.CompanySoftwareID : null;
                        teamMembersInfo.SoftwareId = dashboardObj.IsSoftware ? dashboardObj.CompanySoftwareID : null;
                        teamMembersInfo.CompanyOrSoftwareName = dashboardObj.CompanySoftwareName;

                        if (Request.Files.Count > 0 && Request.Files[0]?.FileName != string.Empty)
                        {
                            string fileName = Request.Files[0]?.FileName;
                            appPath = Request.ApplicationPath == "/" ? "" : Request.ApplicationPath;
                            if (fileName != null && fileName.Contains("\\"))
                            {
                                int lastIndex = fileName.LastIndexOf('\\') + 1;
                                int len = fileName.Length - lastIndex;
                                fileName = fileName.Substring(lastIndex, len);
                                fileName = fileName.Replace(" ", "");
                            }

                            extension = System.IO.Path.GetExtension(fileName);
                            teamMembersInfo.PictureName = extension;
                        }

                        int teamMemberId = new TeamMembersService().SaveTeamMembers(teamMembersInfo);

                        if (teamMemberId != 0 && Request.Files.Count > 0 && Request.Files[0]?.FileName != string.Empty)
                        {
                            string SMP = teamMembersInfo.IsService
                                ? Server.MapPath(appPath + "/images/CompanyMembers/" +
                                                 Convert.ToString(teamMembersInfo.CompanyId))
                                : Server.MapPath(appPath + "/images/SoftwareMembers/" +
                                                 Convert.ToString(teamMembersInfo.SoftwareId));

                            string fullPath = SMP + "/" + Convert.ToString(teamMemberId) + extension;
                            if (System.IO.Directory.Exists(SMP))
                            {
                                FileInfo file = new FileInfo(fullPath);
                                if (file.Exists)
                                {
                                    file.Delete();
                                }

                                Request.Files[0]?.SaveAs(fullPath);
                            }
                            else
                            {
                                System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(SMP);
                                Request.Files[0]?.SaveAs(fullPath);
                            }

                            if (dashboardObj != null &&
                                Utility.CacheHandler.Exists(
                                    dashboardObj.CompanySoftwareName.ToLower().Replace(" ", "-")))
                            {
                                UpVotes.Utility.CacheHandler.Clear(dashboardObj.CompanySoftwareName.ToLower()
                                    .Replace(" ", "-"));
                            }
                        }
                    }

                    jsonData = new
                    {
                        IsSuccess = true,
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

        public ActionResult DeleteTeamMember(int teamMemberId, string imageUrl)
        {
            dynamic jsonData = default(dynamic);
            try
            {
                DashboardViewModel dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                bool deleted = new TeamMembersService().DeleteTeamMember(teamMemberId, dashboardObj != null && dashboardObj.IsService);
                if (deleted)
                {
                    string appPath = Request.ApplicationPath == "/" ? "" : Request.ApplicationPath;
                    string fullPath = Server.MapPath(appPath + imageUrl);
                    FileInfo file = new FileInfo(fullPath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    if (dashboardObj != null && Utility.CacheHandler.Exists(dashboardObj.CompanySoftwareName.ToLower().Replace(" ", "-")))
                    {
                        UpVotes.Utility.CacheHandler.Clear(dashboardObj.CompanySoftwareName.ToLower().Replace(" ", "-"));
                    }

                    jsonData = new
                    {
                        IsSuccess = true,
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

        public ActionResult DeletePortFolio(int portfolioID, string ImageUrl)
        {
            dynamic jsonData = default(dynamic);
            try
            {
                int deleted = new CompanyService().DeletePortFolio(portfolioID);

                if (deleted > 0)
                {
                    string AppPath = string.Empty;
                    AppPath = Request.ApplicationPath == "/" ? "" : Request.ApplicationPath;
                    string fullPath = Server.MapPath(AppPath + "/images/CompanyPortfolio/" + ImageUrl);
                    FileInfo file = new FileInfo(fullPath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    DashboardViewModel dashboardObj = new DashboardViewModel();
                    dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                    if (Utility.CacheHandler.Exists(dashboardObj.CompanySoftwareName.ToLower().Replace(" ", "-")))
                    {
                        UpVotes.Utility.CacheHandler.Clear(dashboardObj.CompanySoftwareName.ToLower().Replace(" ", "-"));
                    }

                    jsonData = new
                    {
                        IsSuccess = true,
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

        [ValidateInput(false)]
        public ActionResult SaveCompanyPortfolio()
        {

            dynamic jsonData = default(dynamic);
            try
            {
                if (Request.Params["PortfolioData"] != null)
                {
                    string AppPath = string.Empty; string fileName = string.Empty; string extension = string.Empty;
                    CompanyPortFolioEntity portfolioInfo = JObject.Parse(Request.Params["PortfolioData"].ToString()).ToObject<CompanyPortFolioEntity>();
                    portfolioInfo.CreatedBy = Convert.ToInt32(Session["UserID"]);

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
                        portfolioInfo.ImageURL = extension;
                    }

                    int portfolioID = new CompanyService().SavePortFolio(portfolioInfo);

                    if (portfolioID != 0 && Request.Files.Count > 0 && Request.Files[0].FileName != string.Empty)
                    {
                        string SMP = Server.MapPath(AppPath + "/images/CompanyPortfolio/" + Convert.ToString(portfolioInfo.CompanyID));
                        string fullPath = SMP + "/" + Convert.ToString(portfolioID) + extension;
                        if (System.IO.Directory.Exists(SMP))
                        {
                            Request.Files[0].SaveAs(fullPath);
                        }
                        else
                        {
                            System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(SMP);
                            Request.Files[0].SaveAs(fullPath);
                        }

                        DashboardViewModel dashboardObj = new DashboardViewModel();
                        dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                        if (Utility.CacheHandler.Exists(dashboardObj.CompanySoftwareName.ToLower().Replace(" ", "-")))
                        {
                            UpVotes.Utility.CacheHandler.Clear(dashboardObj.CompanySoftwareName.ToLower().Replace(" ", "-"));
                        }
                    }

                    jsonData = new
                    {
                        IsSuccess = true,
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
        public ActionResult CreateNewPortFolioForm(int portfolioID)
        {
            if (Session["UserDashboardInfo"] != null)
            {
                DashboardViewModel dashboardObj = new DashboardViewModel();
                dashboardObj = (Session["UserDashboardInfo"] as DashboardViewModel);
                if (Convert.ToBoolean(dashboardObj.IsUserApproved) && Convert.ToBoolean(dashboardObj.IsAdminApproved))
                {
                    CompanyPortFolioEntity portfolioObj = new CompanyPortFolioEntity();
                    if (portfolioID > 0)
                    {
                        CompanyPortFolioEntity portfolioFilter = new CompanyPortFolioEntity
                        {
                            CompanyPortFolioID = portfolioID
                        };
                        portfolioObj = new CompanyService().GetPortfolioInfoByID(portfolioFilter);
                        portfolioObj.CompanyName = dashboardObj.CompanySoftwareName;
                        portfolioObj.CompanyID = Convert.ToInt32(dashboardObj.CompanySoftwareID);
                    }
                    else
                    {
                        portfolioObj.CompanyName = dashboardObj.CompanySoftwareName;
                        portfolioObj.CompanyID = Convert.ToInt32(dashboardObj.CompanySoftwareID);
                        portfolioObj.CompanyPortFolioID = 0;
                        portfolioObj.Description = "";
                        portfolioObj.ImageURL = "";
                        portfolioObj.Title = "";
                    }
                    return PartialView("~/Views/Authenticated/Center/UserPortfolio.cshtml", portfolioObj);

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

        public ActionResult CreateNewSoftwareAdmin()
        {
            SoftwareEntity softwareEntity = new SoftwareEntity()
            {
                SoftwareName = string.Empty,
                LogoName = string.Empty,
                CreatedBy = Convert.ToInt32(Session["UserID"]),
                SoftwareCatagoryIds = string.Empty
            };

            SoftwareViewModel softwareViewModel = new SoftwareViewModel { SoftwareList = new List<SoftwareEntity> { softwareEntity } };
            softwareViewModel.IsAdmin = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false;
            softwareViewModel.LoggedInUser = Convert.ToInt32(Session["UserID"]);

            return PartialView("~/Views/Authenticated/Center/UserSoftware.cshtml", softwareViewModel);
        }

        public ActionResult UserCompany(string companyName = "")
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            CompanyViewModel companyViewModel = null;
            companyViewModel = new CompanyService().GetUserCompanyies(userId, string.Empty);

            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.IsAdmin = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false;
                return PartialView("~/Views/Authenticated/Center/UserCompanyList.cshtml", companyViewModel);
            }
            else
            {
                CompanyEntity newCompany = new CompanyEntity
                {
                    CompanyName = companyName,
                    TotalEmployees = "0",
                    AveragHourlyRate = "0",
                    CreatedBy = companyName != string.Empty ? companyViewModel.CompanyList.Where(a => a.CompanyName.Trim().ToUpper() == companyName.Trim().ToUpper()).Select(a => a.CreatedBy).FirstOrDefault() : Convert.ToInt32(Session["UserID"]),
                    IsAdminUser = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false,
                    CompanyBranches = new List<CompanyBranchEntity>
                {
                    new CompanyBranchEntity()
                }
                };

                if (companyViewModel.CompanyList.Count > 0)
                {
                    companyViewModel.CompanyList = new List<CompanyEntity>();
                }

                companyViewModel.CompanyList.Add(newCompany);

                return PartialView("~/Views/Authenticated/Center/UserCompany.cshtml", companyViewModel);
            }

        }

        public ActionResult GetClaimListings()
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            CompanyViewModel companyViewModel = null;

            companyViewModel = new CompanyService().GetClaimListingsForApproval(userID);
            companyViewModel.IsAdmin = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false;

            return PartialView("~/Views/Authenticated/Center/ClaimList.cshtml", companyViewModel);
        }

        public ActionResult CreateNewCompanyAdmin()
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            CompanyViewModel companyViewModel = null;

            companyViewModel = new CompanyService().GetUserCompanyies(userID, string.Empty);
            CompanyEntity newCompany = new CompanyEntity
            {
                CompanyName = "",
                TotalEmployees = "0",
                AveragHourlyRate = "0",
                CreatedBy = Convert.ToInt32(Session["UserID"]),
                IsAdminUser = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false,
                CompanyBranches = new List<CompanyBranchEntity>
                {
                    new CompanyBranchEntity()
                }
            };

            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.CompanyList = new List<CompanyEntity>();
            }

            companyViewModel.CompanyList.Add(newCompany);

            return PartialView("~/Views/Authenticated/Center/UserCompany.cshtml", companyViewModel);
        }

        /// <summary>
        /// Method to get the user created companies.
        /// </summary>
        /// <param name="companyName">Passing company Name. if empty get all compaies created by respective user else get corresponding company.</param>
        /// <returns>Returns view along with the empty company model.</returns>
        public ActionResult GetUserCompanyDetail(string companyName = "")
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            CompanyViewModel companyViewModel = null;
            if (Session["CompanyViewModel"] != null)
            {
                companyViewModel = (CompanyViewModel)Session["CompanyViewModel"];
            }
            else
            {
                companyViewModel = new CompanyService().GetUserCompanyies(userID, string.Empty);
            }

            CompanyEntity newCompany = new CompanyEntity
            {
                CompanyName = companyName,
                TotalEmployees = "0",
                AveragHourlyRate = "0",
                CreatedBy = companyName != string.Empty ? companyViewModel.CompanyList.Where(a => a.CompanyName.Trim().ToUpper() == companyName.Trim().ToUpper()).Select(a => a.CreatedBy).FirstOrDefault() : Convert.ToInt32(Session["UserID"]),
                IsAdminUser = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false,
                CompanyBranches = new List<CompanyBranchEntity>
                {
                    new CompanyBranchEntity()
                }
            };

            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.CompanyList = new List<CompanyEntity>();
            }

            companyViewModel.CompanyList.Add(newCompany);

            return PartialView("~/Views/Authenticated/Center/UserCompany.cshtml", companyViewModel);
        }

        public ActionResult GetUserSoftwareDetail(string softwareName)
        {
            SoftwareViewModel softwareViewModel = new SoftwareService().GetUserSoftwareByName(softwareName);
            softwareViewModel.IsAdmin = ((UserEntity)Session["UserObj"]).UserType == 4 ? true : false;
            softwareViewModel.LoggedInUser = Convert.ToInt32(Session["UserID"]);
            return PartialView("~/Views/Authenticated/Center/UserSoftware.cshtml", softwareViewModel);
        }

        /// <summary>
        /// Mthod to save the company information.
        /// </summary>
        /// <returns Returns json result based the status.></returns>
        [ValidateInput(false)]
        public JsonResult SaveCompanyData()
        {
            dynamic jsonData = default(dynamic);
            try
            {
                if (Request.Params["ProfileData"] != null)
                {
                    string AppPath = string.Empty; string fileName = string.Empty; string extension = string.Empty;
                    CompanyEntity company = JObject.Parse(Request.Params["ProfileData"].ToString()).ToObject<CompanyEntity>();
                    company.LoggedInUser = Convert.ToInt32(Session["UserID"]);
                    company.CreatedBy = company.CompanyID == 0 ? company.LoggedInUser : company.CreatedBy;
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
                        company.LogoName = company.CompanyName.Replace(" ", "") + extension;
                    }

                    int companyID = new CompanyService().SaveCompany(company);

                    if (companyID != 0 && Request.Files.Count > 0 && Request.Files[0].FileName != string.Empty)
                    {
                        //bool isFileUploaded = Helper.FTPFileUpload.UploadFile(Request.Files[0]);
                        string SMP = Server.MapPath(AppPath + "/images/CompanyLogos");
                        string fullPath = SMP + "/" + company.CompanyName.Replace(" ", "") + extension;
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

                    UpVotes.Utility.CacheHandler.Clear(company.CompanyName.Trim().ToLower().Replace(" ", "-"));


                    jsonData = new
                    {
                        IsSuccess = true,
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

        [ValidateInput(false)]
        public JsonResult SaveSoftwareDetails()
        {
            dynamic jsonData = default(dynamic);
            try
            {
                if (Request.Params["ProfileData"] != null)
                {
                    string appPath = string.Empty;
                    string extension = string.Empty;
                    SoftwareEntity software = JObject.Parse(Request.Params["ProfileData"].ToString()).ToObject<SoftwareEntity>();
                    software.LoggedInUserName = Convert.ToInt32(Session["UserID"]);
                    if (Request.Files.Count > 0 && Request.Files[0]?.FileName != string.Empty)
                    {
                        string fileName = Request.Files[0]?.FileName;
                        appPath = Request.ApplicationPath == "/" ? "" : Request.ApplicationPath;
                        if (fileName != null && fileName.Contains("\\"))
                        {
                            int lastIndex = fileName.LastIndexOf('\\') + 1;
                            int len = fileName.Length - lastIndex;
                            fileName = fileName.Substring(lastIndex, len);
                            fileName = fileName.Replace(" ", "");
                        }

                        extension = System.IO.Path.GetExtension(fileName);
                        software.LogoName = software.SoftwareName.Replace(" ", "") + extension;
                    }

                    int softwareID = new SoftwareService().SaveSoftwareDetails(software);

                    if (softwareID != 0 && Request.Files.Count > 0 && Request.Files[0]?.FileName != string.Empty)
                    {
                        //bool isFileUploaded = Helper.FTPFileUpload.UploadFile(Request.Files[0]);
                        string smp = Server.MapPath(appPath + "/images/SoftwareLogos");
                        string fullPath = smp + "/" + software.SoftwareName.Replace(" ", "") + extension;
                        if (System.IO.Directory.Exists(smp))
                        {
                            Request.Files[0]?.SaveAs(fullPath);
                        }
                        else
                        {
                            System.IO.DirectoryInfo di = System.IO.Directory.CreateDirectory(smp);
                            Request.Files[0]?.SaveAs(fullPath);
                        }
                    }

                    UpVotes.Utility.CacheHandler.Clear(software.SoftwareName.Trim().ToLower().Replace(" ", "-"));


                    jsonData = new
                    {
                        IsSuccess = true,
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

        /// <summary>
        /// Method to get the user company data in edit mode.
        /// </summary>
        /// <param name="companyName">Passing company name.</param>
        /// <returns>Returns company data</returns>
        public ActionResult GetUserCompanyData(string companyName)
        {
            CompanyViewModel userCompanyData = new CompanyService().GetUserCompanyies(Convert.ToInt32(Session["UserID"]), companyName);

            if (companyName != string.Empty)
            {
                var jsonData = new
                {
                    companyData = userCompanyData.CompanyList[0],
                    isAdminUser = (Session["UserObj"] as UserEntity).UserType == 4 ? true : false
                };

                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View("~/Views/UserCompanyList/UserCompany.cshtml", userCompanyData);
            }
        }

        /// <summary>
        /// Method to save the file in the server.
        /// </summary>
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
                            string SMP = Server.MapPath(AppPath + "/images/CompanyLogos");
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
                            string SMP = Server.MapPath(AppPath + "/images/CompanyLogos");
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
                {
                    Response.Write("<script>var x=window.open('','_self','','');window.opener = null;x.close();</script>");
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Method to check the attachment whether exists or not.
        /// </summary>
        /// <returns></returns>
        public string CheckAttachment()
        {
            try
            {
                string FileName = BasicDecryptString(Request.Params["FileName"].ToString());
                int CompanyID = Convert.ToInt32(Request.Params["CompanyID"]);
                JObject Jobj = JObject.Parse(Request.Params["jObjectFileNames"].ToString());
                string returnValue = string.Empty;

                int Count = Jobj["rows"].Count();
                for (int i = 0; i < Count; i++)
                {
                    FileName = ((dynamic)Jobj["rows"].ElementAt(i)).Name.Value;
                    FileName = BasicDecryptString(FileName);
                    if (CompanyID != 0)
                    {
                        if (FileName.Contains("\\"))
                        {
                            int lastIndex = FileName.LastIndexOf('\\') + 1;
                            int len = FileName.Length - lastIndex;
                            FileName = FileName.Substring(lastIndex, len);
                        }
                    }
                    else if (CompanyID == 0)
                    {
                        if (FileName.Contains("\\"))
                        {
                            int lastIndex = FileName.LastIndexOf('\\') + 1;
                            int len = FileName.Length - lastIndex;
                            FileName = FileName.Substring(lastIndex, len);
                        }
                    }

                    if (returnValue == "Yes")
                    {
                        return "Yes";
                    }
                }
            }
            catch (Exception)
            {

            }
            return "No";
        }

        /// <summary>
        /// Method to provide time limit for the execution.
        /// </summary>
        /// <param name="timeSpan"></param>
        /// <param name="codeBlock"></param>
        /// <returns></returns>
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

        public static string BasicDecryptString(string str)
        {
            if (str.IndexOf("%lthash%") != -1)
            {
                str = str.Replace("%lthash%", "&#");
            }

            return System.Web.HttpUtility.HtmlDecode(Uri.UnescapeDataString(str));
        }

        public JsonResult GetCountry()
        {
            try
            {
                dynamic jsonResult = default(dynamic);
                List<CountryEntity> countryList = new CompanyService().GetCountry();
                jsonResult = new
                {
                    countryList
                };

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetStates(int countryID)
        {
            try
            {
                dynamic jsonResult = default(dynamic);
                List<StateEntity> statesList = new CompanyService().GetStates(countryID);
                jsonResult = new
                {
                    statesList
                };

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetSubFocusAreaByFocusID(int focusAreaID)
        {
            try
            {
                dynamic jsonResult = default(dynamic);
                List<SubFocusAreaEntity> subfocusList = new CompanyService().GetSubFocusAreaByFocusID(focusAreaID);
                jsonResult = new
                {
                    subfocusList
                };

                return Json(jsonResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ValidateInput(false)]
        public ActionResult CompanyVerificationByUser(string UID, string CID, string COMPID)
        {
            int uID = Convert.ToString(Request.Params["UID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt(Request.Params["UID"]));
            string cID = Request.Params["CID"] == string.Empty ? "XX" : EncryptionAndDecryption.Decrypt(Request.Params["CID"]);
            int compID = Convert.ToString(Request.Params["KID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt((Request.Params["KID"])));
            bool isUserVerified = new CompanyService().CompanyVerificationByUser(uID, cID, compID);
            if (Session["calledPage"] == null)
            {
                Session["calledPage"] = "H";
            }
            return View("~/Views/UserCompanyList/UserVerification.cshtml", isUserVerified);
        }

        [ValidateInput(false)]
        public ActionResult CompanyClaimVerificationByUser(string CID, string WID, string KID)
        {
            int cID = Convert.ToString(Request.Params["CID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt(Request.Params["CID"]));
            string wID = Request.Params["WID"] == string.Empty ? "XX" : EncryptionAndDecryption.Decrypt(Request.Params["WID"]);
            int compID = Convert.ToString(Request.Params["KID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt((Request.Params["KID"])));
            ClaimApproveRejectListingRequest claimListingRequest = new ClaimApproveRejectListingRequest
            {
                ClaimListingID = cID,
                companyID = compID,
                IsUserVerify = true,
                Email = wID
            };
            bool isUserVerified = true;
            if (Session["calledPage"] == null)
            {
                Session["calledPage"] = "H";
            }
            string message = new CompanyService().ClaimListing(claimListingRequest);
            if (message != "Successfully Claimed")
            {
                isUserVerified = false;
            }
            return View("~/Views/UserCompanyList/UserVerification.cshtml", isUserVerified);
        }

        [ValidateInput(false)]
        public ActionResult SoftwareClaimVerificationByUser(string CID, string WID, string KID)
        {
            int cID = Convert.ToString(Request.Params["CID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt(Request.Params["CID"]));
            string wID = Request.Params["WID"] == string.Empty ? "XX" : EncryptionAndDecryption.Decrypt(Request.Params["WID"]);
            int softID = Convert.ToString(Request.Params["KID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt((Request.Params["KID"])));
            ClaimApproveRejectListingRequest claimListingRequest = new ClaimApproveRejectListingRequest
            {
                ClaimListingID = cID,
                softwareID = softID,
                IsUserVerify = true,
                Email = wID
            };
            bool isUserVerified = true;
            if (Session["calledPage"] == null)
            {
                Session["calledPage"] = "H";
            }
            string message = new SoftwareService().ClaimSoftwareListing(claimListingRequest);
            if (message != "Successfully Claimed")
            {
                isUserVerified = false;
            }
            return View("~/Views/UserCompanyList/UserVerification.cshtml", isUserVerified);
        }

        public ActionResult UpdateRejectionComments()
        {
            if (Request.Params["companyRejectComments"] != null)
            {
                JObject jobj = JObject.Parse(Request.Params["companyRejectComments"]);
                CompanyRejectComments companyRejectComments = jobj.ToObject<CompanyRejectComments>();
                UserEntity loggedInUser = (UserEntity)Session["UserObj"];
                companyRejectComments.RejectedBy = loggedInUser.FirstName + " " + loggedInUser.LastName;
                bool isRejectCommentsUpdated = new CompanyService().UpdateRejectionComments(companyRejectComments);
                return View("~/Views/UserCompanyList/UserCompany.cshtml", companyRejectComments.CompanyName);
            }

            return null;
        }

        public string AdminClaimApprove(int claimlistingID, int companyID, string Rejectioncomment, string Email, string CompanyName, string Type)
        {
            string message = new CompanyService().AdminApproveRejectForClaim(Convert.ToInt32(Session["UserID"]), claimlistingID, companyID, true, Rejectioncomment, Email, CompanyName, Type);
            if (Utility.CacheHandler.Exists(CompanyName.ToLower().Replace(" ", "-")))
            {
                Utility.CacheHandler.Clear(CompanyName.ToLower().Replace(" ", "-"));
            }
            return message;
        }

        public string AdminClaimReject(int claimlistingID, int companyID, string Rejectioncomment, string Email, string CompanyName, string Type)
        {
            string message = new CompanyService().AdminApproveRejectForClaim(Convert.ToInt32(Session["UserID"]), claimlistingID, companyID, false, Rejectioncomment, Email, CompanyName, Type);

            return message;
        }

        [ValidateInput(false)]
        public ActionResult SoftwareVerificationByUser()
        {
            int uId = Convert.ToString(Request.Params["UID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt(Request.Params["UID"]));
            string cId = Request.Params["CID"] == string.Empty ? "XX" : EncryptionAndDecryption.Decrypt(Request.Params["CID"]);
            int softId = Convert.ToString(Request.Params["KID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt((Request.Params["KID"])));
            bool isUserVerified = new SoftwareService().SoftwareVerificationByUser(uId, cId, softId);
            if (Session["calledPage"] == null)
            {
                Session["calledPage"] = "H";
            }
            return View("~/Views/UserCompanyList/UserVerification.cshtml", isUserVerified);
        }

        public JsonResult DeleteCompany(int companyId)
        {
            string message = new CompanyService().DeleteCompany(companyId);
            dynamic jsonResult = default(dynamic);
            jsonResult = new
            {
                message
            };

            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}