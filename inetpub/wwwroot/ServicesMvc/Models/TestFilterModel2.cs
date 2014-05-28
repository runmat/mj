using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using GeneralTools.Models;
using GeneralTools.Resources;
using GeneralTools.Services;

namespace ServicesMvc.Models
{
    public class TestFilterModel2
    {
        [LocalizedDisplay(LocalizeConstants.DepartmentHeadChristName)]
        public string Vorname { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadChristName)]
        public string Vorname3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadChristName)]
        public string Vorname4 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadLastName)]
        public string Nachname { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadLastName)]
        public string Nachname3 { get; set; }

        [LocalizedDisplay(LocalizeConstants.DepartmentHeadLastName)]
        public string Nachname4 { get; set; }

        [LocalizedDisplay(LocalizeConstants.NumberOfDrivers)]
        public int FahrerAnzahl { get; set; }

        [LocalizedDisplay(LocalizeConstants.AssignmentType)]
        public FahrerTagBelegungsTyp BelegungsTyp { get; set; }

        [LocalizedDisplay(LocalizeConstants.AssignmentType)]
        public int BelegungsTypAsInt { get { return Int32.Parse(BelegungsTyp.ToString("d")); } set { BelegungsTyp = (FahrerTagBelegungsTyp)value; } }


        public List<FahrerTagBelegung> FahrerTagBelegungen { get; set; }

        [XmlIgnore]
        //public List<Feiertag> Feiertage { get { return DateService.Feiertage; } }
        public string FeiertageAsString { get { return DateService.FeiertageAsString; } }

        [XmlIgnore]
        public static string BelegungsTypen
        {
            get
            {
                return string.Format("{0},{1};{2},{3};{4},{5};{6},{7};{8},{9}",
                                        FahrerTagBelegungsTyp.Leer.ToString("d") , "k. A.",
                                        FahrerTagBelegungsTyp.Verfuegbar.ToString("d"), "Verfügbar",
                                        FahrerTagBelegungsTyp.NichtVerfuegbar.ToString("d"), "Nicht verfügbar",
                                        FahrerTagBelegungsTyp.Urlaub.ToString("d"), "Urlaub",
                                        FahrerTagBelegungsTyp.Krank.ToString("d"), "Krank"
                                        );
            }
        }

        
        public TestFilterModel2()
        {
            FahrerAnzahl = 1;
            BelegungsTyp = FahrerTagBelegungsTyp.Verfuegbar;
        }
   }
}