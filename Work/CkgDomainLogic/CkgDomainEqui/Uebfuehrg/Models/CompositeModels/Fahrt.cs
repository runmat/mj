using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace CkgDomainLogic.Uebfuehrg.Models
{
    public class Fahrt
    {
        public string TypName { get; set; }

        public string TypNr { get; set; }

        public string Title { get; set; }

        public string FahrtIndex { get; set; }

        [XmlIgnore]
        public string FahrzeugIndex { get { return Fahrzeug == null ? "1" : Fahrzeug.FahrzeugIndex; } }

        [XmlIgnore]
        public string ReihenfolgeTmp { get { return FahrtIndex.PadLeft(6, '0'); } }

        public Fahrzeug Fahrzeug { get; set; }

        public Adresse StartAdresse { get; set; }

        public Adresse ZielAdresse { get; set; }

        public string VorgangsNummer { get; set; }

        public List<Dienstleistung> AvailableDienstleistungen { get; set; }


        [XmlIgnore]
        public DateTime? Datum { get { return ZielAdresse == null ? null : ZielAdresse.Datum; } }

        [XmlIgnore]
        public string Uhrzeit { get { return ZielAdresse == null ? "" : ZielAdresse.Uhrzeitwunsch; } }

        [XmlIgnore]
        public string EmptyString { get { return ""; } }

        [XmlIgnore]
        public bool IstZusatzFahrt { get { return StartAdresse.GetAlleTransportTypen().Any(at => at.IstZusatzTransport && at.ID == TypNr); } }

        [XmlIgnore]
        public bool IstHauptFahrt { get { return !IstZusatzFahrt; } }
    }
}
