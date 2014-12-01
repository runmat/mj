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
    public class Zulassungsdaten
    {
        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationType)]
        public string ZulassungsartMatNr { get; set; }

        public Material Zulassungsart
        {
            get
            {
                if (MaterialList == null)
                    return new Material();

                var option = MaterialList.FirstOrDefault(m => m.MaterialNr == ZulassungsartMatNr);
                if (option == null)
                    return new Material();

                return option;
            }
        }

        [XmlIgnore]
        static public List<Material> MaterialList { get; set; }

        public string Belegtyp { get { return Zulassungsart.Belegtyp; } }

        [Required]
        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [Required]
        [StringLength(3)]
        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string Zulassungskreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDistrict)]
        public string ZulassungskreisBezeichnung { get; set; }

        [Required]
        [StringLength(7)]
        [LocalizedDisplay(LocalizeConstants.EvbNumber)]
        public string EvbNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedNumberPlate)]
        public bool Wunschkennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo2)]
        public string Wunschkennzeichen2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.PersonalisedLicenseNo3)]
        public string Wunschkennzeichen3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReserveLicenseNo)]
        public bool KennzeichenReservieren { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationNo)]
        public string ReservierungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationName)]
        public string ReservierungsName { get; set; }

        public string GetSummaryString()
        {
            var s = "";

            if (Zulassungsart != null)
                s += String.Format("{0}: {1}", Localize.RegistrationType, Zulassungsart.MaterialText);
 
            s += String.Format("<br/>{0}: {1}", Localize.RegistrationDate, Zulassungsdatum);
            s += String.Format("<br/>{0}: {1} {2}", Localize.RegistrationDistrict, Zulassungskreis, ZulassungskreisBezeichnung);
            s += String.Format("<br/>{0}: {1}", Localize.EvbNumber, EvbNr);
            s += String.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo, Kennzeichen);

            if (Wunschkennzeichen)
            {
                s += String.Format("<br/>{0}", Localize.PersonalisedNumberPlate);
                s += String.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo2, Wunschkennzeichen2);
                s += String.Format("<br/>{0}: {1}", Localize.PersonalisedLicenseNo3, Wunschkennzeichen3);
            }

            if (KennzeichenReservieren)
            {
                s += String.Format("<br/>{0}", Localize.ReserveLicenseNo);
                s += String.Format("<br/>{0}: {1}", Localize.ReservationNo, ReservierungsNr);
                s += String.Format("<br/>{0}: {1}", Localize.ReservationName, ReservierungsName);
            }

            return s;
        }
    }
}
