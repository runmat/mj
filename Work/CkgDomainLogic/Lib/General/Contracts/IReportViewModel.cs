using System;
using System.Collections;

namespace CkgDomainLogic.General.Contracts
{
    public interface IReportViewModel
    {
        object ReportSelector { get; set; }

        IEnumerable ReportItems { get; }

        void ReportItemsLoad(Action<string, string> addModelError);
    }
}
