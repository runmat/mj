using System.Collections.Generic;

namespace CarDocu.Models
{
    public class EnterpriseSettings
    {
        private List<DocumentType> _documentTypes = new List<DocumentType>();
        public List<DocumentType> DocumentTypes
        {
            get { return _documentTypes; }
            set { _documentTypes = value; }
        }
    }
}
