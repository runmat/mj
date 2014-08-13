using System.Collections.Generic;
using GeneralTools.Models;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class SummaryItem 
    {
        public int Step { get; set; }

        public string Header { get; set; }

        public List<GeneralEntity> Descriptions { get; set; }

        public string IconFileName { get { return string.Format("images/summaryIcon{0}.png", Step); } }
    }
}
