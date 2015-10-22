using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Leasing.Contracts;
using CkgDomainLogic.Leasing.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Leasing.Models.AppModelMappings;

namespace CkgDomainLogic.Leasing.Services
{
    public class UeberfaelligeRuecksendungenDataServiceSAP : CkgGeneralDataServiceSAP, IUeberfaelligeRuecksendungenDataService
    {
        public UeberfaelligeRuecksendungenSuchparameter Suchparameter { get; set; }

        public List<UeberfaelligeRuecksendung> UeberfaelligeRuecksendungen { get { return PropertyCacheGet(() => LoadUeberfaelligeRuecksendungenFromSap().ToList()); } }

        public UeberfaelligeRuecksendungenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new UeberfaelligeRuecksendungenSuchparameter();
        }

        public void MarkForRefreshUeberfaelligeRuecksendungen()
        {
            PropertyCacheClear(this, m => m.UeberfaelligeRuecksendungen);
        }

        private IEnumerable<UeberfaelligeRuecksendung> LoadUeberfaelligeRuecksendungenFromSap()
        {
            Z_M_FAELLIGE_EQUI_LP.Init(SAP, "I_KUNNR, I_FAELLIGKEIT", LogonContext.KundenNr.ToSapKunnr(), Suchparameter.AnzahlTage);

            return AppModelMappings.Z_M_FAELLIGE_EQUI_LP_GT_WEB_To_UeberfaelligeRuecksendung.Copy(Z_M_FAELLIGE_EQUI_LP.GT_WEB.GetExportListWithExecute(SAP));
        }

        public void SaveUeberfaelligeRuecksendung(UeberfaelligeRuecksendung item, bool fristVerlaengern = false)
        {
            Z_M_FAELLIGE_EQUI_UPDATE_LP.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("I_EQUNR", item.EquiNr);
            SAP.SetImportParameter("I_TEXT200", item.Memo);

            if (fristVerlaengern)
                SAP.SetImportParameter("I_FALLIG_VERLAENGERN", "X");

            SAP.Execute();
        }
    }
}
