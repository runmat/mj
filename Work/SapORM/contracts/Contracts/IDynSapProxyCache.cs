using System;

namespace SapORM.Contracts
{
    [CLSCompliant(true)]
    public interface IDynSapProxyCache
    {
        string BapiName { get; set; }

        IDynSapProxyObject GetProxy();
    }
}