using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Database.Models
{
    public enum AdminLevel
    {
        Master = 4,
        FirstLevel = 3,
        Customer = 2,
        Organization = 1,
        None = 0
    }

    [Table("WebUser")]
    public class User : IUserSecurityRuleDataProvider
    {
        [Key]
        public int UserID { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Reference { get; set; }

        public string Reference2 { get; set; }

        public string Reference3 { get; set; }

        public bool? Reference4 { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int CustomerID { get; set; }

        public bool TestUser { get; set; }

        public bool AccountIsLockedOut { get; set; }

        public bool Approved { get; set; }

        public DateTime? LastPwdChange { get; set; }
        
        public int FailedLogins { get; set; }

        public string PasswordChangeRequestKey { get; set; }

        public string UserSalutation { get { return string.Format("{0} {1}", Title, LastName); } }

        public string UrlRemoteLoginKey { get; set; }

        public bool CustomerAdmin { get; set; }

        public bool FirstLevelAdmin { get; set; }
        
        
        #region User Security Rules

        public bool UserIsApproved { get { return Approved; } }

        public bool UserIsDisabled { get { return AccountIsLockedOut; } }
        
        public int UserCountFailedLogins { get { return FailedLogins; } }

        #endregion
    }
}
