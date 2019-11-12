namespace CloudApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class imagelanguages : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CImages", "Language", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CImages", "Language");
        }
    }
}
