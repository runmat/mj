using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiZusatzdatenRemarketing
    {
        [LocalizedDisplay(LocalizeConstants.HcDispatch)]
        public DateTime? HcAusgang { get; set; }

        [LocalizedDisplay(LocalizeConstants.SurveyOrderDate)]
        public DateTime? GutachtenAuftragsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractYear)]
        public string Vertragsjahr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RrpPrice)]
        public string UpePreis { get; set; }

        [LocalizedDisplay(LocalizeConstants.RrpPrice)]
        public string UpePreisFormatted { get { return (string.IsNullOrEmpty(UpePreis) ? "" : string.Format("{0} €", UpePreis)); } }
    }
}
