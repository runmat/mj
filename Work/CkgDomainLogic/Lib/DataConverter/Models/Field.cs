using System.Collections.Generic;

namespace CkgDomainLogic.DataConverter.Models
{
    public class Field
    {
        public string Id { get; set; }
        public string Bezeichnung { get; set; }
        public string Feldgruppe { get; set; }

        public List<string> Records { get; set; }   // All values (records) of this field

        public Field(string bezeichnung, string feldgruppe = "")
        {
            Id = System.Guid.NewGuid().ToString();
            Bezeichnung = bezeichnung;
            Feldgruppe = feldgruppe;
            Records = new List<string>();
        }
    }    
}