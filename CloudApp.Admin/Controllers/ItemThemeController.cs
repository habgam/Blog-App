using CloudApp.Admin.Core;
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class ItemThemeController : BaseController
    {
        private int PageSize = 15;
        public ActionResult Index(string id, string page)
        {

            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
            breadCrumb3.Name = "";
            breadCrumb3.Url = "";
            breadCrumb3.Level = -1;
            breadCrumbList.Add(breadCrumb3);

            ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
            breadCrumbAll.Name = "Tüm Temalar";
            breadCrumbAll.Url = "";
            breadCrumbAll.Level = 999999;
            breadCrumbList.Add(breadCrumbAll);


            DbDataContext db = new DbDataContext("CloudAppWebSite");
            List<CItemTheme> ItemList = new List<CItemTheme>();
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int intPage = Convert.ToInt32(page);
            if (id == "0")
            {
                ItemList = db.ItemThemes.Where(item => item.OrganizationId == orgId && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(item => item.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.ListCount = db.ItemThemes.Where(item => item.OrganizationId == orgId && item.ActiveStatus == EActiveStatus.Active).Count();
            }
            else
            {
                EItemTheme type = (EItemTheme)Convert.ToInt32(id);
                ItemList = db.ItemThemes.Where(item => item.OrganizationId == orgId && item.ThemeType == type && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(item => item.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.ListCount = db.ItemThemes.Where(item => item.OrganizationId == orgId && item.ThemeType == type && item.ActiveStatus == EActiveStatus.Active).Count();
            }
            ViewBag.CurrentPage = intPage;
            ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            return View(ItemList);
        }
        [HttpPost]
        public ActionResult Insert(CItemTheme item)
        {
            if (ModelState.IsValid)
            {
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                if (item.Id == 0)
                {
                  
                    item.OrganizationId = GetOrganizationId();
                    if (item.OrganizationId == 0)
                        return Redirect("/blog/Admin/Home/Index");
                    item.CreatedUserId = GetUserId();
                    item.CreatedDate = DateTime.Now;
                    item.ActiveStatus = EActiveStatus.Active;
                    item.ThemeType = (EItemTheme)Convert.ToInt32(Request.Form["ThemeType"]);
                    db.ItemThemes.Add(item);
                    db.SaveChanges();
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Modül teması başarılı bir şekilde eklenmiştir.", Title = "Başarılı" });
                }
                else
                {
                    int orgId = GetOrganizationId();
                    if (orgId == 0)
                        return Redirect("/blog/Admin/Home/Index");
                    CItemTheme cte = db.ItemThemes.Where(itemdb=>itemdb.OrganizationId==orgId && itemdb.ActiveStatus==EActiveStatus.Active && itemdb.Id==item.Id).FirstOrDefault();
                    if(cte!=null)
                    {
                        cte.Name = item.Name;
                        cte.ThemePath = item.ThemePath;
                        cte.ThemeType = (EItemTheme)Convert.ToInt32(Request.Form["ThemeType"]);
                        db.SaveChanges();
                        InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Modül teması başarılı bir şekilde düzenlenmiştir.", Title = "Başarılı" });
                        return RedirectToAction("Insert", "ItemTheme", new { Id = item.Id });
                    }
                    else
                    {
                        InsertAlert(new ViewAlert {AlertType = EAlertType.Warning , Title = "Uyarı" , Desc="Yetkisiz Giriş" });
                        RedirectToAction("Dashboard","Home");
                    }
                }
                return RedirectToAction("Insert", "ItemTheme");
            }
            if(item.Id!=0)
            {
                List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                breadCrumb3.Name = "";
                breadCrumb3.Url = "";
                breadCrumb3.Level = -1;
                breadCrumbList.Add(breadCrumb3);

                ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
                breadCrumbAll.Name = "Modül Düzenle";
                breadCrumbAll.Url = "";
                breadCrumbAll.Level = 999999;
                breadCrumbList.Add(breadCrumbAll);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(tt => tt.Level).ToList();
            }
            else
            { 
            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
            breadCrumb3.Name = "";
            breadCrumb3.Url = "";
            breadCrumb3.Level = -1;
            breadCrumbList.Add(breadCrumb3);

            ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
            breadCrumbAll.Name = "Modül Ekle";
            breadCrumbAll.Url = "";
            breadCrumbAll.Level = 999999;
            breadCrumbList.Add(breadCrumbAll);
            ViewBag.BreadCrumbList = breadCrumbList.OrderBy(tt => tt.Level).ToList();
            }
        
                return View(item);

        }

        [HttpGet]
        public bool Delete(int id)
        {
            int organizationId = GetOrganizationId();
            int realId = Convert.ToInt32(id);
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var cat = db.ItemThemes.Where(item => item.OrganizationId == organizationId && item.Id == id).FirstOrDefault();
            if (cat != null)
            {
                cat.ActiveStatus = EActiveStatus.Passive;
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Modül teması başarılı bir şekilde kaldırıldı.", Title = "Başarılı" });
                return true;
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Error, Title = "Hata", Desc = "Modül Teması Silinirken Bir Hata Oluştu." });
                return false;
            }

        }

        [HttpGet]
        public ActionResult Insert(string id)
        {
            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            if (!String.IsNullOrEmpty(id))
            {
                int realId = Convert.ToInt32(id);
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                int orgId = GetOrganizationId();
                if (orgId == 0)
                    return Redirect("/blog/Admin/Home/Index");
                CItemTheme cte = db.ItemThemes.Where(p=>p.Id == realId && p.OrganizationId == orgId ).FirstOrDefault();


                ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                breadCrumb3.Name = "";
                breadCrumb3.Url = "";
                breadCrumb3.Level = -1;
                breadCrumbList.Add(breadCrumb3);

                ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
                breadCrumbAll.Name = "Modül Düzenle";
                breadCrumbAll.Url = "";
                breadCrumbAll.Level = 999999;
                breadCrumbList.Add(breadCrumbAll);

                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
                return View(cte);
            }
            else
            {
                int orgId = GetOrganizationId();
                if (orgId == 0)
                    return Redirect("/blog/Admin/Home/Index");
                ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                breadCrumb3.Name = "";
                breadCrumb3.Url = "";
                breadCrumb3.Level = -1;
                breadCrumbList.Add(breadCrumb3);

                ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
                breadCrumbAll.Name = "Modül Ekle";
                breadCrumbAll.Url = "";
                breadCrumbAll.Level = 999999;
                breadCrumbList.Add(breadCrumbAll);
                CItemTheme cte = new CItemTheme();
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
                return View(cte);
            }

        }
        [HttpGet]
        public JsonResult GetThemeByType(string type)
        {
            EItemTheme enumType = (EItemTheme)Convert.ToInt32(type);
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int id = GetOrganizationId();
            List<SelectListItem> ListItemTheme = new List<SelectListItem>();
            ListItemTheme = db.ItemThemes.Where(p => p.ActiveStatus == EActiveStatus.Active && p.ThemeType == enumType && p.OrganizationId == id).Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
            return Json(ListItemTheme, JsonRequestBehavior.AllowGet);
        }
    }
}