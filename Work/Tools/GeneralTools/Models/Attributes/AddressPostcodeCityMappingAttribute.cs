using System;

namespace GeneralTools.Models
{
    public class AddressPostcodeCityMappingAttribute : Attribute 
    {
        public string PostCodePropertyName { get; private set; }

        public string CountryPropertyName { get; private set; }


        public AddressPostcodeCityMappingAttribute(string postCodePropertyName, string countryPropertyName = null)
        {
            PostCodePropertyName = postCodePropertyName;
            CountryPropertyName = countryPropertyName;
        }
    }
}

