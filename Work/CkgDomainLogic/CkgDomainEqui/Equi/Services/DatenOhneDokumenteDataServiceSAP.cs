using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class DatenOhneDokumenteDataServiceSAP : CkgGeneralDataServiceSAP, IDatenOhneDokumenteDataService
    {
        public DatenOhneDokumenteFilter DatenFilter { get; set; }

        private List<DatenOhneDokumente> DatenOhneDokumenteGesamt { get { return PropertyCacheGet(() => LoadDatenOhneDokumenteFromSap().ToList()); } }

        public List<DatenOhneDokumente> DatenOhneDokumente 
        { 
            get
            {
                if (DatenFilter.Selektion == "K")
                {
                    return DatenOhneDokumenteGesamt.Where(b => b.Vertragsstatus == "1").ToList();
                }
                return DatenOhneDokumenteGesamt;
            } 
        }

        public DatenOhneDokumenteDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            DatenFilter = new DatenOhneDokumenteFilter();
        }

        public void MarkForRefreshDatenOhneDokumente()
        {
            PropertyCacheClear(this, m => m.DatenOhneDokumenteGesamt);
        }

        private IEnumerable<DatenOhneDokumente> LoadDatenOhneDokumenteFromSap()
        {
            var sapList = Z_DPM_DAT_OHNE_DOKUMENT_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_DAT_OHNE_DOKUMENT_01_GT_OUT_To_DatenOhneDokumente.Copy(sapList);
        }

        public List<DatenOhneDokumente> SaveDatenOhneDokumente(List<DatenOhneDokumente> vorgaenge, ref string message)
        {
            List<DatenOhneDokumente> erg = new List<DatenOhneDokumente>();

            Z_DPM_MARK_DAT_OHNE_DOKUM_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            var vList = AppModelMappings.Z_DPM_MARK_DAT_OHNE_DOKUM_01_GT_DAT_From_DatenOhneDokumente.CopyBack(vorgaenge).ToList();
            SAP.ApplyImport(vList);

            SAP.Execute();

            var outList = Z_DPM_MARK_DAT_OHNE_DOKUM_01.GT_DAT.GetExportList(SAP);

            foreach (var vs in vorgaenge)
            {
                var outItem = outList.Find(v => v.CHASSIS_NUM == vs.Fahrgestellnummer);

                vs.Vertragsstatus = outItem.VERTRAGS_STAT;

                if (String.IsNullOrEmpty(outItem.BEM))
                {
                    if (vs.Loeschkennzeichen)
                    {
                        vs.Status = Localize.Deleted;
                    }
                    else
                    {
                        vs.Status = Localize.OK;
                    }
                }
                else
                {
                    vs.Status = Localize.Error + ": " + outItem.BEM;
                }

                erg.Add(vs);
            }

            if (vorgaenge.Any(v => v.Status != Localize.OK && v.Status != Localize.Deleted))
            {
                message = Localize.SaveFailed + ": " + SAP.ResultMessage;
            }

            return erg;
        }
    }
}
