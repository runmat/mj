using System;
using KBSBase;

namespace TimeRegistration
{
    public class TimeRegUser : ErrorHandlingClass
    {
        private string sKarNum = "";
        private string sUsername = "";

        #region Properties

        public string Kartennummer
        {
            get{ return sKarNum; }
        }

        public string Username
        {
            get { return sUsername; }
        }

        #endregion

        public TimeRegUser(string Kartennummer)
        {
            sKarNum = Kartennummer;
            sUsername = GetUsernameToKarNum();
        }

        /// <summary>
        /// Liefert den Benutzernamen zur Kartennummer
        /// </summary>
        /// <returns>Benutzername</returns>
        private string GetUsernameToKarNum()
        {
            ClearErrorState();

            sUsername = "";

            try
            {
                S.AP.Init("Z_HR_ZE_GET_POSTINGS_OF_PERIOD", "BD_NR", sKarNum);

                S.AP.SetImportParameter("VDATE", DateTime.Today.ToShortDateString());
                S.AP.SetImportParameter("BDATE", DateTime.Today.ToShortDateString());
                S.AP.SetImportParameter("MODUS", TimeRegistrator.TranslateMode(TimeRegistrator.TimeMode.Zeiterfassung).ToString());

                S.AP.Execute();

                if (S.AP.ResultCode == 0)
                {
                    sUsername = S.AP.GetExportParameter("NAME");
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

            return sUsername;
        }
    }
}
