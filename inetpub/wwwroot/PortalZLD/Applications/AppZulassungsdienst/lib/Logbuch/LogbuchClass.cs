using System;
using System.Collections.Generic;
using System.Data;
using CKG.Base.Business;
using SapORM.Models;

namespace AppZulassungsdienst.lib.Logbuch
{
    /// <summary>
    /// Arbeitsklasse die ein Filialbuch darstellt. Sie beinhaltet alle Funktionen zur Kommunikation mit SAP und hält die Daten
    /// die ein Filialbuch kennzeichnen
    /// </summary>
    public class LogbuchClass : SapOrmBusinessBase
    {
        #region "Enumeratoren"

        public enum EntryTyp
        {
            Alle,
            Rückfragen,
            Aufgaben
        }

        public enum Rolle
        {
            Zulassungsdienst,
            Filiale,
            Gebietsleiter
        }

        public enum Antwortart
        {
            Erledigt,
            Gelesen,
            Antwort
        }

        public enum StatusFilter
        {
            Neu,
            Alle
        }

        #endregion

        #region "Globale Objekte"

        private List<VorgangsartRolleDetails> lstVorgangsartenRolle = new List<VorgangsartRolleDetails>();
        private List<VorgangsartDetails> lstVorgangsarten = new List<VorgangsartDetails>();

        #endregion

        #region "Properties"

        public Protokoll Protokoll { get; set; }
        public FilialbuchUser UserLoggedIn { get; set; }
        public StatusFilter letzterStatus { get; set; }
        public DateTime? Von { get; set; }
        public DateTime? Bis { get; set; }

        public List<VorgangsartDetails> Vorgangsarten { get { return lstVorgangsarten; } }

        public List<VorgangsartRolleDetails> VorgangsartenRolle { get { return lstVorgangsartenRolle; } }

        #endregion

        public LogbuchClass(string userReferenz)
        {
            VKORG = ZLDCommon.GetVkOrgFromUserReference(userReferenz);
            VKBUR = ZLDCommon.GetVkBurFromUserReference(userReferenz);

            Protokoll = new Protokoll(ref lstVorgangsarten);
        }

        #region "Shared Functions"

        public static string TranslateRolle(Rolle rolle)
        {
            switch (rolle)
            {
                case Rolle.Zulassungsdienst:
                    return "ZUL";
                case Rolle.Filiale:
                    return "FIL";
                case Rolle.Gebietsleiter:
                    return "GL";
                default:
                    return "FIL";
            }
        }

        public static Rolle TranslateRolle(string rolle)
        {
            switch (rolle)
            {
                case "ZUL":
                    return Rolle.Zulassungsdienst;
                case "FIL":
                    return Rolle.Filiale;
                case "GL":
                    return Rolle.Gebietsleiter;
                default:
                    return Rolle.Filiale;
            }
        }

        public static char TranslateAntwortart(Antwortart antwortart)
        {
            switch (antwortart)
            {
                case Antwortart.Antwort:
                    return 'A';
                case Antwortart.Gelesen:
                    return 'G';
                case Antwortart.Erledigt:
                    return 'E';
                default:
                    return 'G';
            }
        }

        public static Antwortart TranslateAntwortart(char antwortart)
        {
            switch (antwortart)
            {
                case 'A':
                    return Antwortart.Antwort;
                case 'G':
                    return Antwortart.Gelesen;
                case 'E':
                    return Antwortart.Erledigt;
                default:
                    return Antwortart.Gelesen;
            }
        }

        public static string TranslateStatus(StatusFilter status)
        {
            switch (status)
            {
                case StatusFilter.Alle:
                    return "ALL";
                case StatusFilter.Neu:
                    return "NEW";
                default:
                    return "ALL";
            }
        }

        public static StatusFilter TranslateStatus(string status)
        {
            switch (status)
            {
                case "NEW":
                    return StatusFilter.Neu;
                case "ALL":
                    return StatusFilter.Alle;
                default:
                    return StatusFilter.Alle;
            }
        }

        #endregion

        public DataTable GetGroups()
        {
            return new DataTable();
        }

