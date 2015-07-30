using System;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    [GridColumnsAutoPersist]
    public class Fzg
    {
        public bool IsFilteredByExcelUpload;

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportName)]
        public string Carportname { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        [Kennzeichen]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoSeries)]
        public string KennzeichenSerie { get; set; }

        [LocalizedDisplay(LocalizeConstants.RegistrationDate)]
        public DateTime? Zulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.UnitNumber)]
        public string Unitnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelID)]
        public string ModelID { get; set; }

        [LocalizedDisplay(LocalizeConstants.ModelName)]
        public string Modell { get; set; }

        [LocalizedDisplay(LocalizeConstants.Model)]
        public string ModellAsText
        {
            get { return string.Format("{0} / {1}", ModelID, Modell); }
        }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Zb2Nummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.BatchID)]
        public string BatchId { get; set; }

        [LocalizedDisplay(LocalizeConstants.SippCode)]
        public string SippCode { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfZb2Receipt)]
        public DateTime? EingangZb2Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfVehicleArrival)]
        public DateTime? EingangFahrzeugDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfReadyIndication)]
        public DateTime? BereitmeldungDatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarManufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string Pdi { get; set; }

        [LocalizedDisplay(LocalizeConstants.Pdi)]
        public string DadPdi { get; set; }

        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string EquiNummer { get; set; }

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

        [LocalizedDisplay(LocalizeConstants.CommentInternal)]
        public string BemerkungIntern { get; set; }

        [LocalizedDisplay(LocalizeConstants.CommentExternal)]
        public string BemerkungExtern { get; set; }

        [LocalizedDisplay(LocalizeConstants.Tires)]
        public string Reifen { get; set; }

        [LocalizedDisplay(LocalizeConstants.NaviAvailable)]
        public string Navi { get; set; }

        [LocalizedDisplay(LocalizeConstants.Towbar)]
        public string Ahk { get; set; }

        public bool IsSelected { get; set; }

        public string InternalID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Amount)]
        public int Amount { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNumber)]
        public string AuftragsNummer { get; set; }

        public string FahrzeugAsText
        {
            get
            {
                return string.Format("{0} / {1}{2}{3}{4}{5}",
                        ModelID, Modell, Fahrgestellnummer.PrependIfNotNull(", FIN "), Zb2Nummer.PrependIfNotNull(", ZBII "), Pdi.PrependIfNotNull(", "),
                        (IsValid ? "" :  ValidationMessage.PrependIfNotNull("<br/>"))
                    );
            }
        }

        [LocalizedDisplay(LocalizeConstants.Message)]
        public string ValidationMessage { get; set; }

        public bool IsValid { get; set; }

        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }
    }
}
