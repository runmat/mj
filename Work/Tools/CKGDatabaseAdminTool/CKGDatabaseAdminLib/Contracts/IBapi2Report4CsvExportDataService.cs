using System.Collections.Generic;
using CKGDatabaseAdminLib.Models;
using CkgDomainLogic.General.Contracts;

namespace CKGDatabaseAdminLib.Contracts
{
    public interface IBapi2Report4CsvExportDataService : ICkgGeneralDataService
    {
        List<Bapi2Report4CsvExport> ListItems { get; }
    }
}