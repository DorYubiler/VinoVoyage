namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductIdType : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ProductModel");
            AddPrimaryKey("dbo.ProductModel", new[] { "ProductID", "ProductName" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ProductModel");
            AddPrimaryKey("dbo.ProductModel", "ProductID");
        }
    }
}
