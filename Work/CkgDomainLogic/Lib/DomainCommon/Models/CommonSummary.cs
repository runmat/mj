using System.Collections.Generic;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class CommonSummary : CommonUiModel
    {
        public string Footer { get; set; }

        public List<GeneralEntity> Items { get; set; }
    }
}
