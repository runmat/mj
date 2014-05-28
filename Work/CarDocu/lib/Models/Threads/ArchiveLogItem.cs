using System.Collections.Generic;

namespace CarDocu.Models
{
    public class ArchiveLogItem : CardocuQueueEntity
    {
        public bool MailDeliveryNeeded { get; set; }

        public int MailItemCount { get; set; }
    }

    public class ArchiveLogItems : List<ArchiveLogItem>
    {
    }
}
