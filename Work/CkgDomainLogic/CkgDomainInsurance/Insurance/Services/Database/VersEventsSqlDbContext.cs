// ReSharper disable RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using CkgDomainLogic.Insurance.Contracts;
using CkgDomainLogic.Insurance.Models;
using CkgDomainLogic.General.Services;
using GeneralTools.Models;
// ReSharper restore RedundantUsingDirective

namespace CkgDomainLogic.Insurance.Services
{
    public class VersEventsSqlDbContext : DbContext
    {
        public DbSet<VersEvent> VersEvents { get; set; }

        public DbSet<VersEventOrt> VersEventOrte { get; set; }

        public DbSet<VersEventOrtBox> VersEventOrtBoxen { get; set; }


        public DbSet<Schadenfall> Schadenfaelle { get; set; }

        public DbSet<TerminSchadenfall> VersEventTermine { get; set; }

        //public IEnumerable<Vorgang> GetVorgaenge()
        //{
        //    return Database.SqlQuery<Vorgang>("SELECT * FROM VersEventOrtBoxVorgang");
        //}

        //public IEnumerable<VersEventTermin> GetVersEventTermine()
        //{
        //    return Database.SqlQuery<VersEventTermin>("SELECT * FROM VersEventOrtBoxVersEventTermin");
        //}


        public VersEventsSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<VersEventsSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        protected override DbEntityValidationResult ValidateEntity(System.Data.Entity.Infrastructure.DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            // Let our ViewModels and Models take full control over all validation issues ;-)
            return null;
        }
    }
}
