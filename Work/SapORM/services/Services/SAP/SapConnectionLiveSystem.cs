using System;
using SapORM.Contracts;

namespace SapORM.Services
{
    class SapConnectionLiveSystem : ISapConnection 
    {
        public bool ProdSAP { get { return false; } }

        public string SAPAppServerHost
        {
            get { return "192.168.10.151"; }
        }

        public int SAPSystemNumber
        {
            get { return 0; }
        }

        public string SAPClient 
        {
            get { return "10"; }
        }

        public string SAPUsername 
        {
            get { return "web_rfc"; }
        }

        public string SAPPassword
        {
            get { return "17LOUNGE"; }
        }

        public string ErpConnectLicense
        {
            get { return "5DVZ5588DC-25444"; }
        }

        public string SqlServerConnectionString
        {
            get { throw new NotSupportedException("SqlServerConnectionString is not supported in class 'SapConnectionLiveSystem'"); }
        }
    }
}
