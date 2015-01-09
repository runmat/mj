using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    public class Kunde
    {
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        public string VkOrg { get; set; }

        public string VkBur { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string HausNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Plz { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [SelectListText]
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string KundenNameNr
        {
            get
            {
                return String.Format("{0}{1}{2} ~ {3}", 
                    Name1, 
                    (String.IsNullOrEmpty(Name2) ? "" : ", " + Name2), 
                    (String.IsNullOrEmpty(Ort) ? "" : ", " + Ort), 
                    KundenNr.NotNullOrEmpty().TrimStart('0'));
            }
        }

        public bool Pauschalkunde { get; set; }

        public bool OhneUmsatzsteuer { get; set; }

        public bool Cpdkunde { get; set; }

        public bool CpdMitEinzugsermaechtigung { get; set; }

        public bool Barkunde { get; set; }
    }
}
