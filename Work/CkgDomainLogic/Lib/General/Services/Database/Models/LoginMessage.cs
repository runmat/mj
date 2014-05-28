using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace CkgDomainLogic.General.Database.Models
{
    [Table("LoginMessage")]
    public class LoginMessage : IMaintenanceSecurityRuleDataProvider 
    {
        // ReSharper disable InconsistentNaming
        
        [Key]
        public int ID { get; set; }

        public DateTime? creationDate { get; set; }

        public DateTime? activeDateFrom { get; set; }

        public DateTime? activeDateTo { get; set; }

        public DateTime? activeTimeFrom { get; set; }

        public DateTime? activeTimeTo { get; set; }

        private string _messageText;
        public string messageText
        {
            get { return _messageText; }
            set { _messageText = ConvertMessage(value); }
        }

        private string _titleText;
        public string titleText
        {
            get { return _titleText; }
            set { _titleText = ConvertMessage(value); }
        }

        public int? messageColor { get; set; }

        public bool? active { get; set; }

        public string enableLogin { get; set; }

        public bool? onlyTEST { get; set; }

        public bool? onlyPROD { get; set; }

        public bool? PortalLoginDisabled { get; set; }

        // ReSharper restore InconsistentNaming

        static string ConvertMessage(string message)
        {
            message = message.NotNullOrEmpty();
            message = message.Replace("{", "<");
            message = message.Replace("}", ">");
            message = message.Replace("</c>", "");
            message = Regex.Replace(message, @"<\s*((c)+)[^/>]*>", "");

            return message;
        }


        #region Maintenance Login Messages

        public string MaintenanceTitle { get { return titleText; } }

        public string MaintenanceText { get { return messageText; } }

        public DateTime MaintenanceStartDateTime { get { return activeDateFrom.GetValueOrDefault().Add(new TimeSpan(activeTimeFrom.GetValueOrDefault().Hour, activeTimeFrom.GetValueOrDefault().Minute, 0)); } }

        public DateTime MaintenanceEndDateTime { get { return activeDateTo.GetValueOrDefault().Add(new TimeSpan(activeTimeTo.GetValueOrDefault().Hour, activeTimeTo.GetValueOrDefault().Minute, 0)); } }

        // ToDo: Check "enableLogin" Flag for customers, etc (see Services App)    

        public bool MaintenanceLoginDisabled { get { return PortalLoginDisabled.GetValueOrDefault(); } }  

        public bool MaintenanceOnTestSystem { get { return onlyTEST.GetValueOrDefault(); } }

        public bool MaintenanceOnProdSystem { get { return onlyPROD.GetValueOrDefault(); } }

        #endregion
    }
}
