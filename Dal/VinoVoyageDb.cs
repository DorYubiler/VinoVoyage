using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinoVoyage.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VinoVoyage.Dal
{
/* This is the creating and keeping the Db. */
    public class VinoVoyageDb : DbContext
    {
/*representing where Db is */
        public VinoVoyageDb() : base("VinoVoyage")
        {
        }
/*here the tables is defined, if we need another table, create it here.*/
        public DbSet<UserModel> Users{ get; set; }
        public DbSet<ProductModel> Products{ get; set; }

        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<WishListModel> wishList {  get; set; }

        /*public DbSet<WishListModel> Wishlist { get; set; }*/

        /* prevents the project to create copies of same tables.*/
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
