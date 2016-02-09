using System;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.AutohausPartnerUndFahrzeugdaten.Models
{
    public class UploadPartnerdaten : Partnerdaten, IUploadItem
    {
        public int DatensatzNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string ValidationStatus
        {
            get
            {
                if (!String.IsNullOrEmpty(ValidationErrorsJson) && ValidationErrorsJson != "[]")
                    return Localize.Error;

                return Localize.OK;
            }
        }

        public string ValidationErrorsJson { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string SaveStatus { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool ValidationOk { get; set; }

        [ModelMappingCompareIgnore]
        [GridHidden]
        public bool IsValid { get { return ValidationOk; } }
    }
}
