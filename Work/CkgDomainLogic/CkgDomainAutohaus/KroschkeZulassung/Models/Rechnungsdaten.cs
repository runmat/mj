using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Rechnungsdaten
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNr { get; set; }

        public Kunde Kunde
        {
            get
            {
                if (KundenList == null)
                    return new Kunde();

                var option = KundenList.FirstOrDefault(k => k.KundenNr == KundenNr);
                if (option == null)
                    return new Kunde();

                return option;
            }
        }

        [XmlIgnore]
        static public List<Kunde> KundenList { get; set; }

        public string GetSummaryString()
        {
            var s = "";

            if (Kunde != null)
                s += String.Format("{0}: {1}", Localize.Customer, Kunde.KundenNameNr.Replace(", ", "<br/>"));

            return s;
        }
    }
}
