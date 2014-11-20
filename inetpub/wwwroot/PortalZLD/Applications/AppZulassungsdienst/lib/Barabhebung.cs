using System;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    public class clsBarabhebung : BankBase
    {
        public string Name
        {
            get;
            set;
        }
        public string Kostenstelle
        {
            get;
            set;
        }
        public string ECNr
        {
            get;
            set;
        }
        public string Datum
        {
            get;
            set;
        }
        public string Uhrzeit
        {
            get;
            set;
        }
        public string Ort
        {
            get;
            set;
        }
        public string Betrag
        {
            get;
            set;
        }
        public string Waehrung
        {
            get;
            set;
        }
        public byte[] PDFXString
        {
            get; 
            set; 
        }

        public clsBarabhebung(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID, string strFilename)
            : base(ref objUser,ref objApp, strAppID,  strSessionID, strFilename )
        {
            Filiale = objUser.Reference.Substring(4, 4);
            Waehrung = "EUR";
        }

        public override void Change()
        {}

        public override void Show()
        {}

        public void Change(String strAppID, String strSessionID, System.Web.UI.Page page) 
        {
            m_strClassAndMethod = "Barabhebung.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ZLD_BARABHEBUNG", ref m_objApp, ref m_objUser, ref page);

                    DataTable tblSAP = myProxy.getImportTable("IS_BARABHEBUNG");

                    DataRow tmpSAPRow = tblSAP.NewRow();
                    tmpSAPRow["VKBUR"] = Kostenstelle;
                    tmpSAPRow["NAME"] = Name;
                    tmpSAPRow["EC_KARTE_NR"] = ECNr;
                    tmpSAPRow["DATUM"] = Datum;
                    // Sap erwartet HHMMSS
                    tmpSAPRow["UZEIT"] = Uhrzeit.Replace(":", "") + "00";
                    tmpSAPRow["ORT"] = Ort;
                    tmpSAPRow["BETRAG"] = Betrag;
                    tmpSAPRow["WAERS"] = Waehrung;
                    tblSAP.Rows.Add(tmpSAPRow);

                    myProxy.callBapi();
                    
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;
                    PDFXString = myProxy.getExportParameterByte("E_PDF");
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }
    }
}
