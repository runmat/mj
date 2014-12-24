using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleDirections.Contracts
{
    public interface IAddress
    {
        string Header { get; set; }

        string PostCode { get; set; }
        string City { get; set; }
        string Street { get; set; }

        bool UseGeoLocation { get; set; }
        string GeoLocation { get; set; }

        string AsString { get; }
        string AsFriendlyString { get; }

        bool Valid  { get; }
    }
}
