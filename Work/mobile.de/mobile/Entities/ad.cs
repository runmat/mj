public class ad
{
    /// <summary>
    /// Belegnummer für mobile.de
    /// BAPI Feld: Equipment
    /// wird aus EQUI-EQUNR ermittelt
    /// </summary>
    public string SellerInventoryKeyValue { get; set; } //  = "17301829";

    /// <summary>
    /// Titel der Anzeige
    /// BAPI Feld: Inseratstitel
    /// </summary>
    public string DescriptionValue { get; set; } // = "Fahrzeug Beschreibung";

    /// <summary>
    /// Text zum fahrzeug, muss unterUmständen zusammen gesetzt werden aus den einzelnen Bausteinen die aus SAP kommen
    /// </summary>
    public string VehicleModelDescriptionValue { get; set; } // = "Model Beschreibung";

    /// <summary>
    /// Kilometerstand
    /// BAPI Feld: Kilometerstand	
    /// Fall keine Angabe vorhanden Dummy Wert 1000000 verwenden
    /// </summary>
    public string VehicleMileageValue { get; set; } // = "14526";

    /// <summary>
    /// Erstzulassungsdatum
    /// BAPI Feld: Erstzulassungsdatum
    /// mobile.de erfordert den Wert im Format yyyy-MM
    /// </summary>
    public string VehicleFirstRegistrationValue { get; set; } // = "2013-06";

    /// <summary>
    /// Wird von BAPI noch nicht geliefert
    /// </summary>
    public string VehicleKbaHsnValue { get; set; } // = "0588";

    /// <summary>
    /// Wird von BAPI nocht nicht geliefert
    /// </summary>
    public string VehicleKbaTsnValue { get; set; } // = "AHZ";

    /// <summary>
    /// Wird von BAPI nicht geliefert
    /// </summary>
    public string PriceGrossPricesConsumerPriceAmountValue { get; set; } // = "32000";


    public string VehicleMakeValue { get; set; } // = "AUDI";
    public string VehicleModelValue { get; set; } // = "Andere";

    public string Visibility { get; set; } // = "RESERVED"

}