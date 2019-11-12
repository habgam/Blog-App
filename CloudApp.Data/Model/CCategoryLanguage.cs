using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CCategoryLanguage : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PageTitle { get; set; }
        public string PageDescription { get; set; }
        public string PageKeyword { get; set; }
        public string Content { get; set; }
        public string CustomProperty1 { get; set; }
        public string CustomProperty2 { get; set; }
        public string CustomProperty3 { get; set; }
        public string CustomProperty4 { get; set; }
        public string CustomProperty5 { get; set; }
        public string CustomProperty6 { get; set; }
        public string CustomProperty7 { get; set; }
        public string CustomProperty8 { get; set; }
        public string CustomProperty9 { get; set; }
        public string CustomProperty10 { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public int? OrganizationId { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public string Lang { get; set; }

        public virtual CCategory Category { get; set; }
    }
}
