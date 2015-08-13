using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiEinsteuerung
    {
        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        public DateTime? Zb2Eingang { get; set; }

        public string AbsenderZb2Name1 { get; set; }

        public string AbsenderZb2Name2 { get; set; }

        public string AbsenderZb2Strasse { get; set; }

        public string AbsenderZb2Hausnummer { get; set; }

        public string AbsenderZb2Plz { get; set; }

        public string AbsenderZb2Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.SenderZb2)]
        public string AbsenderZb2
        {
            get
            {
                return String.Format("{0} {1}" + (String.IsNullOrEmpty(AbsenderZb2Strasse) && String.IsNullOrEmpty(AbsenderZb2Hausnummer) ? "" : Environment.NewLine)
                    + "{2} {3}" + (String.IsNullOrEmpty(AbsenderZb2Plz) && String.IsNullOrEmpty(AbsenderZb2Ort) ? "" : Environment.NewLine)
                    + "{4} {5}",
                    AbsenderZb2Name1, AbsenderZb2Name2,
                    AbsenderZb2Strasse, AbsenderZb2Hausnummer,
                    AbsenderZb2Plz, AbsenderZb2Ort);
            }
        }

        public string PdiNr { get; set; }

        public string PdiName { get; set; }

        public string PdiOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string Pdi { get { return String.Format("{0} {1} {2}", PdiNr, PdiName, PdiOrt); } }

        [LocalizedDisplay(LocalizeConstants.PdiReceipt)]
        public DateTime? PdiEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.PdiReady)]
        public DateTime? PdiBereit { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModellId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        public string ModellName { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationAssignment)]
        public DateTime? BeauftragungZulassung { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        public string HalterName1 { get; set; }

        public string HalterName2 { get; set; }

        public string HalterStrasse { get; set; }

        public string HalterHausnummer { get; set; }

        public string HalterPlz { get; set; }

        public string HalterOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter
        {
            get
            {
                return String.Format("{0} {1}" + (String.IsNullOrEmpty(HalterStrasse) && String.IsNullOrEmpty(HalterHausnummer) ? "" : Environment.NewLine)
                    + "{2} {3}" + (String.IsNullOrEmpty(HalterPlz) && String.IsNullOrEmpty(HalterOrt) ? "" : Environment.NewLine)
                    + "{4} {5}",
                    HalterName1, HalterName2,
                    HalterStrasse, HalterHausnummer,
                    HalterPlz, HalterOrt);
            }
        }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationStop)]
        public bool Zulassungssperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.BlockingNotice)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.KeyReceipt)]
        public DateTime? SchluesselEingang { get; set; }
    }
}
