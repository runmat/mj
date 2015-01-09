using System.ComponentModel.DataAnnotations.Schema;
using GeneralTools.Models;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    [Table("KennzeichGroesse")]
    public class Kennzeichengroesse
    {
        [SelectListKey]
        public int Id { get; set; }

        public int? MatNr { get; set; }

        [SelectListText]
        public string Groesse { get; set; }

        public int? Position { get; set; }
    }
}
