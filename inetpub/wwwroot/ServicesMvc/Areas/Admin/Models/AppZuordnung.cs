using CkgDomainLogic.General.Database.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace ServicesMvc.AppUserOverview.Models
{
    public class AppZuordnung
    {
        public int AppID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Applicationname1)]      
        public string AppName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Applicationname2)]
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

        [LocalizedDisplay(LocalizeConstants.Assigned)]
        public bool IsAssignedToCustomer { get; set; }

        public int GroupID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Group)]
        public string GroupName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Assigned)]
        public bool IsAssignedToGroup { get; set; }

        public AppZuordnung(Application anwendung)
        {
            AppID = anwendung.AppID;
            AppName = anwendung.AppName;
            AppFriendlyName = anwendung.AppFriendlyName;
            AppUrl = anwendung.AppURL;
            AppTechType = anwendung.AppTechType;
            AppDescription = anwendung.AppDescription;
        }

        public AppZuordnung(Application anwendung, Customer kunde, bool kundeZugeordnet)
        {
            AppID = anwendung.AppID;
            AppName = anwendung.AppName;
            AppFriendlyName = anwendung.AppFriendlyName;
            AppUrl = anwendung.AppURL;
            AppTechType = anwendung.AppTechType;
            AppDescription = anwendung.AppDescription;
            CustomerID = kunde.CustomerID;
            Customername = kunde.Customername;
            IsAssignedToCustomer = kundeZugeordnet;
        }

        public AppZuordnung(Application anwendung, Customer kunde, bool kundeZugeordnet, UserGroup gruppe, bool gruppeZugeordnet)
        {
            AppID = anwendung.AppID;
            AppName = anwendung.AppName;
            AppFriendlyName = anwendung.AppFriendlyName;
            AppUrl = anwendung.AppURL;
            AppTechType = anwendung.AppTechType;
            AppDescription = anwendung.AppDescription;
            CustomerID = kunde.CustomerID;
            Customername = kunde.Customername;
            IsAssignedToCustomer = kundeZugeordnet;
            GroupID = gruppe.GroupID;
            GroupName = gruppe.GroupName;
            IsAssignedToGroup = gruppeZugeordnet;
        }
    }
}