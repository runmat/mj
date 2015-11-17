using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Serialization;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.Zulassung.MobileErfassung.Contracts;
using CkgDomainLogic.Zulassung.MobileErfassung.Models;
using GeneralTools.Contracts;
using System.Linq;

namespace CkgDomainLogic.Zulassung.MobileErfassung.ViewModels
{
    public class ZulMobileErfassungViewModel
    {
        [XmlIgnore]
        public IZulMobileErfassungDataService DataService { get; private set; }

        [XmlIgnore]
        public ILogonContext LogonContext { get; private set; }

        [XmlIgnore]
        public IDictionary<string, string> PropertyDisplayNames { get; set; }

        [XmlIgnore]
        public List<Anwendung> Anwendungen { get; set; }

        public Datencontainer ZLDMobileData { get; set; }

        public string VkBurNeuanlage { get; private set; }

        public StammdatenNeuanlage StammdatenNeuanlage { get; private set; }

        /// <summary>
        /// Leerer Konstruktor
        /// </summary>
        public ZulMobileErfassungViewModel()
        {
        }

        /// <summary>
        /// Konstruktor inkl. Objektinitialisierung
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="logonContext"></param>
        public ZulMobileErfassungViewModel(IZulMobileErfassungDataService dataService, ILogonContextDataService logonContext)
        {
            Init(dataService, logonContext);
        }

        /// <summary>
        /// Objektinitialisierung
        /// </summary>
        /// <param name="dataService"></param>
        /// <param name="logonContext"></param>
        public void Init(IZulMobileErfassungDataService dataService, ILogonContext logonContext)
        {
            LogonContext = logonContext;
            DataService = dataService;
            LoadData();
        }

        /// <summary>
        /// Füllt Anwendungsliste, Stammdaten und DisplayName-Dictionary
        /// </summary>
        private void LoadData()
        {
            Anwendungen = DataService.GetAnwendungen();

            PropertyDisplayNames = GetPropertyDisplaynames(typeof(Vorgang));
            var posNames = GetPropertyDisplaynames(typeof(VorgangPosition));
            foreach (var posName in posNames)
            {
                if (!PropertyDisplayNames.ContainsKey(posName.Key))
                {
                    PropertyDisplayNames.Add(posName);
                }
            }

            ZLDMobileData = new Datencontainer(LogonContext.UserName) { Stammdaten = DataService.GetStammdaten() };
        }

        /// <summary>
        /// Liste der Ämter, für die aktuelle Vorgänge existieren, laden und zurückgeben
        /// </summary>
        /// <returns></returns>
        public List<AmtVorgaenge> GetAemterVorgaenge()
        {
            //List<AmtVorgaenge> aemterMitVorgaengen;
            //List<Vorgang> vorgaenge;

            //DataService.GetAemterMitVorgaengen(out aemterMitVorgaengen, out vorgaenge);

            //ZLDMobileData.AemterMitVorgaengen = aemterMitVorgaengen;
            //ZLDMobileData.Vorgaenge = vorgaenge;

            ZLDMobileData.Vorgaenge = new List<Vorgang>();
            for (var i = 0; i < 20; i++)
            {
                ZLDMobileData.Vorgaenge.Add(new Vorgang
                {
                    Id = (12345600 + i).ToString(),
                    BlTyp = "NZ",
                    VkOrg = "1010",
                    VkBur = "4837",
                    Kunnr = "471100",
                    Kunname = "Dummykunde " + i.ToString(),
                    Referenz1 = "Max Mustermann",
                    Referenz2 = "WWW123456789000" + i.ToString("D2"),
                    ZulDat = DateTime.Today,
                    Amt = "B",
                    Kennzeichen = "B-RD1000" + i.ToString("D2"),
                    Status = "A",
                    Infotext = "dgfgsgfdggsdf",
                    Bemerkung = "rtzrtztrzrtzrt",
                    Positionen = new List<VorgangPosition> { new VorgangPosition { KopfId = (12345600 + i).ToString(), PosNr = "10", DienstleistungId = "593", DienstleistungBez = "Neuzulassung", Gebuehr = 5, GebuehrenMaterial = "520" } }
                });
            }

            ZLDMobileData.AemterMitVorgaengen = new List<AmtVorgaenge> { new AmtVorgaenge
            {
                KurzBez = "B",
                Bezeichnung = "Berlin",
                ZulDatText = "17.11.2015",
                AnzVorgaenge = ZLDMobileData.Vorgaenge.Count
            } };

            return ZLDMobileData.AemterMitVorgaengen;
        }

        /// <summary>
        /// Aktuelle Vorgangsliste zurückgeben
        /// </summary>
        /// <returns></returns>
        public List<Vorgang> GetVorgaenge()
        {
            return ZLDMobileData.Vorgaenge;
        }

        /// <summary>
        /// Gibt für den angegebenen Typ ein Dictionary mit Property- und Anzeigenamen zurück 
        /// (z.B. für die modelgestützte Labelbeschriftung im View)
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public IDictionary<string, string> GetPropertyDisplaynames(Type T)
        {
            Dictionary<string, string> liste = new Dictionary<string, string>();

            PropertyInfo[] propInfos = T.GetProperties();
            foreach (PropertyInfo pi in propInfos)
            {
                var attributes = pi.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (attributes.Length > 0)
                {
                    liste.Add(pi.Name, ((DisplayAttribute)attributes[0]).Name);
                }
            }

            return liste;
        }

        /// <summary>
        /// Speichert den angegebenen Vorgang und gibt ggf. einen Fehlertext zurück
        /// </summary>
        /// <param name="vorg"></param>
        /// <returns>leeren String, wenn Speichern ok, sonst den Fehlertext</returns>
        public string SaveVorgang(Vorgang vorg)
        {
            if (vorg == null)
                return "Kein Vorgang ausgewählt";

            List<Vorgang> zuSpeicherndeVorgaenge = new List<Vorgang> { vorg };

            return DataService.SaveVorgaenge(zuSpeicherndeVorgaenge);
        }
   
        /// <summary>
        /// Ermittelt die BEB-Stati zu den angegebenen Vorgängen
        /// </summary>
        /// <param name="vorgIds"></param>
        /// <returns></returns>
        // ReSharper disable InconsistentNaming
        public List<VorgangStatus> GetVorgangBEBStatus(List<string> vorgIds)
        // ReSharper restore InconsistentNaming
        {
            return DataService.GetVorgangBebStatus(vorgIds);
        }

        /// <summary>
        /// Aktuelle Vorgangsliste zurückgeben
        /// </summary>
        /// <returns></returns>
        public List<string> GetVkBurs()
        {
            return DataService.GetVkBurs();
        }

        /// <summary>
        /// Gewähltes VkBur übernehmen und entsprechende Stammdaten laden
        /// </summary>
        public void ApplyVkBur(string vkBur)
        {
            VkBurNeuanlage = vkBur;

            if (StammdatenNeuanlage == null)
                StammdatenNeuanlage = new StammdatenNeuanlage { Aemter = DataService.GetStammdatenAemter().Where(a => !String.IsNullOrEmpty(a.KurzBez) && a.KurzBez != "3").ToList()};

            List<Kunde> kundenList;
            List<Dienstleistung> dienstleistungenList;
            DataService.GetStammdatenKundenUndHauptdienstleistungen(VkBurNeuanlage, out kundenList, out dienstleistungenList);

            StammdatenNeuanlage.Kunden = kundenList;
            StammdatenNeuanlage.Dienstleistungen = dienstleistungenList;
        }
    }
}