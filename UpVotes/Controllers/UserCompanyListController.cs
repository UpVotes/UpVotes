using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Method to return respective view for the controller
        /// </summary>
        /// <returns>Returns view along with the attached model if any</returns>
        public ActionResult UserCompanyList()
        {
            int userID = Convert.ToInt32(Session["UserID"]);
            CompanyViewModel companyViewModel = new CompanyService().GetUserCompanyies(userID, string.Empty);
            Session["CompanyViewModel"] = companyViewModel;
            Session["calledPage"] = "U";
            return View(companyViewModel);
        }

        /// <summary>
        /// Method to get the user created companies.
        /// </summary>
        /// <param name="companyName">Passing company Name. if empty get all compaies created by respective user else get corresponding company.</param>
        /// <returns>Returns view along with the empty company model.</returns>
        public ActionResult UserCompany(string companyName = "")
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

            if (companyViewModel.CompanyList.Count > 0)
            {
                companyViewModel.CompanyList = new List<CompanyEntity>();
                //companyViewModel.CompanyList.RemoveAt(0);
            }

            CompanyEntity newCompany = new CompanyEntity
            {
                CompanyName = companyName,
                TotalEmployees = "0",
                AveragHourlyRate = "0",
                CompanyBranches = new List<CompanyBranchEntity>
            {
                new CompanyBranchEntity()
            }
            };

            companyViewModel.CompanyList.Add(newCompany);

            return View(companyViewModel);
        }

        /// <summary>
        /// Mthod to save the company information.
        /// </summary>
        /// <returnsReturns json result based the status.></returns>
        [ValidateInput(false)]
        public JsonResult SaveCompanyData()
        {
            var jsonData = default(dynamic);
            try
            {
                if (Request.Params["ProfileData"] != null)
                {
                    string AppPath = string.Empty; string fileName = string.Empty; string extension = string.Empty;
                    CompanyEntity company = JObject.Parse(Request.Params["ProfileData"].ToString()).ToObject<CompanyEntity>();
                    company.UserID = Convert.ToInt32(Session["UserID"]);
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

                    UpVotes.Utility.CacheHandler.Clear(company.CompanyName);


                    jsonData = new
                    {
                        IsSuccess = true,
                    };
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
                    Response.Write("<script>var x=window.open('','_self','','');window.opener = null;x.close();</script>");
            }
            catch (Exception ex)
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
            catch (Exception ex)
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
                var jsonResult = default(dynamic);
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
                var jsonResult = default(dynamic);
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

        [ValidateInput(false)]
        public ActionResult CompanyVerificationByUser(string UID, string CID, string COMPID)
        {
            int uID = Convert.ToString(Request.Params["UID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt(Request.Params["UID"]));
            string cID = Request.Params["CID"] == string.Empty ? "XX" : EncryptionAndDecryption.Decrypt(Request.Params["CID"]);
            int compID = Convert.ToString(Request.Params["KID"]) == string.Empty ? 0 : Convert.ToInt32(EncryptionAndDecryption.Decrypt((Request.Params["KID"])));
            bool isUserVerified = new CompanyService().CompanyVerificationByUser(uID, cID, compID);
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
    }
}