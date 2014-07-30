using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class DienstleistungsAuswahl : CommonUiModel
    {
        public string FahrtIndex
        {
            get { return _fahrtIndex; }
            set
            {
                _fahrtIndex = value;
                Bemerkungen.FahrtIndex = value;
            }
        }

        public string FahrtTyp { get; set; }

        public Bemerkungen Bemerkungen { get; set; }

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
        private string _fahrtIndex;

        public string NichtGewaehlteDienstleistungenString
        {
            get { return _nichtGewaehlteDienstleistungenString.NotNullOrEmpty(); }
            set { _nichtGewaehlteDienstleistungenString = value; }
        }

        [XmlIgnore]
        public List<Dienstleistung> NichtGewaehlteDienstleistungen { get { return AvailableDienstleistungen.Where(dl => NichtGewaehlteDienstleistungenString.Split(',').Contains(dl.ID)).ToList(); } }

        [XmlIgnore]
        public bool DienstleistungenInitialized
        {
            get
            {
                return AvailableDienstleistungen != null && (!string.IsNullOrEmpty(GewaehlteDienstleistungenString) || !string.IsNullOrEmpty(NichtGewaehlteDienstleistungenString));
            }
        }


        public DienstleistungsAuswahl()
        {
            Bemerkungen = new Bemerkungen();
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
