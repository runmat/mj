using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmRechercheprotokollVersuch
    {
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public string DatumText
        {
            get { return Datum.ToString("dd.MM.yyyy"); }
        }

        [LocalizedDisplay(LocalizeConstants.By)]
        public string User { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb1Available)]
        public bool Zb1Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Zb2Available)]
        public bool Zb2Vorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.Front)]
        public bool KennzeichenVorne { get; set; }

        [LocalizedDisplay(LocalizeConstants.Rear)]
        public bool KennzeichenHinten { get; set; }

        [LocalizedDisplay(LocalizeConstants.Voided)]
        public bool KennzeichenEntwertet { get; set; }

        [LocalizedDisplay(LocalizeConstants.Theft)]
        public bool KennzeichenDiebstahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.VeAvailable)]
        public bool VeVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.NotReached)]
        public bool NichtErreicht { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChecksAndAnswers)]
        public bool PrueftUndMeldetSich { get; set; }

        [LocalizedDisplay(LocalizeConstants.GaHasDocuments)]
        public bool GaHatUnterlagen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Memorandum)]
        public string Vermerk { get; set; }
    }
}
