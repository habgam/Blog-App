using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data
{
    interface IModel
    {
        DateTime CreatedDate { get; set; }
        int CreatedUserId { get; set; }
        int? OrganizationId { get; set; }
        EActiveStatus ActiveStatus { get; set; }
    }
}
