using System;

namespace SapORM.Contracts
{
    [CLSCompliant(true)]
    public interface ISapConnection
    {
        bool ProdSAP { get; }

        string SAPAppServerHost { get;  }
        
        int SAPSystemNumber { get;  }
        
        string SAPClient { get;  }
        
        string SAPUsername { get;  }

        string SAPPassword { get; }

        string ErpConnectLicense { get; }

        string SqlServerConnectionString { get; }
    }
}
