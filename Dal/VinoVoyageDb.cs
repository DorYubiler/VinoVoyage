using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinoVoyage.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace VinoVoyage.Dal
{
    /// <summary>
    /// Represents the database context of the VinoVoyage application, providing access to the application's database tables.
    /// This class is responsible for the connection and operations with the database, utilizing Entity Framework functionalities.
    /// </summary>
    public class VinoVoyageDb : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VinoVoyageDb"/> class, setting the database name or connection string to "VinoVoyage".
        /// This constructor ensures the DbContext connects to the correct database as configured in the application's connection strings.
        /// </summary>
        public VinoVoyageDb() : base("VinoVoyage")
        {
        }

        /// <summary>
        /// Gets or sets the database set representing the Users table. This property provides access to user-related data stored in the database.
        /// </summary>
        public DbSet<UserModel> Users{ get; set; }

        /// <summary>
        /// Gets or sets the database set representing the Products table. This property provides access to product-related data stored in the database.
        /// </summary>
        public DbSet<ProductModel> Products{ get; set; }

        /// <summary>
        /// Gets or sets the database set representing the Orders table. This property provides access to order-related data stored in the database.
        /// </summary>
        public DbSet<OrderModel> Orders { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the Wishlist table. This property provides access to wishlist-related data stored in the database.
        /// </summary>
        public DbSet<WishListModel> wishList { get; set; }

        /// <summary>
        /// Gets or sets the database set representing the ShippingList table. This property provides access to shipping information stored in the database.
        /// </summary>
        public DbSet<ShippingModel> ShippingList { get; set; }

        /// <summary>
        /// Configures model conventions upon model creation, particularly by removing the pluralizing table name convention to ensure table names are singular as defined in the DbSet properties.
        /// This method is automatically called when the model for a derived context has been initialized, but before the model has been locked down and used to initialize the context.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
