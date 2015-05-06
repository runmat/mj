using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Finance.Contracts;
using CkgDomainLogic.Finance.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Finance.Models.AppModelMappings;


namespace CkgDomainLogic.Finance.Services
{
    public class FinanceTempVersandZweitschluesselDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceTempZb2VersandZweitschluesselDataService
    {

        public List<TempVersandZweitschluessel> TempVersandZweitschluessels { get { return PropertyCacheGet(() => LoadTempVersandZweitschluesselFromSap().ToList()); } }

        public FinanceTempVersandZweitschluesselDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefresh()
        {
            PropertyCacheClear(this, m => m.TempVersandZweitschluessels);
        }

        IEnumerable<TempVersandZweitschluessel> LoadTempVersandZweitschluesselFromSap()
        {
            Z_M_SCHLUE_TEMP_VERS_MAHN_001.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            var sapList = Z_M_SCHLUE_TEMP_VERS_MAHN_001.GT_WEB.GetExportListWithExecute(SAP);
            
            return AppModelMappings.Z_M_SCHLUE_TEMP_VERS_MAHN_001_To_TempVersandZweitschluessel.Copy(sapList);
        }


        public void SetTempVersandZweitschluesselMahnsperreToSap(string eqnr)
        {
            Z_M_SCHLUE_SET_MAHNSP_001.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("I_EQUNR", eqnr);
            SAP.SetImportParameter("I_ZZMANSP", "X");

            SAP.Execute();           
        }    
    }
}
