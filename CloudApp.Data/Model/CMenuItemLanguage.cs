using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CMenuItemLanguage : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public int? OrganizationId { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public int MenuItemId { get; set; }
        public string Lang { get; set; }

        public virtual CMenuItem MenuItem { get; set; }
    }
}
