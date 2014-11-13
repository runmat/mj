using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHaendlerdaten
    {
        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Name3 { get; set; }

        public string Strasse { get; set; }

        public string Hausnummer { get; set; }

        public string Plz { get; set; }

        public string Ort { get; set; }

        public string Land { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerAddress)]
        public string HaendlerAdresse
        {
            get
            {
                return String.Format("{0} {1} {2}" + (String.IsNullOrEmpty(Strasse) && String.IsNullOrEmpty(Hausnummer) ? "" : Environment.NewLine)
                    + "{3} {4}" + (String.IsNullOrEmpty(Plz) && String.IsNullOrEmpty(Ort) ? "" : Environment.NewLine)
                    + "{5} {6}" + (String.IsNullOrEmpty(Land) ? "" : Environment.NewLine)
                    + "{7}",
                    Name1, Name2, Name3,
                    Strasse, Hausnummer,
                    Plz, Ort,
                    Land);
            }
        }

        [LocalizedDisplay(LocalizeConstants.FinancingType)]
        public string Finanzierungsart { get; set; }

        public string GetSummaryString()
        {
            var strText = String.Format("{0}: {1}", Localize.DealerNo, HaendlerNr);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.DealerAddress, HaendlerAdresse);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.FinancingType, Finanzierungsart);

            return strText;
        }
    }
}
