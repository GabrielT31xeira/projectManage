using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using projectManage.Models;

namespace projectManage.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AppDbContext() : base("AppDbContext")
        {
            // Cria o banco de dados caso não exista
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());
        }

        //Remove os "s" dos identificadores das classes
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /* remove o plural dos models*/
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}