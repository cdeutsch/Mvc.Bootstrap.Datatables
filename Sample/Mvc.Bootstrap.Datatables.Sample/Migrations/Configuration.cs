namespace Mvc.Bootstrap.Datatables.Sample.Migrations
{
    using Mvc.Bootstrap.Datatables.Sample.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Mvc.Bootstrap.Datatables.Sample.Models.SampleDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Mvc.Bootstrap.Datatables.Sample.Models.SampleDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Cars.AddOrUpdate(
                oo => new { oo.Year, oo.Make, oo.Model },
                new Car { Year = 1996, Make = "Toyota", Model = "Supra" },
                new Car { Year = 1992, Make = "Mitsubishi", Model = "Eclipse" },
                new Car { Year = 1996, Make = "Mitsubishi", Model = "3000GT" },
                new Car { Year = 2006, Make = "Mitsubishi", Model = "Lancer Evolution" },
                new Car { Year = 1999, Make = "Mazda", Model = "Miata" },
                new Car { Year = 1994, Make = "Mazda", Model = "RX7" },
                new Car { Year = 1994, Make = "Nissan", Model = "300Z" },
                new Car { Year = 1992, Make = "Nissan", Model = "Skyline GTR" },
                new Car { Year = 2004, Make = "BMW", Model = "M3" }
            );
        }
    }
}
