using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.General.ViewModels;
using CkgDomainLogic.Fahrzeuge.Contracts;
using CkgDomainLogic.Fahrzeuge.Models;
using DocumentTools.Services;
using GeneralTools.Models;
using SapORM.Contracts;

namespace CkgDomainLogic.Fahrzeuge.ViewModels
{
    public class CarporterfassungViewModel : CkgBaseViewModel
    {
        [XmlIgnore]
        public ICarporterfassungDataService DataService { get { return CacheGet<ICarporterfassungDataService>(); } }

        public CarporterfassungModel AktuellesFahrzeug { get; set; }

        [XmlIgnore]
        public List<CarporterfassungModel> Fahrzeuge
        {
            get { return PropertyCacheGet(() => new List<CarporterfassungModel>()); }
            private set { PropertyCacheSet(value); }
        }

        [XmlIgnore]
        public List<CarporterfassungModel> FahrzeugeFiltered
        {
            get { return PropertyCacheGet(() => Fahrzeuge); }
            protected set { PropertyCacheSet(value); }
        }

        public bool EditMode { get; set; }

        public void Init()
        {
            DataMarkForRefresh();
            LoadFahrzeugModel();
        }

        public void DataMarkForRefresh()
        {
            PropertyCacheClear(this, m => m.FahrzeugeFiltered);
        }

        public void LoadFahrzeugModel(string kennzeichen = null)
        {
            EditMode = true;

            if (kennzeichen != null)
            {
                AktuellesFahrzeug = Fahrzeuge.FirstOrDefault(f => f.Kennzeichen == kennzeichen);
                if (AktuellesFahrzeug != null)
                    return;
            }            

            AktuellesFahrzeug = new CarporterfassungModel
                {
                    CarportId = LogonContext.User.Reference,
                    KundenNr = LogonContext.KundenNr.ToSapKunnr(),
                    DemontageDatum = DateTime.Today
                };
        }

        public void LoadFahrzeugdaten(string kennzeichen)
        {
            AktuellesFahrzeug = DataService.LoadFahrzeugdaten(kennzeichen.NotNullOrEmpty().ToUpper());
        }

        public void AddFahrzeug(CarporterfassungModel item)
        {
            item.Kennzeichen = item.Kennzeichen.NotNullOrEmpty().ToUpper();
            item.FahrgestellNr = item.FahrgestellNr.NotNullOrEmpty().ToUpper();

            // Nur einen Datensatz zu einem Kennzeichen zulassen
            if (Fahrzeuge.Any(f => f.Kennzeichen == item.Kennzeichen))
                Fahrzeuge.RemoveAll(f => f.Kennzeichen == item.Kennzeichen);

            Fahrzeuge.Add(item);
            DataMarkForRefresh();
        }

        public void RemoveFahrzeug(CarporterfassungModel item)
        {
            Fahrzeuge.Remove(item);
            DataMarkForRefresh();
        }

        public void SaveFahrzeuge()
        {
            EditMode = false;
            Fahrzeuge = DataService.SaveFahrzeuge(Fahrzeuge);
            DataMarkForRefresh();
        }

        public void ClearList()
        {
            Fahrzeuge.RemoveAll(f => String.IsNullOrEmpty(f.Status));
            DataMarkForRefresh();
        }

        public byte[] GetLieferschein()
        {
            var tblLieferschein = new DataTable("Lieferschein");
            tblLieferschein.Columns.Add("Nr");
            tblLieferschein.Columns.Add("Kennzeichen");
            tblLieferschein.Columns.Add("Fahrgestellnummer");
            tblLieferschein.Columns.Add("Hersteller");
            tblLieferschein.Columns.Add("Demontagedatum");
            tblLieferschein.Columns.Add("Vorlage ZBI");
            tblLieferschein.Columns.Add("Anzahl Kennzeichen");
            tblLieferschein.Columns.Add("Web User");
            tblLieferschein.Columns.Add("Carport Nr");
            tblLieferschein.Columns.Add("Erfassungsdatum");
            tblLieferschein.AcceptChanges();

            var tblKopf = new DataTable("Kopf");
            tblKopf.Columns.Add("CarportID");
            tblKopf.Columns.Add("Name1");
            tblKopf.Columns.Add("Name2");
            tblKopf.Columns.Add("LieferscheinNummer");
            tblKopf.Columns.Add("Kundenname");
            tblKopf.AcceptChanges();

            var nr = 1;
            foreach (var fzg in Fahrzeuge.Where(f => String.IsNullOrEmpty(f.Status)).OrderBy(f => f.Kennzeichen).ToList())
            {
                if (nr == 1)
                {
                    var newKopfRow = tblKopf.NewRow();
                    newKopfRow["CarportID"] = fzg.CarportId;
                    newKopfRow["Name1"] = LogonContext.User.LastName;
                    newKopfRow["Name2"] = LogonContext.User.FirstName;
                    newKopfRow["LieferscheinNummer"] = fzg.LieferscheinNr;
                    newKopfRow["Kundenname"] = LogonContext.CustomerName;
                    tblKopf.Rows.Add(newKopfRow);
                }

                var newRow = tblLieferschein.NewRow();
                newRow["Nr"] = nr;
                newRow["Kennzeichen"] = fzg.Kennzeichen;
                newRow["Fahrgestellnummer"] = fzg.FahrgestellNr;
                newRow["Hersteller"] = "";
                newRow["Demontagedatum"] = fzg.DemontageDatum.ToString("dd.MM.yyyy");
                newRow["Vorlage ZBI"] = "";
                newRow["Anzahl Kennzeichen"] = fzg.AnzahlKennzeichen;
                newRow["Web User"] = LogonContext.UserName;
                newRow["Carport Nr"] = fzg.CarportId;
                newRow["Erfassungsdatum"] = DateTime.Now.ToShortDateString();
                tblLieferschein.Rows.Add(newRow);
                nr++;
            }

            var docFactory = new WordDocumentFactory(tblLieferschein, null);

            return docFactory.CreateDocumentAndReturnBytes(Localize.Fahrzeuge_Carporterfassung, Path.Combine(AppSettings.RootPath, @"Documents\Templates\Bestellung.doc"), tblKopf);
        }

        public void FilterFahrzeuge(string filterValue, string filterProperties)
        {
            FahrzeugeFiltered = Fahrzeuge.SearchPropertiesWithOrCondition(filterValue, filterProperties);
        }
    }
}
