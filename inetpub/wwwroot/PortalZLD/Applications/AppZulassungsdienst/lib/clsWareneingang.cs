using System;
using System.Data;
using CKG.Base.Business;
using GeneralTools.Models;
using SapORM.Models;

namespace AppZulassungsdienst.lib
{
    /// <summary>
    /// Klasse für die Wareneingangsprüfung.
    /// </summary>
    public class clsWareneingang : SapOrmBusinessBase
    {
        #region "Properties"

        public DataTable Bestellpositionen { get; set; }
        public DataTable ErwarteteLieferungen { get; set; }

        public String BELNR { get; set; }
        public String BestellnummerSelection { get; set; }
        public String LieferantSelection { get; set; }
        public String Lieferant { get; set; }

        #endregion

        #region "Methods"

        public clsWareneingang(string userReferenz)
		{
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz); 
		}

        public void getUmlPositionenFromSAP()
        {
            DataTable tblTemp = new DataTable();

            ExecuteSapZugriff(() =>
                {
                    Z_FIL_EFA_UML_OFF_POS.Init(SAP, "I_BELNR", BELNR);

                    CallBapi();

                    tblTemp = SAP.GetExportTable("GT_OFF_UML_POS");
                });

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
                DataRow rowNew = Bestellpositionen.NewRow();
                Int32 iTemp = 0;

                if (tmpRow["POSNR"].ToString().IsNumeric())
                {
                    Int32.TryParse(tmpRow["POSNR"].ToString(), out iTemp);
                }
                rowNew["Bestellposition"] = iTemp;
                rowNew["Materialnummer"] = tmpRow["MATNR"].ToString();
                rowNew["Artikelbezeichnung"] = tmpRow["MAKTX"].ToString();
                rowNew["MaterialnummerLieferant"] = tmpRow["MATNR"].ToString();

                if (tmpRow["MENGE"].ToString().IsNumeric())
                {
                    Int32.TryParse(tmpRow["MENGE"].ToString(), out iTemp);
                }
                rowNew["BestellteMenge"] = iTemp;
                rowNew["Buchungsdatum"] = tmpRow["BUDAT"].ToString();
                rowNew["Mengeneinheit"] = "";
                rowNew["EAN"] = tmpRow["EAN11"].ToString();
                rowNew["PositionLieferMenge"] = DBNull.Value;
                rowNew["PositionAbgeschlossen"] = "";
                rowNew["PositionVollstaendig"] = "";
                rowNew["Freitext"] = tmpRow["TEXT"].ToString();
                rowNew["TextNr"] = tmpRow["LTEXT_NR"].ToString();
                LongStringToSap LSTS = new LongStringToSap();
                if (rowNew["TextNr"].ToString() != "")
                {
                    rowNew["LangText"] = LSTS.ReadString(rowNew["TextNr"].ToString());
                }
                rowNew["KennzForm"] = tmpRow["KennzForm"].ToString();
                Bestellpositionen.Rows.Add(rowNew);
            }
        }

        public void getErwarteteLieferungenFromSAP()
        {
            ExecuteSapZugriff(() =>
                {
                    Z_FIL_READ_OFF_BEST_001.Init(SAP, "I_LGORT", VKBUR);

                    CallBapi();

                    DataTable tblTemp = SAP.GetExportTable("GT_OFF_UML");

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
                        if (tmpRow["BUDAT"].ToString().IsDate())
                        {
                            DateTime.TryParse(tmpRow["BUDAT"].ToString(), out tmpDate);
                            rowNew["AnzeigeText"] = tmpRow["BELNR"].ToString() + " " + tmpRow["KTEXT"].ToString() + " " + tmpDate.ToShortDateString();
                        }
                        else
                        {
                            DateTime.TryParse(tmpRow["BUDAT"].ToString(), out tmpDate);
                            rowNew["AnzeigeText"] = tmpRow["BELNR"].ToString() + " " + tmpRow["BELNR"].ToString();

                        }
                        ErwarteteLieferungen.Rows.Add(rowNew);
                    }
                });
        }

        public void sendUmlToSAP(string Belegdatum)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_FIL_EFA_UML_STEP2.Init(SAP);

                    SAP.SetImportParameter("I_KOSTL", VKBUR);
                    SAP.SetImportParameter("I_BELNR", BELNR);
                    SAP.SetImportParameter("I_BUDAT", Belegdatum);

                    DataTable tblSap = SAP.GetImportTable("GT_OFF_UML_POS");

                    Boolean skip = false;
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
                        {
                            Int32 i = 0;
                            if (tmprow["PositionLieferMenge"].ToString().IsNumeric())
                            { Int32.TryParse(tmprow["PositionLieferMenge"].ToString(), out i); }
                            if (i > 0)
                            { tmpSAPRow["MENGE"] = tmprow["PositionLieferMenge"].ToString(); }
                            else { skip = true; }

                        }

                        if (!skip)
                        {
                            tblSap.Rows.Add(tmpSAPRow);
                        }

                        skip = false;
                    }

                    CallBapi();
                });
        }

        /// <summary>
        /// Reinitialisiert die Daten nach der Rückkehr aus der Detailansicht (ohne SAP-Neuladen)
        /// </summary>
        /// <param name="umlStatus">B = Abbruch, C = Abgeschlossen</param>
        public void ReInit(string umlStatus)
        {
            if (umlStatus == "C")
            {
                // Abgeschlossene Umlagerung aus Liste entfernen
                var tmpRows = ErwarteteLieferungen.Select("Bestellnummer='" + BELNR + "'");
                if (tmpRows.Length > 0)
                {
                    ErwarteteLieferungen.Rows.Remove(tmpRows[0]);
                }
            }

            // Selektion zurücksetzen
            Bestellpositionen = null;
            BELNR = null;
            Lieferant = null;
        }

        #endregion
    }
}
