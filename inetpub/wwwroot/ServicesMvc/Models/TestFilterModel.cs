using System;
using System.Collections.Generic;
using GeneralTools.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace ServicesMvc.Models
{
    public class TestFilterModel
    {
        [LocalizedDisplay(LocalizeConstants.DepartmentHeadChristName)]
        public string Vorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadChristName)]
        public string Vorname2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadLastName)]
        public string Nachname { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadLastName)]
        public string Nachname2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateTime LieferDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateTime LieferDatum2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.PrintCoC)]
        public bool CocDrucken { get; set; }

        [LocalizedDisplay(LocalizeConstants.PrintZBII)]
        public bool Zb2Drucken { get; set; }


        
        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        public DateTime RechnungsDatum { get; set; }

        public List<string> Betriebe { get { return new List<string> { "DAD", "CKG", "Termani" }; } }

        public List<string> Standorte { get { return new List<string> { "Berlin", "Frankfurt", "Hamburg", "Hannover", "München" }; } }


        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string FilterBetrieb { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string[] FilterStandorte { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateRange FilterRechnungsDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        public DateRange FilterLieferDatum { get; set; }

        
        public bool IsValid { get; set; }
    }
}