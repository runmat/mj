using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.ComponentModel;

namespace ZLDMobile.Models
{
    //public class Anwendung
    //{
    //    [Display(Name = "Anwendungs-ID")]
    //    public int AppId { get; set; }

    //    [Display(Name = "Anwendungsname")]
    //    public string AppName { get; set; }

    //    [Display(Name = "Controllername")]
    //    public string AppController { get; set; }

    //    [Display(Name = "Actionname")]
    //    public string AppAction { get; set; }

    //    public Anwendung(int appid, string appname, string appcontroller, string appaction)
    //    {
    //        this.AppId = appid;
    //        this.AppName = appname;
    //        this.AppController = appcontroller;
    //        this.AppAction = appaction;
    //    }
    //}

    //public class Zulassungskreis
    //{
    //    [Display(Name = "Kurzbezeichnung")]
    //    public string KurzBez { get; set; }

    //    [Display(Name = "Bezeichnung")]
    //    public string Bezeichnung { get; set; }

    //    [Display(Name = "Detail-Bezeichnung")]
    //    public string DetailBez
    //    {
    //        get { return KurzBez + ".." + Bezeichnung; }
    //    }

    //    public Zulassungskreis(string kuerzel, string langtext)
    //    {
    //        this.KurzBez = kuerzel;
    //        this.Bezeichnung = langtext;
    //    }
    //}

    //public class Dienstleistung
    //{
    //    [Display(Name = "Dienstleistungs-ID")]
    //    public string Id { get; set; }

    //    [Display(Name = "Bezeichnung")]
    //    public string Bezeichnung { get; set; }

    //    [Display(Name = "Detail-Bezeichnung")]
    //    public string DetailBez
    //    {
    //        get { return Id + ".." + Bezeichnung; }
    //    }

    //    public Dienstleistung(string id, string bez)
    //    {
    //        this.Id = id;
    //        this.Bezeichnung = bez;
    //    }
    //}

    ///// <summary>
    ///// ZLD-Vorgang/-Kopfsatz
    ///// </summary>
    //public class Vorgang
    //{
    //    [Display(Name = "Vorgang")]
    //    public string Id { get; set; }

    //    [Display(Name = "Verkaufsbüro")]
    //    public string VkBuero { get; set; }

    //    [Display(Name = "Kundennummer")]
    //    public string Kunnr { get; set; }

    //    [Display(Name = "Kundenname")]
    //    public string Kunname { get; set; }

    //    [Display(Name = "Kunde")]
    //    public string Kunde
    //    {
    //        get { return Kunnr + " " + Kunname; }
    //    }

    //    [Display(Name = "Referenz 1")]
    //    public string Referenz1 { get; set; }

    //    [Display(Name = "Referenz 2")]
    //    public string Referenz2 { get; set; }

    //    [Display(Name = "Zulassungsdatum")]
    //    public string ZulDat { get; set; }

    //    [Display(Name = "Amt")]
    //    public string Amt { get; set; }

    //    [Display(Name = "Kennzeichen Teil 1")]
    //    public string KennzKZ { get; set; }

    //    [Display(Name = "Kennzeichen Teil 2")]
    //    public string KennzAbc { get; set; }

    //    [Display(Name = "Kennzeichen")]
    //    public string Kennzeichen
    //    {
    //        get { return KennzKZ + " " + KennzAbc; }
    //    }

    //    [Display(Name = "Status")]
    //    public string Status { get; set; }

    //    [Display(Name = "Zahlart")]
    //    public string Zahlart
    //    {
    //        get
    //        {
    //            string erg = "";

    //            if (ZahlartEC)
    //            {
    //                erg = "EC";
    //            }
    //            if (ZahlartBar)
    //            {
    //                erg = "Bar";
    //            }
    //            if (ZahlartRE)
    //            {
    //                erg = "RE";
    //            }
    //            return erg;
    //        }
    //    }