        public void LoginUser(string vbur, string LoginValue)
        {
            ExecuteSapZugriff(() =>
                {
                    Z_MC_CONNECT.Init(SAP, "I_VKBUR, I_BD_NR", vbur, LoginValue);

                    CallBapi();

                    string expUname = SAP.GetExportParameter("E_UNAME");
                    string expBdname = SAP.GetExportParameter("E_BD_NAME");
                    string expRolle = SAP.GetExportParameter("E_ROLLE");
                    string expBezei = SAP.GetExportParameter("E_BEZEI");
                    string expRollepa = SAP.GetExportParameter("E_ROLLE_PA");
                    string expNamepa = SAP.GetExportParameter("E_UNAME_PA");

                    DataTable tblVorgart = SAP.GetExportTable("GT_VORGART");
                    DataTable tblVorgartRolle = SAP.GetExportTable("GT_ROLLE_VGART");

                    // User auswerten
                    if (String.IsNullOrEmpty(LoginValue))
                    {
                        RaiseError(9999, "Es konnte kein Benutzer ermittelt werden.");
                    }
                    else if (String.IsNullOrEmpty(expUname))
                    {
                        RaiseError(9998, "Es konnte keine Rolle ermittelt werden.");
                    }
                    else
                    {
                        UserLoggedIn = new FilialbuchUser(expUname, expBdname, expRolle, expBezei, expRollepa, expNamepa);
                    }

                    // Vorgangsarten auslesen
                    if ((tblVorgart != null))
                    {
                        lstVorgangsarten.Clear();

                        foreach (DataRow dRow in tblVorgart.Rows)
                        {
                            string vgart = dRow["VGART"].ToString();
                            string bezeichnung = dRow["BEZEI"].ToString();
                            var antwArt = dRow["ANTW_ART"].ToString();
                            char antwortart = (String.IsNullOrEmpty(antwArt) ? ' ' : antwArt[0]);
                            bool filialbuchvorgang = false;
                            if ((dRow["FILIALBUCH_VG"] != null))
                            {
                                if (dRow["FILIALBUCH_VG"].ToString().ToUpper() == "F")
                                {
                                    filialbuchvorgang = true;
                                }
                            }
                            lstVorgangsarten.Add(new VorgangsartDetails(vgart, bezeichnung, antwortart, filialbuchvorgang));
                        }
                    }

                    // Vorgangsarten zur Rolle auslesen
                    if ((tblVorgartRolle != null))
                    {
                        lstVorgangsartenRolle.Clear();

                        foreach (DataRow dRow in tblVorgartRolle.Rows)
                        {
                            Rolle rolle = TranslateRolle(dRow["ROLLE"].ToString());
                            string vgart = dRow["VGART"].ToString();
                            var stf = dRow["STUFE"].ToString();
                            char stufe = (String.IsNullOrEmpty(stf) ? ' ' : stf[0]);
                            bool close = false;
                            if ((dRow["CLOSE_EMPF"] != null))
                            {
                                if (dRow["CLOSE_EMPF"].ToString().ToUpper() == "X")
                                {
                                    close = true;
                                }
                            }
                            lstVorgangsartenRolle.Add(new VorgangsartRolleDetails(rolle, vgart, stufe, close));
                        }
                    }
                });
        }

