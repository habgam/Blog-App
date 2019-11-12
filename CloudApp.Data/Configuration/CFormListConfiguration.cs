using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CFormListConfiguration  : EntityTypeConfiguration<CFormList>
    {
        public CFormListConfiguration()
        {
            HasKey(model => model.Id);

            
        }
    }
}
