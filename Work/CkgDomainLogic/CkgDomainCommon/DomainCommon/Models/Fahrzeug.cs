﻿namespace CkgDomainLogic.DomainCommon.Models
{
    public class Fahrzeug
    {
        public string EquiNr { get; set; }

        public string FIN { get; set; }

        public string Kennzeichen { get; set; }

        public string Info { get; set; }

        public bool IstZugelassen { get; set; }

        public bool IstAbgemeldet { get; set; }
        
        public bool IstInAbmeldung { get; set; }

        public bool IstFehlerhaft { get; set; }
    }
}
