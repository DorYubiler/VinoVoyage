using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using VinoVoyage.Models;

namespace VinoVoyage.Dal
{
    public class CustomerDal : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserModel>().ToTable("UsersDB");
        }
        public DbSet<UserModel> Users { get; set; }
/*        public CustomerDal() : base("name=Data Source=DOR_ZENBOOK;Initial Catalog=VinoVoyage;Integrated Security=True;Encrypt=False")
        {
        }*/
    }
}