using AutoAct.Bapi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SapORM.Models;

namespace AutoActTests.Bapi
{
    [TestClass]
    public class KundeTests
    {
        private const string Anmeldename = "dsfrgrt/%&/%";
        private const string Passwort = "gfhjhukt&/%&/";
        private const string Kundennumer = "123412342345";
        private const string Strasse = "sdfgrtTUIRTZ";
        private const string Plz = "12345";
        private const string Ort = "München";
        private const string Land = "DE";
        private const int AnzahlDerBilder = 6;

        [TestMethod]
        public void InvokingConstructorForKundeReadsContentFromQuelle()
        {
            // Arrange
            Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB quelle = new Z_DPM_READ_ZDAD_AUFTR_006.GT_WEB
                {
                    NAME1 = Anmeldename,
                    POS_TEXT = Passwort,
                    KUNNR = Kundennumer,
                    STRAS = Strasse,
                    LAND1 = Land,
                    PSTLZ = Plz,
                    ORT01 = Ort,
                    INTNR = AnzahlDerBilder.ToString()
                };

            // Act
            var result = new Kunde(quelle);

            // Assert
            Assert.AreEqual(Anmeldename, result.Anmeldename);
            Assert.AreEqual(Passwort, result.Passwort);
            Assert.AreEqual(AnzahlDerBilder, result.AnzahlDerBilder);
            Assert.IsTrue(result.Adresse.Contains(Strasse));
            Assert.IsTrue(result.Adresse.Contains(Plz));
            Assert.IsTrue(result.Adresse.Contains(Ort));
            Assert.IsTrue(result.Adresse.Contains(Land));
        }
    }
}
