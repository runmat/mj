using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.Ueberfuehrung.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class Adresse : DomainCommon.Models.Adresse 
    {
        public string GroupName { get; set; }

        public string SubGroupName { get; set; }

        public string Header { get; set; }

        private string _headerShort;
        public string HeaderShort
        {
            get { return !string.IsNullOrEmpty(_headerShort) ? _headerShort : Header; }
            set { _headerShort = value; }
        }

        public AdressenTyp AdressTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContactpersonRequired)]
        public string Ansprechpartner { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport { get; set; }

        [XmlIgnore, SelectListText]
        public string FirmaOrt { get { return Ort.IsNullOrEmpty() ? Name1 : String.Format("{0}, {1}", Name1, Ort); } }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public int SelectedID { get; set; }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Uhrzeitwunsch)]
        public string Uhrzeitwunsch { get; set; }

        [XmlIgnore]
        public string UhrzeitwunschOptions { get { return "; 08:00; 09:00; 10:00; 11:00; 12:00; 13:00; 14:00; 15:00; 16:00; 17:00; 18:00"; } }
        public bool UhrzeitwunschAvailable { get; set; }

        [XmlIgnore]
        static public List<Ueberfuehrung.Models.Adresse> RechnungsAdressen { get; set; }

        private List<Ueberfuehrung.Models.Adresse> _filteredRechnungsAdressen;
        [XmlIgnore]
        public List<Ueberfuehrung.Models.Adresse> FilteredRechnungsAdressen { get { return (_filteredRechnungsAdressen ?? (_filteredRechnungsAdressen = RechnungsAdressen.Where(a => a.SubTyp == SubTyp).ToList())); } }

        private string _transportTyp = "";

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.TransportType)]
        public string TransportTyp
        {
            get { return _transportTyp; }
            set { _transportTyp = value; }
        }
        public bool TransportTypAvailable { get; set; }

        [XmlIgnore]
        public string TransportTypName { get { return GetTransportTypName(TransportTyp, HeaderShort); } }

        [XmlIgnore]
        public static List<Ueberfuehrung.Models.TransportTyp> AlleTransportTypen { get; set; }
        [XmlIgnore]
        public List<Ueberfuehrung.Models.TransportTyp> ValideTransportTypen
        {
            get
            {
                if (GroupName.IsNullOrEmpty())
                    return AlleTransportTypen;
                if (SubGroupName.IsNullOrEmpty())
                    return AlleTransportTypen;

                var fahrzeugIndexString = GroupName.Substring(GroupName.Length - 1);
                int fahrzeugIndex;
                if (!Int32.TryParse(fahrzeugIndexString, out fahrzeugIndex))
                    return AlleTransportTypen;

                var intelligentFahrzeugMatches = new List<string>
                                                     {
                                                         String.Format("fzg.{0}", fahrzeugIndex),
                                                         String.Format("fzg. {0}", fahrzeugIndex),
                                                         String.Format("fahrzeug{0}", fahrzeugIndex),
                                                         String.Format("fahrzeug {0}", fahrzeugIndex),
                                                     };
                var intelligentTransportTypes = new List<Ueberfuehrung.Models.TransportTyp>();
                AlleTransportTypen.ForEach(transportTyp =>
                {
                    var matchTransportType = intelligentFahrzeugMatches.FirstOrDefault(match => transportTyp.Name.ToLower().Contains(match));
                    if (matchTransportType != null)
                        intelligentTransportTypes.Add(transportTyp);
                });
                if (intelligentTransportTypes.Any())
                {
                    if (intelligentTransportTypes.Any(t => t.Name.ToLower().Contains("zusatz")))
                    {
                        if (SubGroupName.ToLower().Contains("zusatz"))
                            intelligentTransportTypes.RemoveAll(t => !t.Name.ToLower().Contains("zusatz"));
                        if (!SubGroupName.ToLower().Contains("zusatz"))
                            intelligentTransportTypes.RemoveAll(t => t.Name.ToLower().Contains("zusatz"));
                    }

                    if (intelligentTransportTypes.Count() > 1)
                        // if we have mor than 1 entry, also insert the choosing option ("bitte auswählen")
                        intelligentTransportTypes = intelligentTransportTypes.CopyAndInsertAtTop(AlleTransportTypen.First(t => t.ID == ""));

                    return intelligentTransportTypes;
                }

                return AlleTransportTypen;
            }
        }

        [XmlIgnore]
        public List<Ueberfuehrung.Models.Fahrt> Fahrten { get; set; }

        [XmlIgnore]
        public string FahrzeugBezeichnung
        {
            get { return String.Format("Fahrzeug {0}", Fahrten == null || Fahrten.None() ? "1" : Fahrten.First().FahrzeugIndex); }
        }

        public string EntfernungKm { get; set; }

        /// <summary>
        /// only for temporary use
        /// </summary>
        public string FahrtIndexAktuellTmp { get; set; }

        /// <summary>
        /// only for temporary use
        /// </summary>
        public string ReAdresseOpticalCheck { get; set; }

        /// <summary>
        /// z. B. bei Rechnungsadressen: RG-Zahler oder abweichende RG-Adresse
        /// </summary>
        public string SubTyp { get; set; }

        public string Mandant { get; set; }

        [XmlIgnore]
        public string FahrtTitleFromAddressType
        {
            get
            {
                if (GroupName.IsNullOrEmpty() || SubGroupName.IsNullOrEmpty())
                    return "";

                var val = "";
                if (GroupName.StartsWith("FAHRZEUG"))
                    val = String.Format("Fzg. {0}", GroupName.Substring(GroupName.Length - 1));

                if (SubGroupName.StartsWith("ZUSATZ"))
                    val += ", Zusatzfahrt";
                if (SubGroupName.StartsWith("ZIEL"))
                    val += ", Hauptfahrt";

                return val;
            }
        }

        public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var dict = base.Validate(validationContext).ToList();

            if (Name1.IsNullOrEmpty())
                return dict;

            if (TransportTypAvailable && TransportTyp.IsNullOrEmpty())
                dict.Add(new ValidationResult("Bitte geben Sie den Transport-Typ an", new[]{ "TransportTyp" }));

            if (AdressTyp == AdressenTyp.FahrtAdresse)
            {
                if (Ansprechpartner.IsNullOrEmpty())
                    dict.Add(new ValidationResult("Bitte geben Sie einen Ansprechpartner an", new[]{ "Ansprechpartner" }));
                if (Telefon.IsNullOrEmpty())
                    dict.Add(new ValidationResult("Bitte geben Sie die Telefonnummer an", new[]{ "Telefon" }));
            }

            if (Datum != null)
            {
                if (Datum < DateTime.Today)
                    dict.Add(new ValidationResult("Bitte geben Sie ein Datum ab heute an", new[]{ "Datum" }));
                else if (Datum.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday)
                    dict.Add(new ValidationResult("Bitte vermeiden Sie Sonntage als Datum", new[]{ "Datum" }));
                else
                {
                    var feiertag = DateService.GetFeiertag(Datum);
                    if (feiertag != null)
                        dict.Add(new ValidationResult(
                                    string.Format("Der {0} ist ein Feiertag, '{1}'. Bitte vermeiden Sie Feiertage.", Datum.GetValueOrDefault().ToString("dd.MM.yy"), feiertag.Name)
                                    , new[]{ "Datum"})); 
                }
            }

            return dict;
        }

        public static Ueberfuehrung.Models.TransportTyp GetTransportTypModel(string transportTyp)
        {
            return (AlleTransportTypen == null ? null : AlleTransportTypen.FirstOrDefault(tt => tt.ID == transportTyp));
        }

        public static string GetTransportTypName(string transportTyp, string defaultTypName = "")
        {
            var transportTypModel = GetTransportTypModel(transportTyp);
            return transportTypModel == null ? "" : (transportTypModel.ID.IsNullOrEmpty() ? defaultTypName : transportTypModel.Name);
        }
    }
}