    //    [Display(Name = "EC")]
    //    public bool ZahlartEC { get; set; }

    //    [Display(Name = "Bar")]
    //    public bool ZahlartBar { get; set; }

    //    [Display(Name = "RE")]
    //    public bool ZahlartRE { get; set; }

    //    [Display(Name = "Positionen")]
    //    public List<VorgangPosition> Positionen { get; set; }

    //    [Display(Name = "Is dirty")]
    //    public bool IsDirty { get; set; }

    //    public Vorgang(string id, string vkbuero, string kunnr, string kunname, string ref1, string ref2, string zuldat, 
    //        string amt, string kennzKZ, string kennzAbc, string status, bool ec, bool bar, bool re)
    //    {
    //        this.Id = id;
    //        this.VkBuero = vkbuero;
    //        this.Kunnr = kunnr;
    //        this.Kunname = kunname;
    //        this.Referenz1 = ref1;
    //        this.Referenz2 = ref2;
    //        this.ZulDat = zuldat;
    //        this.Amt = amt;
    //        this.KennzKZ = kennzKZ;
    //        this.KennzAbc = kennzAbc;
    //        this.Status = status;
    //        this.ZahlartEC = ec;
    //        this.ZahlartBar = bar;
    //        this.ZahlartRE = re;
    //        this.Positionen = new List<VorgangPosition>();
    //        this.IsDirty = false;
    //    }
    //}

    ///// <summary>
    ///// ZLD-Positionssatz
    ///// </summary>
    //public class VorgangPosition
    //{
    //    [Display(Name = "Positionsnummer")]
    //    public int PosNr { get; set; }

    //    [Display(Name = "Dienstleistung-Id")]
    //    public string DienstleistungId { get; set; }

    //    [Display(Name = "Dienstleistung")]
    //    public string DienstleistungBez { get; set; }

    //    [Display(Name = "Gebühr")]
    //    public double Gebuehr { get; set; }

    //    public VorgangPosition(int posNr, string dlId, string dlBez, double gebuehr)
    //    {
    //        this.PosNr = posNr;
    //        this.DienstleistungId = dlId;
    //        this.DienstleistungBez = dlBez;
    //        this.Gebuehr = gebuehr;
    //    }
    //}

    ///// <summary>
    ///// Dieses Objekt enthält die Vorgangsliste sowie z.B. div. Stammdatenlisten und dient u.a. als 
    ///// Hauptdatenstruktur für die LocalStorage-(Offline-)Speicherung
    ///// </summary>
    //public class Datencontainer
    //{
    //    [Display(Name = "Benutzername")]
    //    public string Username { get; set; }

    //    [Display(Name = "Vorgänge")]
    //    public List<Vorgang> Vorgaenge { get; set; }

    //    [Display(Name = "Zulassungskreise")]
    //    public List<Zulassungskreis> Zulassungskreise { get; set; }

    //    [Display(Name = "Dienstleistungen")]
    //    public List<Dienstleistung> Dienstleistungen { get; set; }

    //    public Datencontainer(string username)
    //    {
    //        Username = username;
    //        Vorgaenge = new List<Vorgang>();
    //        Zulassungskreise = new List<Zulassungskreis>();
    //        Dienstleistungen = new List<Dienstleistung>();
    //    }
    //}

    ///// <summary>
    ///// Wird als Antwort beim Empfang von Vorgängen vom Client vom Server geschickt
    ///// </summary>
    //public class Speicherresultat
    //{
    //    [Display(Name = "Id")]
    //    public string Id { get; set; }

    //    [Display(Name = "Ergebniscode")]
    //    public string Ergebniscode { get; set; }

    //    public Speicherresultat(string id)
    //    {
    //        this.Id = id;
    //        this.Ergebniscode = "";
    //    }

    //    public Speicherresultat(string id, string ergebniscode)
    //    {
    //        this.Id = id;
    //        this.Ergebniscode = ergebniscode;
    //    }
    //}

}
