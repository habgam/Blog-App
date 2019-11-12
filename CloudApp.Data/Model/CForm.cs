using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class CForm:IModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu Alan Boş Geçilemez.")]
        [StringLength(9999999, MinimumLength = 3, ErrorMessage = "Minimum 3 karakter girmelisiniz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu Alan Boş Geçilemez.")]
        [StringLength(9999999, MinimumLength = 3, ErrorMessage = "Minimum 3 karakter girmelisiniz.")]
        public string Properties { get; set; }
        [Required(ErrorMessage = "Bu Alan Boş Geçilemez.")]
        [StringLength(9999999, MinimumLength = 3, ErrorMessage = "Minimum 3 karakter girmelisiniz.")]
        public string ViewProperties { get; set; }

        public EActiveStatus ActiveStatus { get; set; }
        public int? OrganizationId { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual COrganization Organization { get; set; }
        public virtual ICollection<CFormList> FormList { get; set; }
    }
}
