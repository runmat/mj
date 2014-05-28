namespace AutoAct.Entities
{
    /// <summary>
    /// Datei dient als Wrapper für die JSON Serialisierung.  Die AutoAct API erwartet dass die Daten des Fahrzeugs als Wert des Members Vehicle übergeben werden.
    /// </summary>
    public class VehiclePayLoad
    {
        public Vehicle Vehicle { get; set; }
    }
}