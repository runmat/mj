using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class Pruefschritt
    {
        [LocalizedDisplay(LocalizeConstants.AccountNo)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        public string BucID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionNo)]
        public string Aktionsnummer { get; set; }

        public string Aktionsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionName)]
        public string Aktionsname { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionText)]
        public string Aktionstext { get; set; }

        [LocalizedDisplay(LocalizeConstants.InfoText)]
        public string Infotext { get; set; }

        [LocalizedDisplay(LocalizeConstants.Done)]
        public bool Erledigt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Checkdate)]
        public DateTime? Pruefdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.WebUser)]
        public string Webuser { get; set; }
    }
}
