namespace AutoAct.Interfaces
{
    /// <summary>
    /// Ermittelt Konfigurationsdaten für die Anwendung
    /// </summary>
    public interface ISettingsReader
    {
        string RootFoleder { get; }
        string Logon { get; }
        string Password { get; }
    }
}
