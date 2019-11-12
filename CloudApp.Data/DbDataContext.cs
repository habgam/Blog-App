namespace CloudApp.Data
{
    using CloudApp.Data.Configuration;
using CloudApp.Data.Model;
using System;
using System.Data.Entity;
using System.Linq;

    public class DbDataContext : DbContext
    {
        // Your context has been configured to use a 'DbDataContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'CloudApp.Data.DbDataContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'DbDataContext' 
        // connection string in the application configuration file.
        public DbDataContext()
            : base("name=CloudAppWebSite")
        {
            
            //Configuration.LazyLoadingEnabled = false;
        }

        public DbDataContext(string ConnectionString = "CloudAppWebSite")
            : base("name=" + ConnectionString)
        {
            //Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new COrganizationConfiguration());
            modelBuilder.Configurations.Add(new CMenuConfiguration());
            modelBuilder.Configurations.Add(new CCategoryConfiguration());
            modelBuilder.Configurations.Add(new CTextConfiguration());
            modelBuilder.Configurations.Add(new CItemThemeConfiguration());
            modelBuilder.Configurations.Add(new CMenuItemConfiguration());
            modelBuilder.Configurations.Add(new CAddressBindingsConfiguration());
            modelBuilder.Configurations.Add(new CUserConfiguration());
            modelBuilder.Configurations.Add(new CImageConfiguration());
            modelBuilder.Configurations.Add(new CSliderConfiguration());
            modelBuilder.Configurations.Add(new CAdminMenuConfiguration());
            modelBuilder.Configurations.Add(new CAnnouncementConfiguration());
            modelBuilder.Configurations.Add(new CFormsConfiguration());
            modelBuilder.Configurations.Add(new CFormListConfiguration());
            modelBuilder.Configurations.Add(new CMenuItemLanguageConfiguration());
            modelBuilder.Configurations.Add(new CTextLanguageConfiguration());
            modelBuilder.Configurations.Add(new CCategoryLanguageConfiguration());
        }
        public DbSet<COrganization> Organizations { get; set; }
        public DbSet<CMenu> Menus { get; set; }
        public DbSet<CMenuItem> MenuItems { get; set; }
        public DbSet<CItemTheme> ItemThemes { get; set; }
        public DbSet<CText> Texts { get; set; }
        public DbSet<CCategory> Categories{ get; set; }
        public DbSet<CUser> Users { get; set; }
        public DbSet<CImage> Images { get; set; }
        public DbSet<CAddressBindings> AddressBindings { get; set; }
        public DbSet<CSlider> Sliders { get; set; }
        public DbSet<CAdminMenu> AdminMenus { get; set; }
        public DbSet<CAnnouncement> Announcements { get; set; }
        public DbSet<CForm> Forms { get; set; }
        public DbSet<CFormList> FormLists { get; set; }
        public DbSet<CCategoryLanguage> CategoryLanguage { get; set; }
        public DbSet<CTextLanguage> TextLanguage { get; set; }
        public DbSet<CMenuItemLanguage> MenuItemLanguage { get; set; }
    }

   
}