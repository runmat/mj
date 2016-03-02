using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace AutohausPortalMvc.Services
{
    public class AppSettings : IAppSettings 
    {
        public string AppName { get { return ""; } }
        public string AppOwnerName { get; private set; }
        public string AppOwnerFullName { get; private set; }
        public string AppOwnerNameAndFullName { get; private set; }
        public string AppOwnerImpressumPartialViewName { get; private set; }
        public string AppOwnerKontaktPartialViewName { get; private set; }

        public string AppCopyRight { get { return string.Format("© {0} {1}", DateTime.Now.Year, ConfigurationManager.AppSettings["AppOwnerShortName"] ?? "Christoph Kroschke GmbH"); } }

        public bool IsClickDummyMode { get { return ConfigurationManager.AppSettings["IsClickDummyMode"].NotNullOrEmpty().ToLower() == "true"; } }

        public bool SapNoSqlCache { get { return ConfigurationManager.AppSettings["SapNoSqlCache"].NotNullOrEmpty().ToLower() == "true"; } }

        public string RootPath { get { return HttpContext.Current.Server.MapPath("~/").NotNullOrEmpty(); } }

        public string BinPath { get { return Path.Combine(RootPath, "bin").NotNullOrEmpty(); } }

        public string DataPath { get { return Path.Combine(RootPath, @"App_Data\XmlData\").NotNullOrEmpty(); } }

        public string UploadFilePath { get { return ConfigurationManager.AppSettings["UploadPathSambaArchive"].NotNullOrEmpty(); } }

        public string UploadFilePathTemp { get { return ConfigurationManager.AppSettings["UploadPathTemp"].NotNullOrEmpty(); } }

        public string TempPath { get { return ConfigurationManager.AppSettings["TempPDFPath"].NotNullOrEmpty(); } }


        public string WebPictureRelativePath { get { return ConfigurationManager.AppSettings["UploadPath"]; } }

        public string WebPictureContactsRelativePath { get { return string.Format("{0}Responsible/", WebPictureRelativePath); } }

        public string WebViewRelativePath { get { return ConfigurationManager.AppSettings["PathView"]; } }

        public string WebViewAbsolutePath { get { return ConfigurationManager.AppSettings["UploadPathSambaShow"]; } }
        

        public string TestKundenNr { get; set; }
        

        public string LogoPath { get; set; }
        
        public int LogoPdfPosX { get; set; }
        
        public int LogoPdfPosY { get; set; }
        

        public ISecurityService SecurityService { get; set; }
        
        public IMailService MailService { get; set; }

        public IEnumerable<string> GetAddressPostcodeCityMappings(string plz)
        {
            return new List<string>();
        }

        public int TokenExpirationMinutes { get; private set; }
        public string CustomCssFile { get { return ""; } }
        public string PortalWelcomeMessageStartLocalized { get { return ""; } }
        public string PortalWelcomeMessageEndLocalized { get { return ""; } }

        #region Implementation of ISmtpSettings

        public string SmtpServer { get { return ConfigurationManager.AppSettings["SmtpMailServer"]; } }

        public string SmtpSender { get { return ConfigurationManager.AppSettings["SmtpMailSender"]; } }

        #endregion
    }
}
