namespace CloudApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditCForm : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CForms", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.CForms", "Properties", c => c.String(nullable: false));
            AlterColumn("dbo.CForms", "ViewProperties", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CForms", "ViewProperties", c => c.String());
            AlterColumn("dbo.CForms", "Properties", c => c.String());
            AlterColumn("dbo.CForms", "Name", c => c.String());
        }
    }
}
