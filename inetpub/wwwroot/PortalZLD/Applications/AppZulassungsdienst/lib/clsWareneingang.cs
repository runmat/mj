using System;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Business;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Wareneingangsprüfung.
    /// </summary>
    public class clsWareneingang : CKG.Base.Business.BankBase
    {
        #region "Properties"

        /// <summary>
        /// Tabelle für die Anzeige der Umlagerungsposition
        /// </summary>
        public DataTable Bestellpositionen
        {
            get;
            set;
        }
        /// <summary>
        /// Tabelle für die Anzeige der Lieferungen
        /// </summary>
        public DataTable ErwarteteLieferungen
        {
            get;
            set;
        }
        /// <summary>
        /// Belegnummer SAP
        /// </summary>
        public String BELNR
        {
            get;
            set;
        }
        /// <summary>
        /// Verfaufsorganisation
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
        /// selektierte Belegnummer für die Detailanzeige(Umlagerung)
        /// </summary>
        public String BestellnummerSelection
        {
            get;
            set;
        }
        /// <summary>
        /// selektierte Belegnummer für die Detailanzeige(Liegerung)
        /// </summary>
        public String LieferantSelection
        {
            get;
            set;
        }
        /// <summary>
        /// Lieferant der Lieferung/Umlagerung
        /// </summary>
        public String Lieferant
        {
            get;
            set;
        }
        #endregion

        #region "Kontruktor"
        /// <summary>
        /// Kontruktor der Klasse.
        /// </summary>
        /// <param name="objUser">User-Objekt</param>
        /// <param name="objApp">Anwendungsobjekt</param>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="strFilename">String.Empty</param>
        public clsWareneingang(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, String strAppID, String strSessionID, string strFilename)
            : base(ref objUser,ref objApp, strAppID,  strSessionID, strFilename )
        {
        }
        #endregion

        #region "Methods"
        
        /// <summary>
        /// Overrides Change() in der CKG.Base.Business.BankBase.
        /// </summary>
        public override void Change()
        {}
        /// <summary>
        /// Overrides Show() in der CKG.Base.Business.BankBase.
        /// </summary>
        public override void Show()
        {}

        /// <summary>
        /// Liefert alle Umlagerungspositionen aus SAP zur aktuellen Belegnummer. Bapi: Z_FIL_EFA_UML_OFF_POS
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Wareneingang.aspx</param>
        public void getUmlPositionenFromSAP(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "clsWareneingang.getUmlPositionenFromSAP";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_UML_OFF_POS", ref m_objApp, ref m_objUser, ref page);


                    myProxy.setImportParameter("I_BELNR", BELNR);

                    myProxy.callBapi();
                    
                    DataTable tblTemp = myProxy.getExportTable("GT_OFF_UML_POS");



                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;

                    Bestellpositionen = new DataTable();
                    Bestellpositionen.Columns.Add("Bestellposition", typeof(Int32));
                    Bestellpositionen.Columns.Add("Materialnummer", typeof(String));
                    Bestellpositionen.Columns.Add("Artikelbezeichnung", typeof(String));
                    Bestellpositionen.Columns.Add("MaterialnummerLieferant", typeof(String));
                    Bestellpositionen.Columns.Add("BestellteMenge", typeof(Int32));
                    Bestellpositionen.Columns.Add("Mengeneinheit", typeof(String));
                    Bestellpositionen.Columns.Add("Buchungsdatum", typeof(String));
                    Bestellpositionen.Columns.Add("EAN", typeof(String));
                    Bestellpositionen.Columns.Add("PositionLieferMenge", typeof(Int32));
                    Bestellpositionen.Columns.Add("PositionAbgeschlossen", typeof(String));
                    Bestellpositionen.Columns.Add("PositionVollstaendig", typeof(String));
                    Bestellpositionen.Columns.Add("Freitext", typeof(String));
                    Bestellpositionen.Columns.Add("TextNr", typeof(String));
                    Bestellpositionen.Columns.Add("LangText", typeof(String));
                    Bestellpositionen.Columns.Add("KennzForm", typeof(String));
                    foreach (DataRow tmpRow in tblTemp.Rows)
                    {
                        DataRow  rowNew = Bestellpositionen.NewRow();
                        Int32 iTemp = 0;

                        if (ZLDCommon.IsNumeric(tmpRow["POSNR"].ToString()))
                        {
                            Int32.TryParse(tmpRow["POSNR"].ToString(), out iTemp);
                        }
                        rowNew["Bestellposition"] = iTemp;
                        rowNew["Materialnummer"] = tmpRow["MATNR"].ToString();
                        rowNew["Artikelbezeichnung"] = tmpRow["MAKTX"].ToString();
                        rowNew["MaterialnummerLieferant"] = tmpRow["MATNR"].ToString();

                        if (ZLDCommon.IsNumeric(tmpRow["MENGE"].ToString()))
                        {
                            Int32.TryParse(tmpRow["MENGE"].ToString(), out iTemp);
                        }
                        rowNew["BestellteMenge"] = iTemp;
                        rowNew["Buchungsdatum"] = tmpRow["BUDAT"].ToString();
                        rowNew["Mengeneinheit"] = "";
                        rowNew["EAN"] = tmpRow["EAN11"].ToString();
                        rowNew["PositionLieferMenge"] = DBNull.Value;
                        rowNew["PositionAbgeschlossen"]= "";
                        rowNew["PositionVollstaendig"] = "";
                        rowNew["Freitext"] = tmpRow["TEXT"].ToString();
                        rowNew["TextNr"] = tmpRow["LTEXT_NR"].ToString();
                        LongStringToSap LSTS = new LongStringToSap(m_objUser, m_objApp, page);
                        if (rowNew["TextNr"].ToString() != "")
                        {
                            rowNew["LangText"] = LSTS.ReadString(rowNew["TextNr"].ToString());
                        }
                        rowNew["KennzForm"] = tmpRow["KennzForm"].ToString();
                        Bestellpositionen.Rows.Add(rowNew);
                    }

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
        /// Gibt alle offenen Lieferungen aus SAP zur aktuellen Belegnummer zurück. Bapi: Z_FIL_READ_OFF_BEST_001
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">Wareneingang.aspx</param>
        public void getErwarteteLieferungenFromSAP(String strAppID, String strSessionID, System.Web.UI.Page page)
        {
            m_strClassAndMethod = "clsWareneingang.getErwarteteLieferungenFromSAP";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_READ_OFF_BEST_001", ref m_objApp, ref m_objUser, ref page);


                    myProxy.setImportParameter("I_LGORT", VKBUR);
                    myProxy.setImportParameter("I_LIFNR", "");

                    myProxy.callBapi();

                    DataTable tblTemp = myProxy.getExportTable("GT_OFF_UML");
                    
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();
                    m_strMessage = sapMessage;

                    ErwarteteLieferungen = new DataTable();
                    ErwarteteLieferungen.Columns.Add("Bestellnummer", typeof(String));
                    ErwarteteLieferungen.Columns.Add("LieferantName", typeof(String));
                    ErwarteteLieferungen.Columns.Add("AnzeigeText", typeof(String));

                    foreach (DataRow tmpRow in tblTemp.Rows)
                    {
                        DataRow rowNew = ErwarteteLieferungen.NewRow();
                        rowNew["Bestellnummer"] = tmpRow["BELNR"].ToString();
                        rowNew["LieferantName"] = tmpRow["KTEXT"].ToString();
                        DateTime tmpDate;
                        if (ZLDCommon.IsDate(tmpRow["BUDAT"].ToString()))
                        {
                            DateTime.TryParse(tmpRow["BUDAT"].ToString(), out tmpDate);
                            rowNew["AnzeigeText"] = tmpRow["BELNR"].ToString() + " " + tmpRow["KTEXT"].ToString() + " " + tmpDate.ToShortDateString();
                        }
                        else 
                        {
                            DateTime.TryParse(tmpRow["BUDAT"].ToString(), out tmpDate);
                            rowNew["AnzeigeText"] = tmpRow["BELNR"].ToString() + " " + tmpRow["BELNR"].ToString() ;

                        }
                        ErwarteteLieferungen.Rows.Add(rowNew);
                    }

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
        /// Schließt Umlagerung in SAP ab. Bapi: Z_FIL_EFA_UML_STEP2
        /// </summary>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">WareneingangDetails.aspx</param>
        /// <param name="Belegdatum">Belegdatum</param>
        public void sendUmlToSAP(String strAppID, String strSessionID, System.Web.UI.Page page, String Belegdatum)
        {
            m_strClassAndMethod = "clsWareneingang.sendUmlToSAP";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_UML_STEP2", ref m_objApp, ref m_objUser, ref page);


                    myProxy.setImportParameter("I_KOSTL", VKBUR);
                    myProxy.setImportParameter("I_BELNR", BELNR);
                    myProxy.setImportParameter("I_BUDAT", Belegdatum);

                    DataTable tblSap = myProxy.getImportTable("GT_OFF_UML_POS");

                    Boolean ueberspringe = false;
                    foreach (DataRow tmprow in Bestellpositionen.Rows)
                    {
                        DataRow tmpSAPRow = tblSap.NewRow();

                        tmpSAPRow["BELNR"] = BELNR;
                        tmpSAPRow["POSNR"] = tmprow["Bestellposition"].ToString();
                        tmpSAPRow["MATNR"] = tmprow["Materialnummer"].ToString();
                        tmpSAPRow["MAKTX"] = tmprow["Artikelbezeichnung"].ToString();
                        tmpSAPRow["BUDAT"] = Belegdatum;
                        tmpSAPRow["EAN11"] = tmprow["EAN"].ToString();
                        tmpSAPRow["Kennzform"] = tmprow["Kennzform"].ToString();
                        if (tmprow["PositionVollstaendig"].ToString() == "X")
                        {
                            tmpSAPRow["MENGE"] = tmprow["BestellteMenge"].ToString();

                        }
                        else 
                        {       Int32 i = 0;
                        if (ZLDCommon.IsNumeric(tmprow["PositionLieferMenge"].ToString()))
                                { Int32.TryParse(tmprow["PositionLieferMenge"].ToString(), out i); }
                                if (i > 0 )
                                { tmpSAPRow["MENGE"] = tmprow["PositionLieferMenge"].ToString(); }
                                else{ueberspringe = true;}   
                                                        
                        }
                       
                        if (!ueberspringe)
                        {
                            tblSap.Rows.Add(tmpSAPRow);
                        }
                           
                        ueberspringe = false;
                    }

                    myProxy.callBapi();

                    
                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    m_intStatus = subrc;
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

        #endregion
    }
}
