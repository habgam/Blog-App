using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CMenuConfiguration : EntityTypeConfiguration<CMenu>
    {
        public CMenuConfiguration()
        {

            HasKey(model => model.Id);

            Property(model => model.Desc).IsOptional();

            HasMany(model => model.MenuItem)
                .WithOptional(model => model.Menu)
                .HasForeignKey(model => model.MenuId);

            
        }
    }
}
