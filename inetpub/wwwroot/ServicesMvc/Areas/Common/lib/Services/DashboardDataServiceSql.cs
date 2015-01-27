// ReSharper disable RedundantUsingDirective
using System.Collections.Generic;
using CkgDomainLogic.DomainCommon.Contracts;
using System.Linq;
using GeneralTools.Models;

namespace CkgDomainLogic.DomainCommon.Services
{
    public class DashboardDataServiceSql : IDashboardDataService 
    {
        public IEnumerable<string> DashboardItems
        {
            get
            {
                return new List<string>
                    {
                        "Walter", "Zabel",
                    };
            }
        }
    }
}
