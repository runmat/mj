using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Archive.Models
{
    public class EasyAccessDetail
    {
        [LocalizedDisplay(LocalizeConstants.DocumentId)]
        public string DocId { get; set; }

        [LocalizedDisplay(LocalizeConstants.LengthInBytes)]
        public int Laenge { get; set; }

        [LocalizedDisplay(LocalizeConstants.CreationDate)]
        public string Erstelldatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChangeDate)]
        public string Aenderungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Title)]
        public string Titel { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfFieldsTotal)]
        public int AnzFelderGesamt { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfFieldsText)]
        public int AnzFelderText { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfFieldsPicture)]
        public int AnzFelderBild { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfFieldsBinary)]
        public int AnzFelderBlob { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

    }
}
