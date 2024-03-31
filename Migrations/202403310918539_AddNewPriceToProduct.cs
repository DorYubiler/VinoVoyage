namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewPriceToProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModel", "NewPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModel", "NewPrice");
        }
    }
}
