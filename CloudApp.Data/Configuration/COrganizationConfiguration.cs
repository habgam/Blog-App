using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.ModelConfiguration;
using System.Text;
using System.Threading.Tasks;
using CloudApp.Data.Model;

namespace CloudApp.Data.Configuration
{
    public class COrganizationConfiguration : EntityTypeConfiguration<COrganization>
    {
        public COrganizationConfiguration() {
            HasKey(model => model.OrganizationId);

            Property(model => model.Name).IsOptional();

            Property(model => model.Title);

            HasMany(model => model.Menus)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.MenuItems)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.ItemThemes)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.Categories)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.Texts)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.AdressBindings)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.Users)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.Sliders)
                .WithOptional(model => model.Organization)
                .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.AdminMenus)
              .WithOptional(model => model.Organization)
              .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.Forms)
             .WithOptional(model => model.Organization)
             .HasForeignKey(model => model.OrganizationId);

            HasMany(model => model.FormList)
             .WithOptional(model => model.Organization)
             .HasForeignKey(model => model.OrganizationId);
        }
    }
}
