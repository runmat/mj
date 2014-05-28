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
    public class Schaden : BankBase
    {

        #region "Declarations"

		protected String m_strFilename2;
		protected DataTable m_tblVermieter;
		protected String m_strAVNr;
		protected String m_strAVName;
		protected String m_Kennzeichen;
		protected String m_Fahrgestellnummer;
		protected String m_CarportNr;
		protected String m_DatumVon;
		protected String m_DatumBis;
        protected String m_strSelectionType;
		protected DataTable m_tblUpload;
		protected DataTable m_tblFehler;

 
        #endregion

        #region "Properties"

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

        public DataTable Vermieter
        {
            get { return m_tblVermieter; }
            set { m_tblVermieter = value; }
        }

        public String SelectionType
        {
            get { return m_strSelectionType; }
            set { m_strSelectionType = value; }
        }

 
        public String AVNr
        {
            get { return m_strAVNr; }
            set { m_strAVNr = value; }
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

        public DataTable tblUpload
        {
            get { return m_tblUpload; }
            set { m_tblUpload = value; }
        }

        public DataTable Fehler
        {
            get { return m_tblFehler; }

        }

        #endregion

        #region "Methods"

               public Schaden(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
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

        public override void Change()
        {

        }

		public override void Show()
		{

		}

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Schaden.FILL";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SCHADENSBERICHT", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));


                if (m_strAVNr != "00")
                {
                    myProxy.setImportParameter("I_AVNR", m_strAVNr);
                }

                if (m_strSelectionType == "Einzel")
                {



                    if (DatumVon != null) { myProxy.setImportParameter("I_DATUM_VON", DatumVon); }
                    if (DatumBis != null) { myProxy.setImportParameter("I_DATUM_BIS", DatumBis); }
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

                            tmpSAPRow["CHASSIS_NUM"] = rowUpload[0].ToString().Trim();
                            if (m_tblUpload.Columns.Count > 1)
                            {
                                tmpSAPRow["LICENSE_NUM"] = rowUpload[1].ToString().Trim();
                            }

                            
                            
                        }

                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();
                    }

                }



                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");


                m_tblFehler = myProxy.getExportTable("GT_ERR_OUT");
                Result = tblTemp;


                if (((Result.Rows.Count == 0)) && ((m_tblFehler.Rows.Count == 0)))
                {

                    m_intStatus = -9999;
                    m_strMessage = "Keine Dokumente zur Anzeige gefunden.";
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
