using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Equi.Contracts;
using CkgDomainLogic.Equi.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Equi.Models.AppModelMappings;

namespace CkgDomainLogic.Equi.Services
{
    public class AbweichungenDataServiceSAP : CkgGeneralDataServiceSAP, IAbweichungenDataService
    {
        #region Halterabweichungen

        public List<Halterabweichung> Halterabweichungen
        {
            get { return PropertyCacheGet(() => LoadHalterabweichungenFromSap().ToList()); }
        }

        public AbweichungenDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkForRefreshHalterabweichungen()
        {
            PropertyCacheClear(this, m => m.Halterabweichungen);
        }

        private IEnumerable<Halterabweichung> LoadHalterabweichungenFromSap()
        {
            var sapList = Z_DPM_DAT_MIT_ABW_ZH_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_AG",
                                                                                      LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_DAT_MIT_ABW_ZH_01_GT_OUT_To_Halterabweichung.Copy(sapList);
        }

        public List<Halterabweichung> SaveHalterabweichungen(List<Halterabweichung> vorgaenge, ref string message)
        {
            var erg = new List<Halterabweichung>();

            Z_DPM_SET_ZH_ABW_ERL_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            var vList =
                AppModelMappings.Z_DPM_SET_ZH_ABW_ERL_01_GT_TAB_From_Halterabweichung.CopyBack(vorgaenge).ToList();
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

        #endregion


        #region Versandabweichungen

        public List<Fahrzeugbrief> VersandAbweichungen
        {
            get
            {
                return AppModelMappings.Z_DPM_ABWEICH_ABRUFGRUND_02_GT_OUT_To_Fahrzeugbrief.Copy(GetSapVersandAbweichungen()).ToList();
            }
        }

        IEnumerable<Z_DPM_ABWEICH_ABRUFGRUND_02.GT_OUT> GetSapVersandAbweichungen()
        {
            return Z_DPM_ABWEICH_ABRUFGRUND_02.GT_OUT.GetExportListWithInitExecute(SAP,
                        "I_AG, I_ABWEICHUNG",
                        LogonContext.KundenNr.ToSapKunnr(), "AAG")
                    .ToList();
        }

        string SaveVersandAbweichungDetails(string equiNr, string memo, DateTime? ausgangsDatum, bool saveAsErledigt)
        {
            var error = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_M_Save_ZABWVERSGRUND.Init(SAP, "IMP_KUNNR", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("IMP_EQUNR", equiNr);
                    SAP.SetImportParameter("IMP_DATAUS", ausgangsDatum.GetValueOrDefault().ToString("dd.MM.yyyy"));
                    SAP.SetImportParameter("IMP_MEMO", memo);
                    if (saveAsErledigt)
                        SAP.SetImportParameter("IMP_ERLEDIGT", "X");
                    SAP.Execute();
                },

                // SAP custom error handling:
                () =>
                {
                    var sapResult = SAP.ResultMessage;
                    if (SAP.ResultMessage.IsNotNullOrEmpty())
                        return sapResult;

                    return "";
                });

            return error;
        }

        public string SaveVersandAbweichungMemo(string equiNr, string memo, DateTime? ausgangsDatum)
        {
            return SaveVersandAbweichungDetails(equiNr, memo, ausgangsDatum, false);
        }

        public string SaveVersandAbweichungAsErledigt(string equiNr, string memo, DateTime? ausgangsDatum)
        {
            return SaveVersandAbweichungDetails(equiNr, memo, ausgangsDatum, true);
        }

        #endregion
    }
}
