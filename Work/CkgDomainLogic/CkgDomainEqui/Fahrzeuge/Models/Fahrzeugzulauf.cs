using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Fahrzeugzulauf
    {
        [LocalizedDisplay(LocalizeConstants.TransitDate)]
        public DateTime? ZulaufDatumDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string ZulaufDatumUhrzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.Time)]
        public string ZulaufDatumUhrzeitFormatted
        {
            get
            {
                if (String.IsNullOrEmpty(ZulaufDatumUhrzeit) || ZulaufDatumUhrzeit.Length < 6)
                    return ZulaufDatumUhrzeit;

                return String.Format("{0}:{1}:{2}", ZulaufDatumUhrzeit.Substring(0, 2), ZulaufDatumUhrzeit.Substring(2, 2), ZulaufDatumUhrzeit.Substring(4, 2));
            }
        }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModellId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string Modell { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string UnitNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.CheckDigit)]
        public string UnitNrPruefziffer { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string UnitNrMitPruefziffer { get { return String.Format("{0}{1}", UnitNr, UnitNrPruefziffer); } }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        public DateTime? EingangZb2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.PdiReceipt)]
        public DateTime? EingangPdi { get; set; }
    }
}
