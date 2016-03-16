using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class Rechnungsdaten
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<KroschkeZulassungViewModel> GetZulassungViewModel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNr { get; set; }

        public bool ModusPartnerportal { get { return GetZulassungViewModel != null && GetZulassungViewModel().ModusPartnerportal; } }

        public string PartnerportalHinweis { get { return Localize.PleaseRememberAuthorityAndSepaMandateHint; } }


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
