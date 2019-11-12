using CloudApp.Data.Enum;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CUser : IModel , IUser
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string PasswordHash { get; set; }
         [NotMapped]
        public string Id
        {
            get { return UserId.ToString(); }
        }
        public int? OrganizationId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public EActiveStatus ActiveStatus { get; set; }
        public virtual COrganization Organization { get; set; }
    }
}
