using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrzeuge.ViewModels;
using GeneralTools.Models;
using GeneralTools.Services;
using GeneralTools.Resources;
using CkgDomainLogic.General.Services;

namespace CkgDomainLogic.Fahrzeuge.Models
{
    public enum ReportType { AG, TG, Services }

    public enum SperrAktion { Freigeben = 0, Sperren = 1, Entsperren = 2 } 

    public class TreuhandverwaltungSelektor : Store 
    {
        public ReportType Reporttype { get; set; }

        public string TGNummer { get { return Kundenkennung.IsNotNullOrEmpty() ? GetKunde(Kundenkennung).TGNummer : "" ; } }

        public string AGNummer { get { return Kundenkennung.IsNotNullOrEmpty() ? GetKunde(Kundenkennung).AGNummer : "" ; } }

        public Treuhandberechtigung Treuhandberechtigung { get; set; }

        public bool HatBerechtigungen
        {
            get
            {
                if (Treuhandberechtigung == null)
                    return false;

                return (Treuhandberechtigung.Freigeben || Treuhandberechtigung.Sperren || Treuhandberechtigung.Entsperren);
            }
        }

        public List<TreuhandverwaltungCsvUpload> UploadItems { get; set; }
      
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Kundenkennung { get; set; }

        [LocalizedDisplay(LocalizeConstants.Action)]
        public string Aktion { get; set; }

        public string Aktionen
        {
            get
            {
                var liste = new List<string>();

                if (Treuhandberechtigung.Freigeben)
                    liste.Add(string.Format("0,{0}", Localize.Approve));

                if (Treuhandberechtigung.Sperren)
                    liste.Add(string.Format("1,{0}", Localize.Lock));

                if (Treuhandberechtigung.Entsperren)
                    liste.Add(string.Format("2,{0}", Localize.Unlock));

                return string.Join(";", liste);
            }
        }

        public SperrAktion Sperraktion
        {
            get
            {
                switch (Aktion)
                {
                    case "0":
                        return SperrAktion.Freigeben;
                    case "1":
                        return SperrAktion.Sperren;
                    case "2":
                        return SperrAktion.Entsperren;
                    default:
                        return SperrAktion.Freigeben;
                }
            }
        }

        [LocalizedDisplay(LocalizeConstants.Processes)]
        public string Selektion { get; set; }

        public static string Selektionen { get { return string.Format("G,{0};A,{1}", Localize.Disabled, Localize.Refused); } } 
       
        #region Report filter

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        #endregion

        TreuhandKunde GetKunde(string kundennummer)
        {
            var kunden = GetViewModel().TreuhandKunden;
            return kunden.FirstOrDefault(x => (x.Selection == "TG" && x.AGNummer == kundennummer) || (x.Selection == "AG" && x.TGNummer == kundennummer));
        }

        #region Customer data

        public static List<SelectItem> TreuhandKunden
        {
            get
            {
                var kunden = GetViewModel().TreuhandKunden;
                return kunden.ConvertAll(Wrap);
            }
        }

        public static List<SelectItem> TreuhandKundenAlle
        {
            get
            {
                return TreuhandKunden.Concat(new List<SelectItem> { new SelectItem("", Localize.DropdownDefaultOptionAll) }).OrderBy(x => x.Key).ToList();
            }
        }

        public static List<SelectItem> TreuhandKundenEmpty
        {
            get
            {
                return TreuhandKunden.Concat(new List<SelectItem> { new SelectItem("", Localize.DropdownDefaultOptionPleaseChoose) }).OrderBy(x => x.Key).ToList();
            }
        }

        private static SelectItem Wrap(TreuhandKunde treuhandkunde)
        {
            return new SelectItem(
                (treuhandkunde.Selection == "TG" ? treuhandkunde.AGNummer : treuhandkunde.TGNummer),
                (treuhandkunde.Selection == "TG" ? treuhandkunde.AGName : treuhandkunde.TGName)
            );
        }

        #endregion

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<TreuhandverwaltungViewModel> GetViewModel { get; set; }
    }
}
