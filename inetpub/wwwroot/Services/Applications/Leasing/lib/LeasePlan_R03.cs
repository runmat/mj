using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CKG.Base.Kernel;
using CKG.Base.Business;
using CKG.Base.Common;
using CKG.Base;
using System.Data;

namespace Leasing.lib
{
    public class LeasePlan_R03 : CKG.Base.Business.BankBase
    {
        #region "Declarations"

        Boolean m_blnChangeMemo;
        Boolean m_blnChangeFaelligkeit;
        DataTable m_tblAuftraege;
        DataTable m_tblRaw;
        String m_equ;
        String m_Memo;

        #endregion

        #region "Properties"

        public DataTable Auftraege
        {
            get { return m_tblAuftraege; }
            set { m_tblAuftraege = value; }
        }

        public String Equimpent
        {
            get { return m_equ; }
            set { m_equ = value; }
        }
        public String Memo
        {
            get { return m_Memo; }
            set { m_Memo = value; }
        }
        #endregion
        public LeasePlan_R03(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID, String strFilename)
            : base(ref objUser,ref objApp,strAppID,strSessionID, strFilename)
        {
            m_blnChangeMemo = false;
            m_blnChangeFaelligkeit = false;
        }

        public override void  Show()
        { }
        public override void Change()
        { }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LeasePlan_R03.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_FAELLIGE_EQUI_LP", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_FAELLIGKEIT", "21");

                myProxy.callBapi();

                m_tblRaw = myProxy.getExportTable("GT_WEB");
                m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID);
                m_tblResultExcel = m_tblAuftraege.Copy();
                m_tblResultExcel.Columns.Remove("EQUNR");

                m_tblAuftraege.Columns.Add("Memo alt", typeof(System.String));
                m_tblAuftraege.Columns.Add("MemoString", typeof(System.String));
                m_tblAuftraege.Columns.Add("DatumAendern", typeof(System.Boolean));

                foreach (DataRow tmpDataRow in m_tblAuftraege.Rows)
                {
                    if (tmpDataRow["Memo"].ToString() != String.Empty)
                    {
                        tmpDataRow["MemoString"] = "Ändern";
                    }
                    else 
                    {
                        tmpDataRow["MemoString"] = "Erfassen";
                    }
                }

            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_WEB":
                        m_intStatus = -1401;
                        m_strMessage = "Keine Web-Tabelle erstellt.";
                        break;
                    case "NO_DATA":
                        m_intStatus = -1;
                        m_strMessage = "Keine Daten gefunden.";
                        break;
                    case "NO_HAENDLER":
                        m_intStatus = -1403;
                        m_strMessage = "Händler nicht vorhanden.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Speichern der Daten: " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }
        
        }

        public void Change(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "LeasePlan_R03.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_Faellige_Equi_Update_Lp", ref m_objApp, ref m_objUser, ref page);


                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EQUNR", m_equ);
                myProxy.setImportParameter("I_TEXT200", m_Memo);

                myProxy.callBapi();


            }
            catch (Exception ex)
            {

                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {
                    case "NO_UPDATE":
                        m_intStatus = -1401;
                        m_strMessage = "Keine Web-Tabelle erstellt.";
                        break;
                    case "Kein EQUI-UPDATE.":
                        m_intStatus = -1;
                        m_strMessage = "Fehler bei der Textänderung.";
                        break;
                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Speichern der Daten: " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                        break;
                }
            }
        
        }
    }
}
