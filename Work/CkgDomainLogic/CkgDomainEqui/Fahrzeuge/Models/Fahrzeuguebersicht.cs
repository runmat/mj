using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Fahrzeuguebersicht
    {
        public bool IsFilteredByExcelUpload;

        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Action { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string Carport { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string Carportname { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string Unitnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModelID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string Modell { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        public int StatusKey { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nummer { get; set; }
             
        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string Auftragsnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.BatchID)]
        public string BatchId { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
// ReSharper disable once InconsistentNaming
        public string SIPPCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        public DateTime? EingangZb2Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfVehicleArrival)]
        public DateTime? EingangFahrzeugDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReadyIndication)]
        public DateTime? BereitmeldungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
        public DateTime? AbmeldeDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDispatch)]
        public DateTime? VersandDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.WinterTires)]
        public bool Winterreifen { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommentInternal)]
        public string BemerkungIntern { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommentExternal)]
        public string BemerkungExtern { get; set; }

        [LocalizedDisplay(LocalizeConstants.ColorCode)]
        public string Farbcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Color)]
        public string Farbname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Color)]
        public string Farbe
        {
            get
            {
                if (String.IsNullOrEmpty(Farbcode))
                    return Farbname;

                return String.Format("{0} ({1})", Farbname, Farbcode);
            }
        }

        [LocalizedDisplay(LocalizeConstants.Navi)]
        public bool Navi { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        public string BemerkungSperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.TrailerHitch)]
        public bool Anhaengerkupplung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Disabled)]
        public bool Gesperrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string Fahrzeugtyp { get; set; }
        [LocalizedDisplay(LocalizeConstants.MessageNo)]
        public string MeldungsNr { get; set; }

        public string DadPdi { get; set; }

        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Bearbeitungsstatus { get; set; }

        [LocalizedDisplay(LocalizeConstants.FuelType)]
        public string KraftstoffArt { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationReady)]
        public bool ZulassungBereit { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationStop)]
        public bool ZulassungsSperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.Supplier)]
        public string Lieferant { get; set; }

        [LocalizedDisplay(LocalizeConstants.City)]
        public string Ort { get; set; }

        [LocalizedDisplay(LocalizeConstants.FinancingType)]
        public string FinanzierungsArt { get; set; }
    }
}
