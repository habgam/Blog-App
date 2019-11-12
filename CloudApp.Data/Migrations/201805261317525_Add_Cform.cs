namespace CloudApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Cform : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CFormLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        JsonData = c.String(),
                        ActiveStatus = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        CreatedUserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CForms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.FormId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Properties = c.String(),
                        ViewProperties = c.String(),
                        ActiveStatus = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        CreatedUserId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CForms", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CFormLists", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CFormLists", "FormId", "dbo.CForms");
            DropIndex("dbo.CForms", new[] { "OrganizationId" });
            DropIndex("dbo.CFormLists", new[] { "OrganizationId" });
            DropIndex("dbo.CFormLists", new[] { "FormId" });
            DropTable("dbo.CForms");
            DropTable("dbo.CFormLists");
        }
    }
}
