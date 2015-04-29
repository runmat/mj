using System;
using SapORM.Models;

namespace AppZulassungsdienst.lib.Logbuch
{
	public class Eingang : LogbuchEntry
	{
        public string Verfasser { get; private set; }

        public Eingang(string vorgid, string lfdnr, DateTime erfassungszeit, string von, string vertr, string betreff, string ltxnr, string antw_lfdnr,
            EntryStatus objstatus, EmpfängerStatus statuse, string vgart, string zerldat, string an)
            : base(vorgid, lfdnr, erfassungszeit, vertr, betreff, ltxnr, antw_lfdnr, objstatus, statuse, vgart, zerldat, an)
		{
            Verfasser = von;
		}

        public void EintragBeantworten(string betr, string Text, string userName)
		{
            LongStringToSap lsts = new LongStringToSap();
            var ltxnr = "";
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
                    Z_MC_SAVE_ANSWER.Init(SAP);

                    SAP.SetImportParameter("I_VORGID", VORGID);
                    SAP.SetImportParameter("I_LFDNR", LFDNR);
                    SAP.SetImportParameter("I_AN", Verfasser);
                    SAP.SetImportParameter("I_VON", Empfänger);
                    SAP.SetImportParameter("I_BD_NR", userName.ToUpper());
                    SAP.SetImportParameter("I_LTXNR", ltxnr);

                    CallBapi();
                });

            if (ErrorOccured && !String.IsNullOrEmpty(ltxnr))
            {
                lsts.DeleteString(ltxnr);
            }
		}

        public void Rückfrage(string betr, string Text, string userName, string kostenstelle)
		{
            LongStringToSap lsts = new LongStringToSap();
            var ltxnr = "";
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

                    SAP.SetImportParameter("I_UNAME", AN);
                    SAP.SetImportParameter("I_BD_NR", userName.ToUpper());
                    SAP.SetImportParameter("I_AN", Verfasser);
                    SAP.SetImportParameter("I_LTXNR", ltxnr);
                    SAP.SetImportParameter("I_BETREFF", betr);
                    SAP.SetImportParameter("I_VGART", "FILL");
                    SAP.SetImportParameter("I_ZERLDAT", "");
                    SAP.SetImportParameter("I_VKBUR", kostenstelle);

                    CallBapi();
                });

            if (ErrorOccured && !String.IsNullOrEmpty(ltxnr))
            {
                lsts.DeleteString(ltxnr);
            }
		}

        public void EintragBeantworten(EmpfängerStatus status, string userName)
		{
            ExecuteSapZugriff(() =>
                {
                    Z_MC_SAVE_STATUS_IN.Init(SAP);

                    SAP.SetImportParameter("I_VORGID", VORGID);
                    SAP.SetImportParameter("I_LFDNR", LFDNR);
                    SAP.SetImportParameter("I_AN", Empfänger);
                    SAP.SetImportParameter("I_BD_NR", userName.ToUpper());
                    SAP.SetImportParameter("I_STATUSE", TranslateEmpfängerStatus(status));

                    CallBapi();
                });
		}
	}
}
