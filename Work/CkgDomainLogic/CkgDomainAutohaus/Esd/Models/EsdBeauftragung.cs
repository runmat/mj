using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Contracts;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Autohaus.Models
{
    public class EsdBeauftragung : IValidatableObject
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<EsdBeauftragungViewModel> GetViewModel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [XmlIgnore]
        public static List<Domaenenfestwert> FahrzeugTypList { get { return GetViewModel == null ? new List<Domaenenfestwert>() : GetViewModel().Fahrzeugarten; } }

        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTypBezeichnung
        {
            get { return (FahrzeugTypList.Any(f => f.Wert == FahrzeugTyp) ? FahrzeugTypList.First(f => f.Wert == FahrzeugTyp).Beschreibung : FahrzeugTyp); }
        }


        [LocalizedDisplay(LocalizeConstants.LeaseCar)]
        public bool IsLeasingFahrzeug { get; set; }

        [LocalizedDisplay(LocalizeConstants.Remark)]
        public string Bemerkung { get; set; }


        [Required]
        [LocalizedDisplay(LocalizeConstants.SelectedCountry)]
        public string Land { get; set; }


        [XmlIgnore]
        public static List<Land> LaenderAuswahlliste { get { return GetViewModel == null ? new List<Land>() : GetViewModel().LaenderAuswahlliste; } }


        [Required]
        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string AnsprechVorname { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string AnsprechNachname { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Email)]
        public string AnsprechEmail { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string AnsprechTelefonNr { get; set; }


        [LocalizedDisplay(LocalizeConstants.FirstName)]
        public string KundeVorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.LastName)]
        public string KundeNachname { get; set; }

        [LocalizedDisplay(LocalizeConstants.Email)]
        public string KundeEmail { get; set; }

        [LocalizedDisplay(LocalizeConstants.PhoneNo)]
        public string KundeTelefonNr { get; set; }


        #region Dienstleistungen

        [XmlIgnore]
        public List<Zusatzdienstleistung> AlleDienstleistungen
        {
            get
            {
                var generalConf = DependencyResolver.Current.GetService<IGeneralConfigurationProvider>();
                if (generalConf == null)
                    return new List<Zusatzdienstleistung>();

                var i = 0;
                return generalConf.GetConfigAllServerVal("Autohaus", "Autohaus_EsdAnforderung_Dienstleistungen_Keys")
                    .NotNullOrEmpty().Split(',')
                    .Select(resourceKey => new Zusatzdienstleistung
                    {
                        MaterialNr = (++i).ToString(),
                        Name = Localize.TranslateResourceKey(resourceKey.Trim(' '))
                    }).ToList();
            }
        }

        [XmlIgnore]
        public List<Zusatzdienstleistung> AvailableDienstleistungen
        {
            get { return AlleDienstleistungen; }
        }

        // Gewählte Dienstleistungen
        private string _gewaehlteDienstleistungenString;

        public string GewaehlteDienstleistungenString
        {
            get { return _gewaehlteDienstleistungenString.NotNullOrEmpty(); }
            set
            {
                _gewaehlteDienstleistungenString = value;

                SetGewaehlteDienstleistungen();
            }
        }

        private void SetGewaehlteDienstleistungen()
        {
            if (AvailableDienstleistungen != null)
            {
                AvailableDienstleistungen.ForEach(dl => dl.IstGewaehlt = false);
                GewaehlteDienstleistungen.ForEach(dl => dl.IstGewaehlt = true);
            }
        }

        [XmlIgnore]
        public List<Zusatzdienstleistung> GewaehlteDienstleistungen
        {
            get { return AvailableDienstleistungen.Where(dl => GewaehlteDienstleistungenString.Split(',').Contains(dl.MaterialNr)).ToList(); }
        }

        [XmlIgnore]
        public List<Zusatzdienstleistung> NichtGewaehlteDienstleistungen
        {
            get { return AvailableDienstleistungen.Except(AlleDienstleistungen).ToList(); }
        }

        public void InitDienstleistungen()
        {
            if (GewaehlteDienstleistungenString.IsNullOrEmpty())
                GewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => dl.IstGewaehlt).Select(dl => dl.MaterialNr).ToList());
            else
                SetGewaehlteDienstleistungen();
        }

        #endregion

        private string FormatHeaderTextForMail(string s)
        {
            return string.Format("<b>{0}</b>", s);
        }

        public string GetMailText()
        {
            const string indent = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

            var mailText = string.Format("{0}:<br/>", Localize.Autohaus_EsdAnforderung_MailTitle);

            mailText += "<br/>";
            mailText += string.Format("{0} <br/>", FormatHeaderTextForMail(Localize.Country));
            mailText += string.Format("{1}{0}<br/>", LaenderAuswahlliste.First(l => l.ID == Land.NotNullOr("-")).Name, indent);

            mailText += "<br/>";
            mailText += string.Format("{0} <br/>", FormatHeaderTextForMail(Localize.VehicleData));
            mailText += string.Format("{2}{0}: {1}<br/>", Localize.VehicleType, FahrzeugTypBezeichnung, indent);
            mailText += string.Format("{2}{0}: {1}<br/>", Localize.LeaseCar, IsLeasingFahrzeug ? Localize.Yes : Localize.No, indent);
            if (Bemerkung.IsNotNullOrEmpty())
                mailText += string.Format("{2}{0}: {1}<br/>", Localize.Remark, Bemerkung, indent);

            mailText += "<br/>";
            mailText += string.Format("{0} <br/>", FormatHeaderTextForMail(Localize.SelectedServices));
            mailText = GewaehlteDienstleistungen.Aggregate(mailText, (current, dienstleistung) => current + string.Format("{1}- {0}<br/>", dienstleistung.Name, indent));

            mailText += "<br/>";
            mailText += string.Format("{0} <br/>", FormatHeaderTextForMail(Localize.Contactperson));
            mailText += string.Format("{3}{0}: {1} {2}<br/>", Localize.Name, AnsprechVorname, AnsprechNachname, indent);
            mailText += string.Format("{2}{0}: {1}<br/>", Localize.Email, AnsprechEmail, indent);
            mailText += string.Format("{2}{0}: {1}<br/>", Localize.PhoneNo, AnsprechTelefonNr, indent);

            mailText += "<br/>";
            mailText += string.Format("{0} <br/>", FormatHeaderTextForMail(Localize.CustomerData));
            mailText += string.Format("{2}{0}: {1}<br/>", Localize.Company, GetViewModel().LogonContext.CustomerName, indent);
            mailText += string.Format("{3}{0}: {1} {2}<br/>", Localize.Name, KundeVorname, KundeNachname, indent);
            mailText += string.Format("{2}{0}: {1}<br/>", Localize.Email, KundeEmail, indent);
            mailText += string.Format("{2}{0}: {1}<br/>", Localize.PhoneNo, KundeTelefonNr, indent);

            return mailText;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GewaehlteDienstleistungen.None())
                yield return new ValidationResult(Localize.PleaseSelectAtLeastOneService, new [] { "" });
        }
    }
}
