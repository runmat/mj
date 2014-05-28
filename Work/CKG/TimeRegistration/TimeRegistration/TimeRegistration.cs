using System;
using System.Data;
using System.Web.Configuration;

namespace TimeRegistration
{
    public class TimeRegistrator
    {
        #region "global Declarations"

        public enum TimeAction { Kommen, Gehen, NoAction }; //, PauseStart, PauseEnde
        public enum TimeRuest { Kommen, Abrechnung, Einzahlung, Abrechnung_Einzahlung, KeinSchlüssel } //Öffnungszeit, ÖffnungszeitOhneRüstzeit
        public enum TimePlus { Abrechnung, Einzahlung};
        public enum TimeMode { Zeiterfassung,Tagesübersicht,Wochenübersicht,Monatsübersicht};

        private TimeRegUser oUser;
        private string strVkBur;
        private string E_MESSAGE = "";
        private string E_SUBRC = "0";
        private DataTable GT_MESSAGE;
        private bool bError;
        private  string sConStr;
        private DataTable dtRuest;
        private RuestzeitList lstRuest = new RuestzeitList();
        private bool m_CanDoEinzahlung;
        private bool m_CanDoAbrechnung;
        private bool m_CanDoÖffnung;

        SAPExecutor.SAPExecutor SAPExc;
        
        #endregion


        #region "Constructors"

        public TimeRegistrator(TimeRegUser user,string verkaufsbüro)
        {
            oUser = user;
            strVkBur = verkaufsbüro;
            sConStr = user.SAPConnectionString;
            SAPExc = new SAPExecutor.SAPExecutor(sConStr);

            // Gebuchte Rüstzeiten neu laden
            getRuestzeiten(strVkBur);
        }

        #endregion


        #region "Properties"

        public TimeRegUser User 
        {
            get {return oUser ;}
        }

        public string ErrorMessage
        {
            get { return E_MESSAGE; }
        }

        public string ErrorCode
        {
            get { return E_SUBRC; }
        }

        public DataTable ErrorTable
        {
            get { return GT_MESSAGE; }
        }

        public bool ErrorOccured
        {
            get { return bError; }
        }

