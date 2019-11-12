namespace CloudApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CAddressBindings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Port = c.String(),
                        OrganizationId = c.Int(),
                        ActiveStatus = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.COrganizations",
                c => new
                    {
                        OrganizationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Title = c.String(),
                        IsOffline = c.Boolean(nullable: false),
                        SmtpServer = c.String(),
                        SmtpUserName = c.String(),
                        SmtpPassword = c.String(),
                        SmtpPort = c.String(),
                        SmtpWhoMail = c.String(),
                        SmtpUseSSL = c.Boolean(nullable: false),
                        ImageUrl = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        GoogleAnalyticsProfileId = c.String(),
                        ActiveStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CAdminMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MenuName = c.String(),
                        MenuUrl = c.String(),
                        SubMenuId = c.Int(),
                        Order = c.Int(),
                        OrganizationId = c.Int(),
                        ActiveStatus = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CAdminMenus", t => t.SubMenuId)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.SubMenuId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        PageTitle = c.String(),
                        PageDescription = c.String(),
                        PageKeyword = c.String(),
                        ItemThemeId = c.Int(),
                        ImageUrl = c.String(),
                        SubCategoryId = c.Int(),
                        OrganizationId = c.Int(),
                        CreatedUserId = c.Int(nullable: false),
                        Content = c.String(),
                        Level = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CItemThemes", t => t.ItemThemeId)
                .ForeignKey("dbo.CCategories", t => t.SubCategoryId)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.ItemThemeId)
                .Index(t => t.SubCategoryId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CItemThemes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ThemeType = c.Int(nullable: false),
                        ThemePath = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        ActiveStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Desc = c.String(),
                        ItemThemeId = c.Int(),
                        OrganizationId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CItemThemes", t => t.ItemThemeId)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.ItemThemeId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CMenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        MenuType = c.Int(nullable: false),
                        Url = c.String(),
                        TextId = c.Int(),
                        CategoryId = c.Int(),
                        OrganizationId = c.Int(),
                        MenuId = c.Int(),
                        Level = c.Int(nullable: false),
                        SubMenuId = c.Int(),
                        Order = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CTexts", t => t.TextId)
                .ForeignKey("dbo.CMenuItems", t => t.SubMenuId)
                .ForeignKey("dbo.CMenus", t => t.MenuId)
                .ForeignKey("dbo.CCategories", t => t.CategoryId)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.TextId)
                .Index(t => t.CategoryId)
                .Index(t => t.OrganizationId)
                .Index(t => t.MenuId)
                .Index(t => t.SubMenuId);
            
            CreateTable(
                "dbo.CTexts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        ImageUrl = c.String(),
                        PageTitle = c.String(),
                        PageDescription = c.String(),
                        PageKeyword = c.String(),
                        CategoryId = c.Int(),
                        OrganizationId = c.Int(),
                        CreatedUserId = c.Int(nullable: false),
                        ItemThemeId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CItemThemes", t => t.ItemThemeId)
                .ForeignKey("dbo.CCategories", t => t.CategoryId)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.CategoryId)
                .Index(t => t.OrganizationId)
                .Index(t => t.ItemThemeId);
            
            CreateTable(
                "dbo.CImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        AltText = c.String(),
                        ImageUrl = c.String(),
                        Type = c.Int(nullable: false),
                        TextId = c.Int(),
                        SliderId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                        CreatedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CSliders", t => t.SliderId)
                .ForeignKey("dbo.CTexts", t => t.TextId)
                .Index(t => t.TextId)
                .Index(t => t.SliderId);
            
            CreateTable(
                "dbo.CSliders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ItemThemeId = c.Int(),
                        OrganizationId = c.Int(),
                        CreatedUserId = c.Int(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CItemThemes", t => t.ItemThemeId)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.ItemThemeId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CUsers",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        OrganizationId = c.Int(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        ActiveStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.COrganizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId);
            
            CreateTable(
                "dbo.CAnnouncements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Content = c.String(),
                        ActiveStatus = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedUserId = c.Int(nullable: false),
                        OrganizationId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CUsers", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CTexts", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CSliders", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CMenus", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CMenuItems", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CItemThemes", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CCategories", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CTexts", "CategoryId", "dbo.CCategories");
            DropForeignKey("dbo.CCategories", "SubCategoryId", "dbo.CCategories");
            DropForeignKey("dbo.CMenuItems", "CategoryId", "dbo.CCategories");
            DropForeignKey("dbo.CTexts", "ItemThemeId", "dbo.CItemThemes");
            DropForeignKey("dbo.CSliders", "ItemThemeId", "dbo.CItemThemes");
            DropForeignKey("dbo.CMenus", "ItemThemeId", "dbo.CItemThemes");
            DropForeignKey("dbo.CMenuItems", "MenuId", "dbo.CMenus");
            DropForeignKey("dbo.CMenuItems", "SubMenuId", "dbo.CMenuItems");
            DropForeignKey("dbo.CMenuItems", "TextId", "dbo.CTexts");
            DropForeignKey("dbo.CImages", "TextId", "dbo.CTexts");
            DropForeignKey("dbo.CImages", "SliderId", "dbo.CSliders");
            DropForeignKey("dbo.CCategories", "ItemThemeId", "dbo.CItemThemes");
            DropForeignKey("dbo.CAddressBindings", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CAdminMenus", "OrganizationId", "dbo.COrganizations");
            DropForeignKey("dbo.CAdminMenus", "SubMenuId", "dbo.CAdminMenus");
            DropIndex("dbo.CUsers", new[] { "OrganizationId" });
            DropIndex("dbo.CSliders", new[] { "OrganizationId" });
            DropIndex("dbo.CSliders", new[] { "ItemThemeId" });
            DropIndex("dbo.CImages", new[] { "SliderId" });
            DropIndex("dbo.CImages", new[] { "TextId" });
            DropIndex("dbo.CTexts", new[] { "ItemThemeId" });
            DropIndex("dbo.CTexts", new[] { "OrganizationId" });
            DropIndex("dbo.CTexts", new[] { "CategoryId" });
            DropIndex("dbo.CMenuItems", new[] { "SubMenuId" });
            DropIndex("dbo.CMenuItems", new[] { "MenuId" });
            DropIndex("dbo.CMenuItems", new[] { "OrganizationId" });
            DropIndex("dbo.CMenuItems", new[] { "CategoryId" });
            DropIndex("dbo.CMenuItems", new[] { "TextId" });
            DropIndex("dbo.CMenus", new[] { "OrganizationId" });
            DropIndex("dbo.CMenus", new[] { "ItemThemeId" });
            DropIndex("dbo.CItemThemes", new[] { "OrganizationId" });
            DropIndex("dbo.CCategories", new[] { "OrganizationId" });
            DropIndex("dbo.CCategories", new[] { "SubCategoryId" });
            DropIndex("dbo.CCategories", new[] { "ItemThemeId" });
            DropIndex("dbo.CAdminMenus", new[] { "OrganizationId" });
            DropIndex("dbo.CAdminMenus", new[] { "SubMenuId" });
            DropIndex("dbo.CAddressBindings", new[] { "OrganizationId" });
            DropTable("dbo.CAnnouncements");
            DropTable("dbo.CUsers");
            DropTable("dbo.CSliders");
            DropTable("dbo.CImages");
            DropTable("dbo.CTexts");
            DropTable("dbo.CMenuItems");
            DropTable("dbo.CMenus");
            DropTable("dbo.CItemThemes");
            DropTable("dbo.CCategories");
            DropTable("dbo.CAdminMenus");
            DropTable("dbo.COrganizations");
            DropTable("dbo.CAddressBindings");
        }
    }
}
