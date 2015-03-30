using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.WFM.Models
{
    public class WfmDokumentInfo
    {
        [LocalizedDisplay(LocalizeConstants.CaseNoDeregistrationOrder)]
        public string VorgangsNrAbmeldeauftrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.DocumentType)]
        public string Dokumentart { get; set; }

        [LocalizedDisplay(LocalizeConstants.Archive)]
        public string ArchivId { get; set; }

        [LocalizedDisplay(LocalizeConstants.FileName)]
        public string Dateiname { get; set; }

        [LocalizedDisplay(LocalizeConstants.ObjectId)]
        public string ObjectId { get; set; }

        [GridHidden, NotMapped]
        public string ErrorMessage { get; set; }
    }
}
