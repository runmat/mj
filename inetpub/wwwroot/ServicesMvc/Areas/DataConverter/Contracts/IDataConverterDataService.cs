using System.Collections.Generic;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Database.Models;

namespace CkgDomainLogic.DataConverter.Contracts
{
    public interface IDataConverterDataService : ICkgGeneralDataService
    {
        List<Customer> GetCustomers();

        List<string> GetProcessStructureNames();

        DataConverterProcessStructure GetProcessStructure(string processName);

        List<DataConverterDataMapping> GetDataMappings(DataMappingSelektor selektor);
    }
}