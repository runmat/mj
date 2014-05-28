using CKG.Base.Business;
using System;
using CKG.Base.Common;

namespace AppZulassungsdienst.lib.Logbuch
{
    public enum EntryStatus
    {
        Neu,
        Gesendet,
        Gelöscht,
        Geschlossen,
        Ausblenden
        //Wird in der Funktion Protokoll.CreateTable() genutzt um Datensätze auszublenden
    }

    public enum EmpfängerStatus
    {
        Neu,
        Gelesen,
        Gelöscht,
        Geantwortet,
        Erledigt,
        AutomatischBeantwortet,
        Ausblenden
        //Wird in der Funktion Protokoll.CreateTable() genutzt um Datensätze auszublenden
    }

	public interface ILogbuchEntry
	{
		string VorgangsID { get; }
		string LaufendeNummer { get; }
        DateTime Erfasst { get; }
		string Vertreter { get; }
		string Betreff { get; }
		string Langtextnummer { get; }
		string AntwortAufLaufendenummer { get; }
		EntryStatus Status { get; }
		EmpfängerStatus StatusEmpfänger { get; }
		string Vorgangsart { get; }
		string ZuErledigenBis { get; }
		string Empfänger { get; }
        int IntStatus { get; }
        string Message { get; }
	}
    
    public class LogbuchEntry : BankBase, ILogbuchEntry
	{

		protected string VORGID;
		protected string LFDNR;
		protected DateTime ERDATZEIT;
		protected string VERTR;
		protected string strBETREFF;
		protected string LTXNR;
		protected string ANTW_LFDNR;
		protected EntryStatus objSTATUS;
		protected EmpfängerStatus STATUSE;
		protected string VGART;
		protected string ZERLDAT;

		protected string AN;

		#region "Properties"

		public string VorgangsID {
			get { return VORGID; }
		}

		public string LaufendeNummer {
			get { return LFDNR; }
		}

        public DateTime Erfasst {
            get { return ERDATZEIT; }
        }

		public string Vertreter {
			get { return VERTR; }
		}

		public string Betreff {
			get { return strBETREFF; }
		}

		public string Langtextnummer {
			get { return LTXNR; }
		}

		public string AntwortAufLaufendenummer {
			get { return ANTW_LFDNR; }
		}

		public EntryStatus Status {
			get { return objSTATUS; }
		}

		public EmpfängerStatus StatusEmpfänger {
			get { return STATUSE; }
		}

		public string Vorgangsart {
			get { return VGART; }
		}

		public string ZuErledigenBis {
			get { return ZERLDAT; }
		}

		public string Empfänger {
			get { return AN; }
		}

        public int IntStatus {
            get { return m_intStatus; }
        }

        public string Message {
            get { return m_strMessage; }
        }

		#endregion

		#region "Methods and Functions"

		public LogbuchEntry(string vorgid, string lfdnr, DateTime erfassungszeit, string vertr, string betreff, string ltxnr, string antw_lfdnr,
            EntryStatus objstatus, EmpfängerStatus statuse, string vgart, string zerldat, string an, ref CKG.Base.Kernel.Security.User objUser, 
            CKG.Base.Kernel.Security.App objApp, string strAppID, string strSessionID, string strFilename)
            : base(ref objUser, ref objApp, strAppID, strSessionID, strFilename)
		{
			this.VORGID = vorgid;
			this.LFDNR = lfdnr;
		    this.ERDATZEIT = erfassungszeit;
			this.VERTR = vertr;
			this.strBETREFF = betreff;
			this.LTXNR = ltxnr;
			this.ANTW_LFDNR = antw_lfdnr;
			this.objSTATUS = objstatus;
			this.STATUSE = statuse;
			this.VGART = vgart;
			this.ZERLDAT = zerldat;
			this.AN = an;
		}

		public static string TranslateEntryStatus(EntryStatus status)
		{
			switch (status) {
				case EntryStatus.Neu:
					return "NEW";
				case EntryStatus.Gelöscht:
					return "LOE";
				case EntryStatus.Geschlossen:
					return "CLOSE";
				case EntryStatus.Gesendet:
					return "SEND";
				default:
					return "";
			}
		}

		public static EntryStatus TranslateEntryStatus(string status)
		{
			switch (status.ToUpper()) {
				case "NEW":
					return EntryStatus.Neu;
				case "LOE":
					return EntryStatus.Gelöscht;
				case "CLOSE":
					return EntryStatus.Geschlossen;
				case "SEND":
					return EntryStatus.Gesendet;
				default:
					return EntryStatus.Ausblenden;
			}
		}

		public static string TranslateEmpfängerStatus(EmpfängerStatus status)
		{
			switch (status) {
				case EmpfängerStatus.Neu:
					return "NEW";
				case EmpfängerStatus.Gelöscht:
					return "LOE";
				case EmpfängerStatus.Gelesen:
					return "READ";
				case EmpfängerStatus.Geantwortet:
					return "ANTW";
				case EmpfängerStatus.Erledigt:
					return "ERL";
				case EmpfängerStatus.AutomatischBeantwortet:
					return "AUT";
				default:
					return "";
			}
		}

		public static EmpfängerStatus TranslateEmpfängerStatus(string status)
		{
			switch (status.ToUpper()) {
				case "NEW":
					return EmpfängerStatus.Neu;
				case "LOE":
					return EmpfängerStatus.Gelöscht;
				case "READ":
					return EmpfängerStatus.Gelesen;
				case "ANTW":
					return EmpfängerStatus.Geantwortet;
				case "ERL":
					return EmpfängerStatus.Erledigt;
				case "AUT":
					return EmpfängerStatus.AutomatischBeantwortet;
				default:
					return EmpfängerStatus.Ausblenden;
			}
		}

        public void EintragStatusÄndern(string strAppID, string strSessionID, System.Web.UI.Page page)
		{
            m_strClassAndMethod = "LogbuchEntry.EintragStatusÄndern";
            m_strAppID = strAppID;
            m_strSessionID = strSessionID;
            m_intStatus = 0;
            m_strMessage = String.Empty;

            if (m_blnGestartet == false)
            {
                m_blnGestartet = true;
                try
                {
                    DynSapProxyObj myProxy = DynSapProxy.getProxy("Z_MC_SAVE_STATUS_OUT", ref m_objApp, ref m_objUser, ref page);

                    myProxy.setImportParameter("I_VORGID", VORGID);
                    myProxy.setImportParameter("I_LFDNR", LFDNR);
                    myProxy.setImportParameter("I_BD_NR", m_objUser.UserName.ToUpper());
                    myProxy.setImportParameter("I_STATUS", LogbuchEntry.TranslateEntryStatus(Status));

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

        public override void Show()
        {
            throw new System.NotImplementedException();
        }

        public override void Change()
        {
            throw new System.NotImplementedException();
        }

		#endregion

	}

}
