using System;
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
    public class FinanceVersandsperreDataServiceSAP : CkgGeneralDataServiceSAP, IFinanceVersandsperreDataService
    {
        public VorgangVersandperreSuchparameter Suchparameter { get; set; }

        public List<VorgangVersandsperre> Vorgaenge { get { return PropertyCacheGet(() => LoadVorgaengeFromSap().ToList()); } }

        public FinanceVersandsperreDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
            Suchparameter = new VorgangVersandperreSuchparameter{ Sperrtyp = "N" };
        }

        public void MarkForRefreshVorgaenge()
        {
            PropertyCacheClear(this, m => m.Vorgaenge);
        }

        private IEnumerable<VorgangVersandsperre> LoadVorgaengeFromSap()
        {
            var liste = new List<VorgangVersandsperre>();

            Z_DPM_VERTRAGSBESTAND_01.Init(SAP, "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            if (Suchparameter.Vertragsart != "alle")
            {
                SAP.SetImportParameter("I_VERT_ART", Suchparameter.Vertragsart);
            }
            if (!String.IsNullOrEmpty(Suchparameter.Kontonummer))
            {
                SAP.SetImportParameter("I_KONTONR", Suchparameter.Kontonummer);
            }
            if (!String.IsNullOrEmpty(Suchparameter.CIN))
            {
                SAP.SetImportParameter("I_CIN", Suchparameter.CIN);
            }
            if (!String.IsNullOrEmpty(Suchparameter.PAID))
            {
                SAP.SetImportParameter("I_PAID", Suchparameter.PAID);
            }

            var sapList = Z_DPM_VERTRAGSBESTAND_01.GT_OUT.GetExportListWithExecute(SAP);

            foreach (var item in sapList)
            {
                var sapList2 = Z_M_BRIEFLEBENSLAUF_001.GT_WEB.GetExportListWithInitExecute(SAP, "I_KUNNR, I_ZZREF1", LogonContext.KundenNr.ToSapKunnr(), item.PAID);
                if (sapList2 != null && sapList2.Count > 0)
                {
                    liste.Add(new VorgangVersandsperre
                        {
                            Vertragsart = item.ZVERT_ART,
                            Kontonummer = item.KONTONR,
                            CIN = item.CIN,
                            PAID = item.PAID,
                            Fahrgestellnummer = item.CHASSIS_NUM,
                            Equipmentnummer = sapList2[0].EQUNR,
                            Versandsperre = (sapList2[0].ZZAKTSPERRE == "X")
                        });
                }
            }
            
            return liste;
        }

        public List<VorgangVersandsperre> SaveVersandsperren(List<VorgangVersandsperre> vorgaenge, ref string message)
        {
            List<VorgangVersandsperre> erg = new List<VorgangVersandsperre>();

            Z_DPM_ZZAKTSPERRE.Init(SAP, "I_AG, I_VERKZ, I_WEB_USER", LogonContext.KundenNr.ToSapKunnr(), Suchparameter.Sperrtyp, LogonContext.UserName);

            var vList = AppModelMappings.Z_DPM_ZZAKTSPERRE_GT_WEB_From_VorgangVersandsperre.CopyBack(vorgaenge).ToList();
            SAP.ApplyImport(vList);

            SAP.Execute();

            if (SAP.ResultCode == 0)
            {
                var outList = Z_DPM_ZZAKTSPERRE.GT_WEB.GetExportList(SAP);

                foreach (var vs in vorgaenge)
                {
                    var outItem = outList.Find(v => v.LIZNR == vs.PAID);

                    if (String.IsNullOrEmpty(outItem.MESSAGE))
                    {
                        vs.Status = Localize.OK;
                    }
                    else
                    {
                        vs.Status = Localize.Error + ": " + outItem.MESSAGE;
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
