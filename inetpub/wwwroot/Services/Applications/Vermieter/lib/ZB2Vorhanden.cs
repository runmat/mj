using System;
using CKG.Base.Business;
using CKG.Base.Common;
using System.Data;


namespace Vermieter.lib
{
    public class ZB2Vorhanden : DatenimportBase
    {

        #region "Declarations"
        Int16 m_EquiTyp = 0;
        #endregion

        #region Properties
        public Int16 Typ
        {
            get { return m_EquiTyp; }
            set { m_EquiTyp = value; }
        }

        DataTable m_ResultTable = new DataTable();

        string m_KUNNR_AG = "";
        string m_CHASSIS_NUM = "";
        string m_FAHRG = "";
        string m_ERDAT_VON = "";
        string m_ERDAT_BIS = "";
        string m_KENNZ = "";


        public DataTable ResultTable
        {
            get { return m_ResultTable; }
            set { m_ResultTable = value; }
        }

        public string I_KUNNR_AG
        {
            get { return m_KUNNR_AG; }
            set { m_KUNNR_AG = value; }
        }
        public string I_CHASSIS_NUM
        {
            get { return m_CHASSIS_NUM; }
            set { m_CHASSIS_NUM = value; }
        }
        public string I_FAHRG
        {
            get { return m_FAHRG; }
            set { m_FAHRG = value; }
        }
        public string I_ERDAT_VON
        {
            get { return m_ERDAT_VON; }
            set { m_ERDAT_VON = value; }
        }
        public string I_ERDAT_BIS
        {
            get { return m_ERDAT_BIS; }
            set { m_ERDAT_BIS = value; }
        }

        public string I_KENNZ
        {
            get { return m_KENNZ; }
            set { m_KENNZ = value; }
        }

        #endregion

        public ZB2Vorhanden(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strFilename)

            : base(ref objUser, objApp, strFilename)
        {
        }


        public void GetZB2Vorhanden(String strAppID, String strSessionID, System.Web.UI.Page page, string erfassDatVon, string erfassDatBis, string fahrgestell, string kennzeichen, bool nurScheinFehlt, bool nurSchildFehlt)
        {

            m_strClassAndMethod = "ZB2Vorhanden.GetZB2Vorhanden";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_OFFENE_ABM_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_ERDAT_VON", erfassDatVon);
                myProxy.setImportParameter("I_ERDAT_BIS", erfassDatBis);
                myProxy.setImportParameter("I_FAHRG", fahrgestell);
                myProxy.setImportParameter("I_KENNZ", kennzeichen);
                if (nurScheinFehlt)
                    myProxy.setImportParameter("I_SCHEIN_FEHLT", "X");
                if (nurSchildFehlt)
                    myProxy.setImportParameter("I_SCHILD_FEHLT", "X");

                myProxy.callBapi();

                DataTable TempTable;

                TempTable = myProxy.getExportTable("GT_OUT");

                
                foreach (DataRow Row in TempTable.Rows)
                {
                
                 //   Row["ZZNAME1_ZS"] = Row["ZZNAME1_ZS"] + "<br>" + Row["ZZSTRAS_ZS"] + "," + Row["ZZPSTLZ_ZS"] + "&nbsp;" + Row["ZZORT01_ZS"];

                }

                TempTable.AcceptChanges();

                ResultTable = TempTable;

                CreateOutPut(TempTable, strAppID);


                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult, false);
            }
            catch
            {
                m_intStatus = -1111;
                m_strMessage = "Keine Informationen gefunden.";

            }


        }


        public void Delete(string strAppID, string strSessionID, System.Web.UI.Page page, string kennz, string fahrg, string brief, string schluess, string komponentennummer, string anforderungsnummer)
        {
            m_strClassAndMethod = "Versandbeauftragungen.Delete";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            DataTable Dummy = new DataTable();

            if (!m_blnGestartet)
            {
                m_blnGestartet = true;


                string strKUNNR = m_objUser.KUNNR.PadLeft(10, '0');


                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_VERSAUFTR_FEHLERHAFTE_DEL", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_KUNNR", strKUNNR);
                    myProxy.setImportParameter("I_LICENSE_NUM", kennz);
                    myProxy.setImportParameter("I_CHASSIS_NUM", fahrg);
                    myProxy.setImportParameter("I_ZZBRFVERS", brief);
                    myProxy.setImportParameter("I_ZZSCHLVERS", schluess);
                    myProxy.setImportParameter("I_IDNRK", komponentennummer);
                    myProxy.setImportParameter("I_ZANF_NR", anforderungsnummer);
                    myProxy.callBapi();

                }
                catch (Exception ex)
                {
                    m_intStatus = -9999;

                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DELETE":
                            m_strMessage = "Datensatz konnte nicht gelöscht werden.";
                            break;
                        case "NO_DATA":
                            m_strMessage = "Datensatz nicht gefunden.";
                            break;
                        default:
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.";
                            break;
                    }
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref Dummy, false);

                }
                finally
                {
                    m_blnGestartet = false;
                }
            }
        }

    }


}
