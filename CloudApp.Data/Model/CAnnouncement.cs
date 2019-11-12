using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CAnnouncement:IModel
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public int? OrganizationId { get; set; }
    }
}
