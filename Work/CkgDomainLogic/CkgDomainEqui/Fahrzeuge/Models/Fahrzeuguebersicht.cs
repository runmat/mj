using System;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public class Fahrzeuguebersicht
    {
        public bool IsFilteredByExcelUpload;

        [LocalizedDisplay(LocalizeConstants.VehicleHistory)]
        public string ShowHistory { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportName)]
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
        public string SIPPCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        public DateTime? EingangZb2Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfVehicleArrival)]
        public DateTime? EingangFahrzeugDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReadyIndication)]
        public DateTime? BereitmeldungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }

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
                if (String.IsNullOrEmpty(Farbname))
                    return Farbcode;

                return String.Format("{0} ({1})", Farbname, Farbcode);
            }
        }

        [LocalizedDisplay(LocalizeConstants.BlockRemark)]
        public string BemerkungSperre { get; set; }

        [LocalizedDisplay(LocalizeConstants.Disabled)]
        public bool Gesperrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.MessageNo)]
        public string MeldungsNr { get; set; }

        public string DadPdi { get; set; }

        public bool IsSelected { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Bearbeitungsstatus { get; set; }
    }
}
