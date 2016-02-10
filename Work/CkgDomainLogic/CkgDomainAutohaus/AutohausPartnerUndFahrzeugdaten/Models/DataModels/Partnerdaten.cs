using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models
{
    public class Partnerdaten
    {
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [StringLength(40)]
        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [StringLength(40)]
        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [StringLength(60)]
        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string HausNr { get; set; }

        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Plz { get; set; }

        [StringLength(40)]
        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Land { get; set; }

        [StringLength(241)]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.Phone)]
        public string Telefon { get; set; }

        [StringLength(30)]
        [LocalizedDisplay(LocalizeConstants.Fax)]
        public string Fax { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string Bemerkung { get; set; }

        [Required]
        [StringLength(10)]
        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [StringLength(50)]
        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Commercial)]
        public bool Gewerblich { get; set; }

        [StringLength(4)]
        [LocalizedDisplay(LocalizeConstants.PartnerRole)]
        public string Partnerrolle { get; set; }

        [LocalizedDisplay(LocalizeConstants.SaveCustomerData)]
        public bool KundendatenSpeichern { get; set; }
    }
}
