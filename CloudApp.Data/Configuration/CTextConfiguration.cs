using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CTextConfiguration : EntityTypeConfiguration<CText>
    {
        public CTextConfiguration()
        {
            HasKey(model => model.Id);
            
            HasMany(model => model.Images)
                .WithOptional(model => model.Text)
                .HasForeignKey(model => model.TextId);
            HasMany(model => model.MenuItems)
                .WithOptional(model => model.ConnectText)
                .HasForeignKey(model => model.TextId);
            HasMany(model => model.LanguageValues)
               .WithRequired(model => model.Text)
               .HasForeignKey(model => model.TextId);
        }
    }
}
