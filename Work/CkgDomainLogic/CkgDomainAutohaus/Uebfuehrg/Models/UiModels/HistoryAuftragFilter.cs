using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class HistoryAuftragFilter : IValidatableObject 
    {
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants._ErfassungsdatumVon)]
        public DateTime? ErfassungsDatumVon { get; set; }

        [LocalizedDisplay(LocalizeConstants._Bis)]
        public DateTime? ErfassungsDatumBis { get; set; }

        [LocalizedDisplay(LocalizeConstants._AuftragsdatumVon)]
        public DateTime? UeberfuehrungsDatumVon { get; set; }

        [LocalizedDisplay(LocalizeConstants._Bis)]
        public DateTime? UeberfuehrungsDatumBis { get; set; }

        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftragsart)]
        public string AuftragsArt { get; set; }
        
        [LocalizedDisplay(LocalizeConstants._AlleOrganisationen)]
        public bool AlleOrganisationen { get; set; }

        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get { return SelectedAuftragGeberAdresse.KundenNr; } }

        [LocalizedDisplay(LocalizeConstants._KundenReferenz)]
        public string KundenReferenz { get; set; }


        public Adresse SelectedAuftragGeberAdresse { get { return AuftragGeberAdressen.FirstOrDefault(a => a.ID == SelectedAuftragGeber); } }

        [LocalizedDisplay(LocalizeConstants.Client)]
        public int SelectedAuftragGeber { get; set; }

        [XmlIgnore]
        public List<Adresse> AuftragGeberAdressen { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}