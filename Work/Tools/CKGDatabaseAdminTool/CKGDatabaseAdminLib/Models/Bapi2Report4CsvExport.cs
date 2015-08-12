using System;
using GeneralTools.Models;

namespace CKGDatabaseAdminLib.Models
{
    public class Bapi2Report4CsvExport
    {
        public string KUNNR { get; set; }

        public string Customername { get; set; }

        public string AppFriendlyName { get; set; }

        public string Bezeichner { get { return String.Format("{0}-{1}-{2}", KUNNR, Customername, AppFriendlyName); } }

        public string UniqueGeckoKey { get { return String.Format("{0}-{1}-{2}", KUNNR, AppFriendlyName, BAPI); } }

        public string AppName { get; set; }

        public string AppURL { get; set; }

        public string BAPI { get; set; }

        public Bapi2Report4CsvExport()
        {         
        }

        public Bapi2Report4CsvExport(string itemAsString, string trennzeichen)
        {
            var parts = itemAsString.NotNullOrEmpty().Split(new[] { trennzeichen }, StringSplitOptions.None);
            if (parts.Length > 6)
            {
                KUNNR = parts[1];
                Customername = parts[2];
                AppFriendlyName = parts[3];
                AppName = parts[4];
                AppURL = parts[5];
                BAPI = parts[6];
            }
        }

        public string ToString(string trennzeichen)
        {
            return String.Join(trennzeichen, Bezeichner, KUNNR, Customername, AppFriendlyName, AppName, AppURL, BAPI);
        }
    }
}
