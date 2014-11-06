using System.Collections.Generic;
using System.Linq;
using GeneralTools.Services;

namespace GeneralTools.Models
{
    public class MaintenanceResult : Store 
    {
        public List<MaintenanceMessage> Messages { get { return PropertyCacheGet(() => new List<MaintenanceMessage>()); } set { PropertyCacheSet(value); } }

        public bool LogonDisabled { get { return Messages.Any(message => message.LogonDisabled); } }

        public List<MaintenanceMessage> AvailableMessages { get { return Messages.Where(message => message.IsActive).ToList(); } }
        public bool MessagesAvailable { get { return AvailableMessages.Any(); } }

        public List<MaintenanceMessage> AvailableMessagesToConfirm { get { return Messages.Where(message => message.IsActiveAndLetConfirmMessageAfterLogin).ToList(); } }
        public bool MessagesToConfirmAvailable { get { return AvailableMessagesToConfirm.Any(); } }
    }
}
