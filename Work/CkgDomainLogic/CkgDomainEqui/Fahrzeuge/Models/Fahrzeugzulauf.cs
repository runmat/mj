using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Fahrzeugzulauf
    {
        public DateTime? ZulaufDatumDatum { get; set; }

        public string ZulaufDatumUhrzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.TransitDate)]
        public DateTime? ZulaufDatum
        {
            get
            {
                if (!ZulaufDatumDatum.HasValue || String.IsNullOrEmpty(ZulaufDatumUhrzeit))
                    return ZulaufDatumDatum;

                return ZulaufDatumDatum.Value
                    .AddHours(ZulaufDatumUhrzeit.Substring(0, 2).ToInt(0))
                    .AddMinutes(ZulaufDatumUhrzeit.Substring(2, 2).ToInt(0))
                    .AddSeconds(ZulaufDatumUhrzeit.Substring(4, 2).ToInt(0));
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
