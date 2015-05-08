﻿using System;
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
using CkgDomainLogic.Fahrzeuge.Models;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class Batcherfassung : Store
    {
        // TODO -> entf. falls n.n
        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string HerstellerCode { get; set; }

        public List<SelectItem> HerstellerList { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        [Required]
        public string HerstellerName { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.BatchID)]
        [Required]
        [Length(8)]
        public string ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        [Required]
        [Length(40)]
        public string ModellId { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        [Required]
        [Length(40)]
        public string Modellbezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        [Required]
        [Length(4)]
        public string SippCode { get; set; }
       
        [LocalizedDisplay(LocalizeConstants.UnitnumberFrom)]
        [Required]
        [Length(8)]
        public string UnitnummerVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitnumberUntil)]
        [Required]
        [Length(8)]
        public string UnitnummerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Unitnumbers)]
        public string Unitnummern { get; set; }

        [LocalizedDisplay(LocalizeConstants.Quantity)]
        [Required]
        [Length(5)]
        public string Anzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        [Required]
        [Length(8)]
        public string LiefermonatBAPIFormat { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryMonth)]
        public string Liefermonat { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryYear)]
        public string Lieferjahr { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityDays)]
        [Required]
        [Length(4)]
        public string Laufzeit { get; set; }

        [LocalizedDisplay(LocalizeConstants.PeriodOfValidityBinding)]
        public bool Laufzeitbindung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumberFrom)]
        [Length(20)]
        public string AuftragsnummerVon { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumberUntil)]
        [Length(20)]
        public string AuftragsnummerBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        public string Fahrzeuggruppe { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }
       
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

        [XmlIgnore, GridHidden, NotMapped]
        public List<SelectItem> AntriebeList { get { return GetViewModel == null ? new List<SelectItem>() : GetViewModel().AntriebeList; } }

        [LocalizedDisplay(LocalizeConstants.Bluetooth)]
        public bool Bluetooth { get; set; }


        [GridHidden, NotMapped]
        public bool InsertModeTmp { get; set; }

        public Batcherfassung SetInsertMode(bool insertMode)
        {
            InsertModeTmp = insertMode;
            return this;
        }



        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<BatcherfassungViewModel> GetViewModel { get; set; }


        [XmlIgnore, NotMapped, GridExportIgnore]
        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }
    }
}
