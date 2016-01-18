using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using CkgDomainLogic.DomainCommon.Models;
using CkgDomainLogic.General.Services;
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
            get { return AlleDienstleistungen == null ? null : AlleDienstleistungen.Where(dl => dl.TransportTyp == FahrtTyp).OrderBy(d => d.Name).ToList(); }
        }


        // Gewählte Dienstleistungen
        private string _gewaehlteDienstleistungenString;
        public string GewaehlteDienstleistungenString
        {
            get { return _gewaehlteDienstleistungenString.NotNullOrEmpty(); }
            set
            {
                _gewaehlteDienstleistungenString = value;

                InitDienstleistungenFlags();
            }
        }

        [XmlIgnore]
        public List<Dienstleistung> GewaehlteDienstleistungen { get { return AvailableDienstleistungen.Where(dl => GewaehlteDienstleistungenString.Split(',').Contains(dl.ID)).ToList(); } }

        [XmlIgnore]
        public List<Dienstleistung> NichtGewaehlteDienstleistungen { get { return AvailableDienstleistungen.Except(AlleDienstleistungen).ToList(); } }

        [XmlIgnore]
        public List<WebUploadProtokoll> UploadProtokolle { get; set; }


        public DienstleistungsAuswahl()
        {
            Bemerkungen = new Bemerkungen();
        }

        public void InitDienstleistungenFlags()
        {
            if (AvailableDienstleistungen == null)
                return;

            AvailableDienstleistungen.ForEach(dl => dl.IstGewaehlt = false);
            GewaehlteDienstleistungen.ForEach(dl => dl.IstGewaehlt = true);
        }

        public void InitDienstleistungen(List<Dienstleistung> dienstleistungen, string fahrtTyp = null, bool resetGewaehlteDienstleistungen = false, List<WebUploadProtokoll> protokolle = null)
        {
            if (fahrtTyp != null)
                FahrtTyp = fahrtTyp;

            AlleDienstleistungen = dienstleistungen.Where(dl => dl.TransportTyp == FahrtTyp).ToListOrEmptyList().Copy((src, dst) =>
                {
                    dst.FahrtIndex = FahrtIndex;
                    dst.TransportTyp = FahrtTyp;
                });

            if (resetGewaehlteDienstleistungen)
            {
                if (dienstleistungen.Any(dl => dl.IstGewaehlt))
                    GewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => dl.IstGewaehlt).Select(dl => dl.ID).ToList());
                else
                    GewaehlteDienstleistungenString = "";
            }

            InitDienstleistungenFlags();

            if (protokolle != null)
            {
                UploadProtokolle = protokolle;
                UploadProtokolle.ForEach(p => p.FahrtIndex = FahrtIndex);
            }
        }

        public override string GetSummaryString()
        {
            var summaryString = string.Join("<br />", GewaehlteDienstleistungen.Select(dienstleistung => dienstleistung.Name));

            if (UploadProtokolle.Any(p => !string.IsNullOrEmpty(p.Dateiname)))
            {
                summaryString += "<br /><br />";
                summaryString += "<b>" + Localize.Protocols + "</b><br />";
                summaryString += string.Join("<br />", UploadProtokolle.Where(p => !string.IsNullOrEmpty(p.Dateiname)).Select(up => string.Format("{0}: {1}", up.Protokollart, up.Dateiname)));
            }

            return summaryString;
        }
    }
}
