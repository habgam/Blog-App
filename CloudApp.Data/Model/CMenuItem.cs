using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CMenuItem : IModel
    {
        public int Id { get; set; }
        //[Required(ErrorMessage = "Bu Alan Boş Geçilemez.")]
        //[StringLength(9999999, MinimumLength = 3, ErrorMessage = "Minimum 3 karakter girmelisiniz.")]
     
        public EMenuType MenuType { get; set; }
        public string Url { get; set; }
        public int? TextId { get; set; }
        public int? CategoryId { get; set; }
        public int? OrganizationId { get; set; }
        public int? MenuId { get; set; }
        public int Level { get; set; }
        public int? SubMenuId { get; set; }
        public int? Order { get; set; }
        public DateTime CreatedDate { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public int CreatedUserId { get; set; }
        public virtual COrganization Organization { get; set; }
        public virtual CMenu Menu { get; set; }
        public virtual ICollection<CMenuItem> SubMenu { get; set; }
        public virtual CMenuItem TopMenu { get; set; }
        public virtual CCategory ConnectCategory { get; set; }
        public virtual CText ConnectText { get; set; }
        public virtual ICollection<CMenuItemLanguage> LanguageValues { get; set; }
    }
}
