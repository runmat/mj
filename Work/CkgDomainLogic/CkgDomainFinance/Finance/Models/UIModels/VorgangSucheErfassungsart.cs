
namespace CkgDomainLogic.Finance.Models
{
    public enum Erfassungsart
    {
        Gutachterwert,
        Vermarktungserloese
    }

    public class VorgangSucheErfassungsart
    {
        public Erfassungsart Typ { get; set; }
        
        public string Bezeichnung { get; set; }

        public VorgangSucheErfassungsart(Erfassungsart typ, string bez)
        {
            Typ = typ;
            Bezeichnung = bez;
        }
    }
}
