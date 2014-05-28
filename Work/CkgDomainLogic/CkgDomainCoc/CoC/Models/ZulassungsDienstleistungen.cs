// ReSharper disable RedundantUsingDirective
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using GeneralTools.Services;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.CoC.Models
{
    public class ZulassungsDienstleistungen 
    {
        [XmlIgnore]
        public List<ZulassungsDienstleistung> AlleDienstleistungen { get; private set; }

        [XmlIgnore]
        public List<ZulassungsDienstleistung> AvailableDienstleistungen { get { return AlleDienstleistungen; } }


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
        public List<ZulassungsDienstleistung> GewaehlteDienstleistungen { get { return AvailableDienstleistungen.Where(dl => GewaehlteDienstleistungenString.Split(',').Contains(dl.ID)).ToList(); } }
        
        [XmlIgnore]
        public List<ZulassungsDienstleistung> NichtGewaehlteDienstleistungen { get { return AvailableDienstleistungen.Except(AlleDienstleistungen).ToList(); } }


        public void InitDienstleistungen(List<ZulassungsDienstleistung> dienstleistungen = null)
        {
            if (dienstleistungen != null)
                AlleDienstleistungen = dienstleistungen;

            if (GewaehlteDienstleistungenString.IsNullOrEmpty())
                GewaehlteDienstleistungenString = string.Join(",", AvailableDienstleistungen.Where(dl => dl.IstGewaehlt).Select(dl => dl.ID).ToList());
        }

        public string GetSummaryString()
        {
            return string.Join("<br />", GewaehlteDienstleistungen.Select(dienstleistung => dienstleistung.Name));
        }
    }
}
