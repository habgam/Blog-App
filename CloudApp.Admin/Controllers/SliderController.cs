using CloudApp.Admin.Core;
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class SliderController : BaseController
    {
        // GET: Slider
        private int PageSize = 15;
        [HttpGet]
        public ActionResult GetSlider(string id , string page)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int intId = Convert.ToInt32(id);
            int intPage = Convert.ToInt32(page);
            List<CSlider> SliderList;
            SliderList = db.Sliders.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active).OrderByDescending(item => item.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.ListCount = db.Sliders.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active).Count();
            ViewBag.CurrentPage = page;


            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
            breadCrumb3.Name = "";
            breadCrumb3.Url = "";
            breadCrumb3.Level = -1;
            breadCrumbList.Add(breadCrumb3);

            ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
            breadCrumbAll.Name = "Slider";
            breadCrumbAll.Url = "";
            breadCrumbAll.Level = 999999;
            breadCrumbList.Add(breadCrumbAll);
            ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            return View(SliderList);
        }

        [HttpGet]
        public bool DeleteSlider(int id)
        {
            int organizationId = GetOrganizationId();
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var cat = db.Sliders.Where(item => item.OrganizationId == organizationId && item.Id == id).FirstOrDefault();
            if (cat != null)
            {
                cat.ActiveStatus = EActiveStatus.Passive;
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Slider başarılı bir şekilde silindi", Title = "Başarılı" });
                return true;
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Error, Title = "Hata", Desc = "Slider Silerken Bir Hata Oluştu." });
                return false;
            }

        }

        public ActionResult InsertSlider()
        {
            return View();
        }
        [HttpPost]
        public ActionResult InsertSlider(CSlider item)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int userId = GetUserId();
            if(item.Id!=0)
            {
                CSlider cte = db.Sliders.Where(p=>p.OrganizationId==orgId && p.ActiveStatus == EActiveStatus.Active && p.Id==item.Id).FirstOrDefault();
                if(cte!=null)
                {
                    cte.Name = item.Name;
                    cte.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                    db.SaveChanges();
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Slider başarılı bir şekilde düzenlenmiştir.", Title = "Başarılı" });
                    return Redirect("/blog/Admin/Slider/InsertSlider/"+cte.Id);
                }
                else
                {
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            else
            {
                item.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                item.ActiveStatus = EActiveStatus.Active;
                item.CreatedDate = DateTime.Now;
                item.CreatedUserId = userId;
                item.OrganizationId = orgId;
                db.Sliders.Add(item);
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Slider başarılı bir şekilde eklenmiştir.", Title = "Başarılı" });
                return Redirect("/blog/Admin/Slider/InsertSlider");
            }
            return View();
        }
        [HttpGet]
        public ActionResult InsertSlider(string id)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
            breadCrumb3.Name = "";
            breadCrumb3.Url = "";
            breadCrumb3.Level = -1;
            breadCrumbList.Add(breadCrumb3);

            ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
            breadCrumbAll.Name = "Slider";
            breadCrumbAll.Url = "";
            breadCrumbAll.Level = 999999;
            breadCrumbList.Add(breadCrumbAll);
            ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();

            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int intId = 0;
            if (!String.IsNullOrEmpty(id))
                intId = Convert.ToInt32(id);
            if(intId!=0)
            {
                CSlider cse = db.Sliders.Where(s=>s.ActiveStatus==EActiveStatus.Active && s.OrganizationId==orgId && s.Id==intId).FirstOrDefault();
                if(cse!=null)
                {
                    ViewBag.ItemThemeId = cse.ItemThemeId.HasValue==true ?cse.ItemThemeId.Value: 0;
                    return View(cse);
                }
                else
                {
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                    return RedirectToAction("Dashboard", "Home");
                }
             
            }
            else
            {
                ViewBag.ItemThemeId = 0;
                CSlider cte = new CSlider();
                return View(cte);
            }
           
        }
    }
}