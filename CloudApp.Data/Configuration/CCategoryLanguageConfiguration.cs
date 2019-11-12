using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CCategoryLanguageConfiguration : EntityTypeConfiguration<CCategoryLanguage>
    {
        public CCategoryLanguageConfiguration()
        {
            HasKey(model => model.Id);
        }
    }
}
