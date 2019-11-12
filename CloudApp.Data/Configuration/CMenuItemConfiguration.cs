using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CMenuItemConfiguration : EntityTypeConfiguration<CMenuItem>
    {
        public CMenuItemConfiguration()
        {
            HasKey(model => model.Id);
            Property(model => model.TextId).IsOptional();
            Property(model => model.CategoryId).IsOptional();
            Property(model => model.Url).IsOptional();
            Property(model => model.SubMenuId).IsOptional();
            HasMany(model => model.SubMenu)
                .WithOptional(model => model.TopMenu)
                .HasForeignKey(model => model.SubMenuId);
            HasMany(model => model.LanguageValues)
                .WithRequired(model => model.MenuItem)
                .HasForeignKey(model => model.MenuItemId);

        }
    }
}
