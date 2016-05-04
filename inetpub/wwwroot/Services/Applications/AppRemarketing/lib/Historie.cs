using System;
using System.Collections.Generic;
using System.Linq;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;
using GeneralTools.Models;

namespace AppRemarketing.lib
{
    public class Historie : DatenimportBase
    {
        #region "Declarations"

        String m_Fahrgestellnummer;
        String m_Kennzeichen;

        #endregion

        #region "Properties"

        public String Fahrgestellnummer
        {
            get { return m_Fahrgestellnummer; }
            set { m_Fahrgestellnummer = value; }
        }

        public String Kennzeichen
        {
            get { return m_Kennzeichen; }
            set { m_Kennzeichen = value; }
        }

        public DataTable CommonData { get; private set; }

        public DataTable Gutachten { get; private set; }

        public DataTable Versand { get; private set; }

        public IList<HistorieEintrag> Lebenslauf { get; private set; }
        public HistorieBelastungsanzeige Belastungsanzeige { get; private set; }
        public HistorieUebersicht Uebersicht { get; private set; }
        public HistorieLinks Links { get; private set; }
        public DataView Vorschaden { get; private set; }

        public DataTable ModelTable { get; set; }
        public DataTable AussenFarbeTable { get; set; }
        public DataTable InnenFarbeTable { get; set; }
        public DataTable AusstattungTable { get; set; }


        #endregion

        public Historie(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)
            : base(ref objUser, objApp, strFilename)
        {
        }

        public void GetHistData(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "GetHistData.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_FAHRZEUGHIST_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);

                myProxy.callBapi();

                CommonData = myProxy.getExportTable("GT_DATEN");
                Gutachten = myProxy.getExportTable("GT_GUTA");
                Versand = myProxy.getExportTable("GT_VERS");

                var lebb = myProxy.getExportTable("GT_LEB_B");
                var lebt = myProxy.getExportTable("GT_LEB_T");
                var addr = myProxy.getExportTable("GT_ADDR_B");
                var belas = myProxy.getExportTable("GT_BELAS");
                var rechng = myProxy.getExportTable("GT_RECHNG");
                var schaden = myProxy.getExportTable("GT_SCHADEN");
                var daten2 = myProxy.getExportTable("GT_DATEN2");
                var ausstattung = myProxy.getExportTable("GT_AUSST");

                Lebenslauf = HistorieEintrag.Parse(CommonData, daten2, addr, Gutachten, lebt, lebb, schaden, belas, rechng).OrderBy(e => e.Date).Distinct().ToList();

                Belastungsanzeige = HistorieBelastungsanzeige.Parse(belas);
                Uebersicht = HistorieUebersicht.Parse(CommonData, daten2, addr, Gutachten, lebt, lebb, schaden, belas, rechng);
                Vorschaden = new HistorieVorschaden(myProxy.getExportTable("GT_SCHADEN")).VorschadenView;

                var histAusstattung = new HistorieAusstattung(myProxy.getExportTable("GT_AUSST"));
                ModelTable = histAusstattung.ModelTable;
                InnenFarbeTable = histAusstattung.InnenFarbeTable;
                AussenFarbeTable = histAusstattung.AussenFarbeTable;
                AusstattungTable = histAusstattung.AusstattungTable;

                var fahrgestellNrRow = CommonData.Rows.Cast<DataRow>().FirstOrDefault();
                var fahrgestellNr = fahrgestellNrRow!=null?fahrgestellNrRow["FAHRGNR"].ToString():string.Empty;

                myProxy = DynSapProxy.getProxy("Z_DPM_REM_SCHADENRG_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FIN", fahrgestellNr);
                myProxy.setImportParameter("I_STATU", "A");

                myProxy.callBapi();

                rechng = myProxy.getExportTable("GT_OUT");
                var rechngRow = rechng.Rows.Cast<DataRow>().FirstOrDefault(r => ((string)r["Status"]) == "Rechnung");

                var maxGutaNr = (Gutachten.Rows.Count > 0 ? Gutachten.Rows.Cast<DataRow>().Max(r => r["LFDNR"].ToString().ToInt(0)) : 0);
                var anzRepKalk = (maxGutaNr > 0 ? Gutachten.Rows.Cast<DataRow>().First(r => r["LFDNR"].ToString().ToInt(0) == maxGutaNr)["REPKALK"].ToString().ToInt(0) : 0);

                Links = new HistorieLinks(
                        strAppID,
                        m_objUser.Customer.CustomerId,
                        fahrgestellNr,
                        Gutachten.Rows.Cast<DataRow>().Select(r => r["GUTA"].ToString()).ToArray(), 
                        rechngRow != null ? rechngRow["RENNR"].ToString() : string.Empty,
                        belas.Rows.Count > 0,
                        Belastungsanzeige != null ? Belastungsanzeige.Date : null,
                        anzRepKalk);

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_DATA":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Informationen gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;

                        break;
                }
            }
        }
    }
}
