using System;
using System.Data;

namespace SapORM.Contracts
{
    [CLSCompliant(true)]
    public interface IDynSapProxyFactory
    {
        IDynSapProxyCache CreateProxyCache(string bapiName, ISapConnection sapConnection, IDynSapProxyFactory dynSapProxyFactory);

        IDynSapProxyObject CreateProxyObject(string bapiName, DateTime sapDatum, DataTable impStruktur, DataTable expStruktur);
    }
}
