using System;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für Umlagerungen der Filiale.
    /// </summary>
    public class clsUmlagerung : CKG.Base.Business.BankBase
    {
        /// <summary>
        /// Tabelle erfasster Umlagerungen.
        /// </summary>
        public DataTable tblUmlagerung
        {
            get;
            set;
        }
        /// <summary>
        /// Stammdatentabelle Artikel, die umgelagert werden können. 
        /// </summary>
        public DataTable tblArtikel
        {
            get;
            set;
        }
        /// <summary>
        /// Stammdatentabelle Kennzeichenform.
        /// </summary>
        public DataTable tblKennzForm
        {
            get;
            set;
        }
        /// <summary>
        /// Kostenstelle an die umgelagert werden soll.
        /// </summary>
        public String KostStelleNeu
        {
            get;
            set;
        }
        /// <summary>
        /// Name der Kostenstelle an die umgelagert werden soll.
        /// </summary>
        public String KostText
        {
            get;
            set;
        }
        /// <summary>
        /// Belegnummer der Umlagerung.
        /// </summary>
        public String BelegNR
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
        /// Konstruktor clsUmlagerung.
        /// </summary>
        /// <param name="objUser">Webuserobjekt</param>
        /// <param name="objApp">Applikationsobjekt</param>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="strFilename">Filename</param>
        public clsUmlagerung(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID, string strFilename)
            : base(ref objUser,ref objApp, strAppID,  strSessionID, strFilename )
        {
            tblUmlagerung = new DataTable();

            tblUmlagerung.Columns.Add("MATNR", typeof(String));
            tblUmlagerung.Columns.Add("UMLGO", typeof(String));
            tblUmlagerung.Columns.Add("MAKTX", typeof(String));
            tblUmlagerung.Columns.Add("EAN11", typeof(String));
            tblUmlagerung.Columns.Add("Menge", typeof(Int32));
            tblUmlagerung.Columns.Add("LTEXT_NR", typeof(String));
            tblUmlagerung.Columns.Add("LTEXT", typeof(String));
            tblUmlagerung.Columns.Add("KENNZFORM", typeof(String));
            KostStelleNeu = "";
        }
        /// <summary>
        /// Overrides Change() BankBase
        /// </summary>
        public override void Change()
        {}
        /// <summary>
        /// Overrides Show() BankBase
        /// </summary>
        public override void Show()
        {}
        /// <summary>
        /// Laden der Umlagerungsartikel aus SAP. Bapi: Z_FIL_EFA_UML_MAT
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Umlagerung.aspx</param>
        public void Show(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "Umlagerung.Show";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_UML_MAT", ref m_objApp, ref m_objUser, ref page);


                    myProxy.setImportParameter("I_KOSTL", VKBUR);

                    myProxy.callBapi();

                    tblArtikel = myProxy.getExportTable("GT_MAT");
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden zum Barcode gefunden.";
                            break;
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
        /// Überprüfen der Kostenstellen ob eine Umlagerung möglich ist. Bapi: Z_FIL_EFA_GET_KOSTL
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Umlagerung.aspx</param>
        /// <param name="NeuKost">Kostenstelle and umgelagert werden soll</param>
        public void CheckKostStelle (String strAppID, String strSessionID, System.Web.UI.Page page, String NeuKost )
        {
            m_strClassAndMethod = "Umlagerung.CheckKostStelle";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_GET_KOSTL", ref m_objApp, ref m_objUser, ref page);


                    myProxy.setImportParameter("I_KOSTL_SEND", VKBUR.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_KOSTL_RECEIVE", NeuKost.PadLeft(10, '0'));

                    myProxy.callBapi();

                    KostText = myProxy.getExportParameter("E_KTEXT").ToString();
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();

                    switch (subrc)
                    {
                        case 102:
                            m_strMessage = "KST " + NeuKost + " ist nicht zulässig! Bitte einen Lieferscheinverkauf eingeben.";
                            break;
                        case 104:
                            m_strMessage = "KST nicht zulässig! Bitte richtige KST eingeben.";
                            break;
                        default:
                            m_strMessage = sapMessage;
                            break;
                    }



                    m_strMessage = sapMessage;
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
                }
                finally { m_blnGestartet = false; }
            }
        }
        /// <summary>
        /// Übergeben der erfassten Umlagerungen an SAP. 
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Umlagerung.aspx</param>
        public void Change(String strAppID, String strSessionID, System.Web.UI.Page page) 
        {
            m_strClassAndMethod = "Umlagerung.Change";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;
            DataTable tblSAP;
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_UML_STEP1", ref m_objApp, ref m_objUser, ref page);


                    myProxy.setImportParameter("I_KOSTL_IN", KostStelleNeu);
                    myProxy.setImportParameter("I_KOSTL_OUT", VKBUR);
                    tblSAP = myProxy.getImportTable("GT_MAT");

                    foreach (DataRow tmpRow in tblUmlagerung.Rows)
                    {
                        DataRow tmpSAPRow = tblSAP.NewRow();
                        tmpSAPRow["MATNR"] = tmpRow["MATNR"].ToString();
                        tmpSAPRow["MENGE"] = tmpRow["MENGE"].ToString();
                        tmpSAPRow["EAN11"] = tmpRow["EAN11"].ToString();
                        LongStringToSap LSTS = new LongStringToSap(m_objUser, m_objApp, page);
                        if (tmpRow["LTEXT_NR"].ToString() == "")
                        {
                            if (tmpRow["LTEXT"].ToString() != "")
                            {
                                tmpSAPRow["LTEXT_NR"] = LSTS.InsertString( tmpRow["LTEXT"].ToString(), VKBUR);
                            }
                        }
                        else
                        {
                            tmpSAPRow["LTEXT_NR"] = tmpRow["LTEXT"].ToString();
                        }
                        tmpSAPRow["KENNZFORM"] = tmpRow["KENNZFORM"].ToString();
                        tblSAP.Rows.Add(tmpSAPRow);
                    }

                    myProxy.callBapi();

                    DataTable BelegTable = new DataTable();

                    BelegTable = myProxy.getExportTable("GT_BELNR");

                    if (BelegTable.Rows.Count > 0) 
                    {
                        for (int i = 0; i < BelegTable.Rows.Count; i++)
                        {
                            BelegNR += BelegTable.Rows[i]["BELNR"].ToString() + Environment.NewLine;               
                        }   
                    }

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;
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
        /// Einfügen der Umlagerungsdaten in die Webtabelle.
        /// </summary>
        /// <param name="Artikelnr">Artikelnr.</param>
        /// <param name="Menge">Menge</param>
        /// <param name="Artikelbezeichnung">Artikelbezeichnung</param>
        /// <param name="EAN">EAN</param>
        /// <param name="LTextNR">Langtextnummer</param>
        /// <param name="LText">Langtext</param>
        /// <param name="Kennzform">Kennzeichenform</param>
        public void insertIntoBestellungen(String Artikelnr, String Menge, String Artikelbezeichnung, String EAN, String LTextNR, String LText, String Kennzform)
        {
            DataRow[] Rows;
            if (Kennzform == "") 
            {
                Rows = tblUmlagerung.Select("MATNR='" + Artikelnr + "'");
            }
            else
            {
                Rows = tblUmlagerung.Select("MATNR='" + Artikelnr + "' AND KennzForm = '" + Kennzform + "'");
            }
            
            DataRow tmpRow;

            if (Rows.GetLength(0) > 0)
            {
                tmpRow = Rows[0];
                tmpRow["UMLGO"] = KostStelleNeu;
                tmpRow["MATNR"] = Artikelnr;
                tmpRow["MAKTX"] = Artikelbezeichnung;
                tmpRow["EAN11"] = EAN;
                tmpRow["Menge"] = Menge;
                tmpRow["LTEXT_NR"] = LTextNR;
                tmpRow["LTEXT"] = LText;
                tmpRow["KENNZFORM"] = Kennzform;
            }
            else
            {
                tmpRow = tblUmlagerung.NewRow();
                tmpRow["UMLGO"] = KostStelleNeu;
                tmpRow["MATNR"] = Artikelnr;
                tmpRow["MAKTX"] = Artikelbezeichnung;
                tmpRow["EAN11"] = EAN;
                tmpRow["Menge"] = Menge;
                tmpRow["LTEXT_NR"] = LTextNR;
                tmpRow["LTEXT"] = LText;
                tmpRow["KENNZFORM"] = Kennzform;
                tblUmlagerung.Rows.Add(tmpRow);
            }
        }
        ///// <summary>
        ///// Laden eines Artikel mittels EAN aus SAP. Bapi: Z_FIL_READ_MATNR_001
        ///// </summary>
        ///// <param name="strAppID">AppID</param>
        ///// <param name="strSessionID">SessionID</param>
        ///// <param name="page">Umlagerung.aspx</param>
        ///// <param name="EAN">EAN</param>
        ///// <param name="Materialnummer">ref Materialnummer</param>
        ///// <param name="Artikelbezeichnung">ref Artikelbezeichnung</param>
        //public void getEANFromSAP(String strAppID, String strSessionID, System.Web.UI.Page page, String EAN,
        //                          ref String Materialnummer, ref String Artikelbezeichnung)
        //{
        //    m_strClassAndMethod = "Umlagerung.getEANFromSAP";
        //    m_strAppID = strAppID;
        //    m_strSessionID = strSessionID;
        //    m_intStatus = 0;
        //    m_strMessage = String.Empty;

        //    if (m_blnGestartet == false)
        //    {
        //        m_blnGestartet = true;
        //        try
        //        {
        //            DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_READ_MATNR_001", ref m_objApp, ref m_objUser, ref page);


        //            myProxy.setImportParameter("I_KOSTL", VKBUR.PadLeft(10, '0'));
        //            myProxy.setImportParameter("I_EAN11", EAN);

        //            myProxy.callBapi();

        //            Materialnummer = myProxy.getExportParameter("E_MATNR").ToString();
        //            Artikelbezeichnung = myProxy.getExportParameter("E_MAKTX").ToString();
        //            Int32 subrc;
        //            Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
        //            String sapMessage;
        //            sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();

        //            switch (subrc)
        //            {
        //                case 103:
        //                    m_strMessage = "Der Artikel ist nicht mehr bestellbar!";
        //                    break;

        //                default:
        //                    m_strMessage = sapMessage;
        //                    break;
        //            }
        //            m_strMessage = sapMessage;
        //        }
        //        catch (Exception ex)
        //        {
        //            switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
        //            {
        //                default:
        //                    m_intStatus = -9999;
        //                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
        //                    break;
        //            }
        //        }
        //        finally { m_blnGestartet = false; }
        //    }
        //}

        /// <summary>
        /// Ermitteln der möglichen Kennzeichengrössen zum Umlagerungsartikel. Bapi: Z_FIL_EFA_UML_MAT_GROESSE
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Umlagerung.aspx</param>
        /// <param name="MATNR">Materialnummer</param>
        public void GetKennzForm(String strAppID, String strSessionID, System.Web.UI.Page page, String MATNR)
        {
            m_strClassAndMethod = "Umlagerung.GetKennzForm";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_UML_MAT_GROESSE", ref m_objApp, ref m_objUser, ref page);


                    myProxy.setImportParameter("I_MATNR", MATNR);

                    myProxy.callBapi();

                    tblKennzForm = myProxy.getExportTable("GT_MAT");
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc); m_intStatus = subrc;
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString(); m_strMessage = sapMessage;
                }
                catch (Exception ex)
                {
                    switch (HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                    {
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden zum Barcode gefunden.";
                            break;
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
