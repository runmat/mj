using System.Collections.Generic;

namespace GeneralTools.Contracts
{
    public interface IAppSettings : ISmtpSettings
    {
        string AppName { get; }

        string AppOwnerName { get; }

        string AppOwnerFullName { get; }

        string AppOwnerNameAndFullName { get; }

        string AppOwnerImpressumPartialViewName { get; }

        string AppOwnerKontaktPartialViewName { get; }

        string AppCopyRight { get;  }
        
        bool IsClickDummyMode { get; }

        bool SapNoSqlCache { get; }
    
        string RootPath { get; }

        string BinPath { get; }

        string DataPath { get; }
        
        string UploadFilePath { get; }

        string UploadFilePathTemp { get; }

        string TempPath { get; }


        string WebPictureRelativePath { get; }

        string WebPictureContactsRelativePath { get; }

        string WebViewRelativePath { get; }

        string WebViewAbsolutePath { get; }

        
        string TestKundenNr { get; }


        string LogoPath { get; }

        int LogoPdfPosX { get; }
        
        int LogoPdfPosY { get; }

        ISecurityService SecurityService { get; set; }

        IMailService MailService { get; set; }

        IEnumerable<string> GetAddressPostcodeCityMappings(string plz);

        int TokenExpirationMinutes { get; }

        string CustomCssFile { get; }

        string PortalWelcomeMessageStartLocalized { get; }

        string PortalWelcomeMessageEndLocalized { get; }
    }
}
