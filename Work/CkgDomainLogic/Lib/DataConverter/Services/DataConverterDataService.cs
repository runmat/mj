using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CkgDomainLogic.DataConverter.Contracts;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
using SapORM.Contracts;

namespace CkgDomainLogic.DataConverter.Services
{
    public class DataConverterDataService : CkgGeneralDataServiceSAP, IDataConverterDataService
    {
        public DataConverterDataService(ISapDataService sap)
            : base(sap)
        {
        }

        private DomainDbContext GetDbContext()
        {
            return new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName);
        }

        public List<Customer> GetCustomers()
        {
            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], UserName))
            {
                return dbContext.GetAllCustomer();
            }
        }

        public List<string> GetProcessStructureNames()
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.GetDataConverterProcessStructureNames().ToListOrEmptyList();
            }
        }

        public DataConverterProcessStructure GetProcessStructure(string processName)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.GetDataConverterProcessStructure(processName);
            }
        }

        public bool SaveProcessStructure(DataConverterProcessStructure processStructure)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.SaveDataConverterProcessStructure(processStructure);
            }
        }

        public bool DeleteProcessStructure(string processName)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.DeleteDataConverterProcessStructure(processName);
            }
        }

        public List<DataConverterMappingInfo> GetDataMappingInfos(DataMappingSelektor selektor)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.GetDataConverterMappingInfos(selektor.CustomerId, selektor.ProzessName).OrderBy(x => x.Customername).ThenBy(x => x.Process).ThenBy(x => x.Title).ToListOrEmptyList();
            }
        }

        public DataConverterMappingData GetDataMapping(int mappingId)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.GetDataConverterMappingData(mappingId);
            }
        }

        public bool SaveDataMapping(DataConverterMappingData mapping)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.SaveDataConverterMapping(mapping);
            }
        }

        public bool DeleteDataMapping(int mappingId)
        {
            using (var dbContext = GetDbContext())
            {
                return dbContext.DeleteDataConverterMapping(mappingId);
            }
        }
    }
}