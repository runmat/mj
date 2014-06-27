using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
namespace CkgDomainLogic.Autohaus.Services
{
    public class FahrzeugakeDataServiceSQL : CkgGeneralDataService, IFahrzeugakteDataService
    {
        private static AutohausSqlDbContext CreateDbContext()
        {
            return new AutohausSqlDbContext();
        }

        public List<BeauftragteZulassung> BeauftragteZulassungenGet(int fahrzeugId)
        {
            var ct = CreateDbContext();
            return ct.BeauftragteZulassungen.Where(e => e.FahrzeugID == fahrzeugId).ToList();
        }
   
    }
}
