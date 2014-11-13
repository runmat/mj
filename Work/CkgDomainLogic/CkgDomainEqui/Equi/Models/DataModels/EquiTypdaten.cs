using System;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiTypdaten
    {
        public string Fahrzeugklasse { get; set; }

        public string Aufbauart { get; set; }

        [LocalizedDisplay(LocalizeConstants.VehicleClass_BodyType)]
        public string Fahrzeugklasse_Aufbau { get { return String.Format("{0} / {1}", Fahrzeugklasse, Aufbauart); } }

        public string Hersteller { get; set; }

        public string HerstSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer_Key)]
        public string Hersteller_HerstSchluessel { get { return String.Format("{0} / {1}", Hersteller, HerstSchluessel); } }

        public string Handelsname { get; set; }

        public string Farbe { get; set; }

        public string Farbcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.Color)]
        public string FarbeText
        {
            get
            {
                if (String.IsNullOrEmpty(Farbcode))
                    return Farbe;
                
                return String.Format("{0} ({1})", Farbe, Farbcode);
            }
        }

        [LocalizedDisplay(LocalizeConstants.TradeName_Color)]
        public string Handelsname_FarbeText { get { return String.Format("{0} / {1}", Handelsname, FarbeText); } }

        public DateTime? Genehmigungsdatum { get; set; }

        public string GenehmigungsNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfApproval_NumberShort)]
        public string Genehmigungsdatum_GenehmigungsNr { get { return String.Format("{0} / {1}", Genehmigungsdatum.NotNullOrEmptyToString(), GenehmigungsNr); } }

        public string Typ { get; set; }

        public string TypSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Type_Key)]
        public string Typ_TypSchluessel { get { return String.Format("{0} / {1}", Typ, TypSchluessel); } }

        [LocalizedDisplay(LocalizeConstants.MakeName)]
        public string Fabrikname { get; set; }

        public string Variante { get; set; }

        public string Version { get; set; }

        [LocalizedDisplay(LocalizeConstants.Variant_Version)]
        public string Variante_Version { get { return String.Format("{0} / {1}", Variante, Version); } }

        [LocalizedDisplay(LocalizeConstants.NoOfSeats)]
        public string AnzahlSitze { get; set; }

        [LocalizedDisplay(LocalizeConstants.GrossWeightKg)]
        public string ZulGesamtgewicht { get; set; }

        [LocalizedDisplay(LocalizeConstants.LengthMm)]
        public string Laenge { get; set; }

        [LocalizedDisplay(LocalizeConstants.WidthMm)]
        public string Breite { get; set; }

        [LocalizedDisplay(LocalizeConstants.HeightMm)]
        public string Hoehe { get; set; }

        [LocalizedDisplay(LocalizeConstants.EngineCapacityCcm)]
        public string Hubraum { get; set; }
        
        public string Leistung { get; set; }

        public string UmdrehungenProMin { get; set; }

        [LocalizedDisplay(LocalizeConstants.PowerKwAtRpm)]
        public string Leistung_UmdrehungenProMin { get { return String.Format("{0} / {1}", Leistung, UmdrehungenProMin); } }

        [LocalizedDisplay(LocalizeConstants.MaxSpeedKmh)]
        public string Hoechstgeschwindigkeit { get; set; }

        public string Standgeraeusch { get; set; }

        public string Fahrgeraeusch { get; set; }

        [LocalizedDisplay(LocalizeConstants.StationaryNoise_RoadNoiseDb)]
        public string Standgeraeusch_Fahrgeraeusch { get { return String.Format("{0} / {1}", Standgeraeusch, Fahrgeraeusch); } }

        public string Kraftstoffart { get; set; }

        public string Kraftstoffcode { get; set; }

        [LocalizedDisplay(LocalizeConstants.FuelType_Code)]
        public string Kraftstoffart_Kraftstoffcode { get { return String.Format("{0} / {1}", Kraftstoffart, Kraftstoffcode); } }

        [LocalizedDisplay(LocalizeConstants.TankCapacityCm)]
        public string FassungsvermoegenTank { get; set; }

        [LocalizedDisplay(LocalizeConstants.Co2EmissionGPerKm)]
        public string Co2Emission { get; set; }

        public string NationaleEmissionsklasseCode { get; set; }

        public string NationaleEmissionsklasse { get; set; }

        [LocalizedDisplay(LocalizeConstants.NationalEmissionClass)]
        public string NationaleEmissionsklasseCode_NationaleEmissionsklasse { get { return String.Format("{0} / {1}", NationaleEmissionsklasseCode, NationaleEmissionsklasse); } }

        [LocalizedDisplay(LocalizeConstants.EmissionStandard)]
        public string Abgasrichtlinie { get; set; }

        [LocalizedDisplay(LocalizeConstants.NoOfAxles)]
        public string AnzahlAchsen { get; set; }

        [LocalizedDisplay(LocalizeConstants.NoOfDriveAxles)]
        public string AnzahlAntriebsachsen { get; set; }

        public string MaxAchslastAchse1 { get; set; }

        public string MaxAchslastAchse2 { get; set; }

        public string MaxAchslastAchse3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.MaxAxleLoadAxles123Kg)]
        public string MaxAchslastAchsen123 { get { return String.Format("{0}, {1}, {2}", MaxAchslastAchse1, MaxAchslastAchse2, MaxAchslastAchse3); } }

        public string BereifungAchse1 { get; set; }

        public string BereifungAchse2 { get; set; }

        public string BereifungAchse3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.TiresAxles123)]
        public string BereifungAchsen123 { get { return String.Format("{0}, {1}, {2}", BereifungAchse1, BereifungAchse2, BereifungAchse3); } }

        [LocalizedDisplay(LocalizeConstants.Comments)]
        public string Bemerkungen { get; set; }

        public string GetSummaryString()
        {
            var strText = Localize.GeneralVehicleData;
            strText += "<br/>";
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.VehicleClass_BodyType, Fahrzeugklasse_Aufbau);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.Manufacturer_Key, Hersteller_HerstSchluessel);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.TradeName_Color, Handelsname_FarbeText);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.DateOfApproval_NumberShort, Genehmigungsdatum_GenehmigungsNr);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.Type_Key, Typ_TypSchluessel);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.MakeName, Fabrikname);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.Variant_Version, Variante_Version);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.NoOfSeats, AnzahlSitze);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.GrossWeightKg, ZulGesamtgewicht);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.LengthMm, Laenge);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.WidthMm, Breite);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.HeightMm, Hoehe);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.DrivetrainData;
            strText += "<br/>";
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.EngineCapacityCcm, Hubraum);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.PowerKwAtRpm, Leistung_UmdrehungenProMin);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.MaxSpeedKmh, Hoechstgeschwindigkeit);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.StationaryNoise_RoadNoiseDb, Standgeraeusch_Fahrgeraeusch);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.Fuel_Tank;
            strText += "<br/>";
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.FuelType_Code, Kraftstoffart_Kraftstoffcode);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.TankCapacityCm, FassungsvermoegenTank);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.Co2EmissionGPerKm, Co2Emission);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.NationalEmissionClass, NationaleEmissionsklasseCode_NationaleEmissionsklasse);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.EmissionStandard, Abgasrichtlinie);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.Axles_Tires;
            strText += "<br/>";
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.NoOfAxles, AnzahlAchsen);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.NoOfDriveAxles, AnzahlAntriebsachsen);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.MaxAxleLoadAxles123Kg, MaxAchslastAchsen123);
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.TiresAxles123, BereifungAchsen123);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.Comments;
            strText += "<br/>";
            strText += "<br/>";
            strText += String.Format("{0}: {1}", Localize.Comments, Bemerkungen);

            return strText;
        }
    }
}
