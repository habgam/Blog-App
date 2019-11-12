using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CAdminMenuConfiguration : EntityTypeConfiguration<CAdminMenu>
    {
        public CAdminMenuConfiguration() 
        {
           HasKey(model => model.Id);
           HasMany(model => model.SubMenu)
                  .WithOptional(model => model.TopMenu)
                  .HasForeignKey(model => model.SubMenuId);
        }
    }
}
