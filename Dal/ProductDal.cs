using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VinoVoyage.Models;

namespace VinoVoyage.Dal
{
    public class ProductDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductModel>().ToTable("Products");
        }
        public DbSet<UserModel> Products { get; set; }

    }
}