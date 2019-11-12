using CloudApp.Admin.Core;
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class FormController : BaseController
    {
        private int PageSize = 15;
        // GET: Form
        [HttpGet]
        public ActionResult CreateForm(string id)
        {
            DbDataContext ctx = new DbDataContext();
            if (!String.IsNullOrEmpty(id))
            {
                int realId = Convert.ToInt32(id);
                CForm form= ctx.Forms.FirstOrDefault(k=>k.Id==realId);
                if(form!=null)
                {
                    return View(form);
                }
                else
                {
                    CForm formNull = new CForm();
                    return View(formNull);
                }
            }
            else
            {
                CForm formNull = new CForm();
                return View(formNull);
            }
        }
        [HttpPost]
        public ActionResult CreateForm(CForm model)
        {
            DbDataContext ctx = new DbDataContext();
            if (model.Id == 0)
            {
                model.OrganizationId = GetOrganizationId();
                model.CreatedUserId = GetUserId();
                model.CreatedDate = DateTime.Now;
                model.ActiveStatus = EActiveStatus.Active;
                ctx.Forms.Add(model);
                ctx.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Form Başarılı Bir Şekilde Eklendi", Title = "Başarılı" });
            }
            else
            {
                var forms = ctx.Forms.FirstOrDefault(e => e.Id == model.Id);
                forms.Name = model.Name;
                forms.Properties = model.Properties;
                forms.ViewProperties = model.ViewProperties;
                ctx.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Form Başarılı Bir Şekilde Güncellendi", Title = "Başarılı" });
            }
            
            return Redirect("/blog/Admin/Form/List?Page=1");
        }
        [HttpGet]
        public ActionResult List( string page)
        {
            DbDataContext db = new DbDataContext();
            int orgId = GetOrganizationId();
            var formList = db.Forms.Where(s=>s.OrganizationId == orgId && s.ActiveStatus == EActiveStatus.Active);
            ViewBag.ListCount = formList.Count();
            ViewBag.CurrentPage = page;
            int intPage = Convert.ToInt32(page);
            return View(formList.OrderByDescending(h=>h.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList());
        }
        public bool DeleteForm(string id)
        {
            int realId = Convert.ToInt32(id);
            DbDataContext db = new DbDataContext();
            CForm ff = db.Forms.FirstOrDefault(g => g.Id == realId);
            db.Forms.Remove(ff);
            db.SaveChanges();
            return true;
        }
        public ActionResult FormList(string id,string page)
        {
            int realId = Convert.ToInt32(id);
            DbDataContext db = new DbDataContext();
            CForm cf = db.Forms.FirstOrDefault(s=>s.Id==realId);
            List<String> headerList = cf.ViewProperties.Split(',').ToList();
            ViewBag.HeaderList = headerList;
            ViewBag.PageHeader = cf.Name;
           
            List<FormViewListModel> mdList = new List<FormViewListModel>();
            var cfList = db.FormLists.Where(h=>h.FormId == realId && h.ActiveStatus == EActiveStatus.Active);
            ViewBag.ListCount = cfList.Count();
            ViewBag.CurrentPage = page;
            int intPage = Convert.ToInt32(page);

            foreach (var h in cfList.OrderByDescending(g=>g.Id).Skip((intPage - 1) * PageSize).Take(PageSize))
            {
                FormViewListModel md = new FormViewListModel();
                md.Parameters = new List<KeyValuePair<string, string>>();
               
                dynamic d = JObject.Parse(h.JsonData);
                foreach(var item in headerList)
                {
                    md.Parameters.Add(new KeyValuePair<string, string>( item,d[item].ToString()));
                }
                md.FormListId = h.Id;
                mdList.Add(md);
               
            }
            return View(mdList);
        }
        public ActionResult FormDetail(string id)
        {
            int realId = Convert.ToInt32(id);
            DbDataContext db = new DbDataContext();
            CFormList fList = db.FormLists.FirstOrDefault(f=>f.Id == realId);

            ViewBag.FormName = fList.Form.Name;

            FormViewListModel md = new FormViewListModel();
            md.Parameters = new List<KeyValuePair<string, string>>();

            dynamic d = JObject.Parse(fList.JsonData);
            foreach (var item in fList.Form.Properties.Split(',').ToList())
            {
                md.Parameters.Add(new KeyValuePair<string, string>(item, d[item].ToString()));
            }
            md.FormListId = fList.Id;
            return View(md);
        }
        public bool DeleteFormList(string id)
        {
            int realId = Convert.ToInt32(id);
            DbDataContext db = new DbDataContext();
            CFormList ff = db.FormLists.FirstOrDefault(g => g.Id == realId);
            db.FormLists.Remove(ff);
            db.SaveChanges();
            return true;
        }
    }
}