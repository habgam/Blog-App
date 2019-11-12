using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CImage : IModel
    {
        public int Id { get; set; }
       
        public EImageType Type { get; set; }
        public string Name { get; set; }
        public string AltText { get; set; }
        public string Description { get; set; }
        public int? TextId { get; set; }
        public int? SliderId { get; set; }
        public string ImageUrl { get; set; }
        public string Language { get; set; }
        public DateTime CreatedDate { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public int? OrganizationId { get; set; }
        public int CreatedUserId { get; set; }
        public virtual CText Text { get; set; }
        public virtual CSlider Slider { get; set; }

    }
}
