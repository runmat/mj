using System.Collections.Generic;

namespace CkgDomainLogic.DataConverter.Models
{
    public class ProcessStructure
    {
        public string StructureName { get; set; }

        public List<Field> Fields { get; set; }

        public ProcessStructure()
        {
            Fields = new List<Field>();
        }
    }
}
