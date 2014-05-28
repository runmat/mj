using System.ComponentModel.DataAnnotations;
using GeneralTools.Models;
using GeneralTools.Resources;

namespace CkgDomainLogic.Equi.Models
{
    /// <summary>
    /// Zielort
    /// </summary>
    public class Zielort
    {
        [LocalizedDisplay(LocalizeConstants.Id)]
        public string Id { get; set; }

        [LocalizedDisplay(LocalizeConstants._Bezeichnung)]
        public string Bezeichnung { get; set; }

        public Zielort()
        {
            this.Id = "";
            this.Bezeichnung = "";
        }

        public Zielort(string id, string bez)
        {
            this.Id = id;
            this.Bezeichnung = bez;
        }
    }
}
