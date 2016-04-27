using System;
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

        [LocalizedDisplay(LocalizeConstants.CreationDate)]
        public DateTime? CreateDate  { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string Time { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string TimeFormatted
        {
            get
            {
                return String.Format("{0}:{1}:{2}", Time.Substring(0, 2), Time.Substring(2, 2), Time.Substring(4, 2));
            }
        }


        [GridHidden, NotMapped]
        public string ErrorMessage { get; set; }
    }
}
