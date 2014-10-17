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

        private static string XarisRootUrl { get { return ConfigurationManager.AppSettings["XarisInventorySystemUrl"]; } }

        public string XarisUrl { get { return string.Format("{0}/?ra={1}&rb={2}", XarisRootUrl, UserNameHashed, ExpirationToken); } }


        public void DataInit()
        {
        }
    }
}
