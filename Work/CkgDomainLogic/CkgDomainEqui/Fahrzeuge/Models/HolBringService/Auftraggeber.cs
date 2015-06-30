using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Auftraggeber 
    {
        [LocalizedDisplay(LocalizeConstants.Auftragsersteller)]
        public string Auftragsersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string AuftragerstellerTel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Factory)]
        public string Betrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.Repco)]
        public string Repco { get; set; }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        public string Ansprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string AnsprechpartnerTel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Kunde { get; set; }

        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string KundeTel { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennnzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleSpecies)]
        public int FahrzeugartId { get; set; }

        //[XmlIgnore, ScriptIgnore]
        //public List<Domaenenfestwert> Fahrzeugarten { get; set; }

    }
}
