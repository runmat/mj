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
	public class GesamtdatenPublic : BankBase
    {
        #region "Declarations"

        String m_strFilename2;
        DataTable m_tblVermieter;
        DataTable m_tblFehler;
        String m_strAVNr;
        String m_strSelectionType;
        String m_strDatumVon;
        String m_strDatumBis;
        DataTable m_tblUpload;

        #endregion

        #region "Properties"


        public DataTable Vermieter
        {
            get { return m_tblVermieter; }
            set { m_tblVermieter = value; }
        }

        public DataTable Fehler
        {
            get { return m_tblFehler; }
            
        }

        public String AVNr
        {
            get { return m_strAVNr; }
            set { m_strAVNr = value; }
        }


                

        public String SelectionType
        {
            get { return m_strSelectionType; }
            set { m_strSelectionType = value; }
        }

        public String DatumVon
        {
            get { return m_strDatumVon; }
            set { m_strDatumVon = value; }
        }

        public String DatimBis
        {
            get { return m_strDatumBis; }
            set { m_strDatumBis = value; }
        }

        public DataTable tblUpload
        {
            get { return m_tblUpload; }
            set { m_tblUpload = value; }
        }


        #endregion

        #region "Methods"


        public GesamtdatenPublic(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
            this.m_strFilename2 = strFilename;

	    }

        public void getVermieter(String strAppID, String strSessionID, System.Web.UI.Page page)
        
        {
            m_strClassAndMethod = "Gesamtdaten.FILL";
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

        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Gesamtdaten.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_UEBERMITTLUNG", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
              

                if (m_strSelectionType == "Einzel") { 
                    myProxy.setImportParameter("I_AVNR", m_strAVNr);
                    if (m_strDatumVon != null) { myProxy.setImportParameter("I_DATUM_VON", m_strDatumVon); }
                    if (m_strDatumBis != null) { myProxy.setImportParameter("I_DATUM_BIS", m_strDatumBis); }    
                }


                if (m_strSelectionType == "FIN") 
                {

                    DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");
                    DataRow rowUpload = null;
                    foreach (DataRow rowUpload_loopVariable in m_tblUpload.Rows)
                    {
                        rowUpload = rowUpload_loopVariable;
                        DataRow tmpSAPRow = SapTable.NewRow();

                        {
                            tmpSAPRow["CHASSIS_NUM"] = rowUpload["F1"].ToString().Trim();
                        }

                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();
                    }

                }

                if (m_strSelectionType == "Kennz")
                {

                    DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");

                    DataRow rowUpload = null;
                    foreach (DataRow rowUpload_loopVariable in m_tblUpload.Rows)
                    {
                        rowUpload = rowUpload_loopVariable;
                        DataRow tmpSAPRow = SapTable.NewRow();

                        {
                            tmpSAPRow["LICENSE_NUM"] = rowUpload["F1"].ToString().Trim();
                        }

                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();
                    }

                }
 
                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");

                
                m_tblFehler = myProxy.getExportTable("GT_ERR_OUT");
                Result = tblTemp;
               // Result = CreateOutPut(tblTemp, m_strAppID);

                if (((Result.Rows.Count == 0)) && ((m_tblFehler.Rows.Count==0)))
                {
                                   
                    m_intStatus = -9999;
                    m_strMessage = "keine dokumente zur anzeige gefunden.";
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
// ************************************************
// $History: Gesamtdaten.cs $
//
//*****************  Version 5  *****************
//User: Jungj        Date: 17.09.10   Time: 15:43
//Updated in $/CKAG2/Applications/AppRemarketing/lib
//ITA 4127 Testfertig
//
// ************************************************ 
