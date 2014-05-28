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
    public class Mahnungen : BankBase
    {
        #region "Properties"
            public String AVNR { set; get; }
            public String AuswahlStatus { set; get; }
            public String Ereignis { get; set; }
            public String Fahrgestellnummer { set; get; }
            public String Kennzeichen { set; get; }
            public String Mahnfrist { set; get; }
            public DataTable Vermieter { set; get; }
            public DataTable UploadTable { set; get; }
            public DataTable ErrorTable { set; get; }
            public String Alle { set; get; }
            public String MeldungZulassung { set; get; }
            public String ZB2Eingang { set; get; }
            public String Schluesseleingang { set; get; }
            public String Gutachteneingang { set; get; }
            public String Inventarnummer { set; get; }
        #endregion

                public Mahnungen(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
        }

        public override void Show()
        {

        }

        public override void Change()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Mahnungen.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_MAHNUNGEN", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);
                myProxy.setImportParameter("I_FLAG", AuswahlStatus);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);

                myProxy.setImportParameter("I_ALLE", Alle);
                myProxy.setImportParameter("I_ZUL", MeldungZulassung);
                myProxy.setImportParameter("I_ZB2", ZB2Eingang);
                myProxy.setImportParameter("I_KEY", Schluesseleingang);
                myProxy.setImportParameter("I_GUTA", Gutachteneingang);


                if (AVNR != "00")
                {
                    myProxy.setImportParameter("I_AVNR", AVNR);
                }

              

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                Result = tblTemp;

                if (Result.Rows.Count == 0)
                {
                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
                }
                else
                {
                    foreach (DataRow dr in Result.Rows)
                    {
                        dr["POS_TEXT"] = dr["AVNR"] + " " + dr["POS_TEXT"];
                    }
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


        public void SetMahnFrist(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Mahnungen.SetMahnFrist";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_UPD_MAHNFRIST", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);
                myProxy.setImportParameter("I_SOLLDAT", Mahnfrist);

                if (Ereignis != "")
                {
                    myProxy.setImportParameter("I_EREIGNIS", Ereignis);
                }

                if (UploadTable != null)
                {
                    DataTable tblTemp = myProxy.getImportTable("GT_FIN_IN");
                    DataRow NewRow = tblTemp.NewRow();

                    foreach (DataRow dr in UploadTable.Rows)
                    {
                        NewRow = tblTemp.NewRow();

                        NewRow[0] = dr["FAHRGNR"];

                        tblTemp.Rows.Add(NewRow);

                    }

                    tblTemp.AcceptChanges();

                }


                myProxy.callBapi();


                ErrorTable = myProxy.getExportTable("GT_ERR_OUT");


                m_intStatus = Convert.ToInt16(myProxy.getExportParameter("E_SUBRC"));
                m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }


        public void RemoveFromMahnlauf(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Mahnungen.RemoveFromMahnlauf";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_UPD_NOMAHNUNG", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);

                if (UploadTable != null)
                {
                    DataTable tblTemp = myProxy.getImportTable("GT_FIN_IN");
                    DataRow NewRow = tblTemp.NewRow();

                    foreach (DataRow dr in UploadTable.Rows)
                    {
                        NewRow = tblTemp.NewRow();

                        NewRow[0] = dr["FAHRGNR"];

                        tblTemp.Rows.Add(NewRow);

                    }

                    tblTemp.AcceptChanges();

                }
               


                myProxy.callBapi();

                ErrorTable = myProxy.getExportTable("GT_ERR_OUT");

                m_intStatus = Convert.ToInt16(myProxy.getExportParameter("E_SUBRC"));
                m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }


        public void SetMahnlauf(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Mahnungen.SetMahnlauf";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_UPD_MAHNUNGEN", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNZ", Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);

                if (UploadTable != null)
                {
                    DataTable tblTemp = myProxy.getImportTable("GT_FIN_IN");
                    DataRow NewRow = tblTemp.NewRow();

                    foreach (DataRow dr in UploadTable.Rows)
                    {
                        NewRow = tblTemp.NewRow();

                        NewRow[0] = dr["FAHRGNR"];

                        tblTemp.Rows.Add(NewRow);

                    }

                    tblTemp.AcceptChanges();

                }


                myProxy.callBapi();

                ErrorTable = myProxy.getExportTable("GT_ERR_OUT");

                m_intStatus = Convert.ToInt16(myProxy.getExportParameter("E_SUBRC"));
                m_strMessage = myProxy.getExportParameter("E_MESSAGE");

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);
            }
        }

    }
}
