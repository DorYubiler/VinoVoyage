namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddWishList : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WishListModel",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 10),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Username, t.ProductID });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WishListModel");
        }
    }
}
