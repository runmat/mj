using System;
using System.Data;

namespace TimeRegistration
{
    public class TimeOverview
    {
        private string _sVon;
        private string _sBis;
        private bool _bOffen;
        private DataTable _dtKopf;
        private DataTable _dtPos;
        private DataTable _dtPosIn;
        private DataTable _dtWeb;
        
        private readonly string _strRuestÖffnung = TimeRegistrator.TranslateRuestzeiten(TimeRegistrator.TimeRuest.Kommen);
        private readonly string _strRuestAbrechnung = TimeRegistrator.TranslateRuestzeiten(TimeRegistrator.TimeRuest.Abrechnung);
        private readonly string _strRuestEinzahlung = TimeRegistrator.TranslateRuestzeiten(TimeRegistrator.TimeRuest.Einzahlung);
        private readonly string _strRuestAbrechnungEinzahlung = TimeRegistrator.TranslateRuestzeiten(TimeRegistrator.TimeRuest.Abrechnung_Einzahlung);
        //private string strÖffnungszeit = TimeRegistrator.TranslateRuestzeiten(TimeRegistrator.TimeRuest.Öffnungszeit);
        //private string strÖffnungszeitOhneRuest = TimeRegistrator.TranslateRuestzeiten(TimeRegistrator.TimeRuest.ÖffnungszeitOhneRüstzeit);

        private DataRow _rLastRow;


        #region Properties
        
        public string Von
        {
            get{return _sVon;}
        }

        public string Bis
        {
            get { return _sBis; }
        }

        public bool OffenePositionen
        {
            get { return _bOffen; }
        }

        public DataTable Kopftabelle
        {
            get {return _dtKopf ;}
        }

        public DataTable Positionstabelle
        {
            get { return _dtPos; }
        }

        //public TimeRegistrator.TimeAction LastAction
        //{
        //    get { return m_LastAction; }
        //}

        #endregion

        #region Constructors

        /// <summary>
        /// Erzeugt ein neues TimeOverview Objekt
        /// </summary>
        /// <param name="kopftabelle"></param>
        /// <param name="positionstabelle"></param>
        /// <param name="bOffen"></param>
        public TimeOverview(DataTable kopftabelle,DataTable positionstabelle)
        {
           initTimeOverview(kopftabelle, positionstabelle);
        }

        /// <summary>
        /// Erzeugt ein neues TimeOverview Objekt
        /// </summary>
        /// <param name="kopftabelle"></param>
        /// <param name="positionstabelle"></param>
        /// <param name="vdate">Von</param>
        /// <param name="bdate">Bis</param>
        /// <param name="bOffen"></param>
        public TimeOverview(DataTable kopftabelle, DataTable positionstabelle,string vdate, string bdate)
        {
            _sVon = vdate;
            _sBis = bdate;
            initTimeOverview(kopftabelle, positionstabelle);
        }

        #endregion


        #region "Öffentliche Funktionen"

        /// <summary>
        /// Liefert alle Daten als Webtabelle
        /// </summary>
        /// <returns></returns>
        public DataTable getWebTabelleKomplett()
        {
            return _dtWeb;
        }

        /// <summary>
        /// Liefert alle Spalten zur Bedienernummer als Webtabelle
        /// </summary>
        /// <param name="bedienernummer"></param>
        /// <returns></returns>
        public DataTable getWebTabelleForBediener(string bedienernummer)
        {
            var dtTemp = _dtWeb.Copy();
            var rowa = dtTemp.Select("BD_NR='"+bedienernummer+"'");
            var dtOut = _dtWeb.Clone();
            
            foreach (DataRow row in rowa)
            {
                dtOut.ImportRow(row);               
            }
            dtOut.AcceptChanges();

            return dtOut;
        }

