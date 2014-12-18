// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using CkgDomainLogic.General.Database.Services;
using GeneralTools.Contracts;

namespace CkgDomainLogic.General.Services
{
    public class PersistenceServiceSql : IPersistenceService 
    {
        public IPersistenceOwnerKeyProvider OwnerKeyProvider { get; set; }

        public IEnumerable<IPersistableObjectContainer> GetObjectContainers(string groupKey)
        {
            var ct = CreateDbContext();



            return null;
        }

        public IPersistableObjectContainer ReadObjectContainer(string groupKey, string objectKey)
        {
            return null;
        }

        public void WriteObjectContainer(string groupKey, string objectKey, IPersistableObjectContainer objecContainer)
        {
        }

        private static DomainDbContext CreateDbContext()
        {
            return new DomainDbContext(ConfigurationManager.AppSettings["Connectionstring"], "");
        }
    }
}
