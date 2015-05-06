using System;
using System.Data;
using System.Web.Configuration;
using KBSBase;

namespace TimeRegistration
{
    public class TimeRegistrator : ErrorHandlingClass
    {
        #region "global Declarations"

        public enum TimeAction { Kommen, Gehen, NoAction };
        public enum TimeRuest { Kommen, Abrechnung, Einzahlung, Abrechnung_Einzahlung, KeinSchlüssel }
        public enum TimePlus { Abrechnung, Einzahlung};
        public enum TimeMode { Zeiterfassung,Tagesübersicht,Wochenübersicht,Monatsübersicht};

        private TimeRegUser oUser;
        private string strVkBur;
        private DataTable dtRuest;
        private RuestzeitList lstRuest = new RuestzeitList();
        private bool m_CanDoEinzahlung;
        private bool m_CanDoAbrechnung;
        private bool m_CanDoÖffnung;
        
        #endregion

        #region "Constructors"

        public TimeRegistrator(TimeRegUser user, string verkaufsbüro)
        {
            oUser = user;
            strVkBur = verkaufsbüro;

            // Gebuchte Rüstzeiten neu laden
            getRuestzeiten(strVkBur);
        }

        #endregion

        #region "Properties"

        public TimeRegUser User 
        {
            get { return oUser; }
        }

        public DataTable Ruestzeiten
        { 
            get{return dtRuest;}
        }

        public string Verkaufsbüro
        {
            get { return strVkBur; }
        }

        public bool CanDoÖffnung
        {
            get { return m_CanDoÖffnung; }
        }

        public bool CanDoAbrechnung
        {
            get { return m_CanDoAbrechnung; }
        }

        public bool CanDoEinzahlung
        {
            get { return m_CanDoEinzahlung; }
        }

        #endregion

        #region "Methods"
        
        /// <summary>
        /// Erfasst eine beliebige Aktion, des aktuellen Bedieners in der aktuellen Filiale mit der aktuelle SAP-Zeit in SAP.
        /// </summary>
        /// <param name="action">Die Aktion welche gestempelt werden soll.</param>
        /// <param name="ruestZeitSchluessel">Rüstzeitschlüssel</param>
        /// <returns>Gibt die in SAP gestempelte Zeit zurück</returns>   
        public string doStampTime(TimeAction action, TimeRuest ruestZeitSchluessel)
        {
            ClearErrorState();

            if (action == TimeAction.NoAction)
            {
                RaiseError("9999", "Es wurde keine gültige Aktion gewählt und daher keine Zeit gebucht!");
                return string.Empty;
            }

            try
            {
                S.AP.Init("Z_HR_ZE_SAVE_POSTING_AUT", "BD_NR, VKBUR", oUser.Kartennummer, strVkBur);

                S.AP.SetImportParameter("SATZART", TranslateAction(action));
                S.AP.SetImportParameter("RUEST", TranslateRuestzeiten(ruestZeitSchluessel));
                S.AP.SetImportParameter("ERNAM", (String.IsNullOrEmpty(oUser.Username) ? "Zeiterf" : oUser.Username.Substring(0, Math.Min(12, oUser.Username.Length))));

                S.AP.Execute();

                if (S.AP.ResultCode != 0)
                {
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage, S.AP.GetExportTable("GT_MESSAGE"));
                    return string.Empty;
                }

                //Auswertung der Export-Parameter
                var strBuZeit = S.AP.GetExportParameter("E_BUZEIT");
                strBuZeit = strBuZeit.Insert(2, ":");
                strBuZeit = strBuZeit.Insert(5, ":");

                // Gebuchte Rüstzeiten neu laden
                getRuestzeiten(strVkBur);

                return strBuZeit;
            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
                return string.Empty;
            }
        }

        #region "Translator" 