        /// <summary>
        /// Liefert die DataRow der zuletzt ausgeführten Aktion zu einer Bedienernummer für den aktuellen Tag
        /// </summary>
        /// <param name="bedienernummer">Nummer für die gesucht werden soll</param>
        /// <returns>Liefert null falls noch kein Eintrag vorliegt</returns>
        public DataRow getLastActionRow(string bedienernummer)
        {
            DataRow[] rows = _dtPosIn.Select("BD_NR='" + bedienernummer + "' AND BUDATE='" + SAPExecutor.SAPExecutor.MakeSAPDate(DateTime.Today) + "'");
            
            if (rows.Length == 0)
            {
                return null;
            }

            // sortieren
            int iLatest = 0;
            string sLatest = rows[0]["BUZEIT"].ToString();
            for(int i = 1;i < rows.Length;i++)
            {
                if (String.CompareOrdinal(sLatest, rows[i]["BUZEIT"].ToString()) < 0)
                {
                    sLatest = rows[i]["BUZEIT"].ToString();
                    iLatest = i;
                }                    
            }

            _rLastRow = rows[iLatest];
            return _rLastRow;
        }

        /// <summary>
        /// Liefert die zuletzt ausgeführte Aktion zu einer Bedienernummer für den aktuellen Tag
        /// </summary>
        /// <param name="bedienernummer">Nummer für die gesucht werden soll</param>
        /// <returns>Liefert NoAction falls noch keine Einträge vorliegen</returns>
        public TimeRegistrator.TimeAction getLastAction(string bedienernummer)
        {
            getLastActionRow(bedienernummer);

            if (_rLastRow == null) 
            {
                return TimeRegistrator.TimeAction.NoAction; 
            }

            return TimeRegistrator.TranslateAction(_rLastRow["SATZART"].ToString());
        }

        #endregion

        #region "Interne Funktionen"

        /// <summary>
        /// Initialisiert die Grundwerte der TimeOverview
        /// </summary>
        /// <param name="kopftabelle"></param>
        /// <param name="positionstabelle"></param>
        private void initTimeOverview(DataTable kopftabelle, DataTable positionstabelle)
        {
            _dtKopf = kopftabelle.Copy();
            _dtPos = positionstabelle.Copy();
            _dtPosIn = positionstabelle;

            //// # Positionsnummern für Sortierung aufarbeiten
            //m_dtPos.Columns.Add("Position");
            //foreach (DataRow row in m_dtPos.Rows)
            //{
            //    row["Position"] = int.Parse(row["POSNR"].ToString());
            //}
            //m_dtPos.AcceptChanges();
            //// #

            // Web-Tabellenstruktur anlegen
            _dtWeb = new DataTable();
            _dtWeb.Columns.Add("BD_NR", Type.GetType("System.String"));
            _dtWeb.Columns.Add("Tag", Type.GetType("System.DateTime"));
            _dtWeb.Columns.Add("Kommen", Type.GetType("System.String"));
            _dtWeb.Columns.Add("Gehen", Type.GetType("System.String"));
            _dtWeb.Columns.Add("Arbeitszeit", Type.GetType("System.String"));
            _dtWeb.Columns.Add("RÖffnung", Type.GetType("System.Boolean"));
            _dtWeb.Columns.Add("RAbrechnung", Type.GetType("System.Boolean"));
            _dtWeb.Columns.Add("REinzahlung", Type.GetType("System.Boolean"));

            getWebTabelle();
        }

        /// <summary>
        /// Erstellt die Web-Tabelle aus der SAP-Kopf- und SAP-Positionstabelle
        /// </summary>
        /// <returns>Webtabelle als DataTable</returns>
        private DataTable getWebTabelle()
        {
            foreach (DataRow row in _dtKopf.Rows)
            {
                string strBedienNr = row["BD_NR"].ToString();
                string strBuDat = row["BUDATE"].ToString();

                getPositionenZuKopf(strBuDat, strBedienNr);
            }

            return _dtWeb;
        }

