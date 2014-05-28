﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.Logs.Models;
using GeneralTools.Log.Models.MultiPlatform;

namespace CkgDomainLogic.Logs.Services
{
    public class LogsSqlDbContext : DbContext
    {
        protected string ConnectionString { get; private set; }

        public DbSet<SapLogItem> SapLogItems { get; set; }

        public DbSet<MpApplicationTranslated> MpApplicationsTranslated { get; set; }

        public DbSet<MpWebUser> MpWebUsers { get; set; }

        public DbSet<MpCustomer> MpCustomers { get; set; }


        public IEnumerable<SapLogItem> GetSapLogItems(SapLogItemSelector sapLogItemSelector)
        {
            return Database.SqlQuery<SapLogItem>(sapLogItemSelector.GetSqlSelectStatement());
        }


        public LogsSqlDbContext(string connectionString) 
            : base(connectionString)
        {
            ConnectionString = connectionString; 
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<LogsSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
