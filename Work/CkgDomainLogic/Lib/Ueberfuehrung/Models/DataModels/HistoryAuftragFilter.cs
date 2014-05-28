using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public class HistoryAuftragFilter : UiModel 
    {
        // ReSharper disable LocalizableElement

        [LocalizedDisplay(LocalizeConstants.OrderID)]
        [RequiredAsGroup]
        public string AuftragsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants._ErfassungsdatumVon)]
        [RequiredAsGroup]
        public DateTime? ErfassungsDatumVon { get; set; }

        [LocalizedDisplay(LocalizeConstants._Bis)]
        [RequiredAsGroup]
        public DateTime? ErfassungsDatumBis { get; set; }

        [LocalizedDisplay(LocalizeConstants._AuftragsdatumVon)]
        [RequiredAsGroup]
        public DateTime? UeberfuehrungsDatumVon { get; set; }

        [LocalizedDisplay(LocalizeConstants._Bis)]
        [RequiredAsGroup]
        public DateTime? UeberfuehrungsDatumBis { get; set; }

        [RequiredAsGroup]
        public string Referenz { get; set; }

        [LocalizedDisplay(LocalizeConstants._Auftragsart)]
        public string AuftragsArt { get; set; }
        
        [LocalizedDisplay(LocalizeConstants._AlleOrganisationen)]
        public bool AlleOrganisationen { get; set; }

        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get { return SelectedAuftragGeberAdresse.KundenNr; } }

        [LocalizedDisplay(LocalizeConstants._KundenReferenz)]
        public string KundenReferenz { get; set; }


        public Adresse SelectedAuftragGeberAdresse { get { return AuftragGeberAdressen.FirstOrDefault(a => a.ID == SelectedAuftragGeber); } }

        [LocalizedDisplay(LocalizeConstants.Client)]
        public int SelectedAuftragGeber { get; set; }

        [XmlIgnore]
        static public List<Adresse> AuftragGeberAdressen { get; set; }


        // ReSharper restore LocalizableElement


        public HistoryAuftragFilter()
        {
            UiIndex = 1;
            ValidationFormHeaderCheckStyleDisabled = true;
        }

        public void Validate(Action<Expression<Func<HistoryAuftragFilter, object>>> addModelError)
        {
            ValidateDateRange("Erfassungsdatum", ErfassungsDatumVon, ErfassungsDatumBis, m => m.ErfassungsDatumVon, m => m.ErfassungsDatumBis, addModelError);
            ValidateDateRange("Auftragsdatum", UeberfuehrungsDatumVon, UeberfuehrungsDatumBis, m => m.UeberfuehrungsDatumVon, m => m.UeberfuehrungsDatumBis, addModelError);

            ValidateAuftragGeber(addModelError);
        }

        void ValidateAuftragGeber(Action<Expression<Func<HistoryAuftragFilter, object>>> addModelError)
        {
            var adresse = SelectedAuftragGeberAdresse;

            if (adresse == null || adresse.IsEmpty)
            {
                ValidationErrorMessage = string.Format("Bitte wählen Sie einen Auftraggeber");
                addModelError(m => m.SelectedAuftragGeber);
                IsValidCustom = false;
            }
        }

        void ValidateDateRange(
            string sectionName,
            DateTime? startDate, DateTime? endDate, 
            Expression<Func<HistoryAuftragFilter, object>> startDateProperty, Expression<Func<HistoryAuftragFilter, object>> endDateProperty, 
            Action<Expression<Func<HistoryAuftragFilter, object>>> addModelError)
        {
            if ((startDate == null && endDate != null) || (startDate != null && endDate == null))
            {
                ValidationErrorMessage = string.Format("Bitte Bereich {0} komplett oder gar nicht angeben", sectionName);
                addModelError(startDateProperty);
                addModelError(endDateProperty);
                IsValidCustom = false;
            }
            if (startDate > endDate)
            {
                ValidationErrorMessage = string.Format("Bereich {0}: Startdatum bitte nicht größer als Enddatum", sectionName);
                addModelError(startDateProperty);
                addModelError(endDateProperty);
                IsValidCustom = false;
            }
            if ((endDate.GetValueOrDefault() - startDate.GetValueOrDefault()).TotalDays > 90)
            {
                ValidationErrorMessage = string.Format("Bitte als Bereich '{0}' maximal 3 Monate angeben", sectionName);
                addModelError(startDateProperty);
                addModelError(endDateProperty);
                IsValidCustom = false;
            }

            if (IsEmptyAsGroup)
            {
                ValidationErrorMessage = "Bitte geben Sie mindestens ein Suchkriterium an";
                IsValidCustom = false;
            }
        }
    }
}