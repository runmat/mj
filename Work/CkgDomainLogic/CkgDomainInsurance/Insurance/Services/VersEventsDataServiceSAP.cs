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
    public class VersEventsDataServiceSAP : CkgGeneralDataServiceSAP, IVersEventsDataService
    {
        public VersEventsDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        #region Termine

        public List<TerminSchadenfall> TermineGet(Schadenfall schadenfall = null, int boxID = 0)
        {
            var sapList = Z_DPM_TAB_ZEVENT_TERMIN_01.GT_TERMIN.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT_SCHADEN, I_EVENT_ORT_BOX",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        schadenfall == null ? null : schadenfall.ID.ToString(),
                        boxID == 0 ? null : boxID.ToString()
                        );

            return AppModelMappings.Z_DPM_TAB_ZEVENT_TERMIN_01_GT_TERMIN_To_TerminSchadenfall.Copy(sapList).Where(item => item.LoeschDatum == null).ToList();
        }

        public List<TerminSchadenfall> TermineAllGet()
        {
            var sapList = Z_DPM_TAB_ZEVENT_TERMIN_02.GT_TERMIN.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_DPM_TAB_ZEVENT_TERMIN_02_GT_TERMIN_To_TerminSchadenfall.Copy(sapList).Where(item => item.LoeschDatum == null).ToList();
        }

        public TerminSchadenfall TerminAdd(TerminSchadenfall item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_TERMIN_01.GT_TERMIN.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION",
                LogonContext.KundenNr.ToSapKunnr(),
                "I");

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_TERMIN_01_GT_TERMIN_To_TerminSchadenfall.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            var exportList = Z_DPM_TAB_ZEVENT_TERMIN_01.GT_TERMIN.GetExportList(SAP);
            var savedSapItem = exportList.FirstOrDefault();
            if (savedSapItem != null && savedSapItem.ERROR.IsNotNullOrEmpty())
                addModelError("", FormatSapErrorMessage(savedSapItem.ERROR));

            return AppModelMappings.Z_DPM_TAB_ZEVENT_TERMIN_01_GT_TERMIN_To_TerminSchadenfall.Copy(savedSapItem);
        }

        public bool TerminDelete(int id)
        {
            var sapList = Z_DPM_TAB_ZEVENT_TERMIN_01.GT_TERMIN.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT_TERMIN",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        id.ToString().PadLeft0(10)
                        );
            var sapItem = sapList.FirstOrDefault();
            if (sapItem == null)
                return false;

            var item = AppModelMappings.Z_DPM_TAB_ZEVENT_TERMIN_01_GT_TERMIN_To_TerminSchadenfall.Copy(sapItem);
            item.LoeschDatum = DateTime.Now;
            item.LoeschUser = LogonContext.UserName;

            TerminSave(item, null);

            return true;
        }

        public TerminSchadenfall TerminSave(TerminSchadenfall item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_TERMIN_01.GT_TERMIN.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION, I_EVENT_TERMIN",
                LogonContext.KundenNr.ToSapKunnr(),
                "U",
                item.ID.ToString().PadLeft0(10));

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_TERMIN_01_GT_TERMIN_To_TerminSchadenfall.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            return item;
        }

        #endregion

        
        #region Events

        public List<VersEvent> VersEventsGet()
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION",
                        LogonContext.KundenNr.ToSapKunnr(), 
                        "R");

            return AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_01_GT_EVENT_To_VersEvent.Copy(sapList).Where(item => item.LoeschDatum == null).ToList();
        }

        public VersEvent VersEventAdd(VersEvent item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION",
                LogonContext.KundenNr.ToSapKunnr(),
                "I");

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_01_GT_EVENT_To_VersEvent.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            var exportList = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetExportList(SAP);
            var savedSapItem = exportList.FirstOrDefault();
            if (savedSapItem != null && savedSapItem.ERROR.IsNotNullOrEmpty())
                addModelError("", FormatSapErrorMessage(savedSapItem.ERROR));

            return AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_01_GT_EVENT_To_VersEvent.Copy(savedSapItem);
        }

        public bool VersEventDelete(int id)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        id.ToString().PadLeft0(10)
                        );
            var sapItem = sapList.FirstOrDefault();
            if (sapItem == null)
                return false;

            var item = AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_01_GT_EVENT_To_VersEvent.Copy(sapItem);
            item.LoeschDatum = DateTime.Now;
            item.LoeschUser = LogonContext.UserName;

            VersEventSave(item, null);

            return true;
        }

        public VersEvent VersEventSave(VersEvent item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_01.GT_EVENT.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION, I_EVENT",
                LogonContext.KundenNr.ToSapKunnr(),
                "U",
                item.ID.ToString().PadLeft0(10));

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_01_GT_EVENT_To_VersEvent.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            return item;
        }

        #endregion


        #region Orte

        public List<VersEventOrt> VersEventOrteGet(VersEvent versEvent = null)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_02.GT_ORT.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        versEvent == null ? null : versEvent.ID.ToString().PadLeft0(10)
                        );

            return AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_02_GT_ORT_To_VersEventOrt.Copy(sapList).Where(item => item.LoeschDatum == null).ToList();
        }

        public VersEventOrt VersEventOrtAdd(VersEventOrt item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_02.GT_ORT.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION",
                LogonContext.KundenNr.ToSapKunnr(),
                "I");

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_02_GT_ORT_To_VersEventOrt.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            var exportList = Z_DPM_TAB_ZEVENT_KONFIG_02.GT_ORT.GetExportList(SAP);
            var savedSapItem = exportList.FirstOrDefault();
            if (savedSapItem != null && savedSapItem.ERROR.IsNotNullOrEmpty())
                addModelError("", FormatSapErrorMessage(savedSapItem.ERROR));

            return AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_02_GT_ORT_To_VersEventOrt.Copy(savedSapItem);
        }

        public bool VersEventOrtDelete(int id)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_02.GT_ORT.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT_ORT",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        id.ToString().PadLeft0(10)
                        );
            var sapItem = sapList.FirstOrDefault();
            if (sapItem == null)
                return false;

            var item = AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_02_GT_ORT_To_VersEventOrt.Copy(sapItem);
            item.LoeschDatum = DateTime.Now;
            item.LoeschUser = LogonContext.UserName;

            VersEventOrtSave(item, null);

            return true;
        }

        public VersEventOrt VersEventOrtSave(VersEventOrt item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_02.GT_ORT.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION, I_EVENT_ORT",
                LogonContext.KundenNr.ToSapKunnr(),
                "U",
                item.ID.ToString().PadLeft0(10));

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_02_GT_ORT_To_VersEventOrt.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            return item;
        }

        #endregion


        #region Boxen

        public List<VersEventOrtBox> VersEventOrtBoxenGet(VersEventOrt versEventOrt)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT_ORT",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        versEventOrt == null ? null : versEventOrt.ID.ToString().PadLeft0(10)
                        );

            return AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_03_GT_BOX_To_VersEventOrtBox.Copy(sapList).Where(item => item.LoeschDatum == null).ToList();
        }

        public VersEventOrtBox VersEventOrtBoxAdd(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION",
                LogonContext.KundenNr.ToSapKunnr(),
                "I");

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_03_GT_BOX_To_VersEventOrtBox.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            var exportList = Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX.GetExportList(SAP);
            var savedSapItem = exportList.FirstOrDefault();
            if (savedSapItem != null && savedSapItem.ERROR.IsNotNullOrEmpty())
                addModelError("", FormatSapErrorMessage(savedSapItem.ERROR));

            return AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_03_GT_BOX_To_VersEventOrtBox.Copy(savedSapItem);
        }

        public bool VersEventOrtBoxDelete(int id)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX.GetExportListWithInitExecute(SAP,
                        "I_KUNNR_AG, I_AKTION, I_EVENT_ORT_BOX",
                        LogonContext.KundenNr.ToSapKunnr(),
                        "R",
                        id.ToString().PadLeft0(10)
                        );
            var sapItem = sapList.FirstOrDefault();
            if (sapItem == null)
                return false;

            var item = AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_03_GT_BOX_To_VersEventOrtBox.Copy(sapItem);
            item.LoeschDatum = DateTime.Now;
            item.LoeschUser = LogonContext.UserName;

            VersEventOrtBoxSave(item, null);

            return true;
        }

        public VersEventOrtBox VersEventOrtBoxSave(VersEventOrtBox item, Action<string, string> addModelError)
        {
            var sapList = Z_DPM_TAB_ZEVENT_KONFIG_03.GT_BOX.GetImportListWithInit(SAP,
                "I_KUNNR_AG, I_AKTION, I_EVENT_ORT_BOX",
                LogonContext.KundenNr.ToSapKunnr(),
                "U",
                item.ID.ToString().PadLeft0(10));

            sapList.Add(AppModelMappings.Z_DPM_TAB_ZEVENT_KONFIG_03_GT_BOX_To_VersEventOrtBox.CopyBack(item));

            SAP.ApplyImport(sapList);
            SAP.Execute();

            return item;
        }

        #endregion
    }
}
