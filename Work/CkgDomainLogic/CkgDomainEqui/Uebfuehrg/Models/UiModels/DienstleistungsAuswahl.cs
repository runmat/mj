using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class DienstleistungsAuswahl
    {
        public string FahrtIndex { get; set; }

        public string FahrtTyp { get; set; }

        // Alle Dienstleistungen
        [XmlIgnore]
        public static List<Dienstleistung> AlleDienstleistungen { get; set; }

        [XmlIgnore]
        public List<Dienstleistung> AvailableDienstleistungen { get { return AlleDienstleistungen.Where(dl => dl.TransportTyp == FahrtTyp).ToList(); } }

        // Gewählte Dienstleistungen
        private string _gewaehlteDienstleistungenString;
        public string GewaehlteDienstleistungenString
        {
            get { return _gewaehlteDienstleistungenString.NotNullOrEmpty(); }
            set { _gewaehlteDienstleistungenString = value; }
        }

        [XmlIgnore]
        public List<Dienstleistung> GewaehlteDienstleistungen
        {
            get
            {
                var list = AvailableDienstleistungen.Where(dl => GewaehlteDienstleistungenString.Split(',').Contains(dl.ID)).ToList();
                list.ForEach(dienstleistung => dienstleistung.FahrtIndex = FahrtIndex);
                return list;
            }
        }

            
        // Nicht gewählte Dienstleistungen
        private string _nichtGewaehlteDienstleistungenString;
        public string NichtGewaehlteDienstleistungenString
        {
            get { return _nichtGewaehlteDienstleistungenString.NotNullOrEmpty(); }
            set { _nichtGewaehlteDienstleistungenString = value; }
        }

        [XmlIgnore]
        public List<Dienstleistung> NichtGewaehlteDienstleistungen { get { return AvailableDienstleistungen.Where(dl => NichtGewaehlteDienstleistungenString.Split(',').Contains(dl.ID)).ToList(); } }

        [XmlIgnore]
        public override string ViewName { get { return "Partial/DienstleistungsAuswahl"; } }

        [XmlIgnore]
        public override bool IsValid { get { return true; } }

        [XmlIgnore]
        public override bool IsEmpty
        {
            get { return GewaehlteDienstleistungen.None(); }
        }

        [XmlIgnore]
        public bool DienstleistungenInitialized
        {
            get
            {
                return AvailableDienstleistungen != null && (!string.IsNullOrEmpty(GewaehlteDienstleistungenString) || !string.IsNullOrEmpty(NichtGewaehlteDienstleistungenString));
            }
        }

        [XmlIgnore]
        public override GeneralEntity SummaryItem
        {
            get
            {
                var sumPreis = GewaehlteDienstleistungen.Sum(p => p.Preis);
                var sumPreisTxt = string.Format(", Summe: {0:c}", sumPreis);
                if (sumPreis == 0)
                    sumPreisTxt = "";

                var anzahl = GewaehlteDienstleistungen.Count();
                var anzahlTxt = string.Format("{0} gewählte Dienstleistung{1}", anzahl, (anzahl > 1 ? "en" : ""));
                if (anzahl == 0)
                    anzahlTxt = "";

                return new GeneralEntity
                {
                    Title = string.Join(",", HeaderShort.Split(',').Take(2).ToArray()),
                    Body = string.Format("{0}{1}", anzahlTxt, sumPreisTxt),
                };
            }
        }

        public void InitAuswahlDienstleistungen(List<Dienstleistung> dienstleistungen)
        {
            if (GewaehlteDienstleistungenString.IsNullOrEmpty())
                GewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => dl.IstStandard).Select(dl => dl.ID).ToList());

            if (NichtGewaehlteDienstleistungenString.IsNullOrEmpty())
                NichtGewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => !dl.IstStandard).Select(dl => dl.ID).ToList());
        }
    }
}
