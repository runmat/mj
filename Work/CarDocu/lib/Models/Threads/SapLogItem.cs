using System.Collections.Generic;

namespace CarDocu.Models
{
    public class SapLogItem : CardocuQueueEntity 
    {
        public List<string> DocumentCodes { get; set; }

        public List<int> ResultCodes { get; set; }

        public List<string> ResultMessages { get; set; }
    }

    public class SapLogItems : List<SapLogItem>
    {
    }
}
