using System;

namespace SapORM.Contracts
{
    [CLSCompliant(true)]
    public interface ISapDataServiceFactory
    {
        ISapDataService Create();
    }
}
