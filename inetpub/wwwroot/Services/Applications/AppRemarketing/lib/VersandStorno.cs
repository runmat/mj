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
    public class VersandStorno : BankBase
    {
        #region "Declarations"

        String m_strFilename2;
        DataTable m_tblAnforderungen;
        String mE_SUBRC;
        String mE_MESSAGE;
        String m_strKennung;
        String m_strDebitor;
        String m_strMaterialNr;
        DataTable m_tblHaendler;
        DataTable m_tblUpload;
        DataTable m_tblMaterial;
        DataTable m_tblError;
        String m_strSelectionType;

        #endregion

        #region "Properties"
        public String E_SUBRC
        {
            get { return mE_SUBRC; }
            set { mE_SUBRC = value; }
        }

        public String E_Message
        {
            get { return mE_MESSAGE; }
            set { mE_MESSAGE = value; }
        }

        public String Kennung
        {
            get { return m_strKennung; }
            set { m_strKennung = value; }
        }

        public String MaterialNr
        {
            get { return m_strMaterialNr; }
            set { m_strMaterialNr = value; }
        }
        public String Debitor
        {
            get { return m_strDebitor; }
            set { m_strDebitor = value; }
        }

        public DataTable tblAnforderungen
        {
            get { return m_tblAnforderungen; }
            set { m_tblAnforderungen = value; }
        }

        public DataTable tblUpload
        {
            get { return m_tblUpload; }
            set { m_tblUpload = value; }
        }

        public String SelectionType
        {
            get { return m_strSelectionType; }
            set { m_strSelectionType = value; }
        }

        public DataTable tblError
        {
            get { return m_tblError; }
            set { m_tblError = value; }
        }
        public DataTable tblMaterial
        {
            get { return m_tblMaterial; }
            set { m_tblMaterial = value; }
        }
        #endregion

        #region "Methods"


        public VersandStorno(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        {
            this.m_strFilename2 = strFilename;

        }

        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void GetVersAnf(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "VersandStorno.GetVersAnf";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_VERSANF_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));



                if (m_strSelectionType == "Haendler")
                {
                    myProxy.setImportParameter("I_KUNNR_ZF", m_strDebitor);
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
                            tmpSAPRow["CHASSIS_NUM"] = rowUpload["F1"].ToString();
                        }

                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();
                    }

                }

                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_OUT");
                mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                tblTemp.Columns.Add("Adresse", System.Type.GetType("System.String"));
                tblTemp.Columns.Add("Select", System.Type.GetType("System.String"));
                tblTemp.Columns.Add("Status", System.Type.GetType("System.String"));
                tblTemp.Columns.Add("Material", System.Type.GetType("System.String"));

                foreach (DataRow SapRow in tblTemp.Rows)
                {

                    //String sAdresse;
                    //sAdresse = SapRow["NAME1_ZF"].ToString();
                    //if (SapRow["NAME2_ZF"].ToString().Length > 0)
                    //{
                    //    sAdresse += " " + SapRow["NAME2_ZF"].ToString();
                    //}
                    //if (SapRow["NAME3_ZF"].ToString().Length > 0)
                    //{
                    //    sAdresse += " " + SapRow["NAME3_ZF"].ToString();
                    //}
                    //sAdresse += "<br/>" + SapRow["STREET_ZF"].ToString();
                    //sAdresse += "<br/>" + SapRow["POST_CODE1_ZF"].ToString() + " " + SapRow["CITY1_ZF"].ToString();
                    //SapRow["Adresse"] = sAdresse;
                    SapRow["Select"] = "";
                    SapRow["Material"] = "";

                    SapRow["Adresse"] = SapRow["NAME1_ZF"] + " " + SapRow["NAME2_ZF"] + "<br>" + SapRow["STREET_ZF"] + "<br>" + SapRow["POST_CODE1_ZF"] + " " + SapRow["CITY1_ZF"];
                    //SapRow["AdresseBank"] = SapRow["NAME1_BANK"] + " " + SapRow["NAME2_BANK"] + "<br>" + SapRow["STREET_BANK"] + "<br>" + SapRow["POST_CODE1_BANK"] + " " + SapRow["CITY1_BANK"];
                    //SapRow["Selected"] = "";

                    if (SapRow["NAME1_BANK"].ToString() != "")
                    {
                        SapRow["Adresse"] = "Bankadresse: \r\n" + SapRow["NAME1_BANK"] + " " + SapRow["NAME2_BANK"] + "\r\n" + SapRow["STREET_BANK"] + "\r\n" + SapRow["POST_CODE1_BANK"] + " " + SapRow["CITY1_BANK"];

                        if (SapRow["NAME1_ZF"].ToString() != "")
                        {
                            SapRow["Adresse"] += "\r\n\r\n";
                        }

                    }

                    if (SapRow["NAME1_ZF"].ToString() != "")
                    {
                        SapRow["Adresse"] += "Händleradresse: \r\n" + SapRow["NAME1_ZF"] + " " + SapRow["NAME2_ZF"] + "\r\n" + SapRow["STREET_ZF"] + "\r\n" + SapRow["POST_CODE1_ZF"] + " " + SapRow["CITY1_ZF"];
                    }


                }


                m_tblAnforderungen = CreateOutPut(tblTemp, m_strAppID);

                ResultExcel = CreateOutPut(tblTemp, m_strAppID);
                foreach (DataRow SapRow in ResultExcel.Rows)
                {
                    String sAdresse;
                    sAdresse = SapRow["Adresse"].ToString().Replace("<br/>", " ");

                    SapRow["Adresse"] = sAdresse;
                }
                ResultExcel.Columns.Remove("Select");
               if (ResultExcel.Columns.Contains("Materialnr"))
               { 
                    ResultExcel.Columns.Remove("Materialnr");
               }
               

                if (mE_SUBRC != "0")
                {
                    m_intStatus = Convert.ToInt32(mE_SUBRC);
                    m_strMessage = mE_MESSAGE;
                }

                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblHaendler);
            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

        public void Change(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "VersandStorno.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_STORNO_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName.PadLeft(40));

                DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");

                foreach (DataRow SelRow in m_tblAnforderungen.Rows)
                {
                    if (SelRow["Select"].ToString() == "99")
                    {
                        DataRow tmpSAPRow = SapTable.NewRow();

                        tmpSAPRow["CHASSIS_NUM"] = SelRow["Fahrgestellnummer"];

                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();
                    }

                }

                myProxy.callBapi();

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");
                if (mE_SUBRC != "0")
                {
                    m_intStatus = Convert.ToInt32(mE_SUBRC);
                    m_strMessage = mE_MESSAGE;
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblAnforderungen);
                }
                else
                {
                    WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblAnforderungen);
                }

                m_tblError = myProxy.getExportTable("GT_ERR_OUT");


            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }
        public void ChangeMaterial(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "VersandStorno.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SET_MATNR_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_MATNR", m_strMaterialNr);
                myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName.PadLeft(40));



                DataTable SapTable = myProxy.getImportTable("GT_FIN_IN");

                foreach (DataRow SelRow in m_tblAnforderungen.Rows)
                {
                    if (SelRow["Select"].ToString() == "99") 
                    { 
                        DataRow tmpSAPRow = SapTable.NewRow();

                        tmpSAPRow["CHASSIS_NUM"] = SelRow["Fahrgestellnummer"];
                        
                        SapTable.Rows.Add(tmpSAPRow);
                        SapTable.AcceptChanges();                    
                    }
                
                }
                
                myProxy.callBapi();

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");
                if (mE_SUBRC != "0")
                {
                    m_intStatus = Convert.ToInt32(mE_SUBRC);
                    m_strMessage = mE_MESSAGE;
                    WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR, ref m_tblAnforderungen);
                }
                else
                {
                    WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblAnforderungen);
                }

           }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }

        public void GetMaterial(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "HaendlerSperren.GetMaterial";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "MATERIAL");



                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");

                DataTable ExportTable = new DataTable();

                ExportTable.Columns.Add("Matnr", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Materialtext", System.Type.GetType("System.String"));


                DataRow Row = null;
                DataRow NewRow = null;


                foreach (DataRow Row_loopVariable in tblTemp.Rows)
                {

                    Row = Row_loopVariable;
                    NewRow = ExportTable.NewRow();

                    NewRow["Matnr"] = Row["POS_KURZTEXT"].ToString();
                    NewRow["Materialtext"] = Row["POS_TEXT"].ToString();
                    ExportTable.Rows.Add(NewRow);
               }

                m_tblMaterial = ExportTable.Copy();
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblHaendler);

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_DATA RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }
        #endregion
    }
}
