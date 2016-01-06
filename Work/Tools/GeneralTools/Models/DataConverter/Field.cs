using System.Collections.Generic;

namespace GeneralTools.Models
{
    public class Field
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<string> Records { get; set; }   // All values (records) of this field

        public Field(string bezeichnung)
        {
            Id = System.Guid.NewGuid().ToString();
            Name = bezeichnung;
            Records = new List<string>();
        }
    }    
}