using System.Collections.Generic;
using System.Xml.Serialization;
using GeneralTools.Models;

namespace CarDocu.Models
{
    public class DomainLocation : ModelBase 
    {
        private string _sapCode;
        public string SapCode
        {
            get { return _sapCode; }
            set { _sapCode = value; SendPropertyChanged("SapCode"); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; SendPropertyChanged("Name"); }
        }

        [XmlIgnore]
        public static List<DomainLocation> FixedStartupDomainLocations
        {
            get
            {
                return new List<DomainLocation>
                           {
                               new DomainLocation {SapCode = "0001", Name = "Standard Standort" },
                           };
            }
        }
    }
}
