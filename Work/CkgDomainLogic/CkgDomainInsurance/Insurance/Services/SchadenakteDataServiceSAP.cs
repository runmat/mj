// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.Insurance.Models.AppModelMappings;

namespace CkgDomainLogic.Insurance.Services
{
    public class SchadenakteDataServiceSAP : CkgGeneralDataServiceSAP, ISchadenakteDataService
    {
        public SchadenakteDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }


        #region Schadenfälle

        public List<Schadenfall> SchadenfaelleGet()
        {
            var sapList = Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R");

            return AppModelMappings.Z_DPM_TAB_ZEVENT_SCHADEN_01_GT_SCHADEN_To_Schadenfall.Copy(sapList).Where(item => item.LoeschDatum == null).ToList();
        }

        public Schadenfall SchadenfallAdd(Schadenfall item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION",
                LogonContext.KundenNr.ToSapKunnr(),
                "I");

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_SCHADEN_01_GT_SCHADEN_To_Schadenfall.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            var exportList = Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN.GetExportList(SAP);
            var savedSapItem = exportList.FirstOrDefault();
            if (savedSapItem != null && savedSapItem.ERROR.IsNotNullOrEmpty())
                addModelError("", FormatSapErrorMessage(savedSapItem.ERROR));

            return AppModelMappings.Z_DPM_TAB_ZEVENT_SCHADEN_01_GT_SCHADEN_To_Schadenfall.Copy(savedSapItem);
        }

        public bool SchadenfallDelete(int id)
        {
            var sapList = Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT_SCHADEN",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        id.ToString().PadLeft0(10)
                        );
            var sapItem = sapList.FirstOrDefault();
            if (sapItem == null)
                return false;

            var item = AppModelMappings.Z_DPM_TAB_ZEVENT_SCHADEN_01_GT_SCHADEN_To_Schadenfall.Copy(sapItem);
            item.LoeschDatum = DateTime.Now;
            item.LoeschUser = LogonContext.UserName;

            SchadenfallSave(item, null);

            return true;
        }

        public Schadenfall SchadenfallSave(Schadenfall item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION, I_EVENT_SCHADEN",
                LogonContext.KundenNr.ToSapKunnr(),
                "U",
                item.ID.ToString().PadLeft0(10));

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_SCHADEN_01_GT_SCHADEN_To_Schadenfall.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            var exportList = Z_DPM_TAB_ZEVENT_SCHADEN_01.GT_SCHADEN.GetExportList(SAP);
            var savedSapItem = exportList.FirstOrDefault();
            if (savedSapItem != null && savedSapItem.ERROR.IsNotNullOrEmpty())
                addModelError("", FormatSapErrorMessage(savedSapItem.ERROR));

            return item;
        }

        #endregion


        #region Schadenfall Status Arten

        public List<SchadenfallStatusArt> SchadenfallStatusArtenGet(string languageKey)
        {
            var sapList = Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATART.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_SCHADEN, I_SPRAS, I_PROZESSNR",
                        LogonContext.KundenNr.ToSapKunnr(),
                        9999999999.ToString(),
                        languageKey,
                        null
                        );

            return AppModelMappings.Z_DPM_EVENT_READ_SCHAD_STAT_01_GT_STATART_To_SchadenfallStatusArt.Copy(sapList).OrderBy(item => item.Sort).ToList();
        }

        #endregion


        #region Schadenfall Status (Werte)

        public List<SchadenfallStatus> SchadenfallStatusWerteGet(string languageKey, int? schadenfallID = null)
        {
            var sapList = Z_DPM_EVENT_READ_SCHAD_STAT_01.GT_STATUS.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_SCHADEN, I_SPRAS, I_PROZESSNR",
                        LogonContext.KundenNr.ToSapKunnr(),
                        schadenfallID == null ? null : schadenfallID.GetValueOrDefault().ToString().PadLeft0(10),
                        languageKey,
                        null
                        );

            return AppModelMappings.Z_DPM_EVENT_READ_SCHAD_STAT_01_GT_STATUS_To_SchadenfallStatus.Copy(sapList).OrderBy(item => item.Sort).ToList();
        }

        public bool SchadenfallStatusWertSave(SchadenfallStatus schadenfallStatus, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_EVENT_SET_SCHAD_STAT_01.GT_STATUS.GetImportListWithInit(SAP,
                "I_KUNNR_AG",
                LogonContext.KundenNr.ToSapKunnr());

            sapList.Add(AppModelMappings.Z_DPM_EVENT_SET_SCHAD_STAT_01_GT_STATUS_To_SchadenfallStatus.CopyBack(schadenfallStatus));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            var exportList = Z_DPM_EVENT_SET_SCHAD_STAT_01.GT_STATUS.GetExportList(SAP);
            var savedSapItem = exportList.FirstOrDefault();
            if (savedSapItem != null && savedSapItem.ERROR.IsNotNullOrEmpty())
            {
                addModelError("", FormatSapErrorMessage(savedSapItem.ERROR));
                return false;
            }

            return true;
        }

        #endregion

    }
}
