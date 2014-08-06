using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Ueberfuehrung.Models
{
    public enum AdressenTyp { FahrtAdresse, RechnungsAdresse }

    public class Adresse : UiModel
    {
        [SelectListKey]
        public int ID { get; set; }

        [LocalizedDisplay(LocalizeConstants.CustomerNo)]
        public string KundenNr { get; set; }

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._FirmaRequired)]
        public string Firma { get; set; }

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.Name2)]
        public string Name2 { get; set; }

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.Nickname)]
        public string NickName { get; set; }

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.StreetRequired)]
        public string Strasse { get; set; }

        [ModelMappingClearable]
        public string HausNr
        {
            get { return _hausNr; }
            set
            {
                _hausNr = value;
                if (_hausNr.IsNotNullOrEmpty() && Strasse.IsNotNullOrEmpty() && !Strasse.EndsWith(" " + _hausNr)) 
                    Strasse += " " + _hausNr;
            }
        }

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._PlzUndOrtRequired)]
        public string PLZ { get; set; }

        [RequiredButModelOptional]
        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._OrtRequired)]
        public string Ort { get; set; }

        [XmlIgnore]
        public string PlzOrt
        {
            get { return String.Format("{0} {1}", PLZ, Ort); }
        }

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string Email { get; set; }

        private string _land = "DE";
        [LocalizedDisplay(LocalizeConstants.Country)]
        public string Land
        {
            get { return _land; }
            set { _land = value; }
        }

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.ContactpersonRequired)]
        public string Ansprechpartner { get; set; }

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.TelefonRequired)]
        public string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Fax)]
        public string Fax { get; set; }

        [LocalizedDisplay(LocalizeConstants.Carport)]
        public string Carport { get; set; }

        [XmlIgnore, SelectListText]
        public string FirmaOrt { get { return Ort.IsNullOrEmpty() ? NickName : String.Format("{0}, {1}", NickName, Ort); } }

        [XmlIgnore]
        public string AdresseAsRouteInfo { get { return String.Format("{0}, {1} {2}", Strasse, PLZ, Ort); } }

        [XmlIgnore]
        public string AdresseAsRouteInfoComplete { get { return String.Format("{0}, {1}", Firma, AdresseAsRouteInfo); } }

        [LocalizedDisplay(LocalizeConstants.Selection)]
        public int SelectedID { get; set; }

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [ModelMappingClearable]
        [LocalizedDisplay(LocalizeConstants._Uhrzeitwunsch)]
        public string Uhrzeitwunsch { get; set; }

        [XmlIgnore]
        public string UhrzeitwunschOptions { get { return "; 08:00; 09:00; 10:00; 11:00; 12:00; 13:00; 14:00; 15:00; 16:00; 17:00; 18:00"; } }
        public bool UhrzeitwunschAvailable { get; set; }

        [XmlIgnore]
        public override string ViewName { get { return String.Format("Partial/{0}", (Typ == AdressenTyp.FahrtAdresse ? "AdressenEdit" : "AdressenSelectableEdit")); } }

        [XmlIgnore]
        static public List<Land> Laender { get; set; }

        [XmlIgnore]
        static public List<Adresse> RechnungsAdressen { get; set; }

        private List<Adresse> _filteredRechnungsAdressen;
        [XmlIgnore]
        public List<Adresse> FilteredRechnungsAdressen { get { return (_filteredRechnungsAdressen ?? (_filteredRechnungsAdressen = RechnungsAdressen.Where(a => a.SubTyp == SubTyp).ToList())); } }

        public AdressenTyp Typ { get; set; }

        private string _transportTyp = "";
        private string _hausNr;

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
        public static List<TransportTyp> AlleTransportTypen { get; set; }
        [XmlIgnore]
        public List<TransportTyp> ValideTransportTypen 
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
                var intelligentTransportTypes = new List<TransportTyp>();
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
        public List<Fahrt> Fahrten { get; set; }

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

        public bool RequestGeoMapInfo { get; set; }

        [XmlIgnore]
        public bool ShowRouteInfo { get { return new[] { "ZIEL", "ZUSATZ" }.Contains(SubGroupName.NotNullOrEmpty().ToUpper()); } }

        [XmlIgnore]
        public override GeneralEntity SummaryItem
        {
            get 
            { 
                return new GeneralEntity
                             {
                                 Title = HeaderShort,
                                 Body = String.Format("{0}, {1} {2}, {3}", Firma, PLZ, Ort, Strasse),
                             }; 
            }
        }

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

        [XmlIgnore]
        public override XmlDictionary<string, string> ValidationAdditionalErrorProperties
        {
            get
            {
                var dict = new XmlDictionary<string, string>();
                
                if (IsEmpty)
                    return dict;

                if (TransportTypAvailable && TransportTyp.IsNullOrEmpty())
                    dict.Add("TransportTyp", "Bitte geben Sie den Transport-Typ an");

                if (Typ == AdressenTyp.FahrtAdresse)
                {
                    if (Ansprechpartner.IsNullOrEmpty())
                        dict.Add("Ansprechpartner", "Bitte geben Sie einen Ansprechpartner an");
                    if (Telefon.IsNullOrEmpty())
                        dict.Add("Telefon", "Bitte geben Sie die Telefonnummer an");
                }

                if (Datum != null)
                {
                    if (Datum < DateTime.Today)
                        dict.Add("Datum", "Bitte geben Sie ein Datum ab heute an");
                    else if (Datum.GetValueOrDefault().DayOfWeek == DayOfWeek.Sunday)
                        dict.Add("Datum", "Bitte vermeiden Sie Sonntage als Datum");
                    else
                    {
                        var feiertag = DateService.GetFeiertag(Datum);
                        if (feiertag != null)
                            dict.Add("Datum", string.Format("Der {0} ist ein Feiertag, '{1}'. Bitte vermeiden Sie Feiertage.", Datum.GetValueOrDefault().ToString("dd.MM.yy"), feiertag.Name));
                    }
                }

                return dict;
            }
        }

        public static TransportTyp GetTransportTypModel(string transportTyp)
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