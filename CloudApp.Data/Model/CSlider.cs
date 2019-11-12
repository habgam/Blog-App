using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CSlider : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ItemThemeId { get; set; }
        public int? OrganizationId { get; set; }
        public int CreatedUserId { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public virtual COrganization Organization { get; set; }
        public virtual ICollection<CImage> Images { get; set; }
        public virtual CItemTheme ItemThemes { get; set; }
    }
}
