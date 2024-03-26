using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VinoVoyage.Models;

namespace VinoVoyage.Dal
{/* %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%  Not necessary!!!!!!! %%%%%%%%%%% */
    public class ProductDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProductModel>().ToTable("Products");
        }
        public DbSet<ProductModel> Products { get; set; }

    }
}