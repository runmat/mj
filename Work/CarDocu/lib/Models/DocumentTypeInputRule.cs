using System.Collections.Generic;
using System.Xml.Serialization;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class DocumentTypeInputRule
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public List<int> AllowedLengths { get; set; }

        public int FileNameAbbreviationAllowedMinimumLength { get; set; }

        private string _inputRuleName;
        [XmlIgnore] 
        public string InputRuleName
        {
            get
            {
                if (_inputRuleName.IsNotNullOrEmpty())
                    return _inputRuleName;

                if (AllowedLengths.None())
                    return "";

                return string.Format("{0}-ID:   {1} Zeichen erlaubt", Name, string.Join(" oder ", AllowedLengths));
            }
            set { _inputRuleName = value; }
        }
    }
}
