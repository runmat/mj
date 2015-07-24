using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeneralTools.Models;

namespace ServicesMvc.AppUserOverview.Models
{
    public class AppUserOverviewResults
    {

        [LocalizedDisplay("Anwendungsname 1")]      //[LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string AppName { get; set; }

        [LocalizedDisplay("Anwendungsname 2")]
        public string AppFriendlyName { get; set; }

        [LocalizedDisplay("Kunde")]
        public string Customername { get; set; }

        [LocalizedDisplay("Gruppe")]
        public string GroupName { get; set; }

        [LocalizedDisplay("Benutzeranzahl")]
        public int WebUserCount { get; set; }

        [LocalizedDisplay("Benutzer aktiv")]
        public string HatAktiveWebUser { get; set; }


        //public int AppId { get; set; }
        //public int CustomerId { get; set; }
        //public string AppName { get; set; }
        //public string AppFriendlyName { get; set; }
        //public string Customername{ get; set; }
        //public string GroupName { get; set; }
        //public int WebUserCount { get; set; }
        //public string HatAktiveWebUser { get; set; }

    }
}