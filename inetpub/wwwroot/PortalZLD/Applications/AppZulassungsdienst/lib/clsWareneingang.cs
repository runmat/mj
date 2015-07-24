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

        public bool IstUmlagerung { get; set; }

        #endregion

        #region "Methods"

        public clsWareneingang(string userReferenz)
		{
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz); 
		}

        public void getBestPositionenFromSAP()
        {
            DataTable tblTemp = new DataTable();

            ExecuteSapZugriff(() =>
            {
                Z_FIL_READ_OFF_BEST_POS_001.Init(SAP, "I_EBELN", BELNR);

                CallBapi();

                tblTemp = SAP.GetExportTable("GT_WEB");
            });

            InitTablePositionen();

            foreach (DataRow tmpRow in tblTemp.Rows)
            {
                DataRow rowNew = Bestellpositionen.NewRow();
                rowNew["Bestellposition"] = tmpRow["EBELP"].ToString().NotNullOrEmpty().Replace(".", "").ToInt(0);
                rowNew["Materialnummer"] = tmpRow["MATNR"].ToString();
                rowNew["Artikelbezeichnung"] = tmpRow["TXZ01"].ToString();
                rowNew["MaterialnummerLieferant"] = tmpRow["IDNLF"].ToString();
                rowNew["BestellteMenge"] = tmpRow["BSTMG"].ToString().NotNullOrEmpty().Replace(".", "").ToInt(0);
                rowNew["Mengeneinheit"] = tmpRow["MEINS"].ToString();
                rowNew["EAN"] = tmpRow["EAN11"].ToString();
                rowNew["PositionLieferMenge"] = DBNull.Value;
                rowNew["PositionAbgeschlossen"] = "";
                rowNew["PositionVollstaendig"] = "";
                rowNew["TextNr"] = "";
                rowNew["LangText"] = "";
                rowNew["KennzForm"] = "";
                Bestellpositionen.Rows.Add(rowNew);
            }
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

            InitTablePositionen();

            foreach (DataRow tmpRow in tblTemp.Rows)
            {
                DataRow rowNew = Bestellpositionen.NewRow();
                rowNew["Bestellposition"] = tmpRow["POSNR"].ToString().NotNullOrEmpty().Replace(".", "").ToInt(0);
                rowNew["Materialnummer"] = tmpRow["MATNR"].ToString();
                rowNew["Artikelbezeichnung"] = tmpRow["MAKTX"].ToString();
                rowNew["MaterialnummerLieferant"] = tmpRow["MATNR"].ToString();
                rowNew["BestellteMenge"] = tmpRow["MENGE"].ToString().NotNullOrEmpty().Replace(".", "").ToInt(0);
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

                    DataTable tblBestellungen = SAP.GetExportTable("GT_WEB");
                    DataTable tblUmlagerungen = SAP.GetExportTable("GT_OFF_UML");

                    InitTableLieferungen();

                    foreach (DataRow bestRow in tblBestellungen.Rows)
                    {
                        var beDat = bestRow["BEDAT"].ToString().ToNullableDateTime();

                        var rowNew = ErwarteteLieferungen.NewRow();
                        rowNew["Bestellnummer"] = bestRow["EBELN"].ToString();
                        rowNew["LieferantName"] = bestRow["NAME1"].ToString() + " " + bestRow["NAME2"].ToString();
                        rowNew["AnzeigeText"] = bestRow["EBELN"].ToString() + ", " + bestRow["NAME1"].ToString() + " " + bestRow["NAME2"].ToString() + " " + bestRow["ORT01"].ToString() + (beDat.HasValue ? ", " + beDat.Value.ToShortDateString() : "");
                        rowNew["IstUmlagerung"] = "";
                        ErwarteteLieferungen.Rows.Add(rowNew);
                    }

                    foreach (DataRow umlRow in tblUmlagerungen.Rows)
                    {
                        var buDat = umlRow["BUDAT"].ToString().ToNullableDateTime();

                        var rowNew = ErwarteteLieferungen.NewRow();
                        rowNew["Bestellnummer"] = umlRow["BELNR"].ToString();
                        rowNew["LieferantName"] = umlRow["KTEXT"].ToString();
                        rowNew["AnzeigeText"] = umlRow["BELNR"].ToString() + ", " + umlRow["KTEXT"].ToString() + (buDat.HasValue ? ", " + buDat.Value.ToShortDateString() : "");
                        rowNew["IstUmlagerung"] = "X";
                        ErwarteteLieferungen.Rows.Add(rowNew);
                    }
                });
        }

        public void sendOrderCheckToSAP(string Lieferscheinnummer, string Belegdatum)
        {
            ExecuteSapZugriff(() =>
            {
                Z_FIL_WE_ZUR_BEST_POS_001.Init(SAP);

                SAP.SetImportParameter("I_LGORT", VKBUR);
                SAP.SetImportParameter("I_EBELN", BELNR);
                SAP.SetImportParameter("I_LFSNR", Lieferscheinnummer);
                SAP.SetImportParameter("I_BLDAT", Belegdatum);

                DataTable tblSap = SAP.GetImportTable("GT_WEB");

                Boolean skip = false;
                foreach (DataRow tmprow in Bestellpositionen.Rows)
                {
                    DataRow tmpSAPRow = tblSap.NewRow();

                    tmpSAPRow["EBELP"] = tmprow["Bestellposition"].ToString();
                    tmpSAPRow["MATNR"] = tmprow["Materialnummer"].ToString();
                    tmpSAPRow["ERFME"] = tmprow["Mengeneinheit"].ToString();
                    tmpSAPRow["EAN11"] = tmprow["EAN"].ToString();
                    if (tmprow["PositionVollstaendig"].ToString() == "X")
                    {
                        var liefMenge = tmprow["PositionLieferMenge"].ToString().NotNullOrEmpty().Replace(".", "").ToInt(0);
                        if (liefMenge > 0)
                        {
                            tmpSAPRow["ERFMG"] = tmprow["PositionLieferMenge"].ToString();
                        }
                        else
                        {
                            tmpSAPRow["ERFMG"] = tmprow["BestellteMenge"].ToString();
                        }

                        tmpSAPRow["ELIKZ"] = "X";
                    }
                    else //wenn lieferung nicht vollständig, dann lieferpositionsmenge / LieferungsAbschluss eintragen
                    {
                        if (tmprow["PositionAbgeschlossen"].ToString() == "J")
                        {
                            tmpSAPRow["ERFMG"] = tmprow["PositionLieferMenge"].ToString();
                            tmpSAPRow["ELIKZ"] = "X";
                        }
                        else
                        {
                            tmpSAPRow["ELIKZ"] = "";
                            var liefMenge = tmprow["PositionLieferMenge"].ToString().NotNullOrEmpty().Replace(".", "").ToInt(0);
                            if (liefMenge > 0)
                            {
                                tmpSAPRow["ERFMG"] = tmprow["PositionLieferMenge"].ToString();
                            }
                            else
                            {
                                skip = true;
                            }
                        }
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
                            var liefMenge = tmprow["PositionLieferMenge"].ToString().NotNullOrEmpty().Replace(".", "").ToInt(0);
                            if (liefMenge > 0)
                            {
                                tmpSAPRow["MENGE"] = tmprow["PositionLieferMenge"].ToString();
                            }
                            else
                            {
                                skip = true;
                            }
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

        private void InitTableLieferungen()
        {
            ErwarteteLieferungen = new DataTable();
            ErwarteteLieferungen.Columns.Add("Bestellnummer", typeof(String));
            ErwarteteLieferungen.Columns.Add("LieferantName", typeof(String));
            ErwarteteLieferungen.Columns.Add("AnzeigeText", typeof(String));
            ErwarteteLieferungen.Columns.Add("IstUmlagerung", typeof(String));
        }

        private void InitTablePositionen()
        {
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
        }

        #endregion
    }
}
