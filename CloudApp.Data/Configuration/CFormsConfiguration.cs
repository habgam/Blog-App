using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CFormsConfiguration : EntityTypeConfiguration<CForm>
    {
        public CFormsConfiguration()
        {
            HasKey(model => model.Id);

            HasMany(model => model.FormList)
            .WithRequired(model => model.Form)
            .HasForeignKey(model => model.FormId);
        }
    }
}
