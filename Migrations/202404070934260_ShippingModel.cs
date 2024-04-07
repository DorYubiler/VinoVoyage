namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShippingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShippingModel",
                c => new
                    {
                        ShippingID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 10),
                        OrderDate = c.DateTime(nullable: false),
                        ShippingDate = c.DateTime(nullable: false),
                        Address = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => new { t.ShippingID, t.UserName });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ShippingModel");
        }
    }
}
