using System;

namespace CkgDomainLogic.General.Database.Models
{
    public class LoginUserMessageConfirmations 
    {
        public int UserID { get; set; }

        public int MessageID { get; set; }

        public DateTime ShowMessageFrom { get; set; }

        public DateTime ConfirmDate { get; set; }
    }
}
