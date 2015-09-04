using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("LoginUserMessage")]
    public class LoginUserMessage : IMaintenanceSecurityRuleDataProvider 
    {
        // ReSharper disable InconsistentNaming
        
        [Key]
        public int ID { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? ShowMessageFrom { get; set; }

        public DateTime? ShowMessageTo { get; set; }

        public DateTime? LockLoginFrom { get; set; }

        public DateTime? LockLoginTo { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = ConvertMessage(value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = ConvertMessage(value); }
        }

        public bool LockForTest { get; set; }

        public bool LockForProd { get; set; }

        [NotMapped]
        public bool MessageIsConfirmedByUser { get; set; }

        // ReSharper restore InconsistentNaming

        public static string ConvertMessage(string message)
        {
            message = message.NotNullOrEmpty();
            message = message.Replace("{", "<");
            message = message.Replace("}", ">");
            message = message.Replace("</c>", "");
            message = Regex.Replace(message, @"<\s*((c)+)[^/>]*>", "");

            return message;
        }


        #region Maintenance Login Messages

        public string MaintenanceTitle { get { return Title; } }

        public string MaintenanceText { get { return Message; } }

        public bool MaintenanceLoginDisabled { get { return DateTime.Now >= LockLoginFrom.GetValueOrDefault() && DateTime.Now <= LockLoginTo.GetValueOrDefault().AddMinutes(1); } }

        public bool MaintenanceShow { get { return DateTime.Now >= ShowMessageFrom.GetValueOrDefault() && DateTime.Now <= ShowMessageTo.GetValueOrDefault().AddMinutes(1); } }  

        public bool MaintenanceOnTestSystem { get { return LockForTest; } }

        public bool MaintenanceOnProdSystem { get { return LockForProd; } }

        public bool MaintenanceShowAndLetConfirmMessageAfterLogin
        {
            get { return !MessageIsConfirmedByUser && MaintenanceShow && !(Title.NotNullOrEmpty().ToLower().Contains("achtung: dies ist der ")); }
        }

        #endregion
    }
}
