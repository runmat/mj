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
    public class HalterabweichungenDataServiceSAP : CkgGeneralDataServiceSAP, IHalterabweichungenDataService
    {
        public List<Halterabweichung> Halterabweichungen { get { return PropertyCacheGet(() => LoadHalterabweichungenFromSap().ToList()); } }

        public HalterabweichungenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshHalterabweichungen()
        {
            PropertyCacheClear(this, m => m.Halterabweichungen);
        }

        private IEnumerable<Halterabweichung> LoadHalterabweichungenFromSap()
        {
            var sapList = Z_DPM_DAT_MIT_ABW_ZH_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_DAT_MIT_ABW_ZH_01_GT_OUT_To_Halterabweichung.Copy(sapList);
        }

        public List<Halterabweichung> SaveHalterabweichungen(List<Halterabweichung> vorgaenge, ref string message)
        {
            List<Halterabweichung> erg = new List<Halterabweichung>();

            Z_DPM_SET_ZH_ABW_ERL_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            var vList = AppModelMappings.Z_DPM_SET_ZH_ABW_ERL_01_GT_TAB_From_Halterabweichung.CopyBack(vorgaenge).ToList();
            SAP.ApplyImport(vList);

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                var outList = Z_DPM_SET_ZH_ABW_ERL_01.GT_TAB.GetExportList(SAP);

                foreach (var vs in vorgaenge)
                {
                    var outItem = outList.Find(v => v.CHASSIS_NUM == vs.Fahrgestellnummer);

                    if (String.IsNullOrEmpty(outItem.RET_BEM))
                    {
                        vs.Status = Localize.OK;
                    }
                    else
                    {
                        vs.Status = Localize.Error + ": " + outItem.RET_BEM;
                    }

                    erg.Add(vs);
                }
            }
            else
            {
                message = Localize.SaveFailed + ": " + SAP.ResultMessage;
            }

            return erg;
        }
    }
}
