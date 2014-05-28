using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Betriebsnummer
    /// </summary>
    public class Betriebsnummer
    {
        [LocalizedDisplay(LocalizeConstants.Id)]
        public string Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.FactoryNo)]
        public string Bezeichnung { get; set; }

        public Betriebsnummer()
        {
            this.Id = "";
        }

        public Betriebsnummer(string id, string bezeichnung)
        {
            this.Id = id;
            this.Bezeichnung = bezeichnung;
        }
    }
}
