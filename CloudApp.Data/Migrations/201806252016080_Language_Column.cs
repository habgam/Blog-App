namespace CloudApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Language_Column : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CImages", t => t.ImageId, cascadeDelete: true)
                .Index(t => t.ImageId);
            
            CreateTable(
                "dbo.CTextLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        PageTitle = c.String(),
                        PageDescription = c.String(),
                        PageKeyword = c.String(),
                        Content = c.String(),
                        CustomProperty1 = c.String(),
                        CustomProperty2 = c.String(),
                        CustomProperty3 = c.String(),
                        CustomProperty4 = c.String(),
                        CustomProperty5 = c.String(),
                        CustomProperty6 = c.String(),
                        CustomProperty7 = c.String(),
                        CustomProperty8 = c.String(),
                        CustomProperty9 = c.String(),
                        CustomProperty10 = c.String(),
                        TextId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        ActiveStatus = c.Int(nullable: false),
                        Lang = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CTexts", t => t.TextId, cascadeDelete: true)
                .Index(t => t.TextId);
            
            CreateTable(
                "dbo.CMenuItemLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        ActiveStatus = c.Int(nullable: false),
                        MenuItemId = c.Int(nullable: false),
                        Lang = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CMenuItems", t => t.MenuItemId, cascadeDelete: true)
                .Index(t => t.MenuItemId);
            
            CreateTable(
                "dbo.CCategoryLanguages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        PageTitle = c.String(),
                        PageDescription = c.String(),
                        PageKeyword = c.String(),
                        Content = c.String(),
                        CustomProperty1 = c.String(),
                        CustomProperty2 = c.String(),
                        CustomProperty3 = c.String(),
                        CustomProperty4 = c.String(),
                        CustomProperty5 = c.String(),
                        CustomProperty6 = c.String(),
                        CustomProperty7 = c.String(),
                        CustomProperty8 = c.String(),
                        CustomProperty9 = c.String(),
                        CustomProperty10 = c.String(),
                        CategoryId = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        ActiveStatus = c.Int(nullable: false),
                        Lang = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            DropColumn("dbo.CCategories", "Name");
            DropColumn("dbo.CCategories", "Description");
            DropColumn("dbo.CCategories", "PageTitle");
            DropColumn("dbo.CCategories", "PageDescription");
            DropColumn("dbo.CCategories", "PageKeyword");
            DropColumn("dbo.CCategories", "Content");
            DropColumn("dbo.CCategories", "CustomProperty1");
            DropColumn("dbo.CCategories", "CustomProperty2");
            DropColumn("dbo.CCategories", "CustomProperty3");
            DropColumn("dbo.CCategories", "CustomProperty4");
            DropColumn("dbo.CCategories", "CustomProperty5");
            DropColumn("dbo.CCategories", "CustomProperty6");
            DropColumn("dbo.CCategories", "CustomProperty7");
            DropColumn("dbo.CCategories", "CustomProperty8");
            DropColumn("dbo.CCategories", "CustomProperty9");
            DropColumn("dbo.CCategories", "CustomProperty10");
            DropColumn("dbo.CMenuItems", "Name");
            DropColumn("dbo.CTexts", "Name");
            DropColumn("dbo.CTexts", "Description");
            DropColumn("dbo.CTexts", "PageTitle");
            DropColumn("dbo.CTexts", "PageDescription");
            DropColumn("dbo.CTexts", "PageKeyword");
            DropColumn("dbo.CTexts", "Content");
            DropColumn("dbo.CTexts", "CustomProperty1");
            DropColumn("dbo.CTexts", "CustomProperty2");
            DropColumn("dbo.CTexts", "CustomProperty3");
            DropColumn("dbo.CTexts", "CustomProperty4");
            DropColumn("dbo.CTexts", "CustomProperty5");
            DropColumn("dbo.CTexts", "CustomProperty6");
            DropColumn("dbo.CTexts", "CustomProperty7");
            DropColumn("dbo.CTexts", "CustomProperty8");
            DropColumn("dbo.CTexts", "CustomProperty9");
            DropColumn("dbo.CTexts", "CustomProperty10");
            DropColumn("dbo.CImages", "Name");
            DropColumn("dbo.CImages", "Description");
            DropColumn("dbo.CImages", "AltText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CImages", "AltText", c => c.String());
            AddColumn("dbo.CImages", "Description", c => c.String());
            AddColumn("dbo.CImages", "Name", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty10", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty9", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty8", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty7", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty6", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty5", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty4", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty3", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty2", c => c.String());
            AddColumn("dbo.CTexts", "CustomProperty1", c => c.String());
            AddColumn("dbo.CTexts", "Content", c => c.String());
            AddColumn("dbo.CTexts", "PageKeyword", c => c.String());
            AddColumn("dbo.CTexts", "PageDescription", c => c.String());
            AddColumn("dbo.CTexts", "PageTitle", c => c.String());
            AddColumn("dbo.CTexts", "Description", c => c.String());
            AddColumn("dbo.CTexts", "Name", c => c.String(nullable: false));
            AddColumn("dbo.CMenuItems", "Name", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty10", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty9", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty8", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty7", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty6", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty5", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty4", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty3", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty2", c => c.String());
            AddColumn("dbo.CCategories", "CustomProperty1", c => c.String());
            AddColumn("dbo.CCategories", "Content", c => c.String());
            AddColumn("dbo.CCategories", "PageKeyword", c => c.String());
            AddColumn("dbo.CCategories", "PageDescription", c => c.String());
            AddColumn("dbo.CCategories", "PageTitle", c => c.String());
            AddColumn("dbo.CCategories", "Description", c => c.String());
            AddColumn("dbo.CCategories", "Name", c => c.String(nullable: false));
            DropForeignKey("dbo.CCategoryLanguages", "CategoryId", "dbo.CCategories");
            DropForeignKey("dbo.CMenuItemLanguages", "MenuItemId", "dbo.CMenuItems");
            DropForeignKey("dbo.CTextLanguages", "TextId", "dbo.CTexts");
            DropForeignKey("dbo.CImageLanguages", "ImageId", "dbo.CImages");
            DropIndex("dbo.CCategoryLanguages", new[] { "CategoryId" });
            DropIndex("dbo.CMenuItemLanguages", new[] { "MenuItemId" });
            DropIndex("dbo.CTextLanguages", new[] { "TextId" });
            DropIndex("dbo.CImageLanguages", new[] { "ImageId" });
            DropTable("dbo.CCategoryLanguages");
            DropTable("dbo.CMenuItemLanguages");
            DropTable("dbo.CTextLanguages");
            DropTable("dbo.CImageLanguages");
        }
    }
}