        public void GetEinträge(FilialbuchUser FilBuUser, StatusFilter status, string an_kst = null, DateTime? vonDate = null, DateTime? bisDate = null)
        {
            ExecuteSapZugriff(() =>
                {
                    this.Von = vonDate;
                    this.Bis = bisDate;
                    this.letzterStatus = status;

                    string strVon = "";
                    string strBis = "";

                    if (status == StatusFilter.Alle)
                    {
                        if (vonDate == null)
                        {
                            RaiseError(9999, "Es wurde kein gültiges Von-Datum für die Auswahl mitgegeben!");
                            return;
                        }

                        if (bisDate == null)
                        {
                            RaiseError(9999, "Es wurde kein gültiges Bis-Datum für die Auswahl mitgegeben!");
                            return;
                        }

                        strVon = ((DateTime)vonDate).ToShortDateString();
                        strBis = ((DateTime)bisDate).ToShortDateString();
                    }

                    Z_MC_GET_IN_OUT.Init(SAP);

                    SAP.SetImportParameter("I_UNAME", FilBuUser.UsernameSAP);
                    SAP.SetImportParameter("I_STATUS", TranslateStatus(status));
                    SAP.SetImportParameter("I_VON", strVon);
                    SAP.SetImportParameter("I_BIS", strBis);

                    CallBapi();

                    DataTable tblEingang = SAP.GetExportTable("GT_IN");
                    DataTable tblAusgang = SAP.GetExportTable("GT_OUT");

                    Protokoll = new Protokoll(ref lstVorgangsarten);

                    // Eingang auswerten
                    if ((tblEingang != null))
                    {
                        foreach (DataRow dRow in tblEingang.Rows)
                        {
                            int erfHour = 0;
                            int erfMinute = 0;
                            int erfSecond = 0;

                            string erfZpt = dRow["ERZEIT"].ToString();
                            if (!String.IsNullOrEmpty(erfZpt))
                            {
                                Int32.TryParse(erfZpt.Substring(0, 2), out erfHour);
                                Int32.TryParse(erfZpt.Substring(2, 2), out erfMinute);
                                Int32.TryParse(erfZpt.Substring(4, 2), out erfSecond);
                            }
                            DateTime erfDat = DateTime.Parse(dRow["ERDAT"].ToString()).AddHours(erfHour).AddMinutes(erfMinute).AddSeconds(erfSecond);

                            if (an_kst != null)
                            {
                                //Filter auf Kostenstelle und User
                                if (dRow["VON"].ToString() == an_kst | dRow["VON"].ToString() == UserLoggedIn.UsernameSAP)
                                {
                                    Protokoll.addEntry(Protokoll.Side.Input, new Eingang(dRow["VORGID"].ToString(), dRow["LFDNR"].ToString(), erfDat,
                                        dRow["VON"].ToString(), dRow["VERTR"].ToString(), dRow["BETREFF"].ToString(), dRow["LTXNR"].ToString(),
                                        dRow["ANTW_LFDNR"].ToString(), LogbuchEntry.TranslateEntryStatus(dRow["STATUS"].ToString()),
                                        LogbuchEntry.TranslateEmpfängerStatus(dRow["STATUSE"].ToString()), dRow["VGART"].ToString(), dRow["ZERLDAT"].ToString(),
                                        UserLoggedIn.UsernameSAP));
                                }
                            }
                            else
                            {
                                Protokoll.addEntry(Protokoll.Side.Input, new Eingang(dRow["VORGID"].ToString(), dRow["LFDNR"].ToString(), erfDat,
                                    dRow["VON"].ToString(), dRow["VERTR"].ToString(), dRow["BETREFF"].ToString(), dRow["LTXNR"].ToString(),
                                    dRow["ANTW_LFDNR"].ToString(), LogbuchEntry.TranslateEntryStatus(dRow["STATUS"].ToString()),
                                    LogbuchEntry.TranslateEmpfängerStatus(dRow["STATUSE"].ToString()), dRow["VGART"].ToString(), dRow["ZERLDAT"].ToString(),
                                    UserLoggedIn.UsernameSAP));
                            }
                        }
                    }

                    // Ausgang auswerten
                    if ((tblAusgang != null))
                    {
                        foreach (DataRow dRow in tblAusgang.Rows)
                        {
                            int erfHour = 0;
                            int erfMinute = 0;
                            int erfSecond = 0;

                            string erfZpt = dRow["ERZEIT"].ToString();
                            if (!String.IsNullOrEmpty(erfZpt))
                            {
                                Int32.TryParse(erfZpt.Substring(0, 2), out erfHour);
                                Int32.TryParse(erfZpt.Substring(2, 2), out erfMinute);
                                Int32.TryParse(erfZpt.Substring(4, 2), out erfSecond);
                            }
                            DateTime erfDat = DateTime.Parse(dRow["ERDAT"].ToString()).AddHours(erfHour).AddMinutes(erfMinute).AddSeconds(erfSecond);

                            if ((an_kst != null))
                            {
                                //Filter auf Kostenstelle und User
                                if (dRow["ZAN"].ToString() == an_kst | dRow["ZAN"].ToString() == UserLoggedIn.UsernameSAP)
                                {
                                    Protokoll.addEntry(Protokoll.Side.Output, new Ausgang(dRow["VORGID"].ToString(), dRow["LFDNR"].ToString(), erfDat,
                                        dRow["ZAN"].ToString(), dRow["VERTR"].ToString(), dRow["BETREFF"].ToString(), dRow["LTXNR"].ToString(),
                                        dRow["ANTW_LFDNR"].ToString(), LogbuchEntry.TranslateEntryStatus(dRow["STATUS"].ToString()),
                                        LogbuchEntry.TranslateEmpfängerStatus(dRow["STATUSE"].ToString()), dRow["VGART"].ToString(), dRow["ZERLDAT"].ToString(),
                                        dRow["ERLDAT"].ToString()));
                                }
                            }
                            else
                            {
                                Protokoll.addEntry(Protokoll.Side.Output, new Ausgang(dRow["VORGID"].ToString(), dRow["LFDNR"].ToString(), erfDat,
                                    dRow["ZAN"].ToString(), dRow["VERTR"].ToString(), dRow["BETREFF"].ToString(), dRow["LTXNR"].ToString(),
                                    dRow["ANTW_LFDNR"].ToString(), LogbuchEntry.TranslateEntryStatus(dRow["STATUS"].ToString()),
                                    LogbuchEntry.TranslateEmpfängerStatus(dRow["STATUSE"].ToString()), dRow["VGART"].ToString(), dRow["ZERLDAT"].ToString(),
                                    dRow["ERLDAT"].ToString()));
                            }
                        }
                    }
                });
        }

