// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using CkgDomainLogic.VersEvents.Contracts;
using CkgDomainLogic.VersEvents.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.VersEvents.Services
{
    public class VersEventSqlDbContext : DbContext
    {
        public DbSet<VersEvent> VersEvents { get; set; }

        public DbSet<VersEventOrt> VersEventOrte { get; set; }

        public DbSet<VersEventOrtBox> VersEventOrtBoxen { get; set; }


        public DbSet<Vorgang> Vorgaenge { get; set; }

        public DbSet<VorgangTermin> VorgangTermine { get; set; }

        //public IEnumerable<Vorgang> GetVorgaenge()
        //{
        //    return Database.SqlQuery<Vorgang>("SELECT * FROM VersEventOrtBoxVorgang");
        //}

        //public IEnumerable<VorgangTermin> GetVorgangTermine()
        //{
        //    return Database.SqlQuery<VorgangTermin>("SELECT * FROM VersEventOrtBoxVorgangTermin");
        //}


        public VersEventSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<VersEventSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        protected override DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            // Let our ViewModels and Models take full control over all validation issues ;-)
            return null;
        }
    }
}
