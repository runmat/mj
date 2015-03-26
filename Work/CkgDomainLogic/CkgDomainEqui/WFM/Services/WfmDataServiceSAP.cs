using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.WFM.Contracts;
using CkgDomainLogic.WFM.Models;
using GeneralTools.Models;
using SapORM.Contracts;
using SapORM.Models;
using AppModelMappings = CkgDomainLogic.WFM.Models.AppModelMappings;

namespace CkgDomainLogic.WFM.Services
{
    public class WfmDataServiceSAP : CkgGeneralDataServiceSAP, IWfmDataService
    {
        public WfmDataServiceSAP(ISapDataService sap)
            :base(sap)
        {
        }

        public List<WfmAuftragFeldname> GetFeldnamen()
        {
            Z_WFM_READ_KONVERTER_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            return AppModelMappings.Z_WFM_READ_KONVERTER_01_GT_DATEN_To_WfmAuftragFeldname.Copy(Z_WFM_READ_KONVERTER_01.GT_DATEN.GetExportListWithExecute(SAP)).ToList();
        }

        public List<WfmAuftrag> GetAbmeldeauftraege(WfmAuftragSelektor selector)
        {
            Z_WFM_READ_AUFTRAEGE_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!String.IsNullOrEmpty(selector.Selektionsfeld1Name))
                SAP.SetImportParameter("I_SELEKTION1", selector.Selektionsfeld1.BoolToX());

            if (!String.IsNullOrEmpty(selector.Selektionsfeld2Name))
                SAP.SetImportParameter("I_SELEKTION2", selector.Selektionsfeld2.BoolToX());

            if (!String.IsNullOrEmpty(selector.Selektionsfeld3Name))
                SAP.SetImportParameter("I_SELEKTION3", selector.Selektionsfeld3.BoolToX());

            if (!String.IsNullOrEmpty(selector.FahrgestellNr))
                SAP.SetImportParameter("I_FIN", selector.FahrgestellNr);

            if (!String.IsNullOrEmpty(selector.Kennzeichen))
                SAP.SetImportParameter("I_KENNZ", selector.Kennzeichen);

            if (!String.IsNullOrEmpty(selector.KundenAuftragsNr))
            {
                var kdAufNrList = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_KDAUF_From_KundenAuftragsNrSelektion.CopyBack(new List<KundenAuftragsNrSelektion> { new KundenAuftragsNrSelektion { KundenAuftragsNrVon = selector.KundenAuftragsNr } });
                SAP.ApplyImport(kdAufNrList);
            }

            if (!String.IsNullOrEmpty(selector.Referenz1))
            {
                var ref1List = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF1_From_Referenz1Selektion.CopyBack(new List<Referenz1Selektion> { new Referenz1Selektion { Referenz1Von = selector.Referenz1 } });
                SAP.ApplyImport(ref1List);
            }

            if (!String.IsNullOrEmpty(selector.Referenz2))
            {
                var ref2List = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF2_From_Referenz2Selektion.CopyBack(new List<Referenz2Selektion> { new Referenz2Selektion { Referenz2Von = selector.Referenz2 } });
                SAP.ApplyImport(ref2List);
            }

            if (!String.IsNullOrEmpty(selector.Referenz3))
            {
                var ref3List = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF3_From_Referenz3Selektion.CopyBack(new List<Referenz3Selektion> { new Referenz3Selektion { Referenz3Von = selector.Referenz3 } });
                SAP.ApplyImport(ref3List);
            }

            if (selector.Abmeldearten.AnyAndNotNull())
            {
                var abmArtList = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_ABMART_From_AbmeldeArtSelektion.CopyBack(selector.Abmeldearten.Select(a => new AbmeldeArtSelektion { AbmeldeArt = a }));
                SAP.ApplyImport(abmArtList);
            }

            if (selector.Abmeldestatus.AnyAndNotNull())
            {
                var abmStatList = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_ABMSTAT_From_AbmeldeStatusSelektion.CopyBack(selector.Abmeldestatus.Select(a => new AbmeldeStatusSelektion { AbmeldeStatus = a }));
                SAP.ApplyImport(abmStatList);
            }

            return AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_DATEN_To_WfmAuftrag.Copy(Z_WFM_READ_AUFTRAEGE_01.GT_DATEN.GetExportListWithExecute(SAP)).ToList();
        }

        public List<WfmInfo> GetInfos(string vorgangsNr)
        {
            Z_WFM_READ_INFO_01.Init(SAP, "I_AG, I_VORG_NR_ABM_AUF", LogonContext.KundenNr.ToSapKunnr(), vorgangsNr);

            return AppModelMappings.Z_WFM_READ_INFO_01_GT_DATEN_To_WfmInfo.Copy(Z_WFM_READ_INFO_01.GT_DATEN.GetExportListWithExecute(SAP)).ToList();
        }

        public List<WfmDokumentInfo> GetDokumentInfos(string vorgangsNr)
        {
            Z_WFM_LIST_DOKU_01.Init(SAP, "I_AG, I_VORG_NR_ABM_AUF", LogonContext.KundenNr.ToSapKunnr(), vorgangsNr);

            return AppModelMappings.Z_WFM_LIST_DOKU_01_GT_DOKUMENTE_To_WfmDokumentInfo.Copy(Z_WFM_LIST_DOKU_01.GT_DOKUMENTE.GetExportListWithExecute(SAP)).ToList();
        }

        public List<WfmToDo> GetToDos(string vorgangsNr)
        {
            Z_WFM_READ_TODO_01.Init(SAP, "I_AG, I_VORG_NR_ABM_AUF_VON", LogonContext.KundenNr.ToSapKunnr(), vorgangsNr);

            return AppModelMappings.Z_WFM_READ_TODO_01_GT_DATEN_To_WfmToDo.Copy(Z_WFM_READ_TODO_01.GT_DATEN.GetExportListWithExecute(SAP)).ToList();
        }
    }
}
