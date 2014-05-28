using System;
using System.Data;

namespace TimeRegistration
{
    public class TimeRegUser
    {
        private string sKarNum = "";
        private string sUsername = "";
        private string sSAPCon = "";
        private string E_MESSAGE = "";
        private string E_SUBRC = "0";
        private bool bError = false;

        #region Properties

        public  string Kartennummer
        {
            get{return sKarNum;}
        }

        public string Username
        {
            get { return sUsername; }
        }

        public string SAPConnectionString
        {
            get { return sSAPCon; }
        }

        public string ErrorMessage
        {
            get { return E_MESSAGE; }
        }

        public string ErrorCode
        {
            get { return E_SUBRC; }
        }

        public bool ErrorOccured
        {
            get { return bError; }
        }

#endregion

        public TimeRegUser(string Kartennummer,string SAPConnectionString)
        {
            sKarNum = Kartennummer;
            sSAPCon = SAPConnectionString;
            GetUsernameToKarNum();
        }

        /// <summary>
        /// Liefert den Benutzernamen zur Kartennummer
        /// </summary>
        /// <returns>Benutzername</returns>
        private string GetUsernameToKarNum()
        {
            ResetError();

            sUsername = "";
            SAPExecutor.SAPExecutor SAPExc = new SAPExecutor.SAPExecutor(sSAPCon);

            // Tabellenschema {Feldname, ParameterDirection(0=Input,1=Output), (optional) Feldinhalt als Object}
            DataTable dt = SAPExecutor.SAPExecutor.getSAPExecutorTable();
            dt.Rows.Add(new object[] { "BD_NR",false,sKarNum,4 });
            dt.Rows.Add(new object[] { "VDATE", false, DateTime.Today.ToShortDateString() });
            dt.Rows.Add(new object[] { "BDATE", false, DateTime.Today.ToShortDateString() });
            dt.Rows.Add(new object[] { "MODUS", false, TimeRegistrator.TranslateMode(TimeRegistrator.TimeMode.Zeiterfassung).ToString() });
            dt.Rows.Add(new object[] { "NAME", true });
            dt.Rows.Add(new object[] { "OFFEN", true });
            
            SAPExc.ExecuteERP("Z_HR_ZE_GET_POSTINGS_OF_PERIOD",ref dt);

            if (!SAPExc.ErrorOccured)
            {
                sUsername = dt.Rows[4]["Data"].ToString().TrimEnd(' ');
            }
            else 
            {
                bError = true;
                E_SUBRC = SAPExc.E_SUBRC;
                E_MESSAGE = SAPExc.E_MESSAGE;               
            }

            return sUsername;
        }

        /// <summary>
        /// Setzt den Fehlerstatus zurück
        /// </summary>
        private void ResetError()
        {
            bError = false;
            E_SUBRC = "0";
            E_MESSAGE = string.Empty;            
        }
    }
}
