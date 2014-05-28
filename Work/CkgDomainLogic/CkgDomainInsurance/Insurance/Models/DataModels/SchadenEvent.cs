
using System;

namespace CkgDomainLogic.Insurance.Models
{
    public class SchadenEvent
    {
        public int EventID { get; set; }

        public string EventName { get; set; }

        public DateTime AnlageDatum { get; set; }

        public string EventText { get { return EventID + " - " + EventName; } }
    }
}
