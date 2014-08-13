using System;

namespace SapORM.Contracts
{
    [CLSCompliant(true)]
    public interface IDynSapProxyCache
    {
        string BapiName { get; set; }

        IDynSapProxyObject GetProxy();

        void GetSerializedBapiStructuresForBapiCheck(string sapFunction, ref byte[] importStructure, ref byte[] exportStructure);
    }
}