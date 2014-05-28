using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Standort
    /// </summary>
    public class Standort
    {
        [LocalizedDisplay(LocalizeConstants.Id)]
        public string Id { get; set; }

        [LocalizedDisplay(LocalizeConstants.Location)]
        public string Bezeichnung { get; set; }

        public Standort()
        {
            this.Id = "";
            this.Bezeichnung = "";
        }

        public Standort(string id, string stort)
        {
            this.Id = id;
            this.Bezeichnung = stort;
        }
    }
}
