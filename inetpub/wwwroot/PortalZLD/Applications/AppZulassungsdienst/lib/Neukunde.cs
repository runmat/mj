using System;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Neukundenanlage.
    /// </summary>
    public class Neukunde : BankBase
    {
        /// <summary>
        /// Ländertabelle aus SAP.
        /// </summary>
        public DataTable tblLaender
        {
            get;
            set;
        }
        /// <summary>
        /// Branchentabelle aus SAP.
        /// </summary>
        public DataTable tblBranchen
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle Funktion des Ansprechpartners.
        /// </summary>
        public DataTable tblFunktion
        {
            get;
            set;
        }
        /// <summary>
        /// Mitarbeiternummer
        /// </summary>
        public String MitarbeiterNr
        {
            get;
            set;
        }
        /// <summary>
        /// Verkaufsorganisation
        /// </summary>
        public String VKORG
        {
            get;
            set;
        }
        /// <summary>
        /// Verkaufsbüro
        /// </summary>
        public String VKBUR
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Barkunde oder Lieferscheinkunde
        /// </summary>
        public String Abruftyp
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Einzugsermächtigung
        /// </summary>
        public String EinzugEr
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Anrede des Ansprechpartners.
        /// </summary>
        public String Anrede
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Branche des Neukunden.
        /// </summary>
        public String Branche
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Branchenfreitext des Neukunden.
        /// </summary>
        public String BrancheFreitext
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Name1 des Neukunden.
        /// </summary>
        public String Name1
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Name1 des Neukunden.
        /// </summary>
        public String Name2
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Strasse des Neukunden.
        /// </summary>
        public String Strasse
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Ort des Neukunden.
        /// </summary>
        public String Ort
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Hausnummer des Neukunden.
        /// </summary>
        public String HausNr
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter PLZ des Neukunden.
        /// </summary>
        public String PLZ
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Land des Neukunden.
        /// </summary>
        public String Land
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter UmsatzsteuerID des Neukunden.
        /// </summary>
        public String UIDNummer
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Vorname des Ansprechpartners.
        /// </summary>
        public String ASPVorname
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Name des Ansprechpartners.
        /// </summary>
        public String ASPName
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Funktion des Ansprechpartners.
        /// </summary>
        public String Funktion
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Telfon des Ansprechpartners.
        /// </summary>
        public String Telefon
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Mobilenummmer des Ansprechpartners.
        /// </summary>
        public String Mobil
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter E-Mail-Adresse des Ansprechpartners.
        /// </summary>
        public String Mail
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Faxnummer des Ansprechpartners.
        /// </summary>
        public String Fax
        {
            get;
            set;
        }
        /// <summary>
        /// Aus SAP gelieferte neue Kundennummmer.
        /// </summary>
        public String NeueKUNNR
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Bankleitzahl des Neukunden.
        /// </summary>
        public String BLZ
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Kontonummer des Neukunden.
        /// </summary>
        public String Kontonr
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Name der Bank des Neukunden.
        /// </summary>
        public String Bankname
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Schlüssel der Bank des Neukunden.
        /// </summary>
        public String Bankkey
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter IBAN
        /// </summary>
        public String IBAN
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter SWIFT-BIC
        /// </summary>
        public String SWIFT
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Einzugsermächtigung des Neukunden.
        /// </summary>
        public Boolean Einzug
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Gebühr mit Umsatzsteuer.
        /// </summary>
        public Boolean UmSteuer
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Kreditversicherung ja/nein.
        /// </summary>
        public Boolean Kreditvers
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Auskunft ja/nein.
        /// </summary>
        public Boolean Auskunft
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter TourenID.
        /// </summary>
        public String TourID
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter Umsatzerwartung/Monat des Neukunden
        /// </summary>
        public String UmStErwartung
        {
            get;
            set;
        }
        /// <summary>
        /// Importparameter interne Bemerkung zum Neukunden.
        /// </summary>
        public String Bemerkung
        {
            get;
            set;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="objUser">Webuserobjekt</param>
        /// <param name="objApp">Apllikationsobjekt</param>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="strFilename">Filename Excel</param>
        public Neukunde(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
        { 
        

        }

        /// <summary>
        /// Overrides Change() Base
        /// </summary>
        public override void Change()
        { }

        /// <summary>
        /// Overrides Fill() Base
        /// </summary>
        public override void Show()
        { }

        /// <summary>
        /// Stammdaten aus SAP ziehen. Bapi: Z_ALL_DEBI_CHECK_TABLES
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Neukundenanlage.aspx</param>
        public void Fill(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Neukunde.Fill";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ALL_DEBI_CHECK_TABLES", ref m_objApp, ref m_objUser, ref page);


                    myProxy.callBapi();

                    tblLaender = myProxy.getExportTable("GT_T005");
                    tblBranchen = myProxy.getExportTable("GT_T016");
                    tblFunktion = myProxy.getExportTable("GT_TPFK");

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

        /// <summary>
        /// Erfasste Kundendaten an SAP senden. Bapi: Z_ALL_DEBI_VORERFASSUNG_WEB
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Neukundenanlage.aspx</param>
        public void Change(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Neukunde.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_ALL_DEBI_VORERFASSUNG_WEB", ref m_objApp, ref m_objUser, ref page);

                    DataTable tblSAP = myProxy.getImportTable("GS_IN");
                    DataRow SapRow = tblSAP.NewRow();
                            SapRow["BUKRS"] = VKORG;
                            SapRow["VKORG"] =VKORG;
                            SapRow["VKBUR"] = VKBUR;
                            SapRow["KALKS"] = Abruftyp;
                            SapRow["EZERM"] =EinzugEr;
                            SapRow["TITLE"] = Anrede;
                            SapRow["BRSCH"] = Branche;
                            SapRow["BRSCH_FREITXT"] = BrancheFreitext;
                            SapRow["NAME1"] =Name1;
                            SapRow["NAME2"] = Name2;
                            SapRow["NAME3"] = "";
                            SapRow["NAME4"] ="";
                            SapRow["STREET"] = Strasse;
                            SapRow["CITY1"] = Ort;
                            SapRow["HOUSE_NUM1"] =HausNr;
                            SapRow["POST_CODE1"] = PLZ;
                            SapRow["LAND1"] = Land;
                            SapRow["STCEG"] = UIDNummer;
                            SapRow["AP_NAMEV"] =ASPVorname;
                            SapRow["AP_NAME1"] = ASPName;
                            SapRow["AP_PAFKT"] = Funktion;
                            SapRow["AP_TEL_NUMBER"] =Telefon;
                            SapRow["AP_MOB_NUMBER"] = Mobil;
                            SapRow["AP_SMTP_ADDR"] = Mail;
                            SapRow["AP_FAX_NUMBER"] =Fax;
                            SapRow["QUELLE"] = "ZLD-Neu";
                            SapRow["ERNAM"] = m_objUser.UserName;
                            if (Bankkey.Length > 0) { 
                                SapRow["BANKS"] = "DE";
                                SapRow["BANKL"] = Bankkey;
                                SapRow["BNKLZ"] = BLZ;
                                SapRow["BANKN"] = Kontonr;
                                SapRow["IBAN"] = IBAN;
                                SapRow["SWIFT"] = SWIFT;
                            }
                            if (TourID.Length > 0) 
                            {
                                TourID = TourID.PadLeft(10, '0');
                            }
                            SapRow["GRUPPE_T"] = TourID;
                            SapRow["UMS_P_MON"] = UmStErwartung;
                            SapRow["GEB_M_UST"] = ZLDCommon.BoolToX(UmSteuer);
                            SapRow["KREDITVS"] = ZLDCommon.BoolToX(Kreditvers);
                            SapRow["AUSKUNFT"] = ZLDCommon.BoolToX(Auskunft);
                            SapRow["BEMERKUNG"] = Bemerkung;
                            tblSAP.Rows.Add(SapRow);

                   myProxy.callBapi();


                   Int32 subrc;
                   Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                   String sapMessage;
                   sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                   m_strMessage = sapMessage;
                   NeueKUNNR = myProxy.getExportParameter("E_VKUNNR").ToString();
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

        /// <summary>
        /// IBAN prüfen und daraus Bankinfos ermitteln. Bapi: Z_FI_CONV_IBAN_2_BANK_ACCOUNT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page"></param>
        public void ProofIBAN(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Neukunde.ProofIBAN";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FI_CONV_IBAN_2_BANK_ACCOUNT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_IBAN", IBAN);

                    myProxy.callBapi();

                    Bankname = myProxy.getExportParameter("E_BANKA");
                    BLZ = myProxy.getExportParameter("E_BANK_NUMBER");
                    Bankkey = BLZ;
                    Kontonr = myProxy.getExportParameter("E_BANK_ACCOUNT");
                    SWIFT = myProxy.getExportParameter("E_SWIFT");

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;
                    if (m_strMessage.Length > 0)
                    {
                        m_strMessage = "IBAN fehlerhaft: " + sapMessage;
                    }
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Fehler bei der IBAN-Prüfung: " + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message);
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }
        }
    }
}
