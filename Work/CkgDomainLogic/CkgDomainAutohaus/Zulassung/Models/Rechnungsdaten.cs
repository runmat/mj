using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Rechnungsdaten
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNr { get; set; }


        public Kunde GetKunde(List<Kunde> kunden)
        {
            if (kunden == null)
                    return new Kunde();

            var option = kunden.FirstOrDefault(k => k.KundenNr == KundenNr);
            if (option == null)
                return new Kunde();

            return option;
        }

        public string GetSummaryString(List<Kunde> kunden)
        {
            var s = "";

            var kunde = GetKunde(kunden);
            if (kunde != null)
                s += String.Format("{0}: {1}", Localize.Customer, kunde.KundenNameNr.Replace(", ", "<br/>"));

            return s;
        }
    }
}
