using System;
using System.Web;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Fahrzeugbrief-Datensatz
    /// </summary>
    [GridColumnsAutoPersist]
    public class Fahrzeugbrief : IAddressStreetHouseNo
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equipmentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo_EquiDelivery)]
        public string Vertragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._ZBIINr)]
        public string TechnIdentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants._AbcKennzeichen)]
        public string AbcKennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Room)]
        public string Raum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string StandortText { get { return (IsMissing ? Localize.NoDataFound : (!String.IsNullOrEmpty(AbcKennzeichen) && AbcKennzeichen == "1") ? Localize.TempDispatchedSing : Standort); } }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateTime? Eingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.SentBy)]
        public string VersendetVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDeactivation)]
        public DateTime? Stilllegungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Address)]
        public string Adresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string PLZ { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string HausNr { get { return Hausnummer; } set { Hausnummer = value; } }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string Hausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string StrasseHausNr { get { return AddressService.FormatStreetAndHouseNo(this); } }

        [LocalizedDisplay(LocalizeConstants.PickDate)]
        public DateTime? Pickdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1_EquiDelivery)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2_EquiDelivery)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Memo)]
        public string Memo { get; set; }

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        public string HerstellerSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string FahrzeugHersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.TypeKey)]
        public string TypSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReRegistrationDate)]
        public DateTime? Ummeldedatum { get; set; }

        public bool BriefVersand { get; set; }

        public bool SchluesselVersand { get; set; }


        [LocalizedDisplay(LocalizeConstants.Holder)]
        public string HalterAdresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.Holder)]
        [GridRawHtml]
        public IHtmlString HalterAdresseAsHtml { get { return new HtmlString(HalterAdresse == null ? "" : HalterAdresse.Replace(",", "<br />")); } }


        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        public string VersandAdresse { get; set; }

        [LocalizedDisplay(LocalizeConstants.ShippingAddress)]
        [GridRawHtml]
        public IHtmlString VersandAdresseAsHtml { get { return new HtmlString(VersandAdresse == null ? "" : VersandAdresse.Replace(",", "<br />")); } }


        [ModelMappingCompareIgnore]
        public bool IsSelected { get; set; }

        [ModelMappingCompareIgnore]
        public bool MarkForDelete { get; set; }

        [ModelMappingCompareIgnore]
        public bool IsMissing { get; set; }

        [LocalizedDisplay(LocalizeConstants.ActionDot)]
        public string Action { get; set; }
    }
}
