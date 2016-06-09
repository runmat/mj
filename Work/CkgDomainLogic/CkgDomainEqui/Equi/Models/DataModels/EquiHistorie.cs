using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Equi.ViewModels;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    public class EquiHistorie
    {
        [LocalizedDisplay(LocalizeConstants.EquipmentNo)]
        public string Equipmentnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        public bool StatusZulassungsfaehig { get; set; }

        public bool StatusZugelassen { get; set; }

        public bool StatusAbgemeldet { get; set; }

        public bool StatusBeiAbmeldung { get; set; }

        public bool StatusOhneZulassung { get; set; }

        public bool StatusGesperrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Status)]
        public string Status
        {
            get
            {
                var erg = "";

                if (StatusZulassungsfaehig)
                {
                    erg = "Zulassungsfähig";
                }
                else if (StatusZugelassen)
                {
                    erg = "Zugelassen";
                }
                else if (StatusAbgemeldet)
                {
                    erg = "Abgemeldet";
                }
                else if (StatusBeiAbmeldung)
                {
                    erg = "Bei Abmeldung";
                }
                else if (StatusOhneZulassung)
                {
                    erg = "Ohne Zulassung";
                }
                else if (StatusGesperrt)
                {
                    erg = "Gesperrt";
                }

                return erg;
            }
        }

        public string AbcKennzeichen { get; set; }

        public DateTime? Versanddatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.StorageLocation)]
        public string Lagerort { get; set; }

        [LocalizedDisplay(LocalizeConstants.Manufacturer)]
        public string Hersteller { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarModel)]
        public string Fahrzeugmodell { get; set; }

        [LocalizedDisplay(LocalizeConstants.ManufacturerKey)]
        public string HerstellerSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.TypeKey)]
        public string TypSchluessel { get; set; }

        [LocalizedDisplay(LocalizeConstants.Variant_Version)]
        public string VarianteVersion { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2No)]
        public string Briefnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.FirstRegistration)]
        public DateTime? Erstzulassungsdatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CancellationDate)]
        public DateTime? Abmeldedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.CocAvailable)]
        public bool CocVorhanden { get; set; }

        public string HalterName1 { get; set; }

        public string HalterName2 { get; set; }

        public string HalterStrasse { get; set; }

        public string HalterHausnummer { get; set; }

        public string HalterPlz { get; set; }

        public string HalterOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarOwner)]
        public string Halter
        {
            get
            {
                return string.Format("{0} {1}" + (string.IsNullOrEmpty(HalterStrasse) && string.IsNullOrEmpty(HalterHausnummer) ? "" : Environment.NewLine)
                    + "{2} {3}" + (string.IsNullOrEmpty(HalterPlz) && string.IsNullOrEmpty(HalterOrt) ? "" : Environment.NewLine)
                    + "{4} {5}",
                    HalterName1, HalterName2,
                    HalterStrasse, HalterHausnummer,
                    HalterPlz, HalterOrt);
            }
        }

        public string StandortName1 { get; set; }

        public string StandortName2 { get; set; }

        public string StandortStrasse { get; set; }

        public string StandortHausnummer { get; set; }

        public string StandortPlz { get; set; }

        public string StandortOrt { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Standort
        {
            get
            {
                return string.Format("{0} {1}" + (string.IsNullOrEmpty(StandortStrasse) && string.IsNullOrEmpty(StandortHausnummer) ? "" : Environment.NewLine)
                    + "{2} {3}" + (string.IsNullOrEmpty(StandortPlz) && string.IsNullOrEmpty(StandortOrt) ? "" : Environment.NewLine)
                    + "{4} {5}",
                    StandortName1, StandortName2,
                    StandortStrasse, StandortHausnummer,
                    StandortPlz, StandortOrt);
            }
        }

        public string VersandgrundId { get; set; }

        [LocalizedDisplay(LocalizeConstants.CauseOfDispatch)]
        public string Versandgrund { get; set; }

        [LocalizedDisplay(LocalizeConstants.CarportArrival)]
        public DateTime? CarportEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicensePlateArrival)]
        public DateTime? KennzeichenEingang { get; set; }

        [LocalizedDisplay(LocalizeConstants.CheckInDeregistrationOrder)]
        public DateTime? CheckInAbmeldeauftrag { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB1)]
        public bool Fahrzeugschein { get; set; }

        [LocalizedDisplay(LocalizeConstants.BothLicensePlatesAvailable)]
        public bool BeideKennzeichenVorhanden { get; set; }

        [LocalizedDisplay(LocalizeConstants.DateOfDeactivation)]
        public DateTime? Stilllegung { get; set; }

        [LocalizedDisplay(LocalizeConstants.ReRegistrationDate)]
        public DateTime? Ummeldedatum { get; set; }

        [LocalizedDisplay(LocalizeConstants.LicenseNoOld)]
        public string KennzeichenAlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2Mobilisation)]
        public bool Briefaufbietung { get; set; }

        [LocalizedDisplay(LocalizeConstants.ZB2NoOld)]
        public string BriefnummerAlt { get; set; }

        [LocalizedDisplay(LocalizeConstants.OrderNo)]
        public string Ordernummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference1)]
        public string Referenz1 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Reference2)]
        public string Referenz2 { get; set; }

        [LocalizedDisplay(LocalizeConstants.Comments)]
        public string Bemerkungen { get; set; }

        [LocalizedDisplay(LocalizeConstants.DealerNo)]
        public string HaendlerNr { get; set; }

        [LocalizedDisplay(LocalizeConstants.FinancingType)]
        public string Finanzierungsart { get; set; }

        public EquiTypdaten Typdaten { get; set; }

        public bool ShowTypdaten { get; set; }

        public List<EquiMeldungsdaten> Meldungen { get; set; }

        public bool ShowMeldungen { get; set; }

        public List<EquiAktionsdaten> Aktionen { get; set; }

        public List<Stuecklisten> Stuecklisten { get; set; }

        public bool ShowAktionen { get; set; }

        public EquiHaendlerdaten Haendlerdaten { get; set; }

        public bool ShowHaendlerdaten { get; set; }

        public bool ShowStuecklisten { get { return (Stuecklisten != null); } }

        public bool HasArchives { get { return (GetViewModel != null && GetViewModel().HasArchives); } }

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<EquiHistorieViewModel> GetViewModel { get; set; }

        public string GetUebersichtSummaryString()
        {
            var strText = Localize.VehicleData;
            strText += "<br/>";
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.Manufacturer, Hersteller);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.CarModel, Fahrzeugmodell);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.Color, Typdaten.FarbeText);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.ManufacturerKey, HerstellerSchluessel);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.TypeKey, TypSchluessel);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.Variant_Version, VarianteVersion);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.ZB2Data;
            strText += "<br/>";
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.ZB2No, Briefnummer);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.FirstRegistration, Erstzulassungsdatum.NotNullOrEmptyToString());
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.CancellationDate, Abmeldedatum.NotNullOrEmptyToString());
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.CocAvailable, CocVorhanden);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.CarOwner, Halter);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.Location, Standort);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.CauseOfDispatch, Versandgrund);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.DeRegistrationData;
            strText += "<br/>";
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.CarportArrival, CarportEingang.NotNullOrEmptyToString());
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.LicensePlateArrival, KennzeichenEingang.NotNullOrEmptyToString());
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.CheckInDeregistrationOrder, CheckInAbmeldeauftrag.NotNullOrEmptyToString());
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.ZB1, Fahrzeugschein);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.BothLicensePlatesAvailable, BeideKennzeichenVorhanden);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.DateOfDeactivation, Stilllegung.NotNullOrEmptyToString());
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.ChangeData;
            strText += "<br/>";
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.ReRegistrationDate, Ummeldedatum.NotNullOrEmptyToString());
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.LicenseNoOld, KennzeichenAlt);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.ZB2NoOld, BriefnummerAlt);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.ZB2Mobilisation, Briefaufbietung);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.References;
            strText += "<br/>";
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.OrderNo, Ordernummer);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.Reference1, Referenz1);
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.Reference2, Referenz2);
            strText += "<br/>";
            strText += "<br/>";
            strText += Localize.Comments;
            strText += "<br/>";
            strText += "<br/>";
            strText += string.Format("{0}: {1}", Localize.Comments, Bemerkungen);

            return strText;
        }

        public string GetMeldungenSummaryString()
        {
            var strText = Localize.Vita;
            strText += "<br/>";
            strText += "<br/>";

            strText += string.Format("{0} {1} {2} {3} {4} {5}",
                Localize.Process, Localize.ExecutionDate, Localize.ShippingAddress, Localize.AcquisitionDate, Localize.DispatchType, Localize.InstructedBy);
            strText += "<br/>";

            foreach (var item in Meldungen)
            {
                strText += string.Format("{0} {1} {2} {3} {4} {5}",
                item.Vorgang, item.Durchfuehrungsdatum.NotNullOrEmptyToString(), item.Versandadresse, item.Erfassungsdatum.NotNullOrEmptyToString(), item.Versandart, item.BeauftragtDurch);
                strText += "<br/>";
            }

            if (strText.EndsWith("<br/>"))
                strText = strText.Substring(0, strText.Length - 5);

            return strText;
        }

        public string GetAktionenSummaryString()
        {
            var strText = Localize.Transmission;
            strText += "<br/>";
            strText += "<br/>";

            strText += string.Format("{0} {1} {2} {3}",
                Localize.ActionCode, Localize.Process, Localize.StatusDate, Localize.TransmissionDate);
            strText += "<br/>";

            foreach (var item in Aktionen)
            {
                strText += string.Format("{0} {1} {2} {3}",
                item.Aktionscode, item.Vorgang, item.Statusdatum.NotNullOrEmptyToString(), item.Uebermittlungsdatum.NotNullOrEmptyToString());
                strText += "<br/>";
            }

            if (strText.EndsWith("<br/>"))
                strText = strText.Substring(0, strText.Length - 5);

            return strText;
        }
    }
}
