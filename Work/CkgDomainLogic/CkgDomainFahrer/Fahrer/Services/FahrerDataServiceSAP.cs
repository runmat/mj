using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.Fahrer.Contracts;
using CkgDomainLogic.Fahrer.Models;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;
using SapORM.Models;
using GeneralTools.Models;
using AppModelMappings = CkgDomainLogic.Fahrer.Models.AppModelMappings;

namespace CkgDomainLogic.Fahrer.Services
{
    public class FahrerDataServiceSAP : CkgGeneralDataServiceSAP, IFahrerDataService
    {
        public string UserReference { get { return (LogonContext == null ? "" : LogonContext.User.Reference); } }

        public string FahrerID { get { return UserReference.ToSapKunnr(); } }

        public int BuchungsKreis { get { return (LogonContext == null ? 0 : LogonContext.Customer.AccountingArea.GetValueOrDefault()); } }

        public List<FahrerTagBelegung> FahrerTagBelegungen { get { return PropertyCacheGet(() => LoadFahrerTagBelegungen().ToList()); } }


        public FahrerDataServiceSAP(ISapDataService sap)
            : base(sap)
        {
        }

        public void MarkDataForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrerTagBelegungen);
        }

        private void EnforceValidUserReference()
        {
            if (UserReference.IsNullOrEmpty())
                throw new Exception(string.Format(Localize.UserReferenceMissing, LogonContext.UserName));
        }

        
        #region Verfügbarkeitsmeldung

        private IEnumerable<FahrerTagBelegung> LoadFahrerTagBelegungen()
        {
            EnforceValidUserReference();

            var sapList = Z_V_UEBERF_VERFUEGBARKEIT1.T_VERFUEG1.GetExportListWithInitExecute(SAP,
                "I_FAHRER, I_VONDAT, I_BISDAT",
                FahrerID, DateTime.Today.ToString("ddMMyyyy"), "31122999");

            return AppModelMappings.Z_V_Ueberf_Verfuegbarkeit1_T_VERFUEG1_To_FahrerTagBelegung.Copy(sapList);
        }

        public void SaveFahrerTagBelegungen(IEnumerable<FahrerTagBelegung> fahrerTagBelegungen)
        {
            Z_V_UEBERF_VERFUEGBARKEIT2.Init(SAP, "I_FAHRER", FahrerID);
            var sapBelegungen = AppModelMappings.Z_V_UEBERF_VERFUEGBARKEIT2_GT_FAHRER_To_FahrerTagBelegung.CopyBack(fahrerTagBelegungen).ToList();
            SAP.ApplyImport(sapBelegungen);

            SAP.Execute();
        }

        #endregion


        #region Fahrer Aufträge

        public IEnumerable<FahrerAuftrag> LoadFahrerAuftraege(string auftragsStatus)
        {
            EnforceValidUserReference();

            auftragsStatus = auftragsStatus.Replace("NEW", " ");

            var sapList = Z_M_GET_FAHRER_AUFTRAEGE.GT_ORDER.GetExportListWithInitExecute(SAP,
                "I_VKORG, I_FAHRER, I_FAHRER_STATUS",
                BuchungsKreis, FahrerID, auftragsStatus);

            return AppModelMappings.Z_M_GET_FAHRER_AUFTRAEGE_GT_ORDER_To_FahrerAuftrag.Copy(sapList);
        }



        public IEnumerable<IFahrerAuftragsFahrt> LoadFahrerAuftragsFahrten()
        {
            EnforceValidUserReference();

#if __TEST
            System.Threading.Thread.Sleep(2000);

            return new List<FahrerAuftragsFahrt>
            {
                new FahrerAuftragsFahrt
                {
                    AuftragsNr = "4711", OrtStart = "Ahrensburg", OrtZiel = "Hamburg", Kennzeichen = "HH-X 133", FahrzeugTyp = "PKW", Fahrt = "H", 
                },
                new FahrerAuftragsFahrt
                {
                    AuftragsNr = "4712", OrtStart = "Hamburg", OrtZiel = "Bargteheide", Kennzeichen = "OD-Z 987", FahrzeugTyp = "LKW", Fahrt = "R", 
                },
            };
#else
            var sapList = Z_V_UEBERF_AUFTR_FAHRER.T_AUFTRAEGE.GetExportListWithInitExecute(SAP, "I_FAHRER", FahrerID).OrderBy(s => s.AUFNR).ThenBy(s => s.FAHRTNR);

            return AppModelMappings.Z_V_UEBERF_AUFTR_FAHRER_T_AUFTRAEGE_to_FahrerAuftragsFahrt.Copy(sapList);
#endif 
        }

        public List<FahrerAuftrag> LoadFreieAuftraege()
        {
            Z_DPM_READ_AUFTR_FAHR_EDISPO_1.Init(SAP);
            Z_DPM_READ_AUFTR_FAHR_EDISPO_1.SetImportParameter_I_FAHRER(SAP, FahrerID);

            var sapList = Z_DPM_READ_AUFTR_FAHR_EDISPO_1.GT_ORDER.GetExportListWithExecute(SAP);

            return AppModelMappings.Z_DPM_READ_AUFTR_FAHR_EDISPO_1_GT_ORDER_To_FahrerAuftrag.Copy(sapList).ToList();

        }

        public IEnumerable<IFahrerAuftragsFahrt> LoadFahrerAuftragsProtokolle()
        {
            EnforceValidUserReference();

#if __TEST
            System.Threading.Thread.Sleep(2000);

            return new List<FahrerAuftragsProtokoll>
            {
                new FahrerAuftragsProtokoll
                {
                    AuftragsNr = "5711", OrtStart = "Ahrensburg", OrtZiel = "Hamburg", Kennzeichen = "HH-X 133", Fahrt = "H", ProtokollArt = "DADAUSLIEF"
                },
                new FahrerAuftragsProtokoll
                {
                    AuftragsNr = "5712", OrtStart = "Hamburg", OrtZiel = "Bargteheide", Kennzeichen = "OD-Z 987", Fahrt = "R", ProtokollArt = "DADRUECK"
                },
            };
#else
            var sapList = Z_V_UEBERF_AUFTR_UPL_PROT_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_FAHRER", FahrerID).OrderBy(s => s.VBELN).ThenBy(s => s.FAHRTNR);

            return AppModelMappings.Z_V_UEBERF_AUFTR_UPL_PROT_01_GT_OUT_to_FahrerAuftragsProtokoll.Copy(sapList);
#endif
        }
        
        public IFahrerAuftragsFahrt LoadFahrerAuftragsEinzelProtokoll(string auftragsnr, string fahrtTyp)
        {
            EnforceValidUserReference();

            Z_V_UEBERF_AUFTR_UPL_PROT_01.Init(SAP);
            SAP.SetImportParameter("I_FAHRER", FahrerID);
            SAP.SetImportParameter("I_VBELN", auftragsnr);
            var sapList = Z_V_UEBERF_AUFTR_UPL_PROT_01.GT_OUT.GetExportListWithExecute(SAP);
            sapList = sapList.Where(s => s.FAHRTNR.NotNullOrEmpty().ToUpper() == fahrtTyp.NotNullOrEmpty().ToUpper()).OrderBy(s => s.FAHRTNR).ToList();

            return AppModelMappings.Z_V_UEBERF_AUFTR_UPL_PROT_01_GT_OUT_to_FahrerAuftragsProtokoll.Copy(sapList).FirstOrDefault();
        }

        public string SetFahrerAuftragsStatus(string auftragsNr, string status)
        {
            Z_M_SET_FAHRER_AUFTRAGS_STATUS.Init(SAP, "I_VBELN, I_FAHRER_STATUS", auftragsNr, status);

            return SAP.ExecuteAndCatchErrors(() => SAP.Execute());
        }

        public byte[] GetAuftragsPdfBytes(string auftragsNr)  // z. B. "24436273"
        {
            byte[] pdfBytes;
            try
            {
                pdfBytes = SAP.GetExportParameterByteWithInitExecute("Z_M_CRE_FAHRER_AUFTRAG_PDF", "E_XSTRING",
                                                                     "I_VBELN", auftragsNr.PadLeft10());
            }
            catch
            {
                return null;
            }

            return pdfBytes;
        }

        #endregion

        
        #region Monitor / QM Auswertung

        public int QmFahrerRankingCount { get; set; }

        public List<QmFahrer> QmFahrerList { get { return PropertyCacheGet(() => new List<QmFahrer>()); } set { PropertyCacheSet(value); } }

        public List<QmFleetMonitor> QmFleetMonitorList { get { return PropertyCacheGet(() => new List<QmFleetMonitor>()); } set { PropertyCacheSet(value); } }


        public bool LoadQmReportFleetData(DateRange dateRange)
        {
            Z_UEB_FAHRER_QM.Init(SAP,
                                 "I_LIFNR, I_DATAB, I_DATBI",
                                 FahrerID, dateRange.StartDate.GetValueOrDefault(), dateRange.EndDate.GetValueOrDefault());
            SAP.Execute();

            QmFahrerList = AppModelMappings.Z_UEB_FAHRER_QM_ET_QM_To_QmFahrer.Copy(Z_UEB_FAHRER_QM.ET_QM.GetExportList(SAP)).ToList();
            QmFleetMonitorList = AppModelMappings.Z_UEB_FAHRER_QM_ET_FLEET_To_QmFleetMonitor.Copy(Z_UEB_FAHRER_QM.ET_FLEET.GetExportList(SAP)).ToList();

            QmFahrerRankingCount = SAP.GetExportParameter("E_RANKING_COUNT").ToInt(0);

            return QmFahrerList.Any() || QmFleetMonitorList.Any();
        }


        #endregion


        #region Protokollarchivierung

        public List<SelectItem> QmCodes { get { return PropertyCacheGet(() => LoadQmCodes().ToList()); } }

        private IEnumerable<SelectItem> LoadQmCodes()
        {
            var sapList = Z_DPM_QM_READ_QPCD.GT_OUTQPCD.GetExportListWithInitExecute(SAP, "I_KATALOGART, I_CODEGRUPPE", "P", "UEBER");

            return AppModelMappings.Z_DPM_QM_READ_QPCD_GT_OUTQPCD_To_SelectItem.Copy(sapList);
        }

       

        public List<string> GetProtokollArchivierungMailAdressenAndReferenz(FahrerAuftragsProtokoll protokoll)
        {
            Z_V_UEBERF_AUFTR_REFERENZ.Init(SAP, "AUFNR", protokoll.AuftragsNr);

            SAP.Execute();

            var sapListAuftrag = Z_V_UEBERF_AUFTR_REFERENZ.T_AUFTRAEGE.GetExportList(SAP);
            var sapListMail = Z_V_UEBERF_AUFTR_REFERENZ.T_SMTP.GetExportList(SAP);

            if (sapListAuftrag.Any())
            {
                var item = sapListAuftrag.First();
                protokoll.VIN = item.ZZFAHRG;
                protokoll.Kennzeichen = item.ZZKENN;
                protokoll.Referenz = item.ZZREFNR;
            }

            return sapListMail.Where(m => m.FAHRTNR == protokoll.Fahrt).Select(m => m.SMTP_ADDR).ToList();
        }

        public string SaveProtokollAndQmDaten(ProtokollEditModel item)
        {
            Z_V_UEBERF_AUFTR_PROTOKOLL_AB.Init(SAP);

            SAP.SetImportParameter("AUFNR", item.Protokoll.AuftragsNr);
            SAP.SetImportParameter("FAHRTNR", item.Protokoll.FahrtNr);
            SAP.SetImportParameter("WADAT_IST", item.UeberfuehrungsDatum);
            SAP.SetImportParameter("ZZKATEGORIE", item.Protokoll.ProtokollArt);
            SAP.SetImportParameter("VERARBEITUNG", " ");
            SAP.SetImportParameter("ABHOL_DAT", item.AbholDatum);
            SAP.SetImportParameter("ABHOL_ZEIT", (String.IsNullOrEmpty(item.AbholUhrzeit) ? "" : String.Format("{0}00", item.AbholUhrzeit.PadLeft0(4))));
            SAP.SetImportParameter("UEBERGABE_DAT", item.UebergabeDatum);
            SAP.SetImportParameter("IUG_ZEIT", (String.IsNullOrEmpty(item.UebergabeUhrzeit) ? "" : String.Format("{0}00", item.UebergabeUhrzeit.PadLeft0(4))));
            SAP.SetImportParameter("KMSTAND", item.Kilometerstand);
            SAP.SetImportParameter("UNTERSCHR_VORH", item.UnterschriftVorhanden.BoolToX());

            if (!String.IsNullOrEmpty(item.QmCode))
            {
                var qmList = new List<Z_V_UEBERF_AUFTR_PROTOKOLL_AB.GT_IMP_QM> {
                    new Z_V_UEBERF_AUFTR_PROTOKOLL_AB.GT_IMP_QM {
                        CHASSIS_NUM = item.Protokoll.VIN,
                        COFEHL = item.QmCode,
                        GRFEHL = "UEBER",
                        I_MASS_TEXT = item.QmBemerkung,
                        LICENSE_NUM = item.Protokoll.Kennzeichen
                    }
                };

                SAP.ApplyImport(qmList);
            }

            return SAP.ExecuteAndCatchErrors(() => SAP.Execute());
        }

        #endregion
    }
}
