using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CCategoryConfiguration : EntityTypeConfiguration<CCategory>
    {
        public CCategoryConfiguration() {
            HasKey(model => model.Id);
            Property(model => model.SubCategoryId).IsOptional();
            Property(model => model.Level).IsOptional();
            HasMany(model => model.SubCategory)
                .WithOptional(model => model.TopCategory)
                .HasForeignKey(model => model.SubCategoryId);
            HasMany(model => model.Texts)
                .WithOptional(model => model.Category)
                .HasForeignKey(model => model.CategoryId);
            HasMany(model => model.MenuItems)
               .WithOptional(model => model.ConnectCategory)
               .HasForeignKey(model => model.CategoryId);
            HasMany(model => model.LanguageValues)
              .WithRequired(model => model.Category)
              .HasForeignKey(model => model.CategoryId);
        }
    }
}
