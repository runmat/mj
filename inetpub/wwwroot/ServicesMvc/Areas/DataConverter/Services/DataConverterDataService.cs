using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.General.Services;
using SapORM.Contracts;

namespace CkgDomainLogic.DataConverter.Services
{
    public class DataConverterDataService : CkgGeneralDataServiceSAP, IDataConverterDataService
    {
        public DataConverterDataService(ISapDataService sap)
            : base(sap)
        {
        }

    }
}