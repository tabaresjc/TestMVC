namespace JCTest.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<JCTest.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(JCTest.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }
}
