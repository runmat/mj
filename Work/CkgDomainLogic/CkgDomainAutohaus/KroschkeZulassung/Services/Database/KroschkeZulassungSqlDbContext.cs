using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using CkgDomainLogic.KroschkeZulassung.Models;

namespace CkgDomainLogic.KroschkeZulassung.Services
{
    public class KroschkeZulassungSqlDbContext : DbContext
    {
        public KroschkeZulassungSqlDbContext()
            : base(ConfigurationManager.AppSettings["Connectionstring"])
        {
        }

        public IEnumerable<Kennzeichengroesse> GetKennzeichengroessen()
        {
            return Database.SqlQuery<Kennzeichengroesse>("SELECT * FROM KennzeichGroesse ORDER BY Position");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<KroschkeZulassungSqlDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
