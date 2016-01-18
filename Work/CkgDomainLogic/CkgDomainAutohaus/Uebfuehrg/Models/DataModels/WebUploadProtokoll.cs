using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class WebUploadProtokoll
    {
        [LocalizedDisplay(LocalizeConstants._Protokollart)]
        public string Protokollart { get; set; }

        [LocalizedDisplay(LocalizeConstants._Protokollart)]
        public string ProtokollartFormatted { get { return Protokollart.NotNullOrEmpty().Trim().Replace(".", "").Replace(" ", ""); } }

        [LocalizedDisplay(LocalizeConstants._Kategorie)]
        public string Kategorie { get; set; }

        [LocalizedDisplay(LocalizeConstants._Fahrt)]
        public string FahrtIndex { get; set; }

        [LocalizedDisplay(LocalizeConstants.FileName)]
        public string Dateiname { get; set; }
    }
}
