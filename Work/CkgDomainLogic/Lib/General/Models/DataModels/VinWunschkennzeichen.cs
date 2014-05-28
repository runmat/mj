using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.General.Models
{
    public class VinWunschkennzeichen
    {
        public string UniqueKey { get { return new[] { "FIN ".AppendIfNotNull(VIN), "ZBII ".AppendIfNotNull(ZBII), "Ref.Nr. ".AppendIfNotNull(AuftragsReferenz) }.JoinIfNotNull(", "); } }

        
        [LocalizedDisplay(LocalizeConstants.VIN)]
        [VIN]
        public string VIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2)]
        public string ZBII { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        public string AuftragsReferenz { get; set; }


        public string ZulassungsKreis
        {
            get { return _zulassungsKreis; }
            set
            {
                _zulassungsKreis = value;
                UpdateAlleKennzeichen(_zulassungsKreis);
            }
        }


        private string _kennzeichen1;
        [LocalizedDisplay(LocalizeConstants.LicenseNoShort, 1)]
        [Kennzeichen]
        public string Kennzeichen1
        {
            get { return _kennzeichen1; }
            set { _kennzeichen1 = value.NotNullOrEmpty().ToUpper(); }
        }

        private string _kennzeichen2;
        [LocalizedDisplay(LocalizeConstants.LicenseNoShort, 2)]
        [Kennzeichen]
        public string Kennzeichen2
        {
            get { return _kennzeichen2; }
            set { _kennzeichen2 = value.NotNullOrEmpty().ToUpper(); }
        }

        private string _kennzeichen3;
        [LocalizedDisplay(LocalizeConstants.LicenseNoShort, 3)]
        [Kennzeichen]
        public string Kennzeichen3
        {
            get { return _kennzeichen3; }
            set { _kennzeichen3 = value.NotNullOrEmpty().ToUpper(); }
        }

        private string _kennzeichenReserviert;
        private string _zulassungsKreis;

        [LocalizedDisplay(LocalizeConstants.LicenseNoReservedShort)]
        [Kennzeichen]
        public string KennzeichenReserviert
        {
            get { return _kennzeichenReserviert; }
            set { _kennzeichenReserviert = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.ReservationNoShort)]
        public string ReservierungNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReservationNameShort)]
        public string ReservierungName { get; set; }

        [LocalizedDisplay(LocalizeConstants._blank)]
        public string OderSeparator { get { return Localize.Or; } }

        [ModelMappingCompareIgnore]
        public bool IsValid { get; set; }


        public string WunschKennzeichenAsString
        {
            get
            {
                return new []
                    {
                        "Kennz. 1 ".AppendIfNotNull(FormatKennzeichen(Kennzeichen1)), 
                        "Kennz. 2 ".AppendIfNotNull(FormatKennzeichen(Kennzeichen2)), 
                        "Kennz. 3 ".AppendIfNotNull(FormatKennzeichen(Kennzeichen3)), 
                        "Res. Kennz. ".AppendIfNotNull(FormatKennzeichen(KennzeichenReserviert)), 
                        "Res. Nr. ".AppendIfNotNull(ReservierungNr), 
                        "Res. Name ".AppendIfNotNull(ReservierungName)
                    }.JoinIfNotNull("<br />");
            }
        }

        static string FormatKennzeichen(string kennzeichen)
        {
            if (kennzeichen.IsNullOrEmpty())
                return "";

            if (kennzeichen[kennzeichen.Length - 1] == '-')
                return "";

            return kennzeichen;
        }

        private void UpdateAlleKennzeichen(string kreis)
        {
            Kennzeichen1 = UpdateKennzeichen(Kennzeichen1, kreis);
            Kennzeichen2 = UpdateKennzeichen(Kennzeichen2, kreis);
            Kennzeichen3 = UpdateKennzeichen(Kennzeichen3, kreis);
            
            KennzeichenReserviert = UpdateKennzeichen(KennzeichenReserviert, kreis);
        }

        private static string UpdateKennzeichen(string kennzeichen, string kreis)
        {
            if (kennzeichen.IsNotNullOrEmpty() && kennzeichen[kennzeichen.Length-1] != '-')
                return kennzeichen;

            return string.Format("{0}-", kreis);
        }
    }
}
