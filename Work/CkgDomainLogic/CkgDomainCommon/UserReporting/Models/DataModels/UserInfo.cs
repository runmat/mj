using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.UserReporting.Models
{
    public class UserInfo
    {
        [LocalizedDisplay(LocalizeConstants.UserName)]
        public string Username { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string CustomerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string Kunnr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Title)]
        public string Title { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string FirstName { get; set; }

        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string LastName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Group)]
        public string GroupName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Organization)]
        public string OrganizationName { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreateDate)]
        public DateTime? CreateDate { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreatedBy)]
        public string CreatedBy { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteDate)]
        public DateTime? DeleteDate { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeleteUser)]
        public string LastChangedBy { get; set; }
    }
}
