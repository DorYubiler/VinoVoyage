namespace VinoVoyage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRatingP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductModel", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductModel", "Rating");
        }
    }
}
