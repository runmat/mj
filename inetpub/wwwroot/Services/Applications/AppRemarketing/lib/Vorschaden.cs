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
    public class Vorschaden : BankBase
        {
        #region "Declarations"

        String m_Filename2;
        #endregion

        #region "Properties"


        public DataTable Vermieter { get; set; }
        public String AVNr {get; set;}
        public String DatumVon {get; set;}
        public String DatumBis {get; set;}
        public String Fahrgestellnummer {get; set;}
        public String Kennzeichen {get; set;}
        public String Inventarnummer { get; set;}

        public String FahrgestellnummerEdit { get; set; }
        public String KennzeichenEdit { get; set; }
        public String LfdNummerEdit { get; set; }
        public String PreisEdit { get; set; }
        public String SchadensdatumEdit { get; set; }
        public String BeschreibungEdit { get; set; }
        public Boolean IsError { get; set; }
        public String HCNr { get; set; }
        public string Vertragsjahr { set; get; }
  
        #endregion

        #region "Methods"


        public Vorschaden(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
                                        : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
	    {
            this.m_Filename2 = strFilename;

	    }


        public override void Change()
        {

        }

        public override void Show()
        {

        }

        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Vorschaden.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_SCHADENSBERICHT_02", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));


                if (AVNr != "00")
                {
                    myProxy.setImportParameter("I_AVNR", AVNr);
                }

                if (Fahrgestellnummer != "" || Kennzeichen != "" )
                {
                    DataTable impTable = myProxy.getImportTable("GT_FIN_IN");

                    DataRow NewRow = impTable.NewRow();

                    NewRow["FAHRGNR"] = Fahrgestellnummer;
                    NewRow["KENNZ"] = Kennzeichen;

                    impTable.Rows.Add(NewRow);

                }
                else
                {
                    if (DatumVon != null) { myProxy.setImportParameter("I_SCHAD_DAT_VON", DatumVon); }
                    if (DatumBis != null) { myProxy.setImportParameter("I_SCHAD_DAT_BIS", DatumBis); }
                    if (Inventarnummer != null) { myProxy.setImportParameter("I_INVENTAR", Inventarnummer); }
                    if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); } 
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
                    foreach (DataRow Row in Result.Rows)
                    {

                        if (Row["SCHAD_DAT"] != null)
                        {

                            if (Row["SCHAD_DAT"].ToString().Length > 0)
                            {
                                Row["SCHAD_DAT"] = Row["SCHAD_DAT"].ToString().Substring(0, 10);
                            }

                        }

                        if (Row["PREIS"] != null)
                        {
                            if (Row["PREIS"].ToString().Contains(',')== false)
                            {
                                Row["PREIS"] = Row["PREIS"] + ",00";
                                
                            }
                        }


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


        public void ChangeVorschaden(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Vorschaden.ChangeVorschaden";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_AEND_SCHADEN_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));

                DataTable tblTmp = myProxy.getImportTable("GT_IN");

                DataRow newRow;

                newRow = tblTmp.NewRow();

                newRow["FAHRGNR"] = FahrgestellnummerEdit;
                newRow["KENNZ"] = KennzeichenEdit;
                newRow["LFDNR"] = LfdNummerEdit;
                newRow["PREIS"] = PreisEdit;
                newRow["SCHAD_DAT"] = SchadensdatumEdit;
                newRow["BESCHREIBUNG"] = BeschreibungEdit;

                tblTmp.Rows.Add(newRow);



                myProxy.callBapi();


                if (myProxy.getExportTable("GT_ERR").Rows.Count > 0)
                {
                    IsError = true;
                }


                WriteLogEntry(true, "KUNNR=" + m_objUser.KUNNR, ref m_tblResult);

            }
            catch (Exception ex)
            {
                IsError = true;

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


        public void ShowGutachten(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Vorschaden.ShowGutachten";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_REM_READ_HC_SCHADEN_01", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_AG", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_AVNR", AVNr);

                if (HCNr != "00")
                {
                    myProxy.setImportParameter("I_HCORT", HCNr);
                }

                myProxy.setImportParameter("I_KENNZ" , Kennzeichen);
                myProxy.setImportParameter("I_FAHRGNR", Fahrgestellnummer);
                myProxy.setImportParameter("I_INVENTAR", Inventarnummer);


                if (DatumVon != null) { myProxy.setImportParameter("I_GUTAUFTRAGDAT_VON", DatumVon); }
                if (DatumBis != null) { myProxy.setImportParameter("I_GUTAUFTRAGDAT_BIS", DatumBis); }
                if (Vertragsjahr != null) { myProxy.setImportParameter("I_VJAHR", Vertragsjahr); } 

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
                    foreach (DataRow Row in Result.Rows)
                    {

                        if (Row["AUSLDAT"] != null)
                        {

                            if (Row["AUSLDAT"].ToString().Length > 0)
                            {
                                Row["AUSLDAT"] = Row["AUSLDAT"].ToString().Substring(0, 10);
                            }

                        }

                        if (Row["HCEINGDAT"] != null)
                        {

                            if (Row["HCEINGDAT"].ToString().Length > 0)
                            {
                                Row["HCEINGDAT"] = Row["HCEINGDAT"].ToString().Substring(0, 10);
                            }

                        }

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

        public void getVermieter(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Fahrzeugbestand.getVermieter";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            try
            {
                DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_DPM_READ_AUFTR6_001", ref m_objApp, ref m_objUser, ref page);

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, '0'));
                myProxy.setImportParameter("I_KENNUNG", "VERMIETER");

                myProxy.callBapi();

                Vermieter = myProxy.getExportTable("GT_WEB");

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
