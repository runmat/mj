using System.Collections.Generic;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Finance.Models;

namespace CkgDomainLogic.Finance.Contracts
{
    public interface IFinanceTempZb2VersandZweitschluesselDataService
    {
        List<TempVersandZweitschluessel> TempVersandZweitschluessels { get; }

        void SetTempVersandZweitschluesselMahnsperreToSap(string eqnr);
      
        void MarkForRefresh();        
    }
}
