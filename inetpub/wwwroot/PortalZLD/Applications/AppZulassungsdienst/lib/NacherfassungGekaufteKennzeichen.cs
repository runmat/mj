using System;
using System.Data;
using CKG.Base.Business;
using System.Collections.Generic;
using SapORM.Contracts;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    public class NacherfassungGekaufteKennzeichen : SapOrmBusinessBase
    {
        private bool m_isZldUser;

        #region "Properties"

        public DataTable tblKennzeichen { get; private set; }
        public DataTable tblArtikel { get; private set; }
        public DataTable tblLieferanten { get; private set; }
        public DataTable tblErfassteKennzeichenKopf { get; private set; }
        public DataTable tblErfassteKennzeichenPositionen { get; private set; }

        #endregion

        public NacherfassungGekaufteKennzeichen(string userReferenz, bool isZldUser)
        {
            m_isZldUser = isZldUser;

            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

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

        private void GetLieferanten()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_FIL_EFA_PLATSTAMM.Init(SAP, "I_KOSTL", VKBUR.PadLeft0(10));

                    if (m_isZldUser)
                        SAP.SetImportParameter("I_ZLD", "X");
                    else
                        SAP.SetImportParameter("I_FIL", "X");

                    CallBapi();

                    switch (SAP.ResultCode)
                    {
                        case 104:
                            RaiseError(SAP.ResultCode, "KST nicht zulässig! Bitte richtige KST eingeben.");
                            break;
                    }

                    tblLieferanten = SAP.GetExportTable("GT_PLATSTAMM");
                });
        }

        internal void GetArtikel(string LieferantenID)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_FIL_EFA_PLATARTIKEL.Init(SAP);

                    SAP.SetImportParameter("I_KOSTL", VKBUR.PadLeft0(10));
                    SAP.SetImportParameter("I_LIFNR", LieferantenID);
                    SAP.SetImportParameter("I_ZLD", "X");

                    CallBapi();

                    switch (SAP.ResultCode)
                    {
                        case 104:
                            RaiseError(SAP.ResultCode, "KST nicht zulässig! Bitte richtige KST eingeben.");
                            break;
                    }

                    tblArtikel = SAP.GetExportTable("GT_PLATART");
                });
        }

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

        internal DataRow AddKennzeichen(string Datum, string ArtikelID, string Bezeichnung, int Menge, string LieferantenNr, string Lieferscheinnummer, double Preis = 0.0, string Langtext = "", string LangtextID = "")
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

            if (Equals(Preis, 0.0))
                NewRow["Preis"] = "";
            else
                NewRow["Preis"] = Preis.ToString();
            
            tblKennzeichen.Rows.Add(NewRow);

            return NewRow;
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

        internal void SendToSap(string userName)
        {
            List<AuftragObj> lstNacherf = CreateNacherfassungen();
            LongStringToSap LSTS = new LongStringToSap();

            foreach (var li in lstNacherf)
            {
                var item = li;

                DataRow[] Rows = tblKennzeichen.Select("LieferantID='" + item.Lieferant + "' AND Lieferscheinnummer='" + item.Lieferscheinnummer + "' AND Datum='" + item.Datum.ToString() + "'");

                foreach (DataRow row in Rows)
                {
                    if (row["LangtextID"] is DBNull || row["LangtextID"].ToString() == "")
                    {
                        if (row["Langtext"].ToString() != "")
                        {
                            row["LangtextID"] = LSTS.InsertString(row["Langtext"].ToString(), userName);
                        }
                    }
                    else
                    {
                        if (row["Langtext"].ToString() != "")
                        {
                            LSTS.UpdateString(row["Langtext"].ToString(), row["LangtextID"].ToString(), userName);
                        }
                        else
                        {
                            LSTS.DeleteString(row["LangtextID"].ToString());
                            row["LangtextID"] = "";
                        }
                    }
                }

                ExecuteSapZugriff(() =>
                    {
                        Z_FIL_EFA_PO_CREATE.Init(SAP);

                        SAP.SetImportParameter("I_KOSTL", VKBUR.PadLeft0(10));
                        SAP.SetImportParameter("I_LIFNR", item.Lieferant);
                        SAP.SetImportParameter("I_VERKAEUFER", "");
                        SAP.SetImportParameter("I_LIEF_KZ", "X");
                        SAP.SetImportParameter("I_LIEF_NR", item.Lieferscheinnummer);
                        SAP.SetImportParameter("I_EEIND", DateToDDMMYYYY(item.Datum));
                        SAP.SetImportParameter("I_BEDAT", DateToDDMMYYYY(item.Datum));

                        DataTable dtImp = SAP.GetImportTable("GT_POS");

                        foreach (DataRow row in Rows)
                        {
                            DataRow newrow = dtImp.NewRow();

                            newrow["ARTLIF"] = row["ArtikelID"];
                            newrow["MENGE"] = row["Menge"];
                            newrow["ZUSINFO_TXT"] = "";
                            newrow["PREIS"] = row["Preis"];
                            newrow["LTEXT_NR"] = row["LangtextID"];

                            dtImp.Rows.Add(newrow);
                        }

                        CallBapi();

                        switch (SAP.ResultCode)
                        {
                            case 0:
                                foreach (DataRow row in Rows)
                                {
                                    row.Delete();
                                    tblKennzeichen.AcceptChanges();
                                }
                                break;
                        }
                    });
            }
        }

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
                DataRow[] rows = tblKennzeichen.Select("Lieferscheinnummer='" + item.Lieferscheinnummer + "'");
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

        internal void GetKaeufe(string LieferantenID)
        {
            ExecuteSapZugriff(() =>
            {
                Z_FIL_EFA_GEPRAEGTE_KENNZ_LIST.Init(SAP);

                SAP.SetImportParameter("I_KOSTL", VKBUR.PadLeft0(10));
                SAP.SetImportParameter("I_LIFNR", LieferantenID);

                CallBapi();

                switch (SAP.ResultCode)
                {
                    case 104:
                        RaiseError(SAP.ResultCode, "KST nicht zulässig! Bitte richtige KST eingeben.");
                        break;
                }

                tblErfassteKennzeichenKopf = SAP.GetExportTable("GT_PO_K");
                tblErfassteKennzeichenPositionen = SAP.GetExportTable("GT_PO_P");

                tblErfassteKennzeichenPositionen.Columns.Add("BEDAT", typeof(DateTime));
                tblErfassteKennzeichenPositionen.Columns.Add("LTEXT", typeof(string));

                var LSTS = new LongStringToSap();

                foreach (DataRow kopfzeile in tblErfassteKennzeichenKopf.Rows)
                {
                    var poszeilen = tblErfassteKennzeichenPositionen.Select("BSTNR = '" + kopfzeile["BSTNR"].ToString() + "'");
                    foreach (var poszeile in poszeilen)
                    {
                        poszeile["BEDAT"] = kopfzeile["BEDAT"];
                        poszeile["LTEXT"] = LSTS.ReadString(poszeile["LTEXT_NR"].ToString());
                    }
                }
            });
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