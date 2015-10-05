using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Zulassung.Models
{
    public class AuslieferAdressen
    {
        public AuslieferAdresse AuslieferAdresseZ7 { get; set; }
        public AuslieferAdresse AuslieferAdresseZ8 { get; set; }
        public AuslieferAdresse AuslieferAdresseZ9  { get; set; }

        public List<SelectItem> Materialien { get; set; }

        public bool IsValid { get; set; }
    }
}
