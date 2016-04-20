using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.DomainCommon.Models;
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

            var sapItems = Z_WFM_READ_KONVERTER_01.GT_DATEN.GetExportListWithExecute(SAP)
                .Where(sap => sap.TABNAME.NotNullOrEmpty().ToUpper() == "ZABMVWL");

            return AppModelMappings.Z_WFM_READ_KONVERTER_01_GT_DATEN_To_WfmAuftragFeldname.Copy(sapItems).ToList();
        }

        public List<WfmAuftrag> GetAbmeldeauftraege(WfmAuftragSelektor selector)
        {
            Z_WFM_READ_AUFTRAEGE_01.Init(SAP, "I_AG", LogonContext.KundenNr.ToSapKunnr());

            if (!string.IsNullOrEmpty(selector.Selektionsfeld1Name))
                SAP.SetImportParameter("I_SELEKTION1", selector.Selektionsfeld1.BoolToX());

            if (!string.IsNullOrEmpty(selector.Selektionsfeld2Name))
                SAP.SetImportParameter("I_SELEKTION2", selector.Selektionsfeld2.BoolToX());

            if (!string.IsNullOrEmpty(selector.Selektionsfeld3Name))
                SAP.SetImportParameter("I_SELEKTION3", selector.Selektionsfeld3.BoolToX());

            if (!string.IsNullOrEmpty(selector.FahrgestellNr))
                SAP.SetImportParameter("I_FIN", selector.FahrgestellNr.ToUpper());

            if (!string.IsNullOrEmpty(selector.Kennzeichen))
                SAP.SetImportParameter("I_KENNZ", selector.Kennzeichen.ToUpper());

            if (selector.Modus == SelektionsModus.KlaerfallWorkplace)
            {
                SAP.SetImportParameter("I_TODO_WER", selector.ToDoWer);

                if (selector.SolldatumVonBis.IsSelected)
                {
                    var solldatumList = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_SOLLDAT_From_SolldatumSelektion.CopyBack(new List<SolldatumSelektion> { new SolldatumSelektion { SolldatumVonBis = selector.SolldatumVonBis } });
                    SAP.ApplyImport(solldatumList);
                }
            }

            if (!string.IsNullOrEmpty(selector.KundenAuftragsNr))
            {
                var kdAufNrList = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_KDAUF_From_KundenAuftragsNrSelektion.CopyBack(new List<KundenAuftragsNrSelektion> { new KundenAuftragsNrSelektion { KundenAuftragsNrVon = selector.KundenAuftragsNr } });
                SAP.ApplyImport(kdAufNrList);
            }

            if (!string.IsNullOrEmpty(selector.Referenz1))
            {
                var ref1List = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF1_From_Referenz1Selektion.CopyBack(new List<Referenz1Selektion> { new Referenz1Selektion { Referenz1Von = selector.Referenz1 } });
                SAP.ApplyImport(ref1List);
            }

            if (!string.IsNullOrEmpty(selector.Referenz2))
            {
                var ref2List = AppModelMappings.Z_WFM_READ_AUFTRAEGE_01_GT_SEL_REF2_From_Referenz2Selektion.CopyBack(new List<Referenz2Selektion> { new Referenz2Selektion { Referenz2Von = selector.Referenz2 } });
                SAP.ApplyImport(ref2List);
            }

            if (!string.IsNullOrEmpty(selector.Referenz3))
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

        public string CreateVersandAdresse(int vorgangNr, WfmAuftrag auftrag, Adresse versandAdresse, string versandOption)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    if (versandAdresse.Name1.IsNotNullOrEmpty())
                    {
                        SAP.Init("Z_DPM_INS_VERSDAT_ZCARPP_01");

                        SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
                        SAP.SetImportParameter("I_ANF_DAT", DateTime.Today);
                        SAP.SetImportParameter("I_WEB_USER", LogonContext.UserName);

                        var fin = auftrag.FahrgestellNr.NotNullOrEmpty().ToUpper().Contains("FAHRZEUG NICHT") ? null : auftrag.FahrgestellNr;
                        SAP.SetImportParameter("I_FAHRG", fin);
                        SAP.SetImportParameter("I_VERS_OPT", versandOption);

                        SAP.SetImportParameter("I_NAME1_ZS", versandAdresse.Name1);
                        SAP.SetImportParameter("I_NAME2_ZS", versandAdresse.Name2);
                        SAP.SetImportParameter("I_STREET_ZS", versandAdresse.Strasse);
                        SAP.SetImportParameter("I_HOUSE_NUM1_ZS", versandAdresse.HausNr);
                        SAP.SetImportParameter("I_POST_CODE1_ZS", versandAdresse.PLZ);
                        SAP.SetImportParameter("I_CITY1_ZS", versandAdresse.Ort);
                        SAP.SetImportParameter("I_COUNTRY_ZS", versandAdresse.Land);

                        SAP.SetImportParameter("I_ZZKENN", auftrag.Kennzeichen);
                        SAP.SetImportParameter("I_BELNR", auftrag.Verkaufsbeleg);

                        SAP.Execute();
                    }
                },

                // SAP custom error handling:
                () => ((SAP.ResultCode == 0) ? "" : SAP.ResultMessage.NotNullOr(Localize.CancellationFailed + ",  SAP Error Code: " + SAP.ResultCode)));

            return errorMessage;
        }

        public string StornoAuftrag(int vorgangNr)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_WFM_STORNO_AUFTRAG_01.Init(SAP);

                    SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_VORG_NR_ABM_AUF", vorgangNr.ToString());
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

        public string ConfirmToDo(int vorgangNr, int lfdNr, string remark)
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
                                    VORG_NR_ABM_AUF = vorgangNr.ToString(),
                                    LFD_NR = lfdNr.ToString(),
                                    STATUS = "2",
                                    INS_FOLGE_TASK = "X",
                                    ANMERKUNG = remark,
                                }
                        };
                    SAP.ApplyImport(list);
                    SAP.Execute();

                    // clear remark in following To-Do (because SAP unfortununately also sets remark in follower To-Do)
                    ClearRemarkInFollowingToDo(vorgangNr);
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

        public string SetOrderToKlaerfall(int vorgangNr, string remark)
        {
            var errorMessage = SAP.ExecuteAndCatchErrors(

                // exception safe SAP action:
                () =>
                {
                    Z_WFM_WRITE_TODO_02.Init(SAP);
                    SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
                    SAP.SetImportParameter("I_FUNKTIONSNAME", "ZWFM_KLAERFALL");
                    SAP.SetImportParameter("I_VORG_NR_ABM_AUF", vorgangNr.ToString());
                    SAP.SetImportParameter("I_ANMERKUNG", remark);

                    SAP.Execute();

                    // clear remark in following To-Do (because SAP unfortununately also sets remark in follower To-Do)
                    ClearRemarkInFollowingToDo(vorgangNr);
                },

                // SAP custom error handling:
                () => ((SAP.ResultCode == 0) 
                    ? "" 
                    : SAP.ResultMessage.NotNullOr(Localize.SetClarificationCase + " " + Localize.Failed.ToLower() + ",  SAP Error Code: " + SAP.ResultCode)));

            return errorMessage;
        }

        void ClearRemarkInFollowingToDo(int vorgangNr)
        {
            // clear remark in following To-Do (because SAP unfortununately also sets remark in follower To-Do)

            var newToDoList = GetToDos(vorgangNr.ToString().ToSapKunnr());
            var maxLfdNr = newToDoList.Max(todo => todo.LaufendeNr.ToInt());
            var followingToDoItem = newToDoList.FirstOrDefault(t => t.LaufendeNr.ToInt() == maxLfdNr);

            Z_WFM_SET_STATUS_01.Init(SAP);
            SAP.SetImportParameter("I_AG", LogonContext.KundenNr.ToSapKunnr());
            SAP.SetImportParameter("I_USER", LogonContext.UserName);
            if (followingToDoItem != null)
            {
                var list = new List<Z_WFM_SET_STATUS_01.GT_DATEN>
                    {
                        new Z_WFM_SET_STATUS_01.GT_DATEN
                            {
                                VORG_NR_ABM_AUF = vorgangNr.ToString(),
                                LFD_NR = followingToDoItem.LaufendeNr,
                                STATUS = "1",
                                INS_FOLGE_TASK = "",
                                ANMERKUNG = "",
                            }
                    };
                SAP.ApplyImport(list);
                SAP.Execute();
            }
        }

        #endregion


        #region Durchlauf

        public void GetDurchlauf(WfmAuftragSelektor selector, Action<IEnumerable<WfmDurchlaufSingle>, IEnumerable<WfmDurchlaufStatistik>> getDataAction)
        {
            Z_WFM_CALC_DURCHLAUFZEIT_01.Init(SAP, "I_KUNNR", LogonContext.KundenNr.ToSapKunnr());

            if (!string.IsNullOrEmpty(selector.Selektionsfeld1Name) || selector.Selektionsfeld1)
                SAP.SetImportParameter("I_SELEKTION1", selector.Selektionsfeld1.BoolToX());

            if (!string.IsNullOrEmpty(selector.Selektionsfeld2Name) || selector.Selektionsfeld2)
                SAP.SetImportParameter("I_SELEKTION2", selector.Selektionsfeld2.BoolToX());

            if (!string.IsNullOrEmpty(selector.Selektionsfeld3Name) || selector.Selektionsfeld3)
                SAP.SetImportParameter("I_SELEKTION3", selector.Selektionsfeld3.BoolToX());

            if (selector.AnlageDatumVonBis.IsSelected)
            {
                SAP.SetImportParameter("I_ANLAGE_VON", selector.AnlageDatumVonBis.StartDate);
                SAP.SetImportParameter("I_ANLAGE_BIS", selector.AnlageDatumVonBis.EndDate);
            }

            if (selector.ErledigtDatumVonBis.IsSelected)
            {
                SAP.SetImportParameter("I_ERLEDIGT_VON", selector.ErledigtDatumVonBis.StartDate);
                SAP.SetImportParameter("I_ERLEDIGT_BIS", selector.ErledigtDatumVonBis.EndDate);
            }

            if (selector.AbmeldeartDurchlauf == "Alle" || selector.AbmeldeartDurchlauf == "Klär")
                SAP.SetImportParameter("I_ABMART_KLAER", "X");

            if (selector.AbmeldeartDurchlauf == "Alle" || selector.AbmeldeartDurchlauf == "Std")
                SAP.SetImportParameter("I_ABMART_STD", "X");

            if (selector.DurchlaufzeitInTagen)
                SAP.SetImportParameter("I_ZEIT_IN_WERKTAGEN", "X");


            SAP.Execute();


            var sapItemsDetails = Z_WFM_CALC_DURCHLAUFZEIT_01.ET_OUT.GetExportList(SAP);
            var webItemsDetails = AppModelMappings.Z_WFM_CALC_DURCHLAUFZEIT_01_ET_OUT_To_WfmDurchlaufSingle.Copy(sapItemsDetails);
            //XmlService.XmlSerializeToFile(webItemsDetails.ToList(), Path.Combine(AppSettings.DataPath, @"WfmDetails.xml"));

            var sapItemsStatistiken = Z_WFM_CALC_DURCHLAUFZEIT_01.ES_STATISTIK.GetExportList(SAP);
            var webItemsStatistiken = AppModelMappings.Z_WFM_CALC_DURCHLAUFZEIT_01_ES_STATISTIK_To_WfmDurchlaufStatistik.Copy(sapItemsStatistiken);
            //XmlService.XmlSerializeToFile(webItemsStatistiken.ToList(), Path.Combine(AppSettings.DataPath, @"WfmStatistiken.xml"));

            getDataAction(webItemsDetails, webItemsStatistiken);
        }

        #endregion
    }
}
