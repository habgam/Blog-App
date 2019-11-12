namespace CloudApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addModelHeader : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CCategories", "HeaderImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CCategories", "HeaderImageUrl");
        }
    }
}
