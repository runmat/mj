using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class AuslieferAdresse : IValidatableObject
    {
        public Adressdaten Adressdaten { get; set; }

        [XmlIgnore]
        public static List<SelectItem> AlleMaterialien
        {
            get
            {
                return new List<SelectItem>
                {
                    new SelectItem("ScheinSchilder", Localize.DeliveryAdr_Zb1LicPlatesFsp),
                    new SelectItem("Zb2Coc", Localize.DeliveryAdr_Zb2Coc),
                    new SelectItem("Sonstiges", Localize.DeliveryAdr_Sonstiges),
                    new SelectItem("Perso", Localize.DeliveryAdr_IdCard)
                };
            }
        }

        [XmlIgnore]
        public List<SelectItem> Materialien { get; set; }

        [LocalizedDisplay(LocalizeConstants.Material)]
        public List<string> ZugeordneteMaterialien { get; set; }

        [LocalizedDisplay(LocalizeConstants.Address)]
        public string TmpSelectedPartnerrolle { get; set; }

        public bool TmpSaveAddressOnly { get; set; }

        public bool HasData { get { return (ZugeordneteMaterialien.AnyAndNotNull() || Adressdaten.AdresseVollstaendig); } }

        public bool IsValid { get; set; }

        public AuslieferAdresse()
        {
            // parameterless dummy constructor for MVC model binder
            Adressdaten = new Adressdaten();
            Materialien = new List<SelectItem>();
            ZugeordneteMaterialien = new List<string>();
        }

        public AuslieferAdresse(string partnerrolle)
        {
            Adressdaten = new Adressdaten("") { Partnerrolle = partnerrolle };
            Materialien = new List<SelectItem>();
            ZugeordneteMaterialien = new List<string>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Adressdaten.AdresseVollstaendig && ZugeordneteMaterialien.None())
                yield return new ValidationResult(Localize.MaterialRequired);

            if (ZugeordneteMaterialien.Contains("Sonstiges") && String.IsNullOrEmpty(Adressdaten.Bemerkung))
                yield return new ValidationResult(Localize.CommentRequired);

            if (ZugeordneteMaterialien.AnyAndNotNull() && !Adressdaten.AdresseVollstaendig)
                yield return new ValidationResult(Localize.CompleteAddressRequired);
        }
    }
}
