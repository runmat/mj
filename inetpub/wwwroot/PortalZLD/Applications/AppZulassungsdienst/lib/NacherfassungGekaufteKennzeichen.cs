using System;
using System.Data;
using CKG.Base.Common;
using CKG.Base.Kernel.Security;
using CKG.Base.Business;
using System.Collections.Generic;

namespace AppZulassungsdienst.lib
{
    public class NacherfassungGekaufteKennzeichen : DatenimportBase
    {
        private System.Web.UI.Page objPage;

        /// <summary>
        /// Basisklasse zur Nacherfassung gekaufter Kennzeichen
        /// </summary>
        /// <param name="user">User-Objekt</param>
        /// <param name="app">App-Objekt</param>
        /// <param name="strAppID">AppID</param>
        /// <param name="strSessionID">SessionID</param>
        /// <param name="page">aktuelle Page</param>
        public NacherfassungGekaufteKennzeichen(ref User user,App app,string strAppID,string strSessionID,System.Web.UI.Page page): 
            base(ref user,app,"")
        {
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;

            VKBUR = m_objUser.Reference.Substring(4, 4);
            VKORG = m_objUser.Reference.Substring(0, 4);

            objPage = page;

            tblKennzeichen = new DataTable();

            tblKennzeichen.Columns.Add("Datum", typeof(string));
            tblKennzeichen.Columns.Add("LieferantID", typeof(string));
            tblKennzeichen.Columns.Add("ArtikelID", typeof(string));
            tblKennzeichen.Columns.Add("Lieferscheinnummer", typeof(string));
            tblKennzeichen.Columns.Add("Artikel", typeof(string));
            tblKennzeichen.Columns.Add("Menge", typeof(string));
            tblKennzeichen.Columns.Add("Preis", typeof(string));
            tblKennzeichen.Columns.Add("LangtextID", typeof(string));
            tblKennzeichen.Columns.Add("Langtext", typeof(string));

            tblKennzeichen.AcceptChanges();

            GetLieferanten();            
        }

        #region "Properties"

        public DataTable tblKennzeichen
        {
            get;
            private set;
        }

        public DataTable tblArtikel
        {
            get;
            private set;
        }

        public DataTable tblLieferanten
        {
            get;
            private set;
        }

        public string VKBUR
        {
            get;
            private set;
        }

        public string VKORG
        {
            get;
            private set;
        }

        #endregion

