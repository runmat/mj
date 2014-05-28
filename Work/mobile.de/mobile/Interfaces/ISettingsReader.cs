namespace Mobile.Interfaces
{
    /// <summary>
    /// Ermittelt Konfigurationsdaten für die Anwendung
    /// </summary>
    public interface ISettingsReader
    {
        string DocumentFolder { get; }
        string SellerId { get; }
        string Token { get; }
        string MobiledeUrl { get; }
        bool SapProdSystem { get; }
    }
}
