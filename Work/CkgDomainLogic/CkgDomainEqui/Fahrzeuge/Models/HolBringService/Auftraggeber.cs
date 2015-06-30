using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;

namespace CkgDomainLogic.Fahrzeuge.Models.HolBringService
{
    public class Auftraggeber 
    {
        public string Auftragersteller { get; set; }
        public string AuftragerstellerTel { get; set; }
        public string Betrieb { get; set; }
        public string Repco { get; set; }
        public string Ansprechpartner { get; set; }
        public string AnsprechpartnerTel { get; set; }
        public string Kunde { get; set; }
        public string KundeTel { get; set; }
        public string Kennnzeichen { get; set; }
        public int FahrzeugartId { get; set; }

        //[XmlIgnore, ScriptIgnore]
        //public List<Domaenenfestwert> Fahrzeugarten { get; set; }


    }
}
