namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderModel",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 10),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Username, t.ProductID });
            
            CreateTable(
                "dbo.ProductModel",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 30),
                        Type = c.String(nullable: false, maxLength: 30),
                        Description = c.String(nullable: false, maxLength: 500),
                        Origin = c.String(nullable: false, maxLength: 50),
                        Amount = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID);
            
            CreateTable(
                "dbo.UserModel",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 10),
                        Password = c.String(nullable: false, maxLength: 10),
                        Email = c.String(nullable: false, maxLength: 30),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Username);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserModel");
            DropTable("dbo.ProductModel");
            DropTable("dbo.OrderModel");
        }
    }
}
