
namespace CkgDomainLogic.Finance.Models
{
    public enum Selektionsart
    {
        GebuehrenInBearbeitung,
        AlleGebuehren
    }

    public class AuftragSucheSelektionsart
    {
        public Selektionsart Typ { get; set; }
        
        public string Bezeichnung { get; set; }

        public AuftragSucheSelektionsart(Selektionsart typ, string bez)
        {
            Typ = typ;
            Bezeichnung = bez;
        }
    }
}
