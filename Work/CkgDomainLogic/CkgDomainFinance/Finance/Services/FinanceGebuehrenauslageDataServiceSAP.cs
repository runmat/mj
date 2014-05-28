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
    public class FinanceGebuehrenauslageDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceGebuehrenauslageDataService
    {
        public AuftragSuchparameter Suchparameter { get; set; }
        public List<Auftrag> Auftraege { get { return PropertyCacheGet(() => LoadAuftraegeFromSap().ToList()); } }

        public FinanceGebuehrenauslageDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new AuftragSuchparameter();
        }

        public void MarkForRefreshAuftraege()
        {
            PropertyCacheClear(this, m => m.Auftraege);
        }

        private IEnumerable<Auftrag> LoadAuftraegeFromSap()
        {
            Z_DPM_EXP_GBA_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.WunschlieferdatumVon.HasValue)
            {
                SAP.SetImportParameter("I_VDATU_VON", Suchparameter.WunschlieferdatumVon.Value);
            }
            if (Suchparameter.WunschlieferdatumBis.HasValue)
            {
                SAP.SetImportParameter("I_VDATU_BIS", Suchparameter.WunschlieferdatumBis.Value);
            }
            if (Suchparameter.Selektion == Selektionsart.AlleGebuehren)
            {
                SAP.SetImportParameter("I_ERL_INCL", "X");
            }

            var sapList = Z_DPM_EXP_GBA_01.GT_OUT.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_EXP_GBA_01_GT_OUT_To_Auftrag.Copy(sapList);
        }

        public void SaveAuftrag(Auftrag auftr)
        {
            var auftraege = new List<Auftrag>{ auftr };

            Z_DPM_SAVE_GBA_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            var aList = AppModelMappings.Z_DPM_SAVE_GBA_01_GT_IN_From_Auftrag.CopyBack(auftraege).ToList();
            SAP.ApplyImport(aList);

            SAP.Execute();
        }
    }
}
