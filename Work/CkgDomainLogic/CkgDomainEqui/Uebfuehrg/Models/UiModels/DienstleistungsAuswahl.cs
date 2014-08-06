using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class DienstleistungsAuswahl : CommonUiModel
    {
        private string _fahrtIndex;
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

        [XmlIgnore]
        public List<Dienstleistung> AlleDienstleistungen { get; set; }

        [XmlIgnore]
        public List<Dienstleistung> AvailableDienstleistungen
        {
            get { return AlleDienstleistungen == null ? null : AlleDienstleistungen.Where(dl => dl.TransportTyp == FahrtTyp).ToList(); }
        }


        // Gewählte Dienstleistungen
        private string _gewaehlteDienstleistungenString;
        public string GewaehlteDienstleistungenString
        {
            get { return _gewaehlteDienstleistungenString.NotNullOrEmpty(); }
            set
            {
                _gewaehlteDienstleistungenString = value;

                if (AvailableDienstleistungen != null)
                {
                    AvailableDienstleistungen.ForEach(dl => dl.IstGewaehlt = false);
                    GewaehlteDienstleistungen.ForEach(dl => dl.IstGewaehlt = true);
                }
            }
        }

        [XmlIgnore]
        public List<Dienstleistung> GewaehlteDienstleistungen { get { return AvailableDienstleistungen.Where(dl => GewaehlteDienstleistungenString.Split(',').Contains(dl.ID)).ToList(); } }

        [XmlIgnore]
        public List<Dienstleistung> NichtGewaehlteDienstleistungen { get { return AvailableDienstleistungen.Except(AlleDienstleistungen).ToList(); } }


        public DienstleistungsAuswahl()
        {
            Bemerkungen = new Bemerkungen();
        }

        public void InitDienstleistungen(List<Dienstleistung> dienstleistungen, bool resetGewaehlteDienstleistungen = false)
        {
            AlleDienstleistungen = dienstleistungen;

            if (resetGewaehlteDienstleistungen)
                _gewaehlteDienstleistungenString = "";

            if (GewaehlteDienstleistungenString.IsNullOrEmpty())
                GewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => dl.IstGewaehlt).Select(dl => dl.ID).ToList());
        }

        public override string GetSummaryString()
        {
            return string.Join("<br />", GewaehlteDienstleistungen.Select(dienstleistung => dienstleistung.Name));
        }
    }
}
