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

            if (selector.Modus == SelektionsModus.KlaerfallWorkplace)
            {
                SAP.SetImportParameter("I_TODO_WER", selector.ToDoWer);

                if (selector.SolldatumVonBis.IsSelected)
                {
                    var solldatumList = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_SOLLDAT_From_SolldatumSelektion.CopyBack(new List<SolldatumSelektion> { new SolldatumSelektion { SolldatumVonBis = selector.SolldatumVonBis } });
                    SAP.ApplyImport(solldatumList);
                }
            }

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

        #region Übersicht/Storno

        public string StornoAuftrag(string vorgangNr)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_WFM_STORNO_AUFTRAG_01.Init(SAP);
                    SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_VORG_NR_ABM_AUF", vorgangNr);
                    SAP.SetImportParameter("I_STORNODATUM", DateTime.Today);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () => ((SAP.ResultCode == 0) ? "" : SAP.ResultMessage.NotNullOr(Localize.CancellationFailed + ",  SAP Error Code: " + SAP.ResultCode)));

            return errorMessage;
        }


        #endregion

        #region Informationen

        public List<WfmInfo> GetInfos(string vorgangsNr)
        {
            Z_WFM_READ_INFO_01.Init(SAP, "I_AG, I_VORG_NR_ABM_AUF", LogonContext.KundenNr.ToSapKunnr(), vorgangsNr);

            return AppModelMappings.Z_WFM_READ_INFO_01_GT_DATEN_To_WfmInfo.Copy(Z_WFM_READ_INFO_01.GT_DATEN.GetExportListWithExecute(SAP)).ToList();
        }

        public string SaveNeueInformation(WfmInfo neueInfo)
        {
            Z_WFM_WRITE_INFO_01.Init(SAP, "I_AG, I_USER", LogonContext.KundenNr.ToSapKunnr(), LogonContext.UserName);

            var infoList = AppModelMappings.Z_WFM_WRITE_INFO_01_GT_DATEN_From_WfmInfo.CopyBack(new List<WfmInfo> { neueInfo }).ToList();
            SAP.ApplyImport(infoList);

            SAP.Execute();

            if (SAP.ResultCode != 0)
                return SAP.ResultMessage;

            var outList = Z_WFM_WRITE_INFO_01.GT_DATEN.GetExportList(SAP);

            if (outList.Any(o => !String.IsNullOrEmpty(o.ERR)))
                return outList.First(o => !String.IsNullOrEmpty(o.ERR)).ERR;

            return "";
        }

        #endregion

        #region Dokumente

        public List<WfmDokumentInfo> GetDokumentInfos(string vorgangsNr)
        {
            Z_WFM_LIST_DOKU_01.Init(SAP, "I_AG, I_VORG_NR_ABM_AUF", LogonContext.KundenNr.ToSapKunnr(), vorgangsNr);

            return AppModelMappings.Z_WFM_LIST_DOKU_01_GT_DOKUMENTE_To_WfmDokumentInfo.Copy(Z_WFM_LIST_DOKU_01.GT_DOKUMENTE.GetExportListWithExecute(SAP)).ToList();
        }

        public WfmDokument GetDokument(WfmDokumentInfo dokInfo)
        {
            Z_WFM_READ_DOKU_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            SAP.SetImportParameter("I_VORG_NR_ABM_AUF", dokInfo.VorgangsNrAbmeldeauftrag);
            SAP.SetImportParameter("I_AR_OBJECT", dokInfo.Dokumentart ?? "DOK");
            SAP.SetImportParameter("I_OBJECT_ID", dokInfo.ObjectId);

            SAP.Execute();

            var dok = AppModelMappings.Z_WFM_READ_DOKU_01_ES_DOKUMENT_To_WfmDokument.Copy(Z_WFM_READ_DOKU_01.ES_DOKUMENT.GetExportList(SAP)).FirstOrDefault();
            if (dok != null)
                dok.FileBytes = SAP.GetExportParameterByte("E_DOC");

            return dok;
        }

        public WfmDokumentInfo SaveDokument(string vorgangsNr, WfmDokument dok)
        {
            WfmDokumentInfo expDokInfo = null;

            var errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_WFM_WRITE_DOKU_01.Init(SAP, "I_AG, I_VORG_NR_ABM_AUF", LogonContext.KundenNr.ToSapKunnr(), vorgangsNr);

                    SAP.ApplyImport(AppModelMappings.Z_WFM_WRITE_DOKU_01_GS_DOKUMENT_From_WfmDokument.CopyBack(new List<WfmDokument> { dok }));
                    SAP.SetImportParameter("E_DOC", dok.FileBytes);

                    SAP.Execute();

                    expDokInfo = AppModelMappings.Z_WFM_WRITE_DOKU_01_ES_EXPORT_To_WfmDokumentInfo.Copy(Z_WFM_WRITE_DOKU_01.ES_EXPORT.GetExportList(SAP)).FirstOrDefault();
                    if (expDokInfo != null)
                        expDokInfo.VorgangsNrAbmeldeauftrag = vorgangsNr;
                },

                // SAP custom error handling:
                () => ((SAP.ResultCode == 0) ? "" : SAP.ResultMessage.NotNullOr(Localize.ErrorFileCouldNotBeSaved + ",  SAP Error Code: " + SAP.ResultCode)));


            if (errorMessage.IsNullOrEmpty() && expDokInfo != null)
                return expDokInfo;

            return new WfmDokumentInfo { ObjectId = null, ErrorMessage = Localize.UploadFailed };
        }

        #endregion

        #region Aufgaben

        public List<WfmToDo> GetToDos(string vorgangsNr)
        {
            Z_WFM_READ_TODO_01.Init(SAP, "I_AG, I_VORG_NR_ABM_AUF_VON", LogonContext.KundenNr.ToSapKunnr(), vorgangsNr);

            return AppModelMappings.Z_WFM_READ_TODO_01_GT_DATEN_To_WfmToDo.Copy(Z_WFM_READ_TODO_01.GT_DATEN.GetExportListWithExecute(SAP)).ToList();
        }

        public string ConfirmToDo(int vorgangsNr, int lfdNr, string remark)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_WFM_SET_STATUS_01.Init(SAP);
                    SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_USER", LogonContext.UserName);

                    var list = new List<Z_WFM_SET_STATUS_01.GT_DATEN>
                        {
                            new Z_WFM_SET_STATUS_01.GT_DATEN
                                {
                                    VORG_NR_ABM_AUF = vorgangsNr.ToString(),
                                    LFD_NR = lfdNr.ToString(),
                                    STATUS = "2",
                                    INS_FOLGE_TASK = "X",
                                    ANMERKUNG = remark,
                                }
                        };
                    SAP.ApplyImport(list);
                    SAP.Execute();


                    // clear remark in following To-Do (because SAP unfortununately also sets remark in follower To-Do)
                    var newToDoList = GetToDos(vorgangsNr.ToString().ToSapKunnr());
                    var followingToDoItem = newToDoList.FirstOrDefault(t => t.LaufendeNr.ToInt() == (lfdNr + 1));

                    Z_WFM_SET_STATUS_01.Init(SAP);
                    SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_USER", LogonContext.UserName);
                    if (followingToDoItem != null)
                    {
                        list = new List<Z_WFM_SET_STATUS_01.GT_DATEN>
                            {
                                new Z_WFM_SET_STATUS_01.GT_DATEN
                                    {
                                        VORG_NR_ABM_AUF = vorgangsNr.ToString(),
                                        LFD_NR = followingToDoItem.LaufendeNr,
                                        STATUS = "1",
                                        INS_FOLGE_TASK = "",
                                        ANMERKUNG = "",
                                    }
                            };
                        SAP.ApplyImport(list);
                        SAP.Execute();
                    }
                },

                // SAP custom error handling:
                () =>
                    {
                        var exportList = Z_WFM_SET_STATUS_01.GT_DATEN.GetExportList(SAP);
                        var errorItem = exportList.ToListOrEmptyList().FirstOrDefault(i => i.ERR.IsNotNullOrEmpty());
                        return (errorItem == null) ? "" : errorItem.ERR;
                    });

            return errorMessage;
        }

        public string SetOrderToKlaerfall(string vorgangNr, string remark)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_WFM_WRITE_TODO_02.Init(SAP);
                    SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_FUNKTIONSNAME", "ZWFM_KLAERFALL");
                    SAP.SetImportParameter("I_VORG_NR_ABM_AUF", vorgangNr);
                    SAP.SetImportParameter("I_ANMERKUNG", remark);

                    SAP.Execute();
                },

                // SAP custom error handling:
                () => ((SAP.ResultCode == 0) 
                    ? "" 
                    : SAP.ResultMessage.NotNullOr(Localize.SetClarificationCase + " " + Localize.Failed.ToLower() + ",  SAP Error Code: " + SAP.ResultCode)));

            return errorMessage;
        }

        #endregion
    }
}
