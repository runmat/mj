using CkgDomainLogic.General.Database.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Admin.Models
{
    public class AppZuordnung
    {
        public int AppID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Application)]      
        public string AppName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name)]
        public string AppFriendlyName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Url)]
        public string AppUrl { get; set; }

        [LocalizedDisplay(LocalizeConstants.Technology)]
        public string AppTechType { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string AppDescription { get; set; }

        public int CustomerID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Customername { get; set; }

        public int GroupID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Group)]
        public string GroupName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Assigned)]
        public bool IsAssigned { get; set; }

        public string ZuordnungID { get { return string.Format("{0}_{1}_{2}", AppID, CustomerID, GroupID); } }

        public AppZuordnung()
        {
        }

        public AppZuordnung(Application anwendung, Customer kunde, UserGroup gruppe, bool zugeordnet)
        {
            AppID = anwendung.AppID;
            AppName = anwendung.AppName;
            AppFriendlyName = anwendung.AppFriendlyName;
            AppUrl = anwendung.AppURL;
            AppTechType = anwendung.AppTechType;
            AppDescription = anwendung.AppDescription;
            CustomerID = kunde.CustomerID;
            Customername = kunde.Customername;
            GroupID = gruppe.GroupID;
            GroupName = gruppe.GroupName;
            IsAssigned = zugeordnet;
        }
    }
}
