using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
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
}