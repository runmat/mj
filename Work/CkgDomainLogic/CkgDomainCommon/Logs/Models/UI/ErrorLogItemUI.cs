using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Logs.Models
{
    public class ErrorLogItemUI
    {
        public string ErrorId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Application)]
        public string Application { get; set; }

        [LocalizedDisplay(LocalizeConstants.Server)]
        public string Host { get; set; }

        [LocalizedDisplay(LocalizeConstants.Type)]
        public string Type { get; set; }

        [LocalizedDisplay(LocalizeConstants.Source)]
        public string Source { get; set; }

        [LocalizedDisplay(LocalizeConstants.Message)]
        public string Message { get; set; }

        public int Sequence { get; set; }

        public string User { get; set; }

        [LocalizedDisplay(LocalizeConstants.User)]
        public string UserName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public int StatusCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime TimeUtc { get; set; }
    }
}
