using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;
using System.Linq;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class FahrzeuguebersichtSelektor : Store
    {
        
        [LocalizedDisplay(LocalizeConstants.Action)]
        [FormPersistable]
        public string Akion { get; set; }

        public string Akionen { get { return string.Format("manuell,{0};upload,{1}", Localize.ManuelInput, Localize.FileUpload); } }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nummer { get; set; }
     
        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModelID { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string Unitnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string Auftragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.BatchID)]
        public string BatchId { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        public string SIPPCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        [FormPersistable]
        public DateRange EingangZb2DatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DateOfVehicleArrival)]
        [FormPersistable]
        public DateRange EingangFahrzeugDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.DateOfReadyIndication)]
        [FormPersistable]
        public DateRange BereitmeldungDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        [FormPersistable]
        public DateRange ZulassungDatumRange { get { return PropertyCacheGet(() => new DateRange(DateRangeType.Last30Days)); } set { PropertyCacheSet(value); } }



        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Herstellerkennung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Statuskennung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string PDIkennung { get; set; }


        public static List<SelectItem> FahrzeugHersteller
        {
            get
            {
                var hersteller = GetViewModel().FahrzeugHersteller;
                return hersteller.ConvertAll(new Converter<Fahrzeughersteller, SelectItem>(WrapManufacturer));              
            }
        }

        public static List<SelectItem> FahrzeugStatus
        {
            get
            {
                var status = GetViewModel().FahrzeugStatus;
                return status.ConvertAll(new Converter<FahrzeuguebersichtStatus, SelectItem>(WrapStatus));
            }
        }

        public static List<SelectItem> PDIStandorte
        {
            get
            {
                var pdi = GetViewModel().PDIStandorte;
                return pdi.ConvertAll(new Converter<FahrzeuguebersichtPDI, SelectItem>(WrapPDI));
            }
        }

        static SelectItem WrapManufacturer(Fahrzeughersteller hersteller)
        {
            if (hersteller.HerstellerName.StartsWith("(")) // wg. empty keys aus sap
                return new SelectItem(String.Empty, hersteller.HerstellerName);
            else            
                return new SelectItem(hersteller.HerstellerName, hersteller.HerstellerName);
        }

        static SelectItem WrapStatus(FahrzeuguebersichtStatus status)
        {            
            return new SelectItem(status.StatusKey, status.StatusText);
        }

        static SelectItem WrapPDI(FahrzeuguebersichtPDI pdi)
        {            
            return new SelectItem(pdi.PDIKey, pdi.PDIText);
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<FahrzeuguebersichtViewModel> GetViewModel { get; set; }

    }
}
