// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Contracts;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Models.DataModels;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using WebTools.Services;

// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.DomainCommon.ViewModels
{
    public class XarisViewModel : CkgBaseViewModel
    {
        private string UserNameHashed { get { return LogonContext.User.UrlRemoteLoginKey; } }

        private static string ExpirationToken { get { return UserSecurityService.UrlRemoteEncryptHourAndDate(); } }

        private static string XarisRootUrl { get { return ConfigurationManager.AppSettings["XarisSepiaRootUrl"]; } }

        private static string XarisAppRelativeUrl { get { return ConfigurationManager.AppSettings["XarisSepiaRelativeUrl"]; } }

        public string XarisUrl { get { return string.Format("{0}{1}&ra={2}&rb={3}", XarisRootUrl, XarisAppRelativeUrl, UserNameHashed, ExpirationToken); } }


        private static string UserNameHashed2ForTest { get { return "10cb949b-d992-49e9-8bf0-96d602bc97ef"; } }
        public string XarisUrl2ForTest { get { return string.Format("{0}{1}&ra={2}&rb={3}", XarisRootUrl, XarisAppRelativeUrl, UserNameHashed2ForTest, ExpirationToken); } }

        private static string UserNameHashed3ForTest { get { return "4302c052-850c-4aae-a80b-b80e9addcf31"; } }
        public string XarisUrl3ForTest { get { return string.Format("{0}{1}&ra={2}&rb={3}", XarisRootUrl, XarisAppRelativeUrl, UserNameHashed3ForTest, ExpirationToken); } }


        public void DataInit()
        {
        }
    }
}
