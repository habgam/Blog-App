using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CAddressBindings:IModel
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string Port { get; set; }
        public int? OrganizationId { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public virtual COrganization Organization { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
    }
}
