using System.Text.RegularExpressions;
using GeneralTools.Contracts;
using GeneralTools.Models;

namespace GeneralTools.Services
{
    public class AddressService
    {
        public static void ExtractStreetAndHouseNo(string streetAndHouseNo, out string street, out string houseNo)
        {
            var match = Regex.Match(streetAndHouseNo, @"(?<strasse>.*?\.*)\s*(?<hausnr>\d+\s*.*)");
            
            street = match.Groups["strasse"].Value;
            houseNo = match.Groups["hausnr"].Value;
            
            if (string.IsNullOrEmpty(houseNo))
                street = streetAndHouseNo;
        }

        public static void ApplyStreetAndHouseNo(IAddressStreetHouseNo addressModel)
        {
            string street, houseNo;

            if (addressModel.HausNr.IsNotNullOrEmpty())
                return;

            ExtractStreetAndHouseNo(addressModel.Strasse, out street, out houseNo);

            addressModel.Strasse = street;
            addressModel.HausNr = houseNo;
        }

        public static string FormatStreetAndHouseNo(IAddressStreetHouseNo addressModel)
        {
            if (addressModel.HausNr.IsNullOrEmpty())
                return addressModel.Strasse;

            return string.Format("{0} {1}", addressModel.Strasse, addressModel.HausNr);
        }
    }
}
