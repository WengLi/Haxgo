using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using Haxgo.Entities;

namespace Haxgo.Data
{
    public class HaxgoContext : DbContext
    {
        public HaxgoContext() : base("name=DefaultConnection") { }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
        public IEnumerable<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<UrlRecord> UrlRecords { get; set; }

    }
}