        /// <summary>
        /// liefert alle Positionen zum Buchungsdatum eines Kopfdatensatzes
        /// </summary>
        /// <param name="kopfsatzDatum">BUDAT-Feld des Kopfdatensatzes</param>
        /// <param name="bedienernummer"></param>
        /// <returns>DataRow-Array mit den gefilterten Positionen</returns>
        private void getPositionenZuKopf(string kopfsatzDatum,string bedienernummer)
        {
            var sapDat = kopfsatzDatum.Replace(".","");
            DataRow[] drPos = _dtPos.Select("BUDATE='" + sapDat + "' AND BD_NR='" + bedienernummer + "'"); //, "Position asc" || "BUZEIT asc"

            var strKommenKey = TimeRegistrator.TranslateAction(TimeRegistrator.TimeAction.Kommen);
            var strGehenKey = TimeRegistrator.TranslateAction(TimeRegistrator.TimeAction.Gehen);

            //var iKopfYear = int.Parse(kopfsatzDatum.Substring(0, 4));
            //var iKopfMonth = int.Parse(kopfsatzDatum.Substring(4, 2));
            //var iKopfDay = int.Parse(kopfsatzDatum.Substring(6, 2));

            var datBudat = SAPExecutor.SAPExecutor.MakeDateTime(kopfsatzDatum);

            for (int i=0;i<drPos.GetLength(0);i++)
            {
                // Rows bestimmen
                DataRow row = drPos[i];

                DataRow rowBefore;
                DataRow rowNext;
                if (i == 0)
                {
                    rowBefore = null;
                    if (drPos.GetLength(0) > 1)
                    {
                        rowNext = drPos[i + 1];
                    }
                    else { rowNext = null; }
                }
                else if (i == drPos.GetLength(0) - 1)
                {
                    rowBefore = drPos[i - 1];
                    rowNext = null;
                }
                else 
                {
                   rowBefore = drPos[i - 1];
                   rowNext = drPos[i + 1];
                }

                // Neue WebRow anlegen
                var webRow = _dtWeb.NewRow();
                webRow["BD_NR"] = bedienernummer;
                webRow["Tag"] = datBudat;

                // Wenn Row = Kommen
                if (row["SATZART"].ToString().Trim() == strKommenKey)
                {
                    // Gebuchtezeit lesen
                    webRow["Kommen"] = ParseTimeFromBuzeit(ref row);
                    // Rüstzeit prüfen
                    webRow["RÖffnung"] = CheckRüstzeitÖffnung(ref row);

                    // nächster Satz wieder Kommen
                    if (rowNext != null && rowNext["SATZART"].ToString().Trim() != strGehenKey)
                    {
                        // Gehen der aktuellen WebRow mit Dummy-Wert füllen
                        webRow["Gehen"] = "00:00";  //new DateTime(iKopfYear, iKopfMonth, iKopfDay, 0, 0, 0);
                        // Rüstzeiten füllen
                        webRow["RAbrechnung"] = false;
                        webRow["REinzahlung"] = false;
                    }
                    else // nächster Satz Gehen
                    {
                        webRow["Gehen"] = ParseTimeFromBuzeit(ref rowNext);
                        // Rüstzeiten prüfen
                        webRow["RAbrechnung"] = CheckRüstzeitAbrechnung(ref rowNext);
                        webRow["REinzahlung"] = CheckRüstzeitEinzahlung(ref rowNext);
                        //Arbeitszeit berechnen
                        //decimal Arbeitszeit = decimal.Parse(WebRow["Gehen"].ToString().Replace(':', ',')) - decimal.Parse(WebRow["Kommen"].ToString().Replace(':', ','));
                        TimeSpan tsArbeitszeit = DateTime.Parse(webRow["Gehen"].ToString()) - DateTime.Parse(webRow["Kommen"].ToString());
                        if (tsArbeitszeit.Hours < 0)
                        {
                            webRow["Arbeitszeit"] = "00:00";
                        }
                        else
                        {
                            string sStunden = tsArbeitszeit.Hours.ToString().PadLeft(2,'0');
                            string sMinuten = tsArbeitszeit.Minutes.ToString().PadLeft(2, '0');
                            webRow["Arbeitszeit"] = sStunden + ":" + sMinuten;//.ToString().Insert(Arbeitszeit.ToString().Length-2,",");
                        }
                        // nächste Row überspringen da bereits geparst
                        i++;
                    }
                }
                // Wenn Row = Gehen 
                else if (row["SATZART"].ToString().Trim() == strGehenKey) 
                {
                    // Gebuchtezeit lesen
                    webRow["Gehen"] = ParseTimeFromBuzeit(ref row);
                    // Rüstzeit prüfen
                    webRow["RAbrechnung"] = CheckRüstzeitAbrechnung(ref rowNext);
                    webRow["REinzahlung"] = CheckRüstzeitEinzahlung(ref rowNext);

                    // Letzter Satz auch Gehen oder Kein Satz vorhanden
                    if (rowBefore == null || rowBefore["SATZART"].ToString().Trim() != strKommenKey)
                    {
                        // Kommen der aktuellen WebRow mit Dummy-Wert füllen
                        webRow["Kommen"] = "00:00";  //new DateTime(iKopfYear, iKopfMonth, iKopfDay, 0, 0, 0);
                        // Rüstzeiten füllen
                        webRow["RÖffnung"] = false;
                    }                   
                }

                // komplette Row in WebTabelle hinzufügen
                _dtWeb.Rows.Add(webRow);
            }
            // Änderungen an der Web-Tabelle übernehmen
            _dtWeb.AcceptChanges();

            // Ausgelesene Rows aus der PositionsTabelle löschen
            foreach (DataRow delrow in drPos)
            {
                _dtPos.Rows.Remove(delrow);
            }
            _dtPos.AcceptChanges();           
        }

