using System;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class AktivcheckTreffer
    {
        public string VorgangsID { get; set; }

        [LocalizedDisplay(LocalizeConstants.VIN)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string ZB2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNoShort)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Created)]
        public DateTime Erstelldatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Checked)]
        public DateTime Pruefdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Collision)]
        public string Kollision { get; set; }

        [LocalizedDisplay(LocalizeConstants.Classification)]
        public string Klassifizierung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Classification)]
        public string Klassifizierungstext { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContactRequest)]
        public bool Kontaktanfrage { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }

        // Die folgenden Properties werden nicht im View angezeigt, müssen aber im Model ent- 
        // halten sein, weil die Datensätze wieder in voller Breite in SAP gespeichert werden
        public string Mandant { get; set; }
        public string Kundennummer { get; set; }
        public string EquiNummer { get; set; }
        public string BestandsAg { get; set; }
        public string Kundennummer_Kol { get; set; }
        public string Fahrgestellnummer_Kol { get; set; }
        public string ZB2_Kol { get; set; }
        public string Vertragsnummer_Kol { get; set; }

    }
}
