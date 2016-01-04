using System.Collections.Generic;

namespace GeneralTools.Models
{
    public class Field
    {
        public string Guid { get; set; }
        public string Caption { get; set; }
        public FieldType FieldType { get; set; }
        public bool IsUsed { get; set; }

        public List<string> Records { get; set; }   // All values (records) of this field

        public Field()
        {
            Guid = System.Guid.NewGuid().ToString();
            Records = new List<string>();
        }
    }    
}