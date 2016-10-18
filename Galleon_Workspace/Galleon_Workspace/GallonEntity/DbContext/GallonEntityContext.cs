using System;
using System.Data.Entity;
using MySql.Data;

namespace GalleonEntity {
	/// <summary>
	/// Gallon entity context
	/// </summary>
	public class GalleonEntityContext : DbContext {

        private const string CONNECTION_STR = "name=GalleonConnectionString";

		public DbSet<User> 		Users 		{ get; set; }
		public DbSet<Customer> 	Customers 	{ get; set; }
		public DbSet<Contact> 	Contacts 	{ get; set; }
		public DbSet<Directory> Directories { get; set; }
		public DbSet<Document> 	Documents 	{ get; set; }
		public DbSet<Content> 	Contents 	{ get; set; }
		public DbSet<Session> 	Sessions 	{ get; set; }
        public DbSet<Category>  Categories  { get; set; }
        public DbSet<City>      Cities      { get; set; }
        public DbSet<Country>   Countries   { get; set; }

		public GalleonEntityContext() : base(CONNECTION_STR) {
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder dbModel) {
            base.OnModelCreating(dbModel);
            /*
            //do not delete on deleting customer record since it's used as data source
            dbModel.Entity<Customer>()
                   .HasRequired(x => x.Category)
                   .WithRequiredDependent()
                   .WillCascadeOnDelete(false);

            //if parent directory is deleted no reference for child ones's 
            //so we assign db to delete those any action on delete on such record
            dbModel.Entity<Directory>()
                   .HasOptional(x => x.Parent)
                   .WithOptionalPrincipal()
                   .WillCascadeOnDelete(true);
            //do not delete on deleting contact record since it's used as data source
            dbModel.Entity<Contact>()
                   .HasRequired(x => x.City)
                   .WithRequiredDependent()
                   .WillCascadeOnDelete(false);
            //do not delete on deleting contact record since it's used as data source
            dbModel.Entity<Contact>()
                   .HasRequired(x => x.Country)
                   .WithRequiredDependent()
                   .WillCascadeOnDelete(false);*/
        }
	}
}

