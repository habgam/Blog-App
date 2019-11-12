using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CCategory : IModel
    {
        public int Id { get; set; }
        public int? ItemThemeId { get; set; }
        public string HeaderImageUrl { get; set; }
        public string ImageUrl { get; set; }
        public int? SubCategoryId { get; set; }
        public int? OrganizationId { get; set; }
        public int CreatedUserId { get; set; }
        public int? Level { get; set; }
        public DateTime CreatedDate { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public virtual COrganization Organization { get; set; }
        public virtual ICollection< CCategory> SubCategory { get; set; }
        public virtual CCategory TopCategory { get; set; }
        public virtual CItemTheme ItemTheme { get; set; }
        public virtual ICollection<CText> Texts { get; set; }
        public virtual ICollection<CMenuItem> MenuItems { get; set; }

        public virtual ICollection<CCategoryLanguage> LanguageValues { get; set; }
    }
}
