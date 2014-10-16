using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Finance.Models
{
    public class Mahnung
    {
        [LocalizedDisplay(LocalizeConstants.Account)]
        public string Kontonummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.CIN)]
        public string CIN { get; set; }

        [LocalizedDisplay(LocalizeConstants.PAID)]
        public string PAID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractType)]
        public string Vertragsart { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaterialText)]
        public string Materialtext { get; set; }

        [LocalizedDisplay(LocalizeConstants.Document)]
        public string Dokument { get { return Materialtext; } }

        [LocalizedDisplay(LocalizeConstants.DunningLevel)]
        public string Mahnstufe { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningProcedure)]
        public string Mahnverfahren { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningProcedure)]
        public string MahnverfahrenText { get { return (Mahnverfahren == "BST1" ? Localize.Approved : Localize.ApprovedWithRestrictions); } }

        [LocalizedDisplay(LocalizeConstants.LastDunOn)]
        public DateTime? LetzteMahnungAm { get; set; }

        [LocalizedDisplay(LocalizeConstants.NextDunOn)]
        public DateTime? NaechsteMahnungAm { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlock)]
        public bool Mahnsperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.DunningBlockUntil)]
        public DateTime? MahnsperreBis { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name1)]
        public string Name1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Street)]
        public string Strasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.HouseNo)]
        public string Hausnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.PostCode)]
        public string Postleitzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }
    }
}
