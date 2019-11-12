using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CAdminMenu : IModel
    {
        public int Id {get;set;}
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public int? SubMenuId { get; set; }
        public int? Order { get; set; }
        public int? OrganizationId { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public virtual COrganization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public virtual ICollection<CAdminMenu> SubMenu { get; set; }
        public virtual CAdminMenu TopMenu { get; set; }
    }
}