        /// <summary>
        /// Übersetzt eine gewählte Aktion in den entsprechenden SAP Steuer-String
        /// </summary>
        /// <param name="action">zu übersetzende Aktion</param>
        /// <returns>SAP Steuer-String</returns>
        public static string TranslateAction(TimeAction action)
        {
            switch(action)
            {
                case TimeAction.Kommen:
                    return "01";
                case TimeAction.Gehen:
                    return "02";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Übersetzt einen SAP Steuer-String in die entsprechende Aktion vom Typ TimeAction
        /// </summary>
        /// <param name="satzart">zu übersetzender SAP Steuer-String</param>
        /// <returns>Aktion</returns>
        public static TimeAction TranslateAction(string satzart)
        {
            switch (satzart.Trim().ToUpper())
            {
                case "01":
                    return TimeAction.Kommen;
                case "02":
                    return TimeAction.Gehen;
                default:
                    return TimeAction.NoAction;
            }
        }

        /// <summary>
        /// Übersetzt einen <code>TimeRuest</code>-Wert in den entsprechenden Rüstzeitschlüssel-String.
        /// </summary>
        /// <param name="ruestzeitschluessel">TimeRuest-Wert</param>
        /// <returns>Rüstzeitschlüssel-String</returns>
        public static string TranslateRuestzeiten(TimeRuest ruestzeitschluessel)
        {
            switch (ruestzeitschluessel)
            { 
                case TimeRuest.Kommen:
                    return "0";                   
                    case TimeRuest.Abrechnung:
                    return "1";                    
                    case TimeRuest.Einzahlung:
                    return "2";                    
                    case TimeRuest.Abrechnung_Einzahlung:
                    return "3";                                       
                default:
                    return string.Empty;                       
            }
        }

        /// <summary>
        /// Übersetzt einen Rüstzeitschlüssel-String in den entsprechenden <code>TimeRuest</code>-Wert.
        /// </summary>
        /// <param name="ruestzeitschluessel">Rüstzeitschlüssel-String</param>
        /// <returns>TimeRuest-Wert</returns>
        public static TimeRuest TranslateRuestzeiten(string ruestzeitschluessel)
        {
            switch (ruestzeitschluessel)
            {
                case "0":
                    return TimeRuest.Kommen;
                case "1":
                    return TimeRuest.Abrechnung;
                case "2":
                    return TimeRuest.Einzahlung;
                case "3":
                    return TimeRuest.Abrechnung_Einzahlung;
                default:
                    return TimeRuest.KeinSchlüssel;
            }
        }

        /// <summary>
        /// Übersetzt ein <code>TimeMode</code> in den für SAP benötigten <code>Charakter</code>
        /// </summary>
        /// <param name="TM">Der zu übersetzende <code>TimeMode</code></param>
        /// <returns>Der <code>Char</code>-Wert</returns>
        public static char TranslateMode(TimeMode TM)
        {
            switch (TM)
            {
                case TimeMode.Zeiterfassung:
                    return 'Z';
                case TimeMode.Tagesübersicht:
                    return 'T';
                case TimeMode.Wochenübersicht:
                    return 'W';
                case TimeMode.Monatsübersicht:
                    return 'M';
                default:
                    return ' ';
            }
        }

        /// <summary>
        /// Übersetzt ein <code>Char</code> in ein für das Web benötigtes <code>TimeMode</code> Objekt
        /// </summary>
        /// <param name="TChar">Der zu übersetzende <code>Char</code>-Wert</param>
        /// <returns>Der übersetzte <code>TimeMode</code></returns>
        public static TimeMode TranslateMode(char TChar)
        {
            switch (TChar)
            {
                case 'Z':
                    return TimeMode.Zeiterfassung;
                case 'T':
                    return TimeMode.Tagesübersicht;
                case 'W':
                    return TimeMode.Wochenübersicht;
                case 'M':
                    return TimeMode.Monatsübersicht;
                default:
                    return TimeMode.Zeiterfassung;
            }
        }

        #endregion

        
        /// <summary>
        /// Liefert die Rüstzeiten zu einem beliebigen Verkaufsbüro.
        /// </summary>
        ///<param name="vkBur">
        /// Kostenstelle des Verkaufsbüros für, dass die Rüstzeiten zurück gegeben werden sollen.
        /// </param>
        public DataTable getRuestzeiten(string vkBur)
        {
            ClearErrorState();

            lstRuest.Clear();

            try
            {
                S.AP.Init("Z_HR_ZE_RUESTZ", "VKBUR", vkBur);

                S.AP.Execute();

                if (S.AP.ResultCode == 0)
                {
                    dtRuest = S.AP.GetExportTable("GT_RUEST");

                    if (dtRuest.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtRuest.Rows)
                        {
                            string Ruestkey = row["RUEST"].ToString();
                            string RuestBezeichnung = row["BEZEI"].ToString();
                            string Ruestzeit = row["RZEIT"].ToString();

                            if (String.IsNullOrEmpty(Ruestzeit) || Ruestzeit == "000")
                                Ruestzeit = "0";

                            string mehrfknz = row["MEHRF"].ToString();
                            bool RuestMulti = (mehrfknz.ToUpper() == "X");

                            lstRuest.Add(new RuestzeitObj(Ruestkey, RuestBezeichnung, RuestMulti, Ruestzeit));
                        }

                        CheckRuestzeiten();
                    }
                    else
                    {
                        RaiseError("9999", "Es konnten keine Rüstzeiten ermittelt werden!");
                    }
                }
                else
                {
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage);
                }
            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
            }

            return dtRuest;
        }

