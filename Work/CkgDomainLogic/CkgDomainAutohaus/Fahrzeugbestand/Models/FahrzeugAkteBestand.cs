using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeugbestand.Models
{
    public class FahrzeugAkteBestand
    {
        private string _fin;

        [LocalizedDisplay(LocalizeConstants.VIN)]
        [Required]
        public string FIN
        {
            get { return _fin.NotNullOrEmpty().ToUpper(); }
            set { _fin = value.NotNullOrEmpty().ToUpper(); }
        }

        [LocalizedDisplay(LocalizeConstants.VinID)]
        public string FinID { get; set; }


        #region Fahrzeug Akte

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        [MaxLength(5)]
        [Required]
        public string HerstellerSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.TypeKey)]
        [MaxLength(3)]
        [Required]
        public string TypSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.VvsKey)]
        [MaxLength(5)]
        [Required]
        public string VvsSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.VvsCheckDigit)]
        [MaxLength(1)]
        public string VvsPruefZiffer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryName)]
        [MaxLength(25)]
        public string FabrikName { get; set; }

        [LocalizedDisplay(LocalizeConstants.TradeName)]
        [MaxLength(25)]
        public string HandelsName { get; set; }

        #endregion



        #region Fahrzeug Bestand

        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string Halter { get; set; }

        [LocalizedDisplay(LocalizeConstants.Buyer)]
        public string Kaeufer { get; set; }

        #endregion

    }
}
