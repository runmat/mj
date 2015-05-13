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

    public enum SperrAktion { Freigeben, Sperren, Entsperren } 

    public class TreuhandverwaltungSelektor : Store 
    {
        public SperrAktion Sperraktion { get; set; }

        public ReportType Reporttype { get; set; }

        public bool HasAction { get; set; }

        public bool IsActionShowGesperrte { get; set; }

        public bool IsActionFreigeben { get; set; }

        public string TGNummer { get { return Kundenkennung.IsNotNullOrEmpty() ? GetKunde(Kundenkennung).TGNummer : "" ; } }

        public string AGNummer { get { return Kundenkennung.IsNotNullOrEmpty() ? GetKunde(Kundenkennung).AGNummer : "" ; } }

        public Treuhandberechtigung Treuhandberechtigung { get; set; }

        public List<TreuhandverwaltungCsvUpload> UploadItems { get; set; }
      
        [LocalizedDisplay(LocalizeConstants.Customer)]
        public string Kundenkennung { get; set; }

       
        #region Report filter

        [LocalizedDisplay(LocalizeConstants.LicenseNo)]
        public string Kennzeichen { get; set; }

        [LocalizedDisplay(LocalizeConstants.ChassisNo)]
        public string Fahrgestellnummer { get; set; }

        [LocalizedDisplay(LocalizeConstants.ContractNo)]
        public string Vertragsnummer { get; set; }

        #endregion

        public string Berechtigung { get; set; }
       
        public string Treuhandberechtigungen { get { return MapBerechtigungen(); } }

        public string Akion { get; set; }

        public string Akionen { get { return string.Format("gesperrte,{0};abgelehnte,{1}", Localize.Disabled, Localize.Refused); } } 

        string MapBerechtigungen()
        {
            SperrAktion result; // aus dem View via PB
            Enum.TryParse(Berechtigung, out result);
            Sperraktion = result;

            HasAction = false;
            
            string ret = String.Empty;

            if(Treuhandberechtigung != null)
            {
                if (!String.IsNullOrEmpty(Treuhandberechtigung.Freigeben))
                {
                    ret = "0,Freigeben";
                    HasAction = true;
                    Akion = "gesperrte";
                }
                if (!String.IsNullOrEmpty(Treuhandberechtigung.Sperren))
                {
                    ret = ret.IsNotNullOrEmpty() ? ret + ";" : "";
                    ret += "1,Sperren";                   
                }
                if (!String.IsNullOrEmpty(Treuhandberechtigung.Entsperren))
                {
                    ret = ret.IsNotNullOrEmpty() ? ret + ";" : "";
                    ret += "2,Entsperren";                    
                }

                if (Berechtigung.IsNullOrEmpty())
                {
                    if (ret.Contains("0"))
                        Berechtigung = "0";
                    else if (ret.Contains("1"))
                        Berechtigung = "1";
                    else if (ret.Contains("2"))
                        Berechtigung = "2";
                }
            }           
            return ret;
        }

        TreuhandKunde GetKunde(string kundennummer)
        {
            var kunden = GetViewModel().TreuhandKunden;           
            return kunden.Where(x => x.AGNummer == kundennummer).FirstOrDefault();
        }


        #region Customer data

        public bool GetAGViewServicesMode
        {
            get
            {
                var kunden = GetViewModel().TreuhandKunden.Where(x => x.IsServicesAGMapping);
                return (kunden.Count() > 0); 
            }

        
        }

        public static List<SelectItem> TreuhandGeber
        {
            get
            {
                var kunden = GetViewModel().TreuhandKunden;
                return kunden.ConvertAll(new Converter<TreuhandKunde, SelectItem>(Wrap));
            }
        }

        public static List<SelectItem> Auftraggeber
        {
            get
            {
                var kunden = GetViewModel().Auftraggeber;
                return kunden.ConvertAll(new Converter<TreuhandKunde, SelectItem>(Wrap));
            }
        }

        public static List<SelectItem> AuftraggeberAlle
        {
            get
            {
                return Auftraggeber.Concat(new List<SelectItem> { new SelectItem("", Localize.DropdownDefaultOptionAll) }).OrderBy(x => x.Key).ToList();            
            }
        }
      
        public static List<SelectItem> TreuhandGeberAlle
        {
            get
            {                
                return TreuhandGeber.Concat(new List<SelectItem> { new SelectItem("", Localize.DropdownDefaultOptionAll) }).OrderBy(x => x.Key).ToList();
            }
        }

        public static List<SelectItem> TreuhandGeberEmpty
        {
            get
            {
                return TreuhandGeber.Concat(new List<SelectItem> { new SelectItem("", Localize.DropdownDefaultOptionPleaseChoose) }).OrderBy(x => x.Key).ToList();
            }
        }

        private static SelectItem Wrap(TreuhandKunde treuhandkunde)
        {            
            return new SelectItem(treuhandkunde.AGNummer, treuhandkunde.AGName);
        }

      

        #endregion

        [GridHidden, NotMapped, XmlIgnore, ScriptIgnore]
        public static Func<TreuhandverwaltungViewModel> GetViewModel { get; set; }

    }
}
