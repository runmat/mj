using GeneralTools.Contracts;
using GeneralTools.Services;
using NUnit.Framework;

namespace GeneralTools.UnitTests.Misc
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void AddressServiceTest()
        {
            var address = new Adresse {Strasse = "Brahmsstr. 30", HausNr = ""};

            AddressService.ApplyStreetAndHouseNo(address);
            Assert.AreEqual("Brahmsstr.", address.Strasse, "AddressService.ApplyStreetAndHouseNo, Strasse extrahieren fehlgeschlagen");
            Assert.AreEqual("30", address.HausNr, "AddressService.ApplyStreetAndHouseNo, Haus-Nr extrahieren fehlgeschlagen");

            AddressService.ApplyStreetAndHouseNo(address);
            Assert.AreEqual("Brahmsstr.", address.Strasse, "AddressService.ApplyStreetAndHouseNo, Strasse ERNEUT extrahieren fehlgeschlagen");
            Assert.AreEqual("30", address.HausNr, "AddressService.ApplyStreetAndHouseNo, Haus-Nr ERNEUT extrahieren fehlgeschlagen");
        }
    }

    public class Adresse : IAddressStreetHouseNo
    {
        public string Name1 { get; set; }

        public string Name2 { get; set; }

        public string Strasse { get; set; }

        public string HausNr { get; set; }

        public string StrasseHausNr
        {
            get { return AddressService.FormatStreetAndHouseNo(this); }
        }
    }
}
