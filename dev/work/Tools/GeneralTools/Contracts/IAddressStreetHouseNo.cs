namespace GeneralTools.Contracts
{
    public interface IAddressStreetHouseNo
    {
        string Strasse { get; set; }
        
        string HausNr { get; set; }
        
        string StrasseHausNr { get; }
    }
}
