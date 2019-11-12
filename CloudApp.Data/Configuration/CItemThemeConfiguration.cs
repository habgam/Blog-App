using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CItemThemeConfiguration : EntityTypeConfiguration<CItemTheme>
    {
        public CItemThemeConfiguration() {
            HasKey(model => model.Id);

            HasMany(model => model.CategoryItemTheme)
                .WithOptional(model => model.ItemTheme)
                .HasForeignKey(model => model.ItemThemeId);
            HasMany(model => model.MenuItemTheme)
                .WithOptional(model => model.ItemTheme)
                .HasForeignKey(model => model.ItemThemeId);
            HasMany(model => model.TextItemTheme)
                .WithOptional(model => model.ItemTheme)
                .HasForeignKey(model => model.ItemThemeId);
            HasMany(model => model.SliderItemTheme)
               .WithOptional(model => model.ItemThemes)
               .HasForeignKey(model => model.ItemThemeId);
        }
    }
}
