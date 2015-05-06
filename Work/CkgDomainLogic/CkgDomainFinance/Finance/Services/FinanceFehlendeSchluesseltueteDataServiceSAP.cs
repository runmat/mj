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
    public class FinanceFehlendeSchluesseltueteDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceFehlendeSchluesseltueteDataService
    {
        public List<FehlendeSchluesseltuete> FehlendeSchluesseltuetes { get { return PropertyCacheGet(() => LoadFehlendeSchluesseltuetenFromSap()); } }

         public FinanceFehlendeSchluesseltueteDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }


        public void DeleteFehlendeSchluesseltueteToSap(FehlendeSchluesseltuete item)
        {                        
            Z_M_Schluesselverloren.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            item.KUNNR = LogonContext.KundenNr.ToSapKunnr();
            
            var lst = AppModelMappings.Z_M_Schluesselverloren_GT_WEB_IN_From_FehlendeSchluesseltuete.CopyBack(new List<FehlendeSchluesseltuete>() {item}).ToList();           

            SAP.ApplyImport(lst);            
            SAP.Execute();                                   
        }

        public void MarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FehlendeSchluesseltuetes);
        }

        private List<FehlendeSchluesseltuete>LoadFehlendeSchluesseltuetenFromSap()
        {
            Z_M_SCHLUESSELDIFFERENZEN.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            var sapList = Z_M_SCHLUESSELDIFFERENZEN.GT_WEB_OUT_BRIEFE.GetExportListWithExecute(SAP);

            Z_M_SCHLUESSELDIFFERENZEN.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());
            var herList = Z_M_SCHLUESSELDIFFERENZEN.GT_WEB_OUT_HERST.GetExportListWithExecute(SAP);

            var webList = AppModelMappings.Z_M_SCHLUESSELDIFFERENZEN_To_FehlendeSchluesseltuete.Copy(sapList).ToList();

            foreach (var item in webList)
            {
                var hersteller = herList.Where(x => x.CHASSIS_NUM == item.Fahrgestellnummer);
                if (hersteller != null && hersteller.Count() > 0)
                    item.Hersteller = hersteller.FirstOrDefault().HERST_T;
            }

            return webList;
        }     

    }
}
