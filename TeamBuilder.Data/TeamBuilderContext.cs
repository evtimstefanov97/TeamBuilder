using Microsoft.Win32;
using TeamBuilder.Data.Configurations;
using TeamBuilder.Models;

namespace TeamBuilder.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;
   
    public class TeamBuilderContext : DbContext
    {
        // Your context has been configured to use a 'TeamBuilderContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'TeamBuilder.Data.TeamBuilderContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TeamBuilderContext' 
        // connection string in the application configuration file.
        public TeamBuilderContext()
            : base("name=TeamBuilderContext")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<TeamBuilderContext>());
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new EventConfiguration());
            modelBuilder.Configurations.Add(new TeamConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}