        /// <summary>
        /// Parst ein DateTime-Objekt aus der "BUZEIT"-Column der angegeben Row
        /// </summary>
        /// <param name="row">zu parsende Row</param>
        /// <returns>Das geparste DateTime-Objekt</returns>
        private static string ParseTimeFromBuzeit(ref DataRow row)
        {
            string dBuzeit = "00:00";
            if (row != null && row["BUZEIT"].ToString() != String.Empty)
            {
                dBuzeit = row["BUZEIT"].ToString().Remove(4);
                dBuzeit = dBuzeit.Insert(2, ":");                
            }
            return dBuzeit;
        }

        /// <summary>
        /// Prüft ob der Rüstzeitschlüssel der Row eine "Öffnung"-Rüstzeit enthält 
        /// </summary>
        /// <param name="row">zu prüfende Row</param>
        /// <returns>Liefert <b>True</b> Wenn eine "Öffnung"-Rüstzeit vorliegt</returns>
        private bool CheckRüstzeitÖffnung(ref DataRow row)
        {
            if(row != null)
            {
                string strRuestKey = row["RUEST"].ToString().Trim();

                if (strRuestKey == _strRuestÖffnung )//|| strRuestKey == strÖffnungszeit || strRuestKey == strÖffnungszeitOhneRuest
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Prüft ob der Rüstzeitschlüssel der Row eine "Abrechnung"-Rüstzeit enthält 
        /// </summary>
        /// <param name="row">zu prüfende Row</param>
        /// <returns>Liefert <b>True</b> Wenn eine "Abrechnung"-Rüstzeit vorliegt</returns>
        private bool CheckRüstzeitAbrechnung(ref DataRow row)
        {
            if (row != null)
            {
                string strRuestKey = row["RUEST"].ToString().Trim();

                if(strRuestKey == _strRuestAbrechnung || strRuestKey == _strRuestAbrechnungEinzahlung)
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Prüft ob der Rüstzeitschlüssel der Row eine "Einzahlung"-Rüstzeit enthält 
        /// </summary>
        /// <param name="row">zu prüfende Row</param>
        /// <returns>Liefert <b>True</b> Wenn eine "Einzahlung"-Rüstzeit vorliegt</returns>
        private bool CheckRüstzeitEinzahlung(ref DataRow row)
        {
            if(row != null)
            {
                string strRuestKey = row["RUEST"].ToString().Trim();

                if  (strRuestKey == _strRuestEinzahlung || strRuestKey == _strRuestAbrechnungEinzahlung)
                {
                    return true;
                }
            }
            
            return false;
        }
        
        //private object getFirstKommen(ref DataRow[] PosRows)
        //{
        //    throw new NotImplementedException();
        //}

#endregion
    }
}
