using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
namespace CkgDomainLogic.Autohaus.Services
{
    public class FahrzeugakteDataServiceSQL : CkgGeneralDataService, IFahrzeugakteDataService
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
   
        public void BeauftragteZulassungSave(int fahrzeugId, string referenzNr, string fahrgestellNr, string zb2Nr, DateTime? zulassungsDatum)
        {
            var ct = CreateDbContext();

            var newItem = ct.BeauftragteZulassungen.Create();
            newItem.FahrzeugID = fahrzeugId;
            newItem.ReferenzNr = referenzNr;
            newItem.FahrgestellNr = fahrgestellNr;
            newItem.ZBIINr = zb2Nr;
            if (zulassungsDatum.HasValue)
            {
                newItem.ZulassungsDatum = zulassungsDatum.Value;
            }
            newItem.AuftragsDatum = DateTime.Today;
            newItem.WebUser = LogonContext.UserName;
            ct.BeauftragteZulassungen.Add(newItem);

            ct.SaveChanges();
        }
    }
}
