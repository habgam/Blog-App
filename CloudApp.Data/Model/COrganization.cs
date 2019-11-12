using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Model
{
    public class COrganization : IModel
    {
        public int? OrganizationId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsOffline { get; set; }
        public string SmtpServer { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpPort { get; set; }
        public string SmtpWhoMail { get; set; }
        public bool SmtpUseSSL { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUserId { get; set; }
        public string GoogleAnalyticsProfileId { get;  set; }
        public EActiveStatus ActiveStatus { get; set; }
        public virtual ICollection<CMenu> Menus { get; set; }
        public virtual ICollection<CMenuItem> MenuItems { get; set; }
        public virtual ICollection<CItemTheme> ItemThemes { get; set; }
        public virtual ICollection<CCategory> Categories { get; set; }
        public virtual ICollection<CText> Texts { get; set; }
        public virtual ICollection<CAddressBindings> AdressBindings { get; set; }
        public virtual ICollection<CUser> Users { get; set; }
        public virtual ICollection<CSlider> Sliders { get; set; }
        public virtual ICollection<CAdminMenu> AdminMenus { get; set; }
        public virtual ICollection<CForm> Forms { get; set; }
        public virtual ICollection<CFormList> FormList { get; set; }
    }
}
