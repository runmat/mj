using System;
using System.Collections.Generic;
using CkgDomainLogic.General.ViewModels;
using GeneralTools.Models;
using ServicesMvc.Models;

namespace ServicesMvc.ViewModels
{
    public class TestViewModel : CkgBaseViewModel
    {
        public string Firma { get { return PropertyCacheGet(() => "Zabel GmbH"); } set { PropertyCacheSet(value); } }

        public string Vorname { get { return PropertyCacheGet(() => "Walter"); } set { PropertyCacheSet(value); } }

        public string Nachname { get { return PropertyCacheGet(() => "Zabel"); } set { PropertyCacheSet(value); } }


        public TestFilterModel FilterModel
        {
            get
            {
                return PropertyCacheGet(() => new TestFilterModel
                {
                    Vorname = "Walter",
                    Vorname2 = "Walter 2",
                    Nachname = "Zabel",
                    Nachname2 = "Zabel 2",
                    Standort = "Hamburg",
                    Standort2 = "München",
                    LieferDatum = DateTime.Parse("31.07.2013"),
                    LieferDatum2 = DateTime.Parse("17.03.1965"),
                    CocDrucken = false,
                    Zb2Drucken = true,

                    Name1 = "Walter Zabel",
                    RechnungsDatum = DateTime.Parse("15.08.2013"),

                    FilterLieferDatum = new DateRange { IsSelected = true, StartDate = DateTime.Parse("17.03.1965"), EndDate = DateTime.Parse("06.12.2013") },
                    FilterRechnungsDatum = new DateRange { IsSelected = false, StartDate = DateTime.Parse("01.01.2013"), EndDate = DateTime.Parse("01.02.2013") },

                    FilterBetrieb = "DAD",
                    FilterStandorte = new[] { "Hamburg", "Frankfurt" },
                });
            }
            set { PropertyCacheSet(value); }
        }
        
        public TestFilterModel2 FilterModel2
        {
            get
            {
                return PropertyCacheGet(() => new TestFilterModel2
                {
                    Vorname = "II Walter",
                    Vorname3 = "II Walter 3",
                    Vorname4 = "II Walter 4",
                    Nachname = "II Zabel",
                    Nachname3 = "II Zabel 3",
                    Nachname4 = "II Zabel 4",

                    FahrerTagBelegungen = new List<FahrerTagBelegung>
                        {
                            new FahrerTagBelegung { Datum = DateTime.Parse("11.02.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Urlaub },
                            new FahrerTagBelegung { Datum = DateTime.Parse("12.02.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Krank },
                            new FahrerTagBelegung { Datum = DateTime.Parse("17.02.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Verfuegbar },
                            new FahrerTagBelegung { Datum = DateTime.Parse("18.02.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Verfuegbar },
                            new FahrerTagBelegung { Datum = DateTime.Parse("19.02.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Verfuegbar },

                            new FahrerTagBelegung { Datum = DateTime.Parse("17.03.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Urlaub },
                            new FahrerTagBelegung { Datum = DateTime.Parse("18.03.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Krank },
                            new FahrerTagBelegung { Datum = DateTime.Parse("19.03.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Urlaub },
                            new FahrerTagBelegung { Datum = DateTime.Parse("20.03.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Krank },
                            new FahrerTagBelegung { Datum = DateTime.Parse("06.03.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Urlaub },
                            new FahrerTagBelegung { Datum = DateTime.Parse("07.03.2014"), BelegungsTyp = FahrerTagBelegungsTyp.Krank },
                        }
                });
            }
            set { PropertyCacheSet(value); }
        }
    }
}