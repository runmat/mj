namespace SapORM.Contracts
{
    public class SapConnectionWrapper
    {
        public bool ProdSAP { get; set; }

        public string SAPAppServerHost { get; set; }

        public int SAPSystemNumber { get; set; }

        public string SAPClient { get; set; }

        public string SAPUsername { get; set; }

        public string SAPPassword { get; set; }

        public string ErpConnectLicense { get; set; }

        public string SqlServerConnectionString { get; set; }
    }
}
