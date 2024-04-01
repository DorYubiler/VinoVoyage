namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wineryFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModel", "Winery", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.ProductModel", "Winary");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductModel", "Winary", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.ProductModel", "Winery");
        }
    }
}
