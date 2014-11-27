using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.KroschkeZulassung.Models
{
    [Table("KennzeichGroesse")]
    public class Kennzeichengroesse
    {
        public int Id { get; set; }

        public int MatNr { get; set; }

        public string Groesse { get; set; }

        public int Position { get; set; }
    }
}
