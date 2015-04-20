// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Contracts;
using System.Linq;
using CkgDomainLogic.General.Contracts;
using CkgDomainLogic.General.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class GridAdminDataServiceSql : IGridAdminDataService 
    {
        static GridAdminSqlDbContext CreateDbContext(string connectionString)
        {
            return new GridAdminSqlDbContext(connectionString);
        }
    }
}
