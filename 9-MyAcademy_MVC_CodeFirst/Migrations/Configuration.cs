namespace _9_MyAcademy_MVC_CodeFirst.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<_9_MyAcademy_MVC_CodeFirst.Data.Context.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(_9_MyAcademy_MVC_CodeFirst.Data.Context.AppDbContext context)
        {
            // Seed data removed - use SeedData.sql script instead
        }
    }
}
