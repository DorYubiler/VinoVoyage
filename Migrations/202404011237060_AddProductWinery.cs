namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductWinery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModel", "Winary", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModel", "Winary");
        }
    }
}
