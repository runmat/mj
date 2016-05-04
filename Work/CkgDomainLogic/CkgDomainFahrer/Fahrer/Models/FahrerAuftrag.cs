using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrer.Models
{
    public class FahrerAuftrag
    {
        [GridHidden]
        public string KundenNr { get; set; }

        [GridHidden]
        public string KundenName { get; set; }
        
        [SelectListKey]
        [LocalizedDisplay(LocalizeConstants.OrderID)]
        [GridResponsiveVisible(GridResponsive.TabletOrWider)]
        public string AuftragsNr { get; set; }

        [SelectListText]
        [GridHidden]
        public string AuftragsNrFriendly { get { return AuftragsNr.NotNullOrEmpty().TrimStart('0'); } }

        [LocalizedDisplay(LocalizeConstants.DeliveryDate)]
        [GridResponsiveVisible(GridResponsive.TabletOrWider)]
        public DateTime? WunschLieferDatum { get; set; }    

        public DateTime? AbholDatum { get; set; }

        [GridHidden]
        public string FahrerStatus { get; set; }

        [GridHidden]
        public bool AuftragIstNeu { get { return FahrerStatus.NotNullOrEmpty().Trim().IsNullOrEmpty(); } }

        [GridHidden]
        public bool AuftragIstAbgelehnt { get { return FahrerStatus.NotNullOrEmpty() == "NO"; } }

        [GridHidden]
        public bool AuftragIstAngenommen { get { return FahrerStatus.NotNullOrEmpty() == "OK"; } }

        [GridHidden]
        public string UebernahmeZeitVon { get; set; }
        [GridHidden]
        public string UebernahmeZeitVonFormatted
        {
            get { return UebernahmeZeitVon.ToTimeFormatted(); }
        }
        [GridHidden]
        public string UebernahmeZeitBis { get; set; }
        [GridHidden]
        public string UebernahmeZeitBisFormatted
        {
            get { return UebernahmeZeitBis.ToTimeFormatted(); }
        }
        [GridHidden]
        public string UebergabeZeitVon { get; set; }
        [GridHidden]
        public string UebergabeZeitVonFormatted
        {
            get { return UebergabeZeitVon.ToTimeFormatted(); }
        }
        [GridHidden]
        public string UebergabeZeitBis { get; set; }
        [GridHidden]
        public string UebergabeZeitBisFormatted
        {
            get { return UebergabeZeitBis.ToTimeFormatted(); }
        }

        [LocalizedDisplay(LocalizeConstants.PostcodeStart)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string PlzStart { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityStart)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string OrtStart { get; set; }

        [LocalizedDisplay(LocalizeConstants.Start)]
        [GridResponsiveVisible(GridResponsive.Tablet)]
        public string PlzOrtStart { get { return OrtStart.FormatIfNotNull("{0} {this}", PlzStart); } }
       
        [LocalizedDisplay(LocalizeConstants.PostcodeDestination)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string PlzZiel { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityDestination)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string OrtZiel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Destination)]
        [GridResponsiveVisible(GridResponsive.Tablet)]
        public string PlzOrtZiel { get { return OrtZiel.FormatIfNotNull("{0} {this}", PlzZiel); } }

        [LocalizedDisplay(LocalizeConstants.PostcodeReturn)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string PlzRueck { get; set; }

        [LocalizedDisplay(LocalizeConstants.CityReturn)]
        [GridResponsiveVisible(GridResponsive.Workstation)]
        public string OrtRueck { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReturnTour)]
        [GridResponsiveVisible(GridResponsive.Tablet)]
        public string PlzOrtRueck { get { return OrtRueck.FormatIfNotNull("{0} {this}", PlzRueck); } }

        [LocalizedDisplay(LocalizeConstants.Order)]
        [GridResponsiveVisible(GridResponsive.Smartphone)]
        [GridRawHtml]
        public string AuftragsDetails { get { return AuftragsDetailsTemplate == null ? "-" : AuftragsDetailsTemplate(this); } }

        [LocalizedDisplay(LocalizeConstants.Action)]
        [GridRawHtml]
        public string AuftragsCommand { get { return AuftragsCommandTemplate == null ? "-" : AuftragsCommandTemplate(this); } }

        public static Func<FahrerAuftrag, string> AuftragsCommandTemplate { get; set; }
        public static Func<FahrerAuftrag, string> AuftragsDetailsTemplate { get; set; }

        
       /* Inline Aktionen für die freien Aufträge (ITA 8871 / are)
        * Selektion + Detailansicht
        */

        [GridHidden]
        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.Details)]
        [GridResponsiveVisible(GridResponsive.Smartphone)]
        [GridRawHtml]
        public string FreierAuftragDetails { get { return FreierAuftragDetailsTemplate == null ? "-" : FreierAuftragDetailsTemplate(this); } }

        [LocalizedDisplay(LocalizeConstants.Accept)]
        [GridRawHtml]
        public string FreierAuftragCommand { get { return FreierAuftragsCommandTemplate == null ? "-" : FreierAuftragsCommandTemplate(this); } }

        [LocalizedDisplay(LocalizeConstants.Details)]
        [GridRawHtml]
        public string FreierAuftragDetailsCommand { get { return FreierAuftragDetailsCommandTemplate == null ? "-" : FreierAuftragDetailsCommandTemplate(this); } }

        public static Func<FahrerAuftrag, string> FreierAuftragDetailsCommandTemplate { get; set; }

        public static Func<FahrerAuftrag, string> FreierAuftragDetailsTemplate { get; set; }
        public static Func<FahrerAuftrag, string> FreierAuftragsCommandTemplate { get; set; }



    }
}
