using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("Customer")]
    public class Customer : IPasswordSecurityRuleDataProvider 
    {
        [Key]
        public int CustomerID { get; set; }

        public string Customername { get; set; }

        public string KUNNR { get; set; }

        public string PortalType { get; set; }

        [NotMapped]
        public bool MvcRawLayout { get { return PortalType.NotNullOrEmpty().ToLower() != "mvc"; } }

        public bool? AllowUrlRemoteLogin { get; set; }

        public int? AccountingArea { get; set; }


        #region Password Settings

        public int PwdNNumeric { get; set; }

        public int PwdLength { get; set; }

        public int PwdNCapitalLetters { get; set; }

        public int PwdNSpecialCharacter { get; set; }

        public int PwdHistoryNEntries { get; set; }

        public int LockedAfterNLogins { get; set; }

        [Obsolete]
        public int NewPwdAfterNDays { get; set; }


        #region Interface IPasswordSecurityRuleDataProvider

        [LocalizedDisplay(LocalizeConstants.PasswordMinNumericChars)]
        public int PasswordMinNumericChars { get { return PwdNNumeric; } }

        [LocalizedDisplay(LocalizeConstants.PasswordMinLength)]
        public int PasswordMinLength { get { return PwdLength; } }

        [LocalizedDisplay(LocalizeConstants.PasswordMinCapitalChars)]
        public int PasswordMinCapitalChars { get { return PwdNCapitalLetters; } }

        [LocalizedDisplay(LocalizeConstants.PasswordMinSpecialChars)]
        public int PasswordMinSpecialChars { get { return PwdNSpecialCharacter; } }

        [LocalizedDisplay(LocalizeConstants.PasswordMinHistoryEntries)]
        public int PasswordMinHistoryEntries { get { return PwdHistoryNEntries; } }

        [LocalizedDisplay(LocalizeConstants.PasswordMaxLoginFailures)]
        public int PasswordMaxLoginFailures { get { return LockedAfterNLogins; } }

        #endregion

        #endregion
    }
}
