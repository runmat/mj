using System.Collections.Generic;

namespace GeneralTools.Models
{
    public class GeneralSummary
    {
        public string Header { get; set; }

        public string Footer { get; set; }

        public List<GeneralEntity> Items { get; set; }
    }
}
