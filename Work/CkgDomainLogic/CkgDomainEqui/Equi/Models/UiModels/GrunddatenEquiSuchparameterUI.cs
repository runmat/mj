using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GeneralTools.Services;

namespace CkgDomainLogic.Equi.Models
{
    //public class GrunddatenEquiSuchparameterUI : Store
    //{
    //    [Display(Name = "Zielort")]
    //    public string Zielort { get; set; }

    //    [Display(Name = "Betriebsnummer")]
    //    public string Betriebsnummer { get; set; }

    //    private string _mFahrgestellnummer;

    //    [Display(Name = "Fahrgestellnummer")]
    //    public string Fahrgestellnummer
    //    {
    //        get { return _mFahrgestellnummer; }
    //        set { _mFahrgestellnummer = (value == null ? "" : value.ToUpper()); }
    //    }

    //    private string _mFahrgestellnummer10;

    //    [Display(Name = "Fahrgestellnummer (10-stellig)")]
    //    public string Fahrgestellnummer10
    //    {
    //        get { return _mFahrgestellnummer10; }
    //        set { _mFahrgestellnummer10 = (value == null ? "" : value.ToUpper()); }
    //    }

        
    //    [Display(Name = "Nur Erstzulassungsdatum im Bereich")]
    //    public bool FilterNurErstzulassungsdatumBereich { get; set; }

    //    [Display(Name = "Erstzulassung von")]
    //    public DateTime ErstzulassungsdatumVon { get { return PropertyCacheGet(() => DateTime.Today.AddMonths(-12)); } set { PropertyCacheSet(value); } }

    //    [Display(Name = "Erstzulassung bis")]
    //    public DateTime ErstzulassungsdatumBis { get { return PropertyCacheGet(() => DateTime.Today.AddDays(0)); } set { PropertyCacheSet(value); } }


    //    [Display(Name = "Nur Abmeldedatum im Bereich")]
    //    public bool FilterNurAbmeldedatumBereich { get; set; }

    //    [Display(Name = "Abmeldedatum von")]
    //    public DateTime AbmeldedatumVon { get { return PropertyCacheGet(() => DateTime.Today.AddMonths(-12)); } set { PropertyCacheSet(value); } }

    //    [Display(Name = "Abmeldedatum bis")]
    //    public DateTime AbmeldedatumBis { get { return PropertyCacheGet(() => DateTime.Today.AddDays(0)); } set { PropertyCacheSet(value); } }


    //    [Display(Name = "Standort")]
    //    public string Standort { get; set; }

    //    [Display(Name = "Zielorte")]
    //    public List<Zielort> Zielorte { get; set; }

    //    [Display(Name = "Betriebsnummern")]
    //    public List<Betriebsnummer> Betriebsnummern { get; set; }

    //    [Display(Name = "Standorte")]
    //    public List<Standort> Standorte { get; set; }

    //    public GrunddatenEquiSuchparameterUI()
    //    {
    //        this.Zielort = "";
    //        this.Betriebsnummer = "";
    //        this.Fahrgestellnummer = "";
    //        this.Fahrgestellnummer10 = "";
    //        //this.ErstzulassungsdatumVon = null;
    //        //this.ErstzulassungsdatumBis = null;
    //        //this.AbmeldedatumVon = null;
    //        //this.AbmeldedatumBis = null;
    //        this.Standort = "";
    //        this.Zielorte = new List<Zielort>();
    //        this.Betriebsnummern = new List<Betriebsnummer>();
    //        this.Standorte = new List<Standort>();
    //    }

    //}
}
