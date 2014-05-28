using System.Xml;
using Mobile.Rest;

namespace Mobile.Interfaces
{
    /// <summary>
    /// Kommunikation mit Mobile via REST Implementierungen
    /// </summary>
    public interface IMobileRest
    {
        /// <summary>
        /// Erstelle einen Inserat für einen Fahrzeug
        /// </summary>
        /// <param name="ad">Vollständige Ineseratsdaten</param>
        /// <returns>Bei Erfolg erhalte ich den Link zum Inserat</returns>
        Result<object> PostAd(ad ad);

        /// <summary>
        /// Ermittle einen Inserat aus dem mobile.de Dienst
        /// </summary>
        /// <param name="inseratId"></param>
        /// <returns>Result objekt mit den Daten des Inserats</returns>
        Result<object> GetAd(string inseratId);
    }
}
