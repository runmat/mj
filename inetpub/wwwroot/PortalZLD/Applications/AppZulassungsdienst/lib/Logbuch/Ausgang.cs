using System;
using SapORM.Models;

namespace AppZulassungsdienst.lib.Logbuch
{
	public class Ausgang : LogbuchEntry
	{
        public string ErledigtAm { get; private set; }

		public Ausgang(string vorgid, string lfdnr, DateTime erfassungszeit, string an, string vertr, string betreff, string ltxnr, string antw_lfdnr,
            EntryStatus objstatus, EmpfängerStatus statuse, string vgart, string zerldat, string erldat)
            : base(vorgid, lfdnr, erfassungszeit, vertr, betreff, ltxnr, antw_lfdnr, objstatus, statuse, vgart, zerldat, an)
		{
            ErledigtAm = erldat;
		}

        public void EintragAbschliessen(EntryStatus status, string userName)
		{
            ExecuteSapZugriff(() =>
                {
                    Z_MC_SAVE_STATUS_OUT.Init(SAP);

                    SAP.SetImportParameter("I_VORGID", VORGID);
                    SAP.SetImportParameter("I_LFDNR", LFDNR);
                    SAP.SetImportParameter("I_BD_NR", userName.ToUpper());
                    SAP.SetImportParameter("I_STATUS", TranslateEntryStatus(status));

                    CallBapi();
                });
		}
	}
}