        /// <summary>
        /// Die Funktion prüft, ob für eine Filiale und den angegebenen Tag bereits Rüstzeiten gebucht sind.
        /// </summary>
        /// <param name="vkBur">Verkaufsbüro</param>
        /// <param name="ruestzeitschluessel">Rüstzeitschlüssel</param>
        /// <param name="date">Tagesdatum für das geprüft werden soll</param>
        /// <returns>Gibt <c>True</c> zurück, falls bereits Zeiten gebucht sind.</returns>
        private bool CheckRuestzeitenGebucht(string vkBur, string ruestzeitschluessel, DateTime date)
        {
            ClearErrorState();

            try
            {
                S.AP.Init("Z_HR_ZE_CHECK_REUST_FOR_FIL", "VKBUR", vkBur);

                S.AP.SetImportParameter("RUEST", ruestzeitschluessel);
                S.AP.SetImportParameter("BUDATE", date.ToString("yyyyMMdd"));

                S.AP.Execute();

                if (S.AP.ResultCode == 0)
                    return false;

                if (S.AP.ResultCode == 105)
                    return true;

                RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage);
                return false;
            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Wertet die möglichen zu buchenden Rüstzeiten aus
        /// </summary>
        private void CheckRuestzeiten()
        {
            try
            {
                RuestzeitObj Ruest = lstRuest.getÖffnung();
                if (Ruest.Zeit == "0" || Ruest.Zeit == string.Empty)
                {
                    m_CanDoÖffnung = false;
                }
                else if (Ruest.Multi)
                {
                    m_CanDoÖffnung = true;
                }
                else if (!CheckRuestzeitenGebucht(strVkBur, TranslateRuestzeiten(TimeRuest.Kommen), DateTime.Today))
                {
                    m_CanDoÖffnung = true;
                }
                else
                {
                    m_CanDoÖffnung = false;
                }
            }
            catch (NullReferenceException)
            {
                m_CanDoÖffnung = false;
            }

            try
            { 
               RuestzeitObj Ruest = lstRuest.getAbrechnung();
               if (Ruest.Zeit == "0" || Ruest.Zeit == string.Empty)
               {
                   m_CanDoAbrechnung = false;
               }
               else if (Ruest.Multi)
               {
                   m_CanDoAbrechnung = true;
               }
               else if (!CheckRuestzeitenGebucht(strVkBur, TranslateRuestzeiten(TimeRuest.Abrechnung), DateTime.Today) && !CheckRuestzeitenGebucht(strVkBur, TranslateRuestzeiten(TimeRuest.Abrechnung_Einzahlung), DateTime.Today)) 
               {
                   m_CanDoAbrechnung = true;
               }
               else
               {
                   m_CanDoAbrechnung = false;
               }
            }
            catch(NullReferenceException)
            {
                m_CanDoAbrechnung = false;
            }

            try
            {
                RuestzeitObj Ruest = lstRuest.getEinzahlung();
                if (Ruest.Zeit == "0" || Ruest.Zeit == string.Empty)
                {
                    m_CanDoEinzahlung = false;
                }
                else if (Ruest.Multi)
                {
                    m_CanDoEinzahlung = true;
                }
                else if (!CheckRuestzeitenGebucht(strVkBur, TranslateRuestzeiten(TimeRuest.Einzahlung), DateTime.Today))
                {
                    m_CanDoEinzahlung = true;
                }
                else 
                {
                    m_CanDoEinzahlung = false;
                }
            }
            catch (NullReferenceException)
            {
                m_CanDoEinzahlung = false;
            }
        }
        
        /// <summary>
        /// Erstellt ein Objekt vom Typ TimeOverview, dass alle benötigten Kopf und Positionsdaten enthält.
        /// </summary>
        /// <param name="page">Page-Objekt zum ableiten der SAP-Connection</param>
        /// <returns>Zeitübersicht als TimeOverview-Objekt</returns>
        public TimeOverview getZeitübersicht(ref System.Web.UI.Page page)
        {
            ClearErrorState();

            try
            {
                var vdate = (DateTime.Today.Day < 10 ? DateTime.Today.AddDays(-10).ToString("yyyyMMdd") : new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyyMMdd"));
                var bdate = DateTime.Today.ToString("yyyyMMdd");

                S.AP.Init("Z_HR_ZE_GET_POSTINGS_OF_PERIOD", "BD_NR", oUser.Kartennummer);

                S.AP.SetImportParameter("VDATE", vdate);
                S.AP.SetImportParameter("BDATE", bdate);
                S.AP.SetImportParameter("MODUS", TranslateMode(TimeMode.Monatsübersicht).ToString());

                S.AP.Execute();

                if (S.AP.ResultCode != 0)
                {
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage);
                    return null;
                }

                DataTable gtKopf = S.AP.GetExportTable("GT_KOPF");
                DataTable gtPos = S.AP.GetExportTable("GT_POS");

                if (gtKopf != null && gtPos != null)
                    return new TimeOverview(gtKopf, gtPos, vdate, bdate);
            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Liefert die letzte ausgeführte Aktion für den aktuellen Tag zum aktuellen Benutzer
        /// </summary>
        /// <returns>Letzte Aktion als TimeAction</returns>
        public TimeAction GetLastAction()
        {
            ClearErrorState();

            try
            {
                S.AP.Init("Z_HR_ZE_GET_POSTINGS_OF_PERIOD", "BD_NR", oUser.Kartennummer);

                S.AP.SetImportParameter("VDATE", DateTime.Today.ToString("yyyyMMdd"));
                S.AP.SetImportParameter("BDATE", DateTime.Today.ToString("yyyyMMdd"));
                S.AP.SetImportParameter("MODUS", TranslateMode(TimeMode.Zeiterfassung).ToString());

                S.AP.Execute();

                if (S.AP.ResultCode != 0)
                {
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage);
                    return TimeAction.NoAction;
                }

                var gtKopf = S.AP.GetExportTable("GT_KOPF");
                var gtPos = S.AP.GetExportTable("GT_POS");

                TimeOverview TO = new TimeOverview(gtKopf, gtPos);
                return TO.getLastAction(oUser.Kartennummer); 
            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
                return TimeAction.NoAction;
            }
        }

        /// <summary>
        /// Liefert die letzte ausgeführte Aktion für den aktuellen Tag zum aktuellen Benutzer
        /// </summary>
        /// <returns>Letzte Aktion als DataRow</returns>
        public DataRow GetLastActionRow()
        {
            ClearErrorState();

            try
            {
                S.AP.Init("Z_HR_ZE_GET_POSTINGS_OF_PERIOD", "BD_NR", oUser.Kartennummer);

                S.AP.SetImportParameter("VDATE", DateTime.Today.ToString("yyyyMMdd"));
                S.AP.SetImportParameter("BDATE", DateTime.Today.ToString("yyyyMMdd"));
                S.AP.SetImportParameter("MODUS", TranslateMode(TimeMode.Zeiterfassung).ToString());

                S.AP.Execute();

                if (S.AP.ResultCode != 0)
                {
                    RaiseError(S.AP.ResultCode.ToString(), S.AP.ResultMessage);
                    return null;
                }

                var gtKopf = S.AP.GetExportTable("GT_KOPF");
                var gtPos = S.AP.GetExportTable("GT_POS");

                var to = new TimeOverview(gtKopf, gtPos);
                return to.getLastActionRow(oUser.Kartennummer);
            }
            catch (Exception ex)
            {
                RaiseError("9999", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Prüft ob die letzte gebuchte Zeit für den aktuellen Tag eine Kommen-Zeit ist
        /// </summary>
        /// <returns>True = Letzte Zeit ist eine Kommen-Zeit</returns>
        public bool IsLastTimeKommen()
        {
            return (GetLastAction() == TimeAction.Kommen);
        }

        /// <summary>
        /// Prüft ob die letzte gebuchte Zeit für den aktuellen Tag eine Gehen-Zeit ist
        /// </summary>
        /// <returns>True = Letzte Zeit ist eine Gehen-Zeit</returns>
        public bool IsLastTimeGehen()
        {
            return (GetLastAction() == TimeAction.Gehen);
        }

        /// <summary>
        /// Liefert die aktuelle SAP-Serverzeit
        /// </summary>
        /// <returns>Serverzeit</returns>
        public static string getServerzeit(bool asBuZeit = false)
        {
            try
            {
                S.AP.Init("Z_HR_ZE_GET_SAP_TIME");

                S.AP.Execute();

                if (S.AP.ResultCode != 0)
                    return DateTime.Now.TimeOfDay.ToString().Remove(5);

                var zeit = S.AP.GetExportParameter("TIME");
                return (asBuZeit ? zeit : ParseTimeFromBuzeit(zeit));
            }
            catch (Exception)
            {
                return DateTime.Now.TimeOfDay.ToString().Remove(5);
            }
        }

        /// <summary>
        /// Wandelt einen BUZEIT-String in einen einfachen ShortTimeString um
        /// </summary>
        /// <param name="BuZeit"></param>
        /// <returns>Das geparste DateTime-Objekt</returns>
        private static string ParseTimeFromBuzeit(string BuZeit)
        {
            var dBuzeit = "00:00";
            if (!string.IsNullOrEmpty(BuZeit))
            {
                dBuzeit = BuZeit.Remove(4);
                dBuzeit = dBuzeit.Insert(2, ":");
            }
            return dBuzeit;
        }

        /// <summary>
        /// Liefert eine leere DataTable mit der Struktur eines Kopfsatzes
        /// </summary>
        /// <returns>KopfTabelle</returns>
        public DataTable getKopftabelleZeiten()
        {
            var dtHead = new DataTable();

            dtHead.Columns.Add("BD_NR", typeof(String));
            dtHead.Columns.Add("BUDATE", typeof(String));
            dtHead.Columns.Add("KSTATUS", typeof(String));
            dtHead.Columns.Add("ESTATUS", typeof(String));
            dtHead.Columns.Add("LFBSTATUS", typeof(String));
            dtHead.Columns.Add("ZAOE", typeof(String));
            dtHead.Columns.Add("AZUE", typeof(String));

            dtHead.AcceptChanges();

            return dtHead;
        }

        /// <summary>
        /// Liefert eine leere DataTable mit der Struktur einer Positionstabelle
        /// </summary>
        /// <returns>Positionstabelle</returns>
        public DataTable getPositionstabelleZeiten()
        {
            var dtPos = new DataTable();

            dtPos.Columns.Add("BD_NR", typeof(String));
            dtPos.Columns.Add("BUDATE", typeof(String));
            dtPos.Columns.Add("POSNR", typeof(String));
            dtPos.Columns.Add("VKBUR", typeof(String));
            dtPos.Columns.Add("LFBSTATUS", typeof(String));
            dtPos.Columns.Add("SATZART", typeof(String));
            dtPos.Columns.Add("BUZEIT", typeof(String));
            dtPos.Columns.Add("RUEST", typeof(String));
            dtPos.Columns.Add("RZEIT", typeof(String));
            dtPos.Columns.Add("ARZEIT", typeof(String));
            dtPos.Columns.Add("ZAOE", typeof(String));

            dtPos.AcceptChanges();

            return dtPos;
        }

        public ZeitnachweisListe GetZeitnachweise()
        {
            var zwl = new ZeitnachweisListe();

            zwl.FillNachweise(oUser.Kartennummer,WebConfigurationManager.AppSettings["DownloadPathSamba"].ToString());
            
            return zwl;
        }

        #endregion
    }    
}
