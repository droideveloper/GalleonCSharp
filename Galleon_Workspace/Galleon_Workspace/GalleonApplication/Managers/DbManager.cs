using GalleonApplication.App_Data;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Managers {
    
    public class DbManager : DbContext {

        public DbSet<Syncable> Syncables { get; set; }
        public DbSet<LogException> LogExceptions { get; set; }

        public DbManager() : base("name=DbContext") {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder dbModelBuilder) {
            dbModelBuilder.Entity<Syncable>();
            //has optional parameter in the entity, with type of Self
            dbModelBuilder.Entity<LogException>()
                          .HasOptional(e => e.Parent);

            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<DbManager>(dbModelBuilder));
        }
    }
}
