using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CMenuItemLanguageConfiguration : EntityTypeConfiguration<CMenuItemLanguage>
    {
        public CMenuItemLanguageConfiguration()
        {
            HasKey(model => model.Id);
        }
    }
}
