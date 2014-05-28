using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MvcTools.Models
{
    public class MvcDbContext : DbContext
    {
        public MvcDbContext() : base("MvcDb") { }

        public DbSet<ContentEntity> ContentEntities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer<MvcDbContext>(null);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
   }
}