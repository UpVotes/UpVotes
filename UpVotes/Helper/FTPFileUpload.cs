using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace UpVotes.Helper
{
    public class FTPFileUpload
    {
        public static bool UploadFile(string fileName)
        {
            try
            {
                string ftpFileUploadPath = ConfigurationManager.AppSettings["FTPFileUploadPath"] + "CompanyLogos";
                string ftpFileUploadUserName = ConfigurationManager.AppSettings["FTPFileUploadUserName"];
                string ftpFileUploadPassword = ConfigurationManager.AppSettings["FTPFileUploadPassword"];


                string fullPath = Path.Combine(ftpFileUploadPath, fileName.Replace(" ", ""));

                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ftpFileUploadUserName, ftpFileUploadPassword);

                // Copy the contents of the file to the request stream.
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(fileName))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {

                }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        internal static bool UploadFile(HttpPostedFileBase httpPostedFileBase)
        {
            try
            {
                string ftpFileUploadPath = ConfigurationManager.AppSettings["FTPFileUploadPath"] + "CompanyLogos";
                string ftpFileUploadUserName = ConfigurationManager.AppSettings["FTPFileUploadUserName"];
                string ftpFileUploadPassword = ConfigurationManager.AppSettings["FTPFileUploadPassword"];


                string fullPath = Path.Combine(ftpFileUploadPath, httpPostedFileBase.FileName.Replace(" ", ""));

                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(ftpFileUploadUserName, ftpFileUploadPassword);

                // Copy the contents of the file to the request stream.
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(httpPostedFileBase.InputStream))
                {
                    fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {

                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class EmailHelper
    {
        private static bool SendEmail(string emailBody)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {                              
                    using (MailMessage message = new MailMessage())
                    {
                        message.IsBodyHtml = true;
                        message.BodyEncoding = System.Text.Encoding.UTF8;
                        message.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["AdminEmail"], System.Configuration.ConfigurationManager.AppSettings["DomainDisplayName"]);
                        message.To.Add(new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailTo"]));
                        GetEmailCc("support@upvotes.co; puneethm@hotmail.com", message);
                        message.Bcc.Add(new MailAddress("jayanth01011985@gmail.com"));
                        message.Subject = "Error at Upvotes portal";
                        message.SubjectEncoding = System.Text.Encoding.UTF8;
                        message.Priority = MailPriority.High;
                        message.Body = emailBody;

                        smtpClient.Host = "smtpout.secureserver.net";
                        smtpClient.Port = 80;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.EnableSsl = false;
                        System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["AdminEmail"], System.Configuration.ConfigurationManager.AppSettings["AdminPassword"]);
                        smtpClient.Credentials = credentials;
                        smtpClient.Send(message);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void GetEmailCc(string emailCC, MailMessage message)
        {
            if (!string.IsNullOrEmpty(emailCC))
            {
                foreach (string email in emailCC.Split(';'))
                {
                    message.CC.Add(new MailAddress(email.Trim()));
                }
            }
        }

        public static void SendErrorEmail(Exception ex)
        {
            System.Text.StringBuilder errorMessageSb = new System.Text.StringBuilder();
            if(ex.InnerException == null)
            {
                errorMessageSb.Append("<p>Error Message :- " + ex.Message + "</p>");
            }
            else
            {
                errorMessageSb.Append("<p>Error Message :- " + ex.InnerException.Message + "</p>");
            }
                        
            errorMessageSb.Append("<p>Stack Trace :- " + ex.StackTrace + "</p>");            
            errorMessageSb.Append("<p>Best Reagrds</p> <p>Upvotes Portal</p>");

            UpVotes.Helper.EmailHelper.SendEmail(errorMessageSb.ToString());
        }
    }
}