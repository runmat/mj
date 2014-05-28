using System;
using System.Data;
using SapORM.Contracts;

namespace SapORM.Services
{
    public class DynSapProxyErp35SqlCacheFactory : IDynSapProxyFactory
    {
        public IDynSapProxyCache CreateProxyCache(string bapiName, ISapConnection sapConnection, IDynSapProxyFactory dynSapProxyFactory)
        {
            return new DynSapProxySqlDbCache(bapiName, sapConnection, dynSapProxyFactory);
        }

        public IDynSapProxyObject CreateProxyObject(string bapiName, DateTime sapDatum, DataTable impStruktur, DataTable expStruktur)
        {
            return new DynSapProxyObjectErp35(bapiName, sapDatum, impStruktur, expStruktur);
        }
    }
}
