using System;
using GeneralTools.Contracts;
using GeneralTools.Services;

namespace CkgDomainLogic.DomainCommon.Models
{
    public class VersandAuftragsAnlage : IAddressStreetHouseNo   
    {   
        public string KundenNr { get; set; }

        public string VIN { get; set ; } 

        public bool BriefVersand { get; set; }
         
        public bool SchluesselVersand { get; set; }

        public string StuecklistenKomponente { get; set; }

        public bool AbmeldeKennzeichen { get; set; }

        public string AbcKennzeichen { get; set; } 

        public string MaterialNr { get; set; }

        public DateTime? DadAnforderungsDatum { get; set; }

        public string ErfassungsUserName { get; set; }

        public string Bemerkung { get; set; }

        public string Versandgrund { get; set; }

        public string Mahnverfahren { get; set; }

        public bool SchluesselKombiVersand { get; set; }


        #region Versand Adresse

        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string StrasseHausNr { get { return AddressService.FormatStreetAndHouseNo(this); } }

        public string HausNr { get; set; }

        public string PLZ { get; set; }

        public string Ort { get; set; }

        public string Land { get; set; }

        #endregion
    }
}
