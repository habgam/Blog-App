using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CFormList : IModel
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string JsonData { get; set; }

        public EActiveStatus ActiveStatus { get; set; }
        public int? OrganizationId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual COrganization Organization { get; set; }
        public virtual CForm Form { get; set; }
    }
}
