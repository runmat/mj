using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CkgDomainLogic.Fahrzeugbestand.ViewModels;
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

        [NotMapped]
        public static Func<FahrzeugbestandViewModel> GetViewModel { get; set; }


        #region Fahrzeug Akte

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        [Length(5)]
        [Required]
        public string HerstellerSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.TypeKey)]
        [Length(3)]
        [Required]
        public string TypSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.VvsKey)]
        [Length(5)]
        [Required]
        public string VvsSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.VvsCheckDigit)]
        [Length(1)]
        public string VvsPruefZiffer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryName)]
        [Length(25)]
        public string FabrikName { get; set; }

        [LocalizedDisplay(LocalizeConstants.TradeName)]
        [Length(25)]
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
