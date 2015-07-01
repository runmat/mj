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

            var sapList = Z_V_Ueberf_Verfuegbarkeit1.T_VERFUEG1.GetExportListWithInitExecute(SAP,
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

            var sapList = Z_V_UEBERF_AUFTR_FAHRER.T_AUFTRAEGE.GetExportListWithInitExecute(SAP, "I_FAHRER", FahrerID).OrderBy(s => s.AUFNR).ThenBy(s => s.FAHRTNR);

            return AppModelMappings.Z_V_UEBERF_AUFTR_FAHRER_T_AUFTRAEGE_to_FahrerAuftragsFahrt.Copy(sapList);
        }

        public IEnumerable<IFahrerAuftragsFahrt> LoadFahrerAuftragsProtokolle()
        {
            EnforceValidUserReference();

            var sapList = Z_V_UEBERF_AUFTR_UPL_PROT_01.GT_OUT.GetExportListWithInitExecute(SAP, "I_FAHRER", FahrerID).OrderBy(s => s.VBELN).ThenBy(s => s.FAHRTNR);

            return AppModelMappings.Z_V_UEBERF_AUFTR_UPL_PROT_01_GT_OUT_to_FahrerAuftragsProtokoll.Copy(sapList);
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
                throw new Exception(string.Format("Fehler: Ihr PDF Auftrag #{0} konnte nicht aus unserem System geladen werden!", auftragsNr));
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
    }
}
