using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Remarketing.Models
{
    public class Gutachten
    {
        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string FahrgestellNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.SequenceNo)]
        public string LfdNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReceipt)]
        public DateTime? Eingangsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.Surveyor)]
        public string Gutachter { get; set; }

        [LocalizedDisplay(LocalizeConstants.SurveyId)]
        public string GutachtenId { get; set; }

        [LocalizedDisplay(LocalizeConstants.SurveyDate)]
        public DateTime? Gutachtendatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageSign)]
        public string Schadenskennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageAmount)]
        public decimal? Schadensbetrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamageAmountAv)]
        public decimal? SchadensbetragAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.RepairSign)]
        public string Reparaturkennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ConditioningAmount)]
        public decimal? Aufbereitungsbetrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.ConditioningAmountAv)]
        public decimal? AufbereitungsbetragAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.ImpairmentAmount)]
        public decimal? Wertminderungsbetrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.ImpairmentAmountAv)]
        public decimal? WertminderungsbetragAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.MissingPartsAmount)]
        public decimal? Fehlteilbetrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.MissingPartsAmountAv)]
        public decimal? FehlteilbetragAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.ResidualValue)]
        public decimal? Restwert { get; set; }

        [LocalizedDisplay(LocalizeConstants.DefectValueAv)]
        public decimal? MaengelwertAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.OptValueAv)]
        public decimal? OptWertAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.DeprecationAv)]
        public decimal? MinderwertAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamagesAv)]
        public decimal? BeschaedigungenAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.PreviousDamagesRepairedAv)]
        public decimal? VorschaedenRepariertAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamagesNotRepairedAv)]
        public decimal? SchaedenUnrepariertAv { get; set; }

        [LocalizedDisplay(LocalizeConstants.PreviousDamagesImpairment)]
        public decimal? VorschaedenWertminderung { get; set; }

        [LocalizedDisplay(LocalizeConstants.DamagesNotRepairedImpairment)]
        public decimal? SchaedenUnrepariertWertminderung { get; set; }
    }
}
