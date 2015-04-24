using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.FzgModelle.ViewModels;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.FzgModelle.Models
{
    [GridColumnsAutoPersist]
    public class ModellId : Store
    {
        [LocalizedDisplay(LocalizeConstants.Model)]
        [Required]
        [Length(20)]
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        [Required]
        [Length(4)]
        public string HerstellerCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string HerstellerName { get; set; }

        [LocalizedDisplay(LocalizeConstants.Description)]
        [Required]
        [Length(40)]
        public string Bezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        [Required]
        [Length(4)]
        public string SippCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.UserName)]
        [Length(12)]
        public string UserName { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityDays)]
        [Length(4)]
        public int LaufzeitTage { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityBinding)]
        public bool LaufzeitBindung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Truck)]
        public bool Lkw { get; set; }

        [LocalizedDisplay(LocalizeConstants.WinterTires)]
        public bool Winterreifen { get; set; }

        [LocalizedDisplay(LocalizeConstants.Towbar)]
        public bool AnhaengerKupplung { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviAvailable)]
        public bool NaviVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.SecurityFleet)]
        public bool SecurityFleet { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateLeaseCar)]
        public bool KennzeichenLeasingFahrzeug { get; set; }

        [GridHidden, GridExportIgnore]
        public string Antrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.EngineType)]
        public string AntriebAsText
        {
            get
            {
                return (HerstellerList.FirstOrDefault(h => h.Code == HerstellerCode) ?? new Hersteller {Name = ""}).Name;
            }
        }

        [XmlIgnore, GridHidden, NotMapped]
        public List<SelectItem> AntriebeList { get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().AntriebeList; } }

        [LocalizedDisplay(LocalizeConstants.Bluetooth)]
        public bool Bluetooth { get; set; }


        [GridHidden, NotMapped]
        public bool InsertModeTmp { get; set; }

        public ModellId SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }

        [XmlIgnore, GridHidden, NotMapped]
        public List<Hersteller> HerstellerList
        {
            get { return GetViewModel == null ? new List<Hersteller>() : GetViewModel().HerstellerList; }
        }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<ModellIdViewModel> GetViewModel { get; set; }


        [XmlIgnore, NotMapped, GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }
    }
}