        public void NeuerEintrag(string Betreff, string Text, string an, string vorgangsart, string zuerledigenBis, string userName)
        {
            LongStringToSap lsts = new LongStringToSap();
            string ltxnr = "";
            if (Text.Trim().Length != 0)
            {
                ltxnr = lsts.InsertString(Text, "MC");
                if (lsts.ErrorOccured)
                {
                    RaiseError(lsts.ErrorCode, lsts.Message);
                    return;
                }
            }

            ExecuteSapZugriff(() =>
                {
                    Z_MC_NEW_VORGANG.Init(SAP);

                    SAP.SetImportParameter("I_UNAME", UserLoggedIn.UsernameSAP);
                    SAP.SetImportParameter("I_BD_NR", userName.ToUpper());
                    SAP.SetImportParameter("I_AN", an);
                    SAP.SetImportParameter("I_LTXNR", ltxnr);
                    SAP.SetImportParameter("I_BETREFF", Betreff);
                    SAP.SetImportParameter("I_VGART", vorgangsart);
                    SAP.SetImportParameter("I_ZERLDAT", zuerledigenBis);
                    SAP.SetImportParameter("I_VKBUR", VKBUR);

                    CallBapi();
                });

            GetEinträge(UserLoggedIn, letzterStatus, an, Von, Bis);
        }

        public string GetAntwortToVorgangsart(string vgart)
        {
            VorgangsartDetails Antwort = lstVorgangsarten.Find(vg =>
            {
                if (vg.Vorgangsart == vgart)
                {
                    return true;
                }
                return false;
            });
            return Antwort.Antwortart.ToString();
        }
    }

    #region "Strukturen"

    /// <summary>
    /// Daten eines angemeldeten Filialbuchbenutzers
    /// </summary>
    /// <remarks></remarks>
    public class FilialbuchUser
    {
        public string UsernameSAP { get; set; }
        public string Bedienername { get; set; }
        public LogbuchClass.Rolle Rolle { get; set; }
        public string RollenName { get; set; }
        public string RollePa { get; set; }
        public string NamePa { get; set; }

        public FilialbuchUser(string usernamesap, string bedienername, string rolle, string rollenname, string rollepa, string namepa)
        {
            this.UsernameSAP = usernamesap;
            this.Bedienername = bedienername;
            this.Rolle = LogbuchClass.TranslateRolle(rolle.Trim().ToUpper());
            this.RollenName = rollenname;
            this.RollePa = rollepa;
            this.NamePa = namepa;
        }
    }

    /// <summary>
    /// Detailwerte Vorgangsart der Rolle
    /// </summary>
    /// <remarks></remarks>
    public class VorgangsartRolleDetails
    {
        public LogbuchClass.Rolle Rolle { get; set; }
        public string Vorgangsart { get; set; }
        public char Stufe { get; set; }
        public bool VonEmpfängerZuSchliessen { get; set; }

        public VorgangsartRolleDetails(LogbuchClass.Rolle rolle, string vorgangsart, char stufe, bool schliessbar)
        {
            this.Rolle = rolle;
            this.Vorgangsart = vorgangsart;
            this.Stufe = stufe;
            this.VonEmpfängerZuSchliessen = schliessbar;
        }
    }

    /// <summary>
    /// Detailwerte Vorgangsart
    /// </summary>
    /// <remarks></remarks>
    public class VorgangsartDetails
    {
        public string Vorgangsart { get; set; }
        public string Bezeichnung { get; set; }
        public char Antwortart { get; set; }
        public bool Filialbuchvorgang { get; set; }

        public VorgangsartDetails(string vorgangsart, string bezeichnung, char antwortart, bool Filialbuchvorgang)
        {
            this.Vorgangsart = vorgangsart;
            this.Bezeichnung = bezeichnung;
            this.Antwortart = antwortart;
            this.Filialbuchvorgang = Filialbuchvorgang;
        }
    }

    #endregion

}

