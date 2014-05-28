using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CkgDomainLogic.Charts.Models
{
    [Table("KbaPlzKgs")]
    public class KbaPlzKgs
    {
        [Key]
        public int ID { get; set; }

        public string KGS { get; set; }

        public string PLZ { get; set; }
        
        public string Ort { get; set; }

        public int Jahr { get; set; }

        public int Monat { get; set; }

        public int FahrzeugGruppe { get; set; }

        public int HerstellerCode { get; set; }

        public string HerstellerName { get; set; }

        public string FabrikatName { get; set; }

        public string ModellName { get; set; }

        public int FahrzeugAnzahl { get; set; }
    }
}
