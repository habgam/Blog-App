namespace CloudApp.Data.Migrations
{
    using CloudApp.Data.Model;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CloudApp.Data.DbDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
           
        }

        protected override void Seed(CloudApp.Data.DbDataContext context)
        {
           
        }
    }
}