        public string SAPConnectionString
        {
            get { return sConStr; }
            set { sConStr = value; }
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
            ResetError();

            if (action == TimeAction.NoAction)
            {
                bError = true;
                E_SUBRC = "w999";
                E_MESSAGE = "Es wurde keine gültige Aktion gewählt und daher keine Zeit gebucht!";
                return string.Empty;
            }
            else
            {
                DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

                // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
                //Import-Parameter
                dt.Rows.Add(new object[] { "BD_NR", false, oUser.Kartennummer, 4 });
                dt.Rows.Add(new object[] { "VKBUR", false, strVkBur });
                dt.Rows.Add(new object[] { "SATZART", false, TranslateAction(action) });
                dt.Rows.Add(new object[] { "RUEST", false, TranslateRuestzeiten(ruestZeitSchluessel) });

               

                if (oUser.Username == string.Empty)
                { dt.Rows.Add(new object[] { "ERNAM", false, "Zeiterf" }); }
                else
                {
                    String sUserName = oUser.Username;
                    if (sUserName.Length > 12) sUserName = oUser.Username.Remove(12);

                    dt.Rows.Add(new object[] { "ERNAM", false, sUserName }); 
                }

                //Export-Parameter
                dt.Rows.Add(new object[] { "E_BUZEIT", true });
                dt.Rows.Add(new object[] { "GT_MESSAGE", true });
               
                SAPExc.ExecuteERP("Z_HR_ZE_SAVE_POSTING_AUT", ref dt);

                if (SAPExc.ErrorOccured)
                {
                    bError = true;
                    E_SUBRC = SAPExc.E_SUBRC;
                    E_MESSAGE = SAPExc.E_MESSAGE;
                    DataRow retRows = dt.Select("Fieldname='GT_MESSAGE'")[0];
                    if (retRows != null) 
                    {
                        GT_MESSAGE = (DataTable)retRows["Data"];
                    }
                    
                    return string.Empty;
                }

                // Gebuchte Rüstzeiten neu laden
                getRuestzeiten(strVkBur);

                //Auswertung der Export-Parameter
                var strBuZeit = (string)dt.Rows[5]["Data"];
                strBuZeit = strBuZeit.Insert(2, ":");
                strBuZeit = strBuZeit.Insert(5, ":");

                return strBuZeit;
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
                /*case TimeAction.PauseStart:
                   return "P15";
                case TimeAction.PauseEnde:
                    return "P25";*/
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
                /*case "P15":
                    return TimeAction.PauseStart;
                case "P25":
                    return TimeAction.PauseEnde;*/
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
                    // case TimeRuest.Öffnungszeit:
                    //return "4";
                    // case TimeRuest.ÖffnungszeitOhneRüstzeit:
                    //return "5";                    
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
                //case "4":
                //    return TimeRuest.Öffnungszeit;
                //case "5":
                //    return TimeRuest.ÖffnungszeitOhneRüstzeit;
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
            ResetError();
            lstRuest.Clear();

            DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

            // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
            dt.Rows.Add(new object[] { "VKBUR", false, vkBur });
            dt.Rows.Add(new object[] { "GT_RUEST", true });
           
            SAPExc.ExecuteERP("Z_HR_ZE_RUESTZ", ref dt);

            if (SAPExc.ErrorOccured)
            {
                bError = true;
                E_SUBRC = SAPExc.E_SUBRC;
                E_MESSAGE = SAPExc.E_MESSAGE;
            }
            else
            {
                dtRuest = (DataTable)dt.Rows[1]["Data"];
                if (dtRuest.Rows.Count > 0)
                {
                    string Ruestkey = " ";
                    string RuestBezeichnung = string.Empty;
                    string Ruestzeit = string.Empty;
                    bool RuestMulti = false;

                    foreach (DataRow row in dtRuest.Rows)
                    {
                        Ruestkey = row["RUEST"].ToString();
                        RuestBezeichnung = row["BEZEI"].ToString();
                        Ruestzeit = row["RZEIT"].ToString();
                        if (Ruestzeit == "000" || Ruestzeit == string.Empty)
                        {
                            Ruestzeit = "0";
                        }
                        
                        string mehrfknz = row["MEHRF"].ToString();
                        if (mehrfknz.ToUpper() == "X")
                        { RuestMulti = true; }
                        else { RuestMulti = false; }

                        lstRuest.Add(new RuestzeitObj(Ruestkey, RuestBezeichnung, RuestMulti, Ruestzeit));                                               
                    }

                    CheckRuestzeiten();
                }
                else 
                {
                    bError = true;
                    E_SUBRC = "w999";
                    E_MESSAGE = "Es konnten keine Rüstzeiten ermittelt werden!";
                }
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
            DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

            // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
            dt.Rows.Add(new object[] { "VKBUR", false, vkBur });
            dt.Rows.Add(new object[] { "RUEST", false, ruestzeitschluessel });
            dt.Rows.Add(new object[] { "BUDATE", false, SAPExecutor.SAPExecutor.MakeSAPDate(date) });
                      
            SAPExc.ExecuteERP("Z_HR_ZE_CHECK_REUST_FOR_FIL",ref dt);

            if (SAPExc.ErrorOccured)
            {
                if(SAPExc.E_SUBRC == "105")
                {
                    return true;
                }
                else
                {
                    bError = true;
                    E_SUBRC = SAPExc.E_SUBRC;
                    E_MESSAGE = SAPExc.E_MESSAGE;
                    return false;
                }
            }
            else
            {
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
            catch (NullReferenceException ex)
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
            catch(NullReferenceException ex)
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
            catch (NullReferenceException ex)
            {
                m_CanDoEinzahlung = false;
            }
        }

        /// <summary>
        /// Setzt den Fehlerstatus zurück
        /// </summary>
        private void ResetError()
        {
            bError = false;
            E_SUBRC = "0";
            E_MESSAGE = string.Empty;
            GT_MESSAGE = null;
        }
        
        
        /// <summary>
        /// Erstellt ein Objekt vom Typ TimeOverview, dass alle benötigten Kopf und Positionsdaten enthält.
        /// </summary>
        /// <param name="page">Page-Objekt zum ableiten der SAP-Connection</param>
        /// <returns>Zeitübersicht als TimeOverview-Objekt</returns>
        public TimeOverview getZeitübersicht(ref System.Web.UI.Page page)
        {
            ResetError();

            string vdate;
            if (DateTime.Today.Day < 10) vdate = SAPExecutor.SAPExecutor.MakeSAPDate(DateTime.Today.AddDays(-10));
            else
            {
                var date = new DateTime(DateTime.Today.Year,DateTime.Today.Month, 1);
                vdate = SAPExecutor.SAPExecutor.MakeSAPDate(date);
            }

            string bdate=SAPExecutor.SAPExecutor.MakeSAPDate(DateTime.Today);

            DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

            // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
            dt.Rows.Add(new object[] { "BD_NR", false, oUser.Kartennummer });
            dt.Rows.Add(new object[] { "VDATE", false, vdate });            
            dt.Rows.Add(new object[] { "BDATE", false, bdate });
            dt.Rows.Add(new object[] { "MODUS", false, TranslateMode(TimeMode.Monatsübersicht).ToString()});
            dt.Rows.Add(new object[] { "NAME", true });
            dt.Rows.Add(new object[] { "OFFEN", true });
            dt.Rows.Add(new object[] { "GT_KOPF", true });
            dt.Rows.Add(new object[] { "GT_POS", true });

            SAPExc.ExecuteERP("Z_HR_ZE_GET_POSTINGS_OF_PERIOD", ref dt);

            if (SAPExc.ErrorOccured)
            {
                bError = true;
                E_SUBRC = SAPExc.E_SUBRC;
                E_MESSAGE = SAPExc.E_MESSAGE;

                return null;
            }

            DataTable gtKopf = null;
            DataTable gtPos = null;
            DataRow retRows = dt.Select("Fieldname='GT_Kopf'")[0];

            if (retRows != null) gtKopf = (DataTable)retRows["Data"];
                
            retRows = dt.Select("Fieldname='GT_POS'")[0];

            if (retRows != null) gtPos = (DataTable)retRows["Data"];
                
            if (gtKopf != null && gtPos != null) return new TimeOverview(gtKopf, gtPos,vdate,bdate);

            return null;
        }

        /// <summary>
        /// Liefert die letzte ausgeführte Aktion für den aktuellen Tag zum aktuellen Benutzer
        /// </summary>
        /// <returns>Letzte Aktion als TimeAction</returns>
        public TimeAction GetLastAction()
        {
            ResetError();
            DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

            // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
            dt.Rows.Add(new object[] { "BD_NR", false, oUser.Kartennummer });
            dt.Rows.Add(new object[] { "VDATE", false, SAPExecutor.SAPExecutor.MakeSAPDate(DateTime.Today) });
            dt.Rows.Add(new object[] { "BDATE", false, SAPExecutor.SAPExecutor.MakeSAPDate(DateTime.Today) });
            dt.Rows.Add(new object[] { "MODUS", false, TranslateMode(TimeMode.Zeiterfassung).ToString() });
            dt.Rows.Add(new object[] { "NAME", true });
            dt.Rows.Add(new object[] { "OFFEN", true });
            dt.Rows.Add(new object[] { "GT_KOPF", true });
            dt.Rows.Add(new object[] { "GT_POS", true });

            SAPExc.ExecuteERP("Z_HR_ZE_GET_POSTINGS_OF_PERIOD", ref dt);

            if (SAPExc.ErrorOccured)
            {
                bError = true;
                E_SUBRC = SAPExc.E_SUBRC;
                E_MESSAGE = SAPExc.E_MESSAGE;

                return TimeAction.NoAction;
            }
            else
            {
                TimeOverview TO = new TimeOverview((DataTable)dt.Rows[6]["Data"], (DataTable)dt.Rows[7]["Data"]);
                return TO.getLastAction(oUser.Kartennummer);        
            }
        }

        /// <summary>
        /// Liefert die letzte ausgeführte Aktion für den aktuellen Tag zum aktuellen Benutzer
        /// </summary>
        /// <returns>Letzte Aktion als DataRow</returns>
        public DataRow GetLastActionRow()
        {
            ResetError();
            DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

            // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
            dt.Rows.Add(new object[] { "BD_NR", false, oUser.Kartennummer });
            dt.Rows.Add(new object[] { "VDATE", false, SAPExecutor.SAPExecutor.MakeSAPDate(DateTime.Today) });
            dt.Rows.Add(new object[] { "BDATE", false, SAPExecutor.SAPExecutor.MakeSAPDate(DateTime.Today) });
            dt.Rows.Add(new object[] { "MODUS", false, TranslateMode(TimeMode.Zeiterfassung).ToString() });
            dt.Rows.Add(new object[] { "NAME", true });
            dt.Rows.Add(new object[] { "OFFEN", true });
            dt.Rows.Add(new object[] { "GT_KOPF", true });
            dt.Rows.Add(new object[] { "GT_POS", true });

            SAPExc.ExecuteERP("Z_HR_ZE_GET_POSTINGS_OF_PERIOD", ref dt);

            if (SAPExc.ErrorOccured)
            {
                bError = true;
                E_SUBRC = SAPExc.E_SUBRC;
                E_MESSAGE = SAPExc.E_MESSAGE;

                return null;
            }

            var to = new TimeOverview((DataTable)dt.Rows[6]["Data"], (DataTable)dt.Rows[7]["Data"]);
            return to.getLastActionRow(oUser.Kartennummer);
        }

        /// <summary>
        /// Prüft ob die letzte gebuchte Zeit für den aktuellen Tag eine Kommen-Zeit ist
        /// </summary>
        /// <returns>True = Letzte Zeit ist eine Kommen-Zeit</returns>
        public bool IsLastTimeKommen()
        {
            if (GetLastAction() == TimeAction.Kommen)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prüft ob die letzte gebuchte Zeit für den aktuellen Tag eine Gehen-Zeit ist
        /// </summary>
        /// <returns>True = Letzte Zeit ist eine Gehen-Zeit</returns>
        public bool IsLastTimeGehen()
        {
            if (GetLastAction() == TimeAction.Gehen)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Liefert die aktuelle SAP-Serverzeit
        /// </summary>
        /// <returns>Serverzeit als Date-Objekt</returns>
        public string getServerzeit()
        {
            ResetError();
            
            try
            {
                DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

                // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
                dt.Rows.Add(new object[] { "TIME", true });

                SAPExc.ExecuteERP("Z_HR_ZE_GET_SAP_TIME", ref dt);

                if (SAPExc.ErrorOccured)
                {
                    bError = true;
                    E_SUBRC = SAPExc.E_SUBRC;
                    E_MESSAGE = SAPExc.E_MESSAGE;

                    return DateTime.Now.TimeOfDay.ToString().Remove(5);
                }
                return ParseTimeFromBuzeit(dt.Rows[0]["Data"].ToString());
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay.ToString().Remove(5);
            }
        }

        public string getServerzeitAsBUZEIT()
        {
            ResetError();

            try
            {
                DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

                // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
                dt.Rows.Add(new object[] { "TIME", true });

                SAPExc.ExecuteERP("Z_HR_ZE_GET_SAP_TIME", ref dt);

                if (SAPExc.ErrorOccured)
                {
                    bError = true;
                    E_SUBRC = SAPExc.E_SUBRC;
                    E_MESSAGE = SAPExc.E_MESSAGE;

                    return DateTime.Now.TimeOfDay.ToString().Remove(5);
                }
                return dt.Rows[0]["Data"].ToString();
            }
            catch (Exception ex)
            {
                return DateTime.Now.TimeOfDay.ToString().Remove(7);
            }
        }

        /// <summary>
        /// Liefert die aktuelle SAP-Serverzeit
        /// </summary>
        /// <returns>Serverzeit als Date-Objekt</returns>
        public static string getServerzeit(string ConStr)
        {
            try
            {
                var sExc = new SAPExecutor.SAPExecutor(ConStr);
                var dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();

                // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object, Feldlänge (0=unbestimmt)}
                dt.Rows.Add(new object[] { "TIME", true });
                
                sExc.ExecuteERP("Z_HR_ZE_GET_SAP_TIME", ref dt);

                if (sExc.ErrorOccured)
                {
                    return DateTime.Now.TimeOfDay.ToString().Remove(5);
                }
                return ParseTimeFromBuzeit(dt.Rows[0]["Data"].ToString());
            }
            catch(Exception ex)
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

            dtHead.Columns.Add("BD_NR", Type.GetType("System.String"));
            dtHead.Columns.Add("BUDATE", Type.GetType("System.String"));
            dtHead.Columns.Add("KSTATUS", Type.GetType("System.String"));
            dtHead.Columns.Add("ESTATUS", Type.GetType("System.String"));
            dtHead.Columns.Add("LFBSTATUS", Type.GetType("System.String"));
            dtHead.Columns.Add("ZAOE", Type.GetType("System.String"));
            dtHead.Columns.Add("AZUE", Type.GetType("System.String"));

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

            dtPos.Columns.Add("BD_NR", Type.GetType("System.String"));
            dtPos.Columns.Add("BUDATE", Type.GetType("System.String"));
            dtPos.Columns.Add("POSNR", Type.GetType("System.String"));
            dtPos.Columns.Add("VKBUR", Type.GetType("System.String"));
            dtPos.Columns.Add("LFBSTATUS", Type.GetType("System.String"));
            dtPos.Columns.Add("SATZART", Type.GetType("System.String"));
            dtPos.Columns.Add("BUZEIT", Type.GetType("System.String"));
            dtPos.Columns.Add("RUEST", Type.GetType("System.String"));
            dtPos.Columns.Add("RZEIT", Type.GetType("System.String"));
            dtPos.Columns.Add("ARZEIT", Type.GetType("System.String"));
            dtPos.Columns.Add("ZAOE", Type.GetType("System.String"));

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
