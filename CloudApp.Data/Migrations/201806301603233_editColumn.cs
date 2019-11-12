namespace CloudApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editColumn : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CImageLanguages", "ImageId", "dbo.CImages");
            DropIndex("dbo.CImageLanguages", new[] { "ImageId" });
            AddColumn("dbo.CImages", "Name", c => c.String());
            AddColumn("dbo.CImages", "AltText", c => c.String());
            AddColumn("dbo.CImages", "Description", c => c.String());
            DropTable("dbo.CImageLanguages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CImageLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        AltText = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        CreatedUserId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                        Lang = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.CImages", "Description");
            DropColumn("dbo.CImages", "AltText");
            DropColumn("dbo.CImages", "Name");
            CreateIndex("dbo.CImageLanguages", "ImageId");
            AddForeignKey("dbo.CImageLanguages", "ImageId", "dbo.CImages", "Id", cascadeDelete: true);
        }
    }
}