        /// <summary>
        /// Liefert eine Liste aller Lieferanten zur aktuellen Kostenstelle
        /// </summary>
        /// <returns>Lieferantenliste</returns>
        private void GetLieferanten()
        {
            m_strClassAndMethod = "NacherfassungGekaufteKennzeichen.GetLieferanten";
            m_intStatus = 0;
            m_strMessage = String.Empty;
            
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_PLATSTAMM", ref m_objApp, ref m_objUser, ref objPage);

                    string IsFil = "";
                    string IsZLD = "";

                    if (m_objUser.CustomerName.Contains("ZLD")) 
                    { 
                        IsZLD = "X"; 
                    }
                    else if (m_objUser.CustomerName.Contains("Kroschke")) 
                    { 
                        IsFil = "X"; 
                    }

                    myProxy.setImportParameter("I_KOSTL",VKBUR.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_SUPER_USER", "");
                    myProxy.setImportParameter("I_FIL", IsFil);
                    myProxy.setImportParameter("I_ZLD", IsZLD);

                    myProxy.callBapi();

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();

                    switch (subrc)
                    {
                        case 104:
                            m_strMessage = "KST nicht zulässig! Bitte richtige KST eingeben.";
                            break;
                        default:
                            m_strMessage = sapMessage;
                            break;
                    }

                    tblLieferanten = myProxy.getExportTable("GT_PLATSTAMM");

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
        /// Liefert eine Liste aller Artikel zum gewählten Lieferanten
        /// </summary>
        /// <param name="LieferantenID">LieferantenID</param>
        /// <returns>Artikelliste</returns>
        internal DataTable GetArtikel(string LieferantenID)
        {
            m_strClassAndMethod = "NacherfassungGekaufteKennzeichen.GetLieferanten";
            m_intStatus = 0;
            m_strMessage = String.Empty;
            
            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_PLATARTIKEL", ref m_objApp, ref m_objUser, ref objPage);


                    myProxy.setImportParameter("I_KOSTL", VKBUR.PadLeft(10, '0'));
                    myProxy.setImportParameter("I_LIFNR", LieferantenID);
                    myProxy.setImportParameter("I_RUECKS", " ");
                    myProxy.setImportParameter("I_FIL", " ");
                    myProxy.setImportParameter("I_ZLD", "X");
                    

                    myProxy.callBapi();

                    Int32 subrc;
                    Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                    String sapMessage;
                    sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();

                    switch (subrc)
                    {
                        case 104:
                            m_strMessage = "KST nicht zulässig! Bitte richtige KST eingeben.";
                            break;
                        default:
                            m_strMessage = sapMessage;
                            break;
                    }

                    tblArtikel = myProxy.getExportTable("GT_PLATART");

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

            return tblArtikel;
        }

        /// <summary>
        /// Prüft ob für einen Artikel ein Preis erfasst werden muss
        /// </summary>
        /// <param name="ArtikelID">Artikelnummer</param>
        /// <returns>True wenn Pflicht sonst False</returns>
        internal bool CheckpreisNeeded(string ArtikelID)
        {
            bool bNeed = false;

            DataRow[] rows = tblArtikel.Select("ARTLIF = '" + ArtikelID + "'");

            if (rows.GetLength(0) > 0)
            {
                if (rows[0]["PREISPFLICHT"].ToString().Trim() == "X")
                { bNeed = true; }                
            }

            return bNeed;
        }

        /// <summary>
        /// Prüft ob für einen Artikel ein Text als Pflicht definiert ist
        /// </summary>
        /// <param name="ArtikelID">Artikelnummer</param>
        /// <returns>True wenn Pflicht sonst False</returns>
        internal bool CheckLangtextNeeded(string ArtikelID)
        {
            bool bNeed = false;

            DataRow[] rows = tblArtikel.Select("ARTLIF = '" + ArtikelID + "'");

            if (rows.GetLength(0) > 0)
            {
                if (rows[0]["TEXTPFLICHT"].ToString().Trim() == "X")
                { bNeed = true; }
            }

            return bNeed;
        }

        /// <summary>
        /// Fügt der tblKennzeichen Tabelle einen neuen Datensatz hinzu
        /// </summary>
        /// <param name="Datum">Datum</param>
        /// <param name="ArtikelID">Materialnummer</param>
        /// <param name="Bezeichnung">Artikelbezeichnung</param>
        /// <param name="Menge">Menge in Stück</param>
        /// <param name="LieferantenNr">Lieferantennummer</param>
        /// <param name="Lieferscheinnummer">Lieferscheinnummer</param>
        /// <returns>die neu hinzugefügte Zeile</returns>
        internal DataRow AddKennzeichen(string Datum, string ArtikelID, string Bezeichnung, int Menge, string LieferantenNr, string Lieferscheinnummer)
        {
            DataRow NewRow = tblKennzeichen.NewRow();

            NewRow["Datum"] = Datum;
            NewRow["LieferantID"] = LieferantenNr;
            NewRow["ArtikelID"] = ArtikelID;
            NewRow["Artikel"] = Bezeichnung;
            NewRow["Lieferscheinnummer"] = Lieferscheinnummer;
            NewRow["Menge"] = Menge;
           
            tblKennzeichen.Rows.Add(NewRow);

            return NewRow;
        }

        /// <summary>
        /// Fügt der tblKennzeichen Tabelle einen neuen Datensatz hinzu
        /// </summary>
        /// <param name="Datum">Datum</param>
        /// <param name="ArtikelID">Materialnummer</param>
        /// <param name="Bezeichnung">Artikelbezeichnung</param>
        /// <param name="Menge">Menge in Stück</param>
        /// <param name="LieferantenNr">Lieferantennummer</param>
        /// <param name="Lieferscheinnummer">Lieferscheinnummer</param>
        /// <param name="Preis">Preis</param>
        /// <returns>die neu hinzugefügte Zeile</returns>
        internal DataRow AddKennzeichen(string Datum, string ArtikelID, string Bezeichnung, int Menge, string LieferantenNr, string Lieferscheinnummer, double Preis)
        {
            DataRow NewRow = tblKennzeichen.NewRow();

            NewRow["Datum"] = Datum;
            NewRow["LieferantID"] = LieferantenNr;
            NewRow["ArtikelID"] = ArtikelID;
            NewRow["Artikel"] = Bezeichnung;
            NewRow["Lieferscheinnummer"] = Lieferscheinnummer;
            NewRow["Menge"] = Menge;
            if (Preis == 0) {
                NewRow["Preis"] = "";
            }
            else {
                NewRow["Preis"] = Preis.ToString();
            }
            
            tblKennzeichen.Rows.Add(NewRow);

            return NewRow;
        }

        /// <summary>
        /// Fügt der tblKennzeichen Tabelle einen neuen Datensatz hinzu
        /// </summary>
        /// <param name="Datum">Datum</param>
        /// <param name="ArtikelID">Materialnummer</param>
        /// <param name="Bezeichnung">Artikelbezeichnung</param>
        /// <param name="Menge">Menge in Stück</param>
        /// <param name="LieferantenNr">Lieferantennummer</param>
        /// <param name="Lieferscheinnummer">Lieferscheinnummer</param>
        /// <param name="Langtext">Langtext</param>
        /// <param name="LangtextID"></param>
        /// <returns>die neu hinzugefügte Zeile</returns>
        internal DataRow AddKennzeichen(string Datum, string ArtikelID, string Bezeichnung, int Menge, string LieferantenNr, string Lieferscheinnummer, string Langtext, string LangtextID)
        {
            DataRow NewRow = tblKennzeichen.NewRow();

            NewRow["Datum"] = Datum;
            NewRow["LieferantID"] = LieferantenNr;
            NewRow["ArtikelID"] = ArtikelID;
            NewRow["Artikel"] = Bezeichnung;
            NewRow["Lieferscheinnummer"] = Lieferscheinnummer;
            NewRow["Menge"] = Menge;
            NewRow["Langtext"] = Langtext;
            NewRow["LangtextID"] = LangtextID;

            tblKennzeichen.Rows.Add(NewRow);

            return NewRow;
        }

        /// <summary>
        /// Fügt der tblKennzeichen Tabelle einen neuen Datensatz hinzu
        /// </summary>
        /// <param name="Datum">Datum</param>
        /// <param name="ArtikelID">Materialnummer</param>
        /// <param name="Bezeichnung">Artikelbezeichnung</param>
        /// <param name="Menge">Menge in Stück</param>
        /// <param name="LieferantenNr">Lieferantennummer</param>
        /// <param name="Lieferscheinnummer">Lieferscheinnummer</param>
        /// <param name="Preis">Preis</param>
        /// <param name="Langtext">Langtext</param>
        /// <param name="LangtextID"></param>
        /// <returns>die neu hinzugefügte Zeile</returns>
        internal DataRow AddKennzeichen(string Datum, string ArtikelID, string Bezeichnung, int Menge, string LieferantenNr, string Lieferscheinnummer, double Preis, string Langtext, string LangtextID)
        {
            DataRow NewRow = tblKennzeichen.NewRow();

            NewRow["Datum"] = Datum;
            NewRow["LieferantID"] = LieferantenNr;
            NewRow["ArtikelID"] = ArtikelID;
            NewRow["Artikel"] = Bezeichnung;
            NewRow["Lieferscheinnummer"] = Lieferscheinnummer;
            NewRow["Menge"] = Menge;
            if (Preis == 0)
            {
                NewRow["Preis"] = "";
            }
            else
            {
                NewRow["Preis"] = MakeMoneyString(Preis);
            }
            NewRow["Langtext"] = Langtext;
            NewRow["LangtextID"] = LangtextID;

            tblKennzeichen.Rows.Add(NewRow);

            return NewRow;
        }

        private string MakeMoneyString(double dZahl)
        {
            string sTemp;
            sTemp = dZahl.ToString();
            string[] sTemp2 = sTemp.Split(',');

            if (sTemp2.Length == 2)
            {
                if (sTemp2[1].Length == 1)
                {
                    sTemp = sTemp2[0] + "," + sTemp2[1] + "0€";
                } 
                else if (sTemp2[1].Length == 2)
                {
                    sTemp = sTemp2[0] + "," + sTemp2[1] + "€";
                }
                else { sTemp = sTemp2[0] + "," + sTemp2[1].Substring(0, 2) + "€"; }
            }
            else { sTemp += ",00€"; }

            return sTemp;
        }

        static string DateToDDMMYYYY(string sDate)
        {
            if (string.IsNullOrEmpty(sDate)) 
                return "";

            const string thisCentury = "20";
            sDate = sDate.Replace(".19", "." + thisCentury);
            if (sDate.Length == 8)
                sDate = sDate.Substring(0, 6) + thisCentury + sDate.Substring(6, 2);

            return sDate;
        }

        internal void SendToSap()
        {
            m_strClassAndMethod = "NacherfassungGekaufteKennzeichen.SendToSap";
            m_intStatus = 0;
            m_strMessage = String.Empty;

            List<AuftragObj> lstNacherf = CreateNacherfassungen();
            LongStringToSap LSTS = new LongStringToSap(m_objUser, m_objApp, objPage);

            foreach (AuftragObj item in lstNacherf)
            {
                DataRow[] Rows = tblKennzeichen.Select("LieferantID='" + item.Lieferant + "' AND Lieferscheinnummer='" + item.Lieferscheinnummer + "' AND Datum='" + item.Datum.ToString() + "'");

                if (m_blnGestartet == false)
                {
                    m_blnGestartet = true;
                    try
                    {
                        DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_FIL_EFA_PO_CREATE", ref m_objApp, ref m_objUser, ref objPage);

                        myProxy.setImportParameter("I_KOSTL", VKBUR.PadLeft(10, '0'));
                        myProxy.setImportParameter("I_LIFNR", item.Lieferant);
                        myProxy.setImportParameter("I_VERKAEUFER", "");
                        myProxy.setImportParameter("I_LIEF_KZ", "X");
                        myProxy.setImportParameter("I_LIEF_NR", item.Lieferscheinnummer);
                        myProxy.setImportParameter("I_EEIND", DateToDDMMYYYY(item.Datum));
                        myProxy.setImportParameter("I_BEDAT", DateToDDMMYYYY(item.Datum));

                        DataTable dtImp = myProxy.getImportTable("GT_POS");

                        foreach (DataRow row in Rows)
                        {
                            if (row["LangtextID"] is DBNull || row["LangtextID"].ToString() == "")
                            {
                                if (row["Langtext"].ToString() != "")
                                {
                                    row["LangtextID"] = LSTS.InsertString(row["Langtext"].ToString(), m_objUser.UserName);
                                }
                            }
                            else
                            {
                                if (row["Langtext"].ToString() != "")
                                {
                                    LSTS.UpdateString(row["Langtext"].ToString(), row["LangtextID"].ToString(), m_objUser.UserName);
                                }
                                else 
                                { 
                                    LSTS.DeleteString(row["LangtextID"].ToString());
                                    row["LangtextID"] = "";
                                }
                            }

                            DataRow newrow = dtImp.NewRow();

                            newrow["ARTLIF"] = row["ArtikelID"];
                            newrow["MENGE"] = row["Menge"];
                            newrow["ZUSINFO_TXT"] = "";
                            newrow["PREIS"] = row["Preis"];
                            newrow["LTEXT_NR"] = row["LangtextID"];

                            dtImp.Rows.Add(newrow);
                        }

                        myProxy.callBapi();

                        Int32 subrc;
                        Int32.TryParse(myProxy.getExportParameter("E_SUBRC").ToString(), out subrc);
                        String sapMessage;
                        sapMessage = myProxy.getExportParameter("E_MESSAGE").ToString();

                        switch (subrc)
                        {
                            case 0:
                                foreach (DataRow row in Rows)
                                {
                                    row.Delete();
                                    tblKennzeichen.AcceptChanges();
                                }
                                break;
                            //case 104:
                                //m_strMessage = "KST nicht zulässig! Bitte richtige KST eingeben.";                               
                                //break;
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
        }

        /// <summary>
        /// Erzeugt eine Liste der Aufträge der Nacherfassung die nach SAP übertragen werden sollen.
        /// </summary>
        /// <returns>Auftragsliste</returns>
        private List<AuftragObj> CreateNacherfassungen()
        {
            List<string> strLieferanten = new List<string>();
            
            foreach(DataRow row in tblKennzeichen.Rows)
            {
                if(!strLieferanten.Contains(row["LieferantID"].ToString()))
                {
                    strLieferanten.Add(row["LieferantID"].ToString());
                }
            }

            List<LieferscheinnummernObj> strLieferscheinnummern = new List<LieferscheinnummernObj>();

            foreach (string item in strLieferanten)
            {
                DataRow[] rows = tblKennzeichen.Select("LieferantID='"+ item +"'");
                foreach(DataRow row in rows)
                {
                    LieferscheinnummernObj LiefObj = new LieferscheinnummernObj(item, row["Lieferscheinnummer"].ToString()) ;
                    bool bAdd = true;
                    foreach(LieferscheinnummernObj finditem in strLieferscheinnummern)
                    {
                        if(LiefObj.CompareTo(finditem)==1)
                        {
                            bAdd = false;
                            break;
                        }                        
                    }
                    if (bAdd)
                    {
                        strLieferscheinnummern.Add(LiefObj);
                    }       
                }
            }

            List<AuftragObj> strAufträge = new List<AuftragObj>();

            foreach (LieferscheinnummernObj item in strLieferscheinnummern)
            {
                DataRow[] rows = tblKennzeichen.Select("Lieferscheinnummer='" + item.Lieferscheinnummer + "'"); //"LieferantID='"+ item.Lieferant +"' AND
                foreach(DataRow row in rows)
                {
                    AuftragObj AufObj = new AuftragObj(item.Lieferant, item.Lieferscheinnummer, row["Datum"].ToString());
                                        
                    bool bAdd = true;
                    foreach(AuftragObj finditem in strAufträge)
                    {
                        if(AufObj.CompareTo(finditem)==1)
                        {    
                            bAdd = false;
                            break;
                        }                        
                    }
                    if (bAdd)
                    {      
                        strAufträge.Add(AufObj);
                    }
                }
            }            
            
            return strAufträge;
        }           
    }

    internal struct LieferscheinnummernObj : IComparable<LieferscheinnummernObj>
    {
        public string Lieferscheinnummer;
        public string Lieferant;
        
        public LieferscheinnummernObj(string strLieferant, string strLieferscheinnummer)
        {
            Lieferant = strLieferant;
            Lieferscheinnummer = strLieferscheinnummer;
        }

        public int CompareTo(LieferscheinnummernObj obj)
        { 
            if(obj.Lieferant == this.Lieferant && obj.Lieferscheinnummer == this.Lieferscheinnummer)
            {
                return 1;
            }

            return -1;
        }
    }

    internal struct AuftragObj : IComparable<AuftragObj>
    {
        public string Lieferscheinnummer;
        public string Lieferant;
        public string Datum;

        public AuftragObj(string strLieferant, string strLieferscheinnummer, string strDatum)
        {
            Lieferant = strLieferant;
            Lieferscheinnummer = strLieferscheinnummer;
            Datum = strDatum;
        }

        public int CompareTo(AuftragObj obj)
        {
            if (obj.Lieferant == this.Lieferant && obj.Lieferscheinnummer == this.Lieferscheinnummer && obj.Datum == this.Datum)
            {
                return 1;
            }

            return -1;
        }
    }
}