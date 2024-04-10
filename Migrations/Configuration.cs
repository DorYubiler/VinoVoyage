namespace VinoVoyage.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using VinoVoyage.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<VinoVoyage.Dal.VinoVoyageDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "VinoVoyage.Dal.VinoVoyageDb";
        }

        protected override void Seed(VinoVoyage.Dal.VinoVoyageDb context)
        {

        }
    }
}
