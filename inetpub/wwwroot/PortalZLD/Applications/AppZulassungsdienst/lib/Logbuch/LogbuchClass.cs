using System;
using System.Collections.Generic;
using System.Data;
using CKG.Base.Business;
using CKG.Base.Common;

namespace AppZulassungsdienst.lib.Logbuch
{
	/// <summary>
	/// Arbeitsklasse die ein Filialbuch darstellt. Sie beinhaltet alle Funktionen zur Kommunikation mit SAP und hält die Daten
	/// die ein Filialbuch kennzeichnen
	/// </summary>
	/// <remarks></remarks>
	public class LogbuchClass : BankBase
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
			LFB
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
        public string VkOrg { get; set; }
        public string VkBur { get; set; }
		public StatusFilter letzterStatus { get; set; }
		public System.DateTime? Von { get; set; }
		public System.DateTime? Bis { get; set; }

		public List<VorgangsartDetails> Vorgangsarten 
        {
			get { return lstVorgangsarten; }
		}

        public List<VorgangsartRolleDetails> VorgangsartenRolle 
        {
			get { return lstVorgangsartenRolle; }
		}

		#endregion

        public LogbuchClass(ref CKG.Base.Kernel.Security.User objUser, CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
		{
            if ((objUser != null) && (!String.IsNullOrEmpty(objUser.Reference)))
            {
                if (objUser.Reference.Length > 4)
                {
                    VkOrg = objUser.Reference.Substring(0, 4);
                    VkBur = objUser.Reference.Substring(4);
                }
                else
                {
                    VkOrg = objUser.Reference;
                    VkBur = "";
                }
            }
            else
            {
                VkOrg = "";
                VkBur = "";
            }

			Protokoll = new Protokoll(ref lstVorgangsarten, ref m_objUser, m_objApp, strAppID, strSessionID, strFilename);
		}

		#region "Shared Functions"

		/// <summary>
		/// Übersetzt Rollenwerte für SAP
		/// </summary>
		/// <param name="rolle">Rolle</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string TranslateRolle(Rolle rolle)
		{
			switch (rolle) {
                case Rolle.Zulassungsdienst:
                    return "ZUL";
				case Rolle.Filiale:
					return "FIL";
				case Rolle.LFB:
					return "LFB";
				default:
					return "FIL";
			}
		}

		/// <summary>
		/// Übersetzt Rollenwerte aus SAP
		/// </summary>
		/// <param name="rolle">Rolle</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static Rolle TranslateRolle(string rolle)
		{
			switch (rolle) {
                case "ZUL":
			        return Rolle.Zulassungsdienst;
				case "FIL":
					return Rolle.Filiale;
				case "LFB":
					return Rolle.LFB;
				default:
					return Rolle.Filiale;
			}
		}

		/// <summary>
		/// Übersetzt Antwortarten für SAP
		/// </summary>
		/// <param name="antwortart">Antwortart</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static char TranslateAntwortart(Antwortart antwortart)
		{
			switch (antwortart) {
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

		/// <summary>
		/// Übersetzt Antwortarten für SAP
		/// </summary>
		/// <param name="antwortart">Antwortart</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static Antwortart TranslateAntwortart(char antwortart)
		{
			switch (antwortart) {
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

		/// <summary>
		/// Übersetzt Eintragsstatus für SAP
		/// </summary>
		/// <param name="status">Status</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static string TranslateStatus(StatusFilter status)
		{
			switch (status) {
				case StatusFilter.Alle:
					return "ALL";
				case StatusFilter.Neu:
					return "NEW";
				default:
					return "ALL";
			}
		}

		/// <summary>
		/// Übersetzt Eintragsstatus für SAP
		/// </summary>
		/// <param name="status">Status</param>
		/// <returns></returns>
		/// <remarks></remarks>
		public static StatusFilter TranslateStatus(string status)
		{
			switch (status) {
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

		public FilialbuchUser LoginUser(string strAppID, string strSessionID, System.Web.UI.Page page, string VkBur, string LoginValue)
		{
            m_strClassAndMethod = "LogbuchClass.LoginUser";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_CONNECT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VKBUR", VkBur);
                    myProxy.setImportParameter("I_BD_NR", LoginValue);

                    myProxy.callBapi();

                    string expUname = myProxy.getExportParameter("E_UNAME");
                    string expBdname = myProxy.getExportParameter("E_BD_NAME");
                    string expRolle = myProxy.getExportParameter("E_ROLLE");
                    string expBezei = myProxy.getExportParameter("E_BEZEI");
                    string expRollepa = myProxy.getExportParameter("E_ROLLE_PA");
                    string expNamepa = myProxy.getExportParameter("E_UNAME_PA");

                    DataTable tblVorgart = myProxy.getExportTable("GT_VORGART");
                    DataTable tblVorgartRolle = myProxy.getExportTable("GT_ROLLE_VGART");

                    // User auswerten
                    if (String.IsNullOrEmpty(LoginValue))
                    {
                        m_intStatus = 9999;
                        m_strMessage = "Es konnte kein Benutzer ermittelt werden.";
                    }
                    else if (String.IsNullOrEmpty(expUname))
                    {
                        m_intStatus = 9998;
                        m_strMessage = "Es konnte keine Rolle ermittelt werden.";
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
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                }
                finally { m_blnGestartet = false; }
            }

            return UserLoggedIn;
		}


		public void GetEinträge(string strAppID, string strSessionID, System.Web.UI.Page page, FilialbuchUser FilBuUser, 
            StatusFilter status, string an_kst = null, DateTime? Von = null, DateTime? Bis = null)
		{
            m_strClassAndMethod = "LogbuchClass.GetEinträge";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                this.Von = Von;
                this.Bis = Bis;
                this.letzterStatus = status;

                string strVon = "";
                string strBis = "";

                if (status == StatusFilter.Alle)
                {
                    if (Von == null)
                    {
                        m_intStatus = 9999;
                        m_strMessage = "Es wurde kein gültiges Von-Datum für die Auswal mitgegeben!";
                        return;
                    }
                    else if (Bis == null)
                    {
                        m_intStatus = 9999;
                        m_strMessage = "Es wurde kein gültiges Bis-Datum für die Auswal mitgegeben!";
                        return;
                    }
                    else
                    {
                        strVon = ((DateTime)Von).ToShortDateString();
                        strBis = ((DateTime)Bis).ToShortDateString();
                    }
                }

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_GET_IN_OUT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_UNAME", FilBuUser.UsernameSAP);
                    myProxy.setImportParameter("I_STATUS", TranslateStatus(status));
                    myProxy.setImportParameter("I_VON", strVon);
                    myProxy.setImportParameter("I_BIS", strBis);

                    myProxy.callBapi();

                    DataTable tblEingang = myProxy.getExportTable("GT_IN");
                    DataTable tblAusgang = myProxy.getExportTable("GT_OUT");

                    Protokoll = new Protokoll(ref lstVorgangsarten, ref m_objUser, m_objApp, strAppID, strSessionID, m_strFileName);

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
                                        UserLoggedIn.UsernameSAP, ref m_objUser, m_objApp, strAppID, strSessionID, m_strFileName));
                                }
                            }
                            else
                            {
                                Protokoll.addEntry(Protokoll.Side.Input, new Eingang(dRow["VORGID"].ToString(), dRow["LFDNR"].ToString(), erfDat,
                                    dRow["VON"].ToString(), dRow["VERTR"].ToString(), dRow["BETREFF"].ToString(), dRow["LTXNR"].ToString(),
                                    dRow["ANTW_LFDNR"].ToString(), LogbuchEntry.TranslateEntryStatus(dRow["STATUS"].ToString()),
                                    LogbuchEntry.TranslateEmpfängerStatus(dRow["STATUSE"].ToString()), dRow["VGART"].ToString(), dRow["ZERLDAT"].ToString(),
                                    UserLoggedIn.UsernameSAP, ref m_objUser, m_objApp, strAppID, strSessionID, m_strFileName));
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
                                        dRow["ERLDAT"].ToString(), ref m_objUser, m_objApp, strAppID, strSessionID, m_strFileName));
                                }
                            }
                            else
                            {
                                Protokoll.addEntry(Protokoll.Side.Output, new Ausgang(dRow["VORGID"].ToString(), dRow["LFDNR"].ToString(), erfDat,
                                    dRow["ZAN"].ToString(), dRow["VERTR"].ToString(), dRow["BETREFF"].ToString(), dRow["LTXNR"].ToString(),
                                    dRow["ANTW_LFDNR"].ToString(), LogbuchEntry.TranslateEntryStatus(dRow["STATUS"].ToString()),
                                    LogbuchEntry.TranslateEmpfängerStatus(dRow["STATUSE"].ToString()), dRow["VGART"].ToString(), dRow["ZERLDAT"].ToString(),
                                    dRow["ERLDAT"].ToString(), ref m_objUser, m_objApp, strAppID, strSessionID, m_strFileName));
                            }
                        }
                    }

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
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
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
		/// Erstellt einen neuen Filialbucheintrag
		/// </summary>
		/// <param name="Betreff"></param>
		/// <param name="Text"></param>
		/// <param name="an"></param>
		/// <param name="vorgangsart"></param>
		/// <param name="zuerledigenBis"></param>
		/// <remarks></remarks>
        public void NeuerEintrag(string strAppID, string strSessionID, System.Web.UI.Page page, string Betreff, string Text, string an, string vorgangsart, string zuerledigenBis)
		{
            m_strClassAndMethod = "LogbuchClass.NeuerEintrag";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;

                LongStringToSap lsts = new LongStringToSap(m_objUser, m_objApp, page);
                string ltxnr = "";
                if (Text.Trim().Length != 0)
                {
                    ltxnr = lsts.InsertString(Text, "MC");
                    if (lsts.E_SUBRC != "0")
                    {
                        int lstsStatus = 0;
                        Int32.TryParse(lsts.E_SUBRC, out lstsStatus);
                        m_intStatus = lstsStatus;
                        m_strMessage = lsts.E_MESSAGE;
                        return;
                    }
                }

                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_NEW_VORGANG", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_UNAME", UserLoggedIn.UsernameSAP);
                    myProxy.setImportParameter("I_BD_NR", m_objUser.UserName.ToUpper());
                    myProxy.setImportParameter("I_AN", an);
                    myProxy.setImportParameter("I_LTXNR", ltxnr);
                    myProxy.setImportParameter("I_BETREFF", Betreff);
                    myProxy.setImportParameter("I_VGART", vorgangsart);
                    myProxy.setImportParameter("I_ZERLDAT", zuerledigenBis);
                    myProxy.setImportParameter("I_VKBUR", m_objUser.Kostenstelle);

                    myProxy.callBapi();

                    GetEinträge(strAppID, strSessionID, page, UserLoggedIn, letzterStatus, an, Von, Bis);

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
                        case "NO_DATA":
                            m_intStatus = -5555;
                            m_strMessage = "Keine Daten gefunden.";
                            break;
                        default:
                            m_intStatus = -9999;
                            m_strMessage = m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" + HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) + ")";
                            break;
                    }
                    if (!string.IsNullOrEmpty(ltxnr))
                    {
                        lsts.DeleteString(ltxnr);
                    }
                }
                finally { m_blnGestartet = false; }
            }
		}

		public string GetAntwortToVorgangsart(string vgart)
		{
			VorgangsartDetails Antwort = lstVorgangsarten.Find((VorgangsartDetails vg) =>
			{
				if (vg.Vorgangsart == vgart) {
					return true;
				}
				return false;
			});
			return Antwort.Antwortart.ToString();
		}

        public override void Show()
        {
            throw new System.NotImplementedException();
        }

        public override void Change()
        {
            throw new System.NotImplementedException();
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

