using System;
using System.Configuration;
using CkgDomainLogic.DataConverter.Models;
using CkgDomainLogic.General.Database.Models;
using CkgDomainLogic.General.Database.Services;
using Newtonsoft.Json;

namespace CkgDomainLogic.DataConverter.Services
{
    public class DataConverterStructureGenerationService
    {
        public static void GenerateStructureForObject(object sourceObject, string customDestinationStructureName = null)
        {
            SaveStructureAsJson(new DataConverterStructure(sourceObject), (string.IsNullOrEmpty(customDestinationStructureName) ? sourceObject.GetType().Name : customDestinationStructureName));
        }

        public static void GenerateStructureForType(Type sourceType, string customDestinationStructureName = null)
        {
            SaveStructureAsJson(new DataConverterStructure(sourceType), (string.IsNullOrEmpty(customDestinationStructureName) ? sourceType.Name : customDestinationStructureName));
        }

        private static void SaveStructureAsJson(DataConverterStructure structure, string destinationStructureName)
        {
            var jsonData = JsonConvert.SerializeObject(structure);

            using (var dbContext = new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], ""))
            {
                dbContext.SaveDataConverterProcessStructure(new DataConverterProcessStructure { ProcessName = destinationStructureName, DestinationStructure = jsonData });
            }
        }
    }
}
