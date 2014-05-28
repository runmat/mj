namespace RefImplBibl.Models
{
    public class EQUI
    {
        public int EQUI_NR { get; set; }
        public string NAME { get; set; }

        public Fahrzeug Fahrzeug { get; set; }

        public override string ToString()
        {
            return string.Format("EQUI_NR:{0} NAME:{1}", EQUI_NR, NAME);
        }
    }
}