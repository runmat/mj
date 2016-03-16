using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public enum AdressenTyp { FahrtAdresse, RechnungsAdresse }

    [XmlType(TypeName = "UebfuehrgAdresse")]
    public class Adresse : DomainCommon.Models.Adresse, IValidatableObject
    {
        private AdressenTyp _adressTyp = AdressenTyp.FahrtAdresse;

        [XmlIgnore, ScriptIgnore, ModelMappingCopyIgnore]
        public AdressenTyp AdressTyp
        {
            get { return _adressTyp; }
            set { _adressTyp = value; }
        }

        [LocalizedDisplay(LocalizeConstants.Contactperson)]
        [Required]
        public string Ansprechpartner { get; set; }
        
        [LocalizedDisplay(LocalizeConstants.Phone)]
        [Required]
        public new string Telefon { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        [EmailAddress]
        public new string Email { get; set; }

        [SelectListText, XmlIgnore]
        public string NameOrt { get { return Ort.IsNullOrEmpty() ? Name1 : String.Format("{0}, {1}", Name1, Ort); } }

        [LocalizedDisplay(LocalizeConstants.Date)]
        public DateTime? Datum { get; set; }

        [LocalizedDisplay(LocalizeConstants._Uhrzeitwunsch)]
        public string Uhrzeitwunsch { get; set; }

        [ModelMappingCopyIgnore]
        public bool UhrzeitwunschAvailable { get; set; }

        [XmlIgnore, ScriptIgnore]
        public string UhrzeitwunschOptions
        {
            get
            {
                return
                    ";" +
                    "08:00-12:00,Vormittags;" +
                    "14:00-20:00,Nachmittags;" +
                    "08:00-10:00,08:00 - 10:00;" +
                    "10:00-12:00,10:00 - 12:00;" +
                    "12:00-14:00,12:00 - 14:00;" +
                    "14:00-16:00,14:00 - 16:00;" +
                    "16:00-18:00,16:00 - 18:00;" +
                    "18:00-20:00,18:00 - 20:00";
            }
        }

        private string _transportTyp = "";

        [LocalizedDisplay(LocalizeConstants.TransportType)]
        public string TransportTyp
        {
            get { return _transportTyp;  }
            set { _transportTyp = value; }
        }
        public bool TransportTypAvailable { get; set; }

        [XmlIgnore, ScriptIgnore]
        public string TransportTypName { get { return GetTransportTypName(TransportTyp, Header); } }

        [XmlIgnore, ScriptIgnore, ModelMappingCopyIgnore]
        public Func<List<TransportTyp>> GetAlleTransportTypen { get; set; }
        [XmlIgnore, ScriptIgnore, ModelMappingCopyIgnore]
        public List<TransportTyp> ValideTransportTypen
        {
            get
            {
                var alleTransportTypen = GetAlleTransportTypen().CopyAndInsertAtTop(new TransportTyp { ID = "", Name = Localize.TranslateResourceKey(LocalizeConstants.DropdownDefaultOptionPleaseChoose) });

                if (GroupName.IsNullOrEmpty())
                    return alleTransportTypen;
                if (SubGroupName.IsNullOrEmpty())
                    return alleTransportTypen;

                var fahrzeugIndexString = GroupName.Substring(GroupName.Length - 1);
                int fahrzeugIndex;
                if (!Int32.TryParse(fahrzeugIndexString, out fahrzeugIndex))
                    return alleTransportTypen;

                var intelligentFahrzeugMatches = new List<string>
                                                     {
                                                         String.Format("fzg.{0}", fahrzeugIndex),
                                                         String.Format("fzg. {0}", fahrzeugIndex),
                                                         String.Format("fahrzeug{0}", fahrzeugIndex),
                                                         String.Format("fahrzeug {0}", fahrzeugIndex),
                                                     };
                var intelligentTransportTypes = new List<TransportTyp>();
                alleTransportTypen.ForEach(transportTyp =>
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

                    //if (intelligentTransportTypes.Count() > 1)
                    //    // if we have mor than 1 entry, also insert the choosing option ("bitte auswählen")
                    //    intelligentTransportTypes = intelligentTransportTypes.CopyAndInsertAtTop(alleTransportTypen.First(t => t.ID == ""));

                    return intelligentTransportTypes;
                }

                return alleTransportTypen;
            }
        }

        [XmlIgnore, ScriptIgnore, ModelMappingCopyIgnore]
        public List<Fahrt> Fahrten { get; set; }

        [XmlIgnore, ScriptIgnore]
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

        [ModelMappingCopyIgnore]
        public string Mandant { get; set; }

        [XmlIgnore]
        public string AdresseAsBlock { get { return GetSummaryString().Replace("<br/>", "\r\n"); } }

        [XmlIgnore, ScriptIgnore]
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

        public TransportTyp GetTransportTypModel(string transportTyp)
        {
            var getAlleTransportTypen = GetAlleTransportTypen;
            return (getAlleTransportTypen == null ? null : getAlleTransportTypen().FirstOrDefault(tt => tt.ID == transportTyp));
        }

        public string GetTransportTypName(string transportTyp, string defaultTypName)
        {
            var transportTypModel = GetTransportTypModel(transportTyp);
            return (transportTypModel == null || transportTypModel.ID.IsNullOrEmpty() ? defaultTypName : transportTypModel.Name);
        }

        public override string GetSummaryString()
        {
            return string.Format("{0}<br/>{1}<br/>{2}{3} {4}", Name1, StrasseHausNr, LandAsFormatted(Land), PLZ, Ort);
        }


        #region Route Info

        [XmlIgnore]
        public string AdresseAsRouteInfo
        {
            get { return String.Format("{0}, {1} {2}", StrasseHausNr, PLZ, Ort); }
        }

        public string StartAdresseAsRouteInfo { get; set; }

        [XmlIgnore]
        public bool ShowRouteInfo
        {
            get { return new[] {"ZIEL", "ZUSATZ"}.Contains(SubGroupName.NotNullOrEmpty().ToUpper()); }
        }

        #endregion
    }
}
