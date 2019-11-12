using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Configuration
{
    public class CSliderConfiguration : EntityTypeConfiguration<CSlider>
    {
        public CSliderConfiguration()
        {
           HasKey(model => model.Id);
           HasMany(model => model.Images)
              .WithOptional(model => model.Slider)
              .HasForeignKey(model => model.SliderId);
        }
    }
}
