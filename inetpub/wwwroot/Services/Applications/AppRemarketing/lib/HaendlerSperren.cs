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
    public class HaendlerSperrenPublic : BankBase
    {
        #region "Declaclarations"

        String m_strFilename2;
        DataTable m_tblHaendler;
        String m_strKennung;
        String m_strHaendlernr;
        String m_strDebitor;
        Int32 inthaendlerTreffer;
        DataView m_vwHaendler;
        DataView m_vwHaendlerGesperrt;
        DataTable m_tblAnforderungen;
        String mE_SUBRC;
        String mE_MESSAGE;
        Boolean mIstGesperrt = false;


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

        public String Debitor
        {
            get { return m_strDebitor; }
            set { m_strDebitor = value; }
        }

        public DataView Haendler
        {
            get { return m_vwHaendler; }
        }

        public DataView HaendlerGesperrt
        {
            get { return m_vwHaendlerGesperrt; }
        }


        public DataTable tblAnforderungen
        {
            get { return m_tblAnforderungen; }
            set { m_tblAnforderungen = value; }
        }

        public Int32 AnzahlHaendler
        {
            get { return inthaendlerTreffer; }
        }


        public Boolean IstGesperrt
        {
            get { return mIstGesperrt; }
            set { mIstGesperrt = value; }
        }
        public String Haendlernr
        {
            get { return m_strHaendlernr; }
            set { m_strHaendlernr = value; }
        }

        #endregion

        #region "Methods"


        public HaendlerSperrenPublic(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
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

        public void GetHaendler(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "HaendlerSperren.GetHaendler";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_M_IMP_AUFTRDAT_007", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", m_strKennung);



                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_WEB");
                
                DataTable ExportTable = new DataTable();

                ExportTable.Columns.Add("Debitor", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("REFERENZ", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Adresse", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Name1", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Name2", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("STRAS", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("PSTLZ", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("ORT01", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("EMAIL", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("LAND1", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("TELNR", System.Type.GetType("System.String"));

                DataRow Row = null;
                DataRow NewRow = null;
                long i = 0;
                string Adresse = null;


                foreach (DataRow Row_loopVariable in tblTemp.Rows)
                {

                    Row = Row_loopVariable;
                    NewRow = ExportTable.NewRow();

                    NewRow["REFERENZ"] = Row["POS_TEXT"].ToString();
                    NewRow["Debitor"] = Row["POS_KURZTEXT"].ToString();
                    NewRow["Name1"] = Row["Name1"].ToString();
                    NewRow["Name2"] = Row["Name2"].ToString();
                    NewRow["STRAS"] = Row["STRAS"].ToString();
                    NewRow["PSTLZ"] = Row["PSTLZ"].ToString();
                    NewRow["ORT01"] = Row["ORT01"].ToString();
                    NewRow["EMAIL"] = Row["EMAIL"].ToString();
                    NewRow["LAND1"] = Row["LAND1"].ToString();
                    NewRow["TELNR"] = Row["TELNR"].ToString();

                    //Adresse für Ausgabe in Dropdown verketten
                    Adresse = Row["Name1"].ToString() + "|";
                    if (Kennung == "EVB")
                    {
                        Adresse = Adresse + Row["POS_TEXT"].ToString();
                    }
                    else
                    {
                        Adresse = Adresse + Row["STRAS"].ToString() + "|";
                        Adresse = Adresse + Row["PSTLZ"].ToString() + "|";
                        Adresse = Adresse + Row["ORT01"].ToString();
                    }


                    NewRow["Adresse"] = Adresse;

                    ExportTable.Rows.Add(NewRow);
                    i = i + 1;
                }
                inthaendlerTreffer = ExportTable.Rows.Count;


               // m_vwHaendler = ExportTable.DefaultView;
                m_vwHaendlerGesperrt = ExportTable.DefaultView;
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
       public void GetHaendlerUngesperrt(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "HaendlerSperren.GetHaendlerUngesperrt";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            mE_SUBRC = "";
            mE_MESSAGE = "";

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_EQTYP", "T");



                myProxy.callBapi();

                DataTable tblTemp = myProxy.getExportTable("GT_ADRS");


                //mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
                //mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

                DataTable ExportTable = new DataTable();

                ExportTable.Columns.Add("Debitor", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Referenz", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Adresse", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Name1", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("Name2", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("STRAS", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("PSTLZ", System.Type.GetType("System.String"));
                ExportTable.Columns.Add("ORT01", System.Type.GetType("System.String"));

                DataRow Row = null;
                DataRow NewRow = null;
                long i = 0;
                string Adresse = null;


                foreach (DataRow Row_loopVariable in tblTemp.Rows)
                {

                    Row = Row_loopVariable;
                    NewRow = ExportTable.NewRow();

                    NewRow["Debitor"] = Row["KUNNR"].ToString();
                    NewRow["Referenz"] = Row["KONZS"].ToString();
                    NewRow["Name1"] = Row["Name1"].ToString();
                    NewRow["Name2"] = Row["Name2"].ToString();
                    NewRow["STRAS"] = Row["STREET"].ToString() + " " + Row["HOUSE_NUM1"].ToString();
                    NewRow["PSTLZ"] = Row["POST_CODE1"].ToString();
                    NewRow["ORT01"] = Row["CITY1"].ToString();


                    //Adresse für Ausgabe verketten
                    Adresse = Row["Name1"].ToString() + " " + Row["Name2"].ToString() + "  -  " + Row["STREET"].ToString() + ", " + Row["POST_CODE1"].ToString() + " " + Row["CITY1"].ToString(); ;

                    NewRow["Adresse"] = Adresse;

                    ExportTable.Rows.Add(NewRow);
                    i = i + 1;
                }
                inthaendlerTreffer = ExportTable.Rows.Count;


                m_vwHaendler = ExportTable.DefaultView;
                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblHaendler);

                //  if (mE_SUBRC != "0")
                //{
                //    m_intStatus = Convert.ToInt32(mE_SUBRC);
                //    m_strMessage = mE_MESSAGE;                    
                //}

            }
            catch (Exception ex)
            {
                switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                {

                    case "EXCEPTION NO_ADRPOOL RAISED":
                        m_intStatus = -1111;
                        m_strMessage = "kein Adresspool zugeordnet.";
                        break;

                    default:
                        m_intStatus = -9999;
                        m_strMessage = "Fehler beim Abfragen der Händlerdaten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                        break;
                }

                WriteLogEntry(false, "KUNNR=" + m_objUser.KUNNR + "," + m_strMessage.Replace("<br>", " "), ref m_tblResult);

            }

        }
       public void EntSperren(String strAppID, String strSessionID, System.Web.UI.Page page)
       {
           m_strClassAndMethod = "HaendlerSperren.EntSperren";
           m_strAppID = strAppID;
           m_strSessionID = strSessionID;
           m_intStatus = 0;
           m_strMessage = String.Empty;
           mE_SUBRC = "";
           mE_MESSAGE = "";
           try
           {
               DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_ENTSP_HAEND_FZG_01", ref m_objApp, ref m_objUser, ref page);

               myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
               myProxy.setImportParameter("I_KUNNR_ZF", m_strDebitor);
               myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName.PadLeft(40));


               myProxy.callBapi();

               mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
               mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

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

       public void Sperren(String strAppID, String strSessionID, System.Web.UI.Page page)
       {
           m_strClassAndMethod = "HaendlerSperren.Sperren";
           m_strAppID = strAppID;
           m_strSessionID = strSessionID;
           m_intStatus = 0;
           m_strMessage = String.Empty;
           mE_SUBRC = "";
           mE_MESSAGE="";
           try
           {
               DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SPERR_HAEND_FZG_01", ref m_objApp, ref m_objUser, ref page);

               myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, '0'));
               myProxy.setImportParameter("I_KUNNR_ZF", m_strDebitor);
               myProxy.setImportParameter("I_WEB_USER", m_objUser.UserName.PadLeft(40));



               myProxy.callBapi();

               mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
               mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

           
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

       public void GetVersAnf(String strAppID, String strSessionID, System.Web.UI.Page page)
       {
           m_strClassAndMethod = "HaendlerSperren.GetVersAnf";
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
               myProxy.setImportParameter("I_KUNNR_ZF", m_strDebitor);

               myProxy.callBapi();

               DataTable tblTemp = myProxy.getExportTable("GT_OUT");
               mE_SUBRC = myProxy.getExportParameter("E_SUBRC");
               mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE");

               tblTemp.Columns.Add("Adresse", System.Type.GetType("System.String"));

               foreach (DataRow SapRow in tblTemp.Rows)
               {

                   String sAdresse;
                   sAdresse = SapRow["NAME1_ZF"].ToString();
                   if (SapRow["NAME2_ZF"].ToString().Length > 0)
                   {
                       sAdresse += " " + SapRow["NAME2_ZF"].ToString();
                   }
                   if (SapRow["NAME3_ZF"].ToString().Length > 0)
                   {
                       sAdresse += " " + SapRow["NAME3_ZF"].ToString();
                   }
                   sAdresse += "<br/>" + SapRow["STREET_ZF"].ToString();
                   sAdresse += "<br/>" + SapRow["POST_CODE1_ZF"].ToString() + " " + SapRow["CITY1_ZF"].ToString();
                   SapRow["Adresse"] = sAdresse;
               }


               m_tblAnforderungen = CreateOutPut(tblTemp, m_strAppID);



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
        #endregion
    }
}
