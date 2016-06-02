using System.Collections.Generic;
using GeneralTools.Contracts;
using GeneralTools.Models;
using System.Web;
using System.IO;
using System.Configuration;

namespace ZLDMobile.Services
{
    public class AppSettings : IAppSettings
    {
        public string SmtpServer { get { return ConfigurationManager.AppSettings["SmtpMailServer"]; } }

        public string SmtpSender { get { return ConfigurationManager.AppSettings["SmtpMailSender"]; } }

        public string AppName { get; set; }

        public string AppOwnerName { get; set; }

        public string AppOwnerFullName { get; set; }

        public string AppOwnerNameAndFullName { get; private set; }
        public string AppOwnerSuffix { get; private set; }

        public string AppOwnerImpressumPartialViewName { get; private set; }

        public string AppOwnerKontaktPartialViewName { get; private set; }

        public string AppCopyRight { get; set; }

        public bool IsClickDummyMode { get { return ConfigurationManager.AppSettings["IsClickDummyMode"].NotNullOrEmpty().ToLower() == "true"; } }

        public bool SapNoSqlCache { get { return ConfigurationManager.AppSettings["SapNoSqlCache"].NotNullOrEmpty().ToLower() == "true"; } }

        public string RootPath { get { return HttpContext.Current.Server.MapPath("~/"); } }

        public string BinPath { get { return Path.Combine(RootPath, "bin"); } }

        public string DataPath { get { return Path.Combine(RootPath, @"App_Data\XmlData\"); } }

        public string UploadFilePath { get { return ""; } }

        public string UploadFilePathTemp { get { return ""; } }

        public string TempPath { get { return ""; } }

        public string WebPictureRelativePath { get; set; }

        public string WebPictureContactsRelativePath { get; set; }

        public string WebViewRelativePath { get { return ""; } }

        public string WebViewAbsolutePath { get { return ""; } }

        public string TestKundenNr { get { return ""; } }

        public string LogoPath { get { return ConfigurationManager.AppSettings["LogoPath"]; } }

        public int LogoPdfPosX { get { return ConfigurationManager.AppSettings["LogoPdfPosX"].ToInt(); } }

        public int LogoPdfPosY { get { return ConfigurationManager.AppSettings["LogoPdfPosY"].ToInt(); } }

        public ISecurityService SecurityService { get; set; }

        public IMailService MailService { get; set; }

        public IEnumerable<string> GetAddressPostcodeCityMappings(string plz)
        {
            throw new System.NotImplementedException();
        }

        public int TokenExpirationMinutes { get; set; }
        public string CustomCssFile { get; private set; }
        public string PortalWelcomeMessageStartLocalized { get; private set; }
        public string PortalWelcomeMessageEndLocalized { get; private set; }

        public string GlobalErrorlogDirectory { get { return ConfigurationManager.AppSettings["GlobalErrorlogDirectory"]; } }
    }
}
