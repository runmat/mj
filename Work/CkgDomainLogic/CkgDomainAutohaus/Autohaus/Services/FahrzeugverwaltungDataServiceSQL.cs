using System;
using System.Collections.Generic;
using System.Linq;
using CkgDomainLogic.General.Services;
using CkgDomainLogic.Autohaus.Contracts;
using CkgDomainLogic.Autohaus.Models;
using GeneralTools.Models;

namespace CkgDomainLogic.Autohaus.Services
{
    public class FahrzeugverwaltungDataServiceSQL : CkgGeneralDataService, IFahrzeugverwaltungDataService
    {
        private static AutohausSqlDbContext CreateDbContext()
        {
            return new AutohausSqlDbContext();
        }

        public List<Fahrzeug> FahrzeugeGet()
        {
            var ct = CreateDbContext();
            var kundenNr = LogonContext.KundenNr;
            return ct.Fahrzeuge.Where(e => e.KundenNr == kundenNr && e.LoeschDatum == null).ToList();
        }

        public Fahrzeug FahrzeugAdd(Fahrzeug item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            ct.Fahrzeuge.Add(item);
            ct.SaveChanges();

            return item;
        }

        public bool FahrzeugDelete(int id)
        {
            var ct = CreateDbContext();
            var savedItem = ct.Fahrzeuge.FirstOrDefault(e => e.ID == id);
            if (savedItem == null)
                return false;

            savedItem.LoeschDatum = DateTime.Now;
            savedItem.LoeschUser = LogonContext.UserName;

            ct.SaveChanges();

            return true;
        }

        public Fahrzeug FahrzeugSave(Fahrzeug item, Action<string, string> addModelError)
        {
            var ct = CreateDbContext();
            var savedItem = ct.Fahrzeuge.FirstOrDefault(e => e.ID == item.ID);
            if (savedItem == null)
                return null;

            ModelMapping.Copy(item, savedItem);
            ct.SaveChanges();

            return savedItem;
        }

    }
}
