using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Kernel;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base;
using System.Data;

namespace AppRemarketing.lib
{
    public class Carport : BankBase
    {
#region "Declarations"

        String m_strFilename2;
        DataTable m_tblVermieter;
        String m_strAVNr;
        String m_strAVName;
        String m_Kennzeichen;
        String m_Fahrgestellnummer;
        String m_CarportNr;
        String m_DatumVon;
        String m_DatumBis;

        #endregion

        #region "Properties"


        public DataTable Vermieter
        {
            get { return m_tblVermieter; }
            set { m_tblVermieter = value; }
        }

        public String AVNr
        {
            get { return m_strAVNr; }
            set { m_strAVNr = value; }
        }

        public String AVName
        {
            get { return m_strAVName; }
            set { m_strAVName = value; }
        }

        public String Kennzeichen
        {
            get { return m_Kennzeichen; }
            set { m_Kennzeichen = value; }
        }
        public String Fahrgestellnummer
        {
            get { return m_Fahrgestellnummer; }
            set { m_Fahrgestellnummer = value; }
        }
        public String CarportNr
        {
            get { return m_CarportNr; }
            set { m_CarportNr = value; }
        }

        public String DatumVon
        {
            get { return m_DatumVon; }
            set { m_DatumVon = value; }
        }

        public String DatumBis
        {
            get { return m_DatumBis; }
            set { m_DatumBis = value; }
        }

        public DataTable Empfaenger { get; set; }

        public String Inventarnummer { get; set; }
        public string Vertragsjahr { set; get; }

        #endregion

        #region "Methods"


        public Carport(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
            this.m_strFilename2 = strFilename;

	    }

        public void getVermieter(String strAppID, String strSessionID, System.Web.UI.Page page)
        
        {
            m_strClassAndMethod = "FehlendeDaten.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_AUFTR6_001", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "VERMIETER");

                myProxy.callBapi();

                m_tblVermieter = myProxy.getExportTable("GT_WEB");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "),ref m_tblResult);

            }


        }


        public void getEmpfaenger(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Carport.getEmpfaenger";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_EMPFAENGER", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "A");

                myProxy.callBapi();

                Empfaenger = myProxy.getExportTable("GT_OUT");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }


        }


        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Carport.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_REM_003", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);
                if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); }

                if (AVNr != "00")
                {
                    myProxy.setImportParameter("I_AVNR", AVNr);
                }

                
                myProxy.setImportParameter("I_HCORT", CarportNr);
                myProxy.setImportParameter("I_HCEING_VON", DatumVon);
                myProxy.setImportParameter("I_HCEING_BIS", DatumBis);
 
                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                tblTemp.Columns["HCORT"].MaxLength = 40;
                tblTemp.AcceptChanges();

                foreach (DataRow dr in tblTemp.Rows)
                {
                    dr["HCORT"] = dr["HCORT"] + " " + dr["HC_NAME1"];
                }


                Result = CreateOutPut(tblTemp, m_strAppID);

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    Result.Columns.Add("Autovermieter", Type.GetType("System.String"));
                    Result.AcceptChanges();

                    String RFilter = "";

                    foreach (DataRow row in Result.Rows)
                    {
                        RFilter = "POS_KURZTEXT = '" + row["AVNR"] + "'";

                        Vermieter.DefaultView.RowFilter = RFilter;

                        row["Autovermieter"] = Vermieter.DefaultView[0].Row["POS_KURZTEXT"].ToString() + " " + Vermieter.DefaultView[0].Row["POS_TEXT"].ToString();
                        row["Kilometerstand"] = row["Kilometerstand"].ToString().TrimStart('0');
                        if (row["Kilometerstand"].ToString().Length == 0) 
                        {
                            row["Kilometerstand"] = "0";
                        }
                         
                        Result.AcceptChanges();
                    }

                    RFilter = "";

                }
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }


        public void ShowTelerik(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Carport.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_REM_003", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);
                if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); }

                if (AVNr != "00")
                {
                    myProxy.setImportParameter("I_AVNR", AVNr);
                }


                myProxy.setImportParameter("I_HCORT", CarportNr);
                myProxy.setImportParameter("I_HCEING_VON", DatumVon);
                myProxy.setImportParameter("I_HCEING_BIS", DatumBis);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                tblTemp.Columns["HCORT"].MaxLength = 40;
                tblTemp.AcceptChanges();

                foreach (DataRow dr in tblTemp.Rows)
                {
                    dr["HCORT"] = dr["HCORT"] + " " + dr["HC_NAME1"];
                }


                Result = tblTemp;

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    Result.Columns.Add("Autovermieter", Type.GetType("System.String"));
                    Result.AcceptChanges();

                    String RFilter = "";

                    foreach (DataRow row in Result.Rows)
                    {
                        RFilter = "POS_KURZTEXT = '" + row["AVNR"] + "'";

                        Vermieter.DefaultView.RowFilter = RFilter;

                        row["Autovermieter"] = Vermieter.DefaultView[0].Row["POS_KURZTEXT"].ToString() + " " + Vermieter.DefaultView[0].Row["POS_TEXT"].ToString();
                        row["KMSTAND"] = row["KMSTAND"].ToString().TrimStart('0');
                        if (row["KMSTAND"].ToString().Length == 0)
                        {
                            row["KMSTAND"] = "0";
                        }

                        Result.AcceptChanges();
                    }

                    RFilter = "";

                }
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

        public void ShowHCAusgang(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Carport.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_REM_006", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);


                if (AVNr != "00")
                {
                    myProxy.setImportParameter("I_AVNR", AVNr);
                }


                myProxy.setImportParameter("I_HCORT", CarportNr);
                myProxy.setImportParameter("I_DAT_HC_AUSG_VON", DatumVon);
                myProxy.setImportParameter("I_DAT_HC_AUSG_BIS", DatumBis);
                myProxy.setImportParameter("I_VJAHR", Vertragsjahr);

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                Result = CreateOutPut(tblTemp, m_strAppID);

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    //Result.Columns.Add("Autovermieter", Type.GetType("System.String"));
                   // Result.AcceptChanges();

                    //String RFilter = "";

                    foreach (DataRow row in Result.Rows)
                    {
                        //RFilter = "POS_KURZTEXT = '" + row["AVNR"] + "'";

                        //Vermieter.DefaultView.RowFilter = RFilter;

                        //row["Autovermieter"] = Vermieter.DefaultView[0].Row["POS_KURZTEXT"].ToString() + " " + Vermieter.DefaultView[0].Row["POS_TEXT"].ToString();
                        row["Kilometerstand"] = row["Kilometerstand"].ToString().TrimStart('0');
                        if (row["Kilometerstand"].ToString().Length == 0)
                        {
                            row["Kilometerstand"] = "0";
                        }
                        //Result.AcceptChanges();
                    }

                    //RFilter = "";

                }
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

       

        #endregion
    }
}