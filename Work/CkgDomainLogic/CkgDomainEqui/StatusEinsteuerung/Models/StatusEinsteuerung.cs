using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.FzgModelle.Models
{
    public class StatusEinsteuerung
    {
        // ACHTUNG: Die Reihenfolge der Properties muss für den DataTable- bzw. Excel-Export GENAU SO bleiben !!!
        [LocalizedDisplay(LocalizeConstants.PDINumber)]
        public string PDINummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string PDIBezeichnung { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleGroup)]
        public string Fahrzeuggruppe { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        public string Sipp { get; set; }

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        public string HerstellerCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModellCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string Modellbezeichnung { get; set; }
       
        [LocalizedDisplay(LocalizeConstants.FeedEnTotal)]
        public int EingangGesamt { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateFromPreviousYear)]
        public int AusVorjahr { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationPrevMonth)]
        public int ZulassungVormonat { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationCurrentMonth)]
        public int ZulassungLfdMonat { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationFullMonth)]
        public int ZulassungGesamtMonat { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationPercCurrentMonth)]
        public int ZulassungProzLfdMonat { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationPercNextMonth)]
        public int ZulassungProzFolgeMonat { get; set; }

        [LocalizedDisplay(LocalizeConstants.Stock)]
        public int Bestand { get; set; }

        [LocalizedDisplay(LocalizeConstants.Equipped)]
        public int Ausgerüstet { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZbIIAvailable)]
        public int MitBrief { get; set; }

        [LocalizedDisplay(LocalizeConstants.Prepared4Registration)]
        public int Zulassungsbereit { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitnumberNotApplied)]
        public int OhneUnitnummer { get; set; }
      
        [LocalizedDisplay(LocalizeConstants.Blocked)]
        public int Gesperrt { get; set; }        
    }
}
