using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using GeneralTools.Log.Models.MultiPlatform;

namespace LogMaintenance.Services
{
    /// <summary>
    /// Multi Database Platform Context 
    /// Entities in this context reside in different sql server types, 
    /// i. e. MySql and MS SQL Server
    /// </summary>
    public class MultiDbPlatformContext : DbContext
    {
        protected string ConnectionString { get; private set; }

        public DbSet<MpWebUser> WebUsers { get; set; }

        public DbSet<MpCustomer> Customers { get; set; }

        public DbSet<MpApplicationTranslated> ApplicationsTranslated { get; set; }


        public IEnumerable<T> GetData<T>(string tableName) where T : class
        {
            return Database.SqlQuery<T>(string.Format("SELECT * FROM {0}", tableName));
        }

        public bool AddData<T>(T model) where T : class
        { 
            var dbSet = this.Set<T>();
             
            try { dbSet.Add(model); }
            catch (InvalidOperationException) { return false; }

            return true;
        }


        public MultiDbPlatformContext(string connectionString)
            : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<MultiDbPlatformContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
