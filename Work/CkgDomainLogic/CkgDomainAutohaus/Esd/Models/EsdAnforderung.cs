using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Autohaus.ViewModels;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using MvcTools.Web;

namespace CkgDomainLogic.Autohaus.Models
{
    public class EsdAnforderung
    {
        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<EsdAnforderungViewModel> GetViewModel { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.VehicleType)]
        public string FahrzeugTyp { get; set; }

        [Required]
        [LocalizedDisplay(LocalizeConstants.Country)]
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


        #region Dienstleistungen

        [XmlIgnore]
        public List<Zusatzdienstleistung> AlleDienstleistungen
        {
            get
            {
                return new List<Zusatzdienstleistung>
                {
                    new Zusatzdienstleistung { MaterialNr = "10", Name = "Zulassung" },
                    new Zusatzdienstleistung { MaterialNr = "20", Name = "Überführung" },
                    new Zusatzdienstleistung { MaterialNr = "30", Name = "Zollzulassung" },
                    new Zusatzdienstleistung { MaterialNr = "40", Name = "Abmeldung" },
                    new Zusatzdienstleistung { MaterialNr = "50", Name = "Sonstiges" },
                };
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

    }
}
