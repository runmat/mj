using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using CkgDomainLogic.Fahrer.Models;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace CkgDomainLogic.Fahrer.ViewModels
{
    public class FahrerBelegungViewModel
    {
        [LocalizedDisplay(LocalizeConstants.NumberOfDrivers)]
        public int FahrerAnzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.AssignmentType)]
        public FahrerTagBelegungsTyp BelegungsTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.AssignmentType)]
        public int BelegungsTypAsInt { get { return Int32.Parse(BelegungsTyp.ToString("d")); } set { BelegungsTyp = (FahrerTagBelegungsTyp)value; } }


        public List<FahrerTagBelegung> FahrerTagBelegungen { get; set; }

        [XmlIgnore]
        public string FeiertageAsString { get { return DateService.FeiertageAsString; } }

        [XmlIgnore]
        public static string BelegungsTypen
        {
            get
            {
                return string.Format("{0},{1};{2},{3};{4},{5};{6},{7};{8},{9};{10},{11}",
                                        FahrerTagBelegungsTyp.Leer.ToString("d"), "k. A.",
                                        FahrerTagBelegungsTyp.Verfuegbar.ToString("d"), "Verfügbar",
                                        FahrerTagBelegungsTyp.EingeschraenktVerfuegbar.ToString("d"), "Eingeschränkt verfügbar",
                                        FahrerTagBelegungsTyp.NichtVerfuegbar.ToString("d"), "Nicht verfügbar",
                                        FahrerTagBelegungsTyp.Urlaub.ToString("d"), "Urlaub",
                                        FahrerTagBelegungsTyp.Krank.ToString("d"), "Krank"
                                        );
            }
        }

        [LocalizedDisplay(LocalizeConstants.Comment)]
        [Length(128)]
        public string Kommentar { get; set; }


        public FahrerBelegungViewModel()
        {
            FahrerAnzahl = 1;
            BelegungsTyp = FahrerTagBelegungsTyp.Verfuegbar;
        }
    }
}
