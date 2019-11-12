using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CText :IModel
    {
        public int Id{get;set;}
        public string ImageUrl { get; set; }
      
        public int? CategoryId { get; set; }
        public int? OrganizationId { get; set; }
        public int CreatedUserId { get; set; }
        public int? ItemThemeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
      
        public virtual COrganization Organization { get; set; }
        public virtual CCategory Category { get; set; }
        public virtual CItemTheme ItemTheme { get; set; }
        public virtual ICollection<CImage> Images { get; set; }
        public virtual ICollection<CMenuItem> MenuItems { get; set; }
        public virtual ICollection<CTextLanguage> LanguageValues { get; set; }
    }
}
