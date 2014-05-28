using System;
using SapORM.Contracts;

namespace SapORM.Services
{
    public class DynSapProxyErp35NoCacheFactory : IDynSapProxyFactory
    {

        IDynSapProxyCache IDynSapProxyFactory.CreateProxyCache(string bapiName, ISapConnection sapConnection, IDynSapProxyFactory dynSapProxyFactory)
        {
            return new DynSapProxyNoCache(bapiName, sapConnection, dynSapProxyFactory);
        }

        IDynSapProxyObject IDynSapProxyFactory.CreateProxyObject(string bapiName, DateTime sapDatum, System.Data.DataTable impStruktur, System.Data.DataTable expStruktur)
        {
            return new DynSapProxyObjectErp35(bapiName, sapDatum, impStruktur, expStruktur);
        }

    }
}
