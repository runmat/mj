using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace ServicesMvc.AppUserOverview.Models
{
    public class AppUserOverviewResults
    {

        [LocalizedDisplay(LocalizeConstants.Applicationname1)]      
        public string AppName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Applicationname2)]
        public string AppFriendlyName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Url)]
        public string AppUrl { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Customername { get; set; }

        [LocalizedDisplay(LocalizeConstants.Group)]
        public string GroupName { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserCount)]
        public int WebUserCount { get; set; }              

        [LocalizedDisplay(LocalizeConstants.UserActive)]
        public string HasActiveWebUsers { get; set; }      

    }
}