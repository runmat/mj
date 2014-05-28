
namespace CkgDomainLogic.Leasing.Models
{
    public class ZB1KopieSucheKunde
    {
        public int ID { get; set; }
        
        public string Bezeichnung { get; set; }

        public ZB1KopieSucheKunde(int id, string bez)
        {
            ID = id;
            Bezeichnung = bez;
        }
    }
}
