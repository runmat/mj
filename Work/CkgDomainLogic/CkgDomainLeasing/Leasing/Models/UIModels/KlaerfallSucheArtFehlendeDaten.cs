
namespace CkgDomainLogic.Leasing.Models
{
    public class KlaerfallSucheArtFehlendeDaten
    {
        public int ID { get; set; }
        
        public string Bezeichnung { get; set; }

        public KlaerfallSucheArtFehlendeDaten(int id, string bez)
        {
            ID = id;
            Bezeichnung = bez;
        }
    }
}
