using System.Collections.Generic;
using System.Linq;
using GeneralTools.Services;

namespace GeneralTools.Models
{
    public class MaintenanceResult : Store 
    {
        public List<MaintenanceMessage> Messages { get { return PropertyCacheGet(() => new List<MaintenanceMessage>()); } set { PropertyCacheSet(value); } }

        public List<MaintenanceMessage> AvailableMessages { get { return Messages.Where(message => message.IsAvailable).ToList(); } }

        public bool LogonDisabled { get { return Messages.Any(message => message.LogonDisabled); } }

        public bool MessagesAvailable { get { return Messages.Any(message => message.IsAvailable); } }
    }
}
