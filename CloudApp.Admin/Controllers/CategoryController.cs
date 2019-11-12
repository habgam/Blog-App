using CloudApp.Admin.Core;
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        // GET: Category
        private List<ViewBreadCrumb> GetCategoryCrumb(int catId)
        {
            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            return null;
        }
        private int PageSize = 15;
        public ActionResult GetCategory(string id, string page)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int intId = Convert.ToInt32(id);
            int intPage = Convert.ToInt32(page);
            List<CCategory> CategoryList;

            ViewBag.CurrentPage = page;
            if (intId == 0)
            {
                CategoryList = db.Categories.Where(item => item.OrganizationId == orgId && item.SubCategoryId == null && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(item => item.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.ListCount = db.Categories.Where(item => item.OrganizationId == orgId && item.SubCategoryId == null && item.ActiveStatus == EActiveStatus.Active).Count();
            }
            else
            {
                CategoryList = db.Categories.Where(item => item.OrganizationId == orgId && item.SubCategoryId == intId && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(item => item.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.ListCount = db.Categories.Where(item => item.OrganizationId == orgId && item.SubCategoryId == intId && item.ActiveStatus == EActiveStatus.Active).Count();
            }
            CCategory cat = null;
            if (intId != 0)
            {
                cat = db.Categories.Find(intId);
            }

            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
            breadCrumb3.Name = "";
            breadCrumb3.Url = "/blog/Admin/Category/GetCategory/?id=0&page=1";
            breadCrumb3.Level = -1;
            breadCrumbList.Add(breadCrumb3);

            ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
            breadCrumbAll.Name = "Tüm Kategoriler";
            breadCrumbAll.Url = "";
            breadCrumbAll.Level = 999999;
            breadCrumbList.Add(breadCrumbAll);

            if (intId == 0)
            {
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            }
            else
            {
                ViewBreadCrumb breadCrumb = new ViewBreadCrumb();
                breadCrumb.Name = cat.LanguageValues.FirstOrDefault(h=>h.Lang=="tr-TR")!=null ? cat.LanguageValues.FirstOrDefault(h => h.Lang == "tr-TR").Name:"";
                breadCrumb.Url = "/blog/Admin/Category/GetCategory/?id=" + cat.Id + "&page=1";
                breadCrumb.Level = cat.Level.HasValue == true ? cat.Level.Value : 0;
                breadCrumbList.Add(breadCrumb);
                bool listContinue = false;
                int breadCrumbId = cat.SubCategoryId.HasValue == true ? cat.SubCategoryId.Value : 0;
                while (listContinue == false)
                {
                    CCategory subCat = db.Categories.Where(item => item.Id == breadCrumbId && item.ActiveStatus == EActiveStatus.Active).FirstOrDefault();
                    if (subCat != null)
                    {
                        ViewBreadCrumb breadCrumbSub = new ViewBreadCrumb();
                        breadCrumbSub.Name = subCat.LanguageValues.FirstOrDefault(h => h.Lang == "tr-TR") != null ? subCat.LanguageValues.FirstOrDefault(h => h.Lang == "tr-TR").Name : "";
                        breadCrumbSub.Url = "/blog/Admin/Category/GetCategory/?id=" + subCat.Id + "&page=1";
                        breadCrumbSub.Level = subCat.Level.HasValue == true ? subCat.Level.Value : 0;
                        breadCrumbList.Add(breadCrumbSub);
                        breadCrumbId = subCat.SubCategoryId.HasValue == true ? subCat.SubCategoryId.Value : 0;
                        if (breadCrumbId == 0)
                            listContinue = true;
                    }
                    else
                    {

                        listContinue = true;
                    }
                }


                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            }

            return View(CategoryList);
        }
        [HttpGet]
        public ActionResult InsertCategory(string id)
        {
            ViewBag.Languages = ConfigurationManager.AppSettings["Languages"].Split(',').ToList();
            if (!String.IsNullOrEmpty(id))
            {
                List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                ViewBreadCrumb breadCrumb = new ViewBreadCrumb();
                breadCrumb.Name = "";
                breadCrumb.Url = "";
                breadCrumb.Level = -1;
                breadCrumbList.Add(breadCrumb);

                ViewBreadCrumb breadCrumb1 = new ViewBreadCrumb();
                breadCrumb1.Name = "Kategori Düzenle";
                breadCrumb1.Url = "";
                breadCrumb1.Level = 1;
                breadCrumbList.Add(breadCrumb1);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            }
            else
            {
                List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                ViewBreadCrumb breadCrumb = new ViewBreadCrumb();
                breadCrumb.Name = "";
                breadCrumb.Url = "";
                breadCrumb.Level = -1;
                breadCrumbList.Add(breadCrumb);

                ViewBreadCrumb breadCrumb1 = new ViewBreadCrumb();
                breadCrumb1.Name = "Kategori Ekle";
                breadCrumb1.Url = "";
                breadCrumb1.Level = 1;
                breadCrumbList.Add(breadCrumb1);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            }



            if (!String.IsNullOrEmpty(id))
            {
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                int intId = Convert.ToInt32(id);
                int organizationId = GetOrganizationId();
                if (organizationId == 0)
                    return Redirect("/blog/Admin/Home/Index");
                CCategory cg = db.Categories.Where(item => item.OrganizationId == organizationId && item.Id == intId).FirstOrDefault();
                if (cg.SubCategoryId.HasValue == true)
                    ViewBag.SubCatId = cg.SubCategoryId;
                else
                    ViewBag.SubCatId = 0;
                if (cg.ItemThemeId.HasValue == true)
                    ViewBag.ItemThemeId = cg.ItemThemeId.Value;
                else
                    ViewBag.ItemThemeId = 0;
                if (cg != null)
                    return View(cg);

            }
            else
            {

                ViewBag.SubCatId = 0;
                ViewBag.ItemThemeId = 0;


            }


            CCategory cgNull = new CCategory();
            return View(cgNull);


        }
        [HttpGet]
        public JsonResult GetCategoryCustomProperty(string catId)
        {
            if (!String.IsNullOrEmpty(catId))
            {
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                int reelId = Convert.ToInt32(catId);
                if (reelId != 0)
                {
                    int orgId = GetOrganizationId();

                    CCategory ct = db.Categories.Where(item => item.OrganizationId == orgId && item.Id == reelId).FirstOrDefault();
                    if (ct != null)
                    {
                        var lg = ct.LanguageValues.FirstOrDefault(h => h.Lang == "TR");
                        if (lg != null)
                            return Json(new
                            {
                                custom1 = lg.CustomProperty1,
                                custom2 = lg.CustomProperty2,
                                custom3 = lg.CustomProperty3,
                                custom4 = lg.CustomProperty4,
                                custom5 = lg.CustomProperty5,
                                custom6 = lg.CustomProperty6,
                                custom7 = lg.CustomProperty7,
                                custom8 = lg.CustomProperty8,
                                custom9 = lg.CustomProperty9,
                                custom10 = lg.CustomProperty10
                            }, JsonRequestBehavior.AllowGet);
                        else
                            return Json(new
                            {
                                custom1 = "",
                                custom2 = "",
                                custom3 = "",
                                custom4 = "",
                                custom5 = "",
                                custom6 = "",
                                custom7 = "",
                                custom8 = "",
                                custom9 = "",
                                custom10 = ""
                            }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    { return null; }
                }
                else
                { return null; }
            }
            else
                return null;
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertCategory(CCategory cat)
        {

            if (ModelState.IsValid)
            {
                var ff = Request.Files["uploadFile"];
                var ffHeader = Request.Files["uploadFileHeader"];
                ImageController ic = new ImageController();
                string uploadResult = ic.UploadImage(ff, Server.MapPath("~/"), GetOrganizationId());
                string uploadResultHeader = ic.UploadImage(ffHeader, Server.MapPath("~/"), GetOrganizationId());
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                cat.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"]);
                cat.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                if (cat.SubCategoryId == 0)
                    cat.SubCategoryId = null;

                if (cat.Id == 0)
                {
                    cat.OrganizationId = GetOrganizationId();
                    if (cat.OrganizationId == 0)
                        return Redirect("/blog/Admin/Home/Index");
                    cat.CreatedUserId = GetUserId();
                    cat.CreatedDate = DateTime.Now;
                    cat.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                    if (uploadResult == "")
                        cat.ImageUrl = "ERROR.png";
                    else
                        cat.ImageUrl = uploadResult;
                    if (uploadResultHeader == "")
                        cat.HeaderImageUrl = "ERROR.png";
                    else
                        cat.HeaderImageUrl = uploadResultHeader;
                    cat.ActiveStatus = Data.Enum.EActiveStatus.Active;
                    db.Categories.Add(cat);
                    db.SaveChanges();

                    foreach (var formItem in Request.Form)
                    {
                        if (formItem.ToString().Contains("|"))
                        {
                            string lang = formItem.ToString().Split('|')[0];
                            string property = formItem.ToString().Split('|')[1];
                            CCategoryLanguage cImageLanguage = null;
                            if(cat!=null && cat.LanguageValues!=null)
                               cImageLanguage = cat.LanguageValues.FirstOrDefault(f => f.Lang == lang);
                            string formValue = Request.Form[formItem.ToString()];
                            if (cImageLanguage != null)
                            {
                                if (property == "Name")
                                    cImageLanguage.Name = formValue;
                                if (property == "Description")
                                    cImageLanguage.Description = formValue;
                                if (property == "Content")
                                    cImageLanguage.Content = formValue;
                                if (property == "PageTitle")
                                    cImageLanguage.PageTitle = formValue;
                                if (property == "PageKeyword")
                                    cImageLanguage.PageKeyword = formValue;
                                if (property == "PageDescription")
                                    cImageLanguage.PageDescription = formValue;
                                if (property == "CustomProperty1")
                                    cImageLanguage.CustomProperty1 = formValue;
                                if (property == "CustomProperty2")
                                    cImageLanguage.CustomProperty2 = formValue;
                                if (property == "CustomProperty3")
                                    cImageLanguage.CustomProperty3 = formValue;
                                if (property == "CustomProperty4")
                                    cImageLanguage.CustomProperty4 = formValue;
                                if (property == "CustomProperty5")
                                    cImageLanguage.CustomProperty5 = formValue;
                                if (property == "CustomProperty6")
                                    cImageLanguage.CustomProperty6 = formValue;
                                if (property == "CustomProperty7")
                                    cImageLanguage.CustomProperty7 = formValue;
                                if (property == "CustomProperty8")
                                    cImageLanguage.CustomProperty8 = formValue;
                                if (property == "CustomProperty9")
                                    cImageLanguage.CustomProperty9 = formValue;
                                if (property == "CustomProperty10")
                                    cImageLanguage.CustomProperty10 = formValue;
                            }
                            else
                            {
                                CCategoryLanguage ln = new CCategoryLanguage();
                                if (property == "Name")
                                    ln.Name = formValue;
                                if (property == "Description")
                                    ln.Description = formValue;
                                if (property == "Content")
                                    ln.Content = formValue;
                                if (property == "PageTitle")
                                    ln.PageTitle = formValue;
                                if (property == "PageKeyword")
                                    ln.PageKeyword = formValue;
                                if (property == "PageDescription")
                                    ln.PageDescription = formValue;
                                if (property == "CustomProperty1")
                                    ln.CustomProperty1 = formValue;
                                if (property == "CustomProperty2")
                                    ln.CustomProperty2 = formValue;
                                if (property == "CustomProperty3")
                                    ln.CustomProperty3 = formValue;
                                if (property == "CustomProperty4")
                                    ln.CustomProperty4 = formValue;
                                if (property == "CustomProperty5")
                                    ln.CustomProperty5 = formValue;
                                if (property == "CustomProperty6")
                                    ln.CustomProperty6 = formValue;
                                if (property == "CustomProperty7")
                                    ln.CustomProperty7 = formValue;
                                if (property == "CustomProperty8")
                                    ln.CustomProperty8 = formValue;
                                if (property == "CustomProperty9")
                                    ln.CustomProperty9 = formValue;
                                if (property == "CustomProperty10")
                                    ln.CustomProperty10 = formValue;
                                ln.ActiveStatus = EActiveStatus.Active;
                                ln.CreatedDate = DateTime.Now;
                                ln.Lang = lang;
                                ln.CreatedUserId = GetUserId();
                                ln.OrganizationId = GetOrganizationId();
                                ln.CategoryId = cat.Id;
                                db.CategoryLanguage.Add(ln);
                                db.SaveChanges();
                            }
                        }
                    }

                    InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Başarılı bir şekilde eklenmiştir. ", AlertType = EAlertType.Success });

                }
                else
                {
                    int orgId = GetOrganizationId();
                    if (orgId == 0)
                        return Redirect("/blog/Admin/Home/Index");
                    CCategory Dbcg = db.Categories.Where(p => p.Id == cat.Id && p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active).FirstOrDefault();
                    if (Dbcg != null)
                    {
                        if (uploadResult != "")
                        {
                            Dbcg.ImageUrl = uploadResult;
                        }
                        if (uploadResultHeader != "")
                            Dbcg.HeaderImageUrl = uploadResultHeader;
                        Dbcg.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);

                        foreach (var formItem in Request.Form)
                        {
                            if (formItem.ToString().Contains("|"))
                            {
                                string lang = formItem.ToString().Split('|')[0];
                                string property = formItem.ToString().Split('|')[1];
                                var cImageLanguage = Dbcg.LanguageValues.FirstOrDefault(f => f.Lang == lang);
                                string formValue = Request.Form[formItem.ToString()];
                                if (cImageLanguage != null)
                                {
                                    if (property == "Name")
                                        cImageLanguage.Name = formValue;
                                    if (property == "Description")
                                        cImageLanguage.Description = formValue;
                                    if (property == "Content")
                                        cImageLanguage.Content = formValue;
                                    if (property == "PageTitle")
                                        cImageLanguage.PageTitle = formValue;
                                    if (property == "PageKeyword")
                                        cImageLanguage.PageKeyword = formValue;
                                    if (property == "PageDescription")
                                        cImageLanguage.PageDescription = formValue;
                                    if (property == "CustomProperty1")
                                        cImageLanguage.CustomProperty1 = formValue;
                                    if (property == "CustomProperty2")
                                        cImageLanguage.CustomProperty2 = formValue;
                                    if (property == "CustomProperty3")
                                        cImageLanguage.CustomProperty3 = formValue;
                                    if (property == "CustomProperty4")
                                        cImageLanguage.CustomProperty4 = formValue;
                                    if (property == "CustomProperty5")
                                        cImageLanguage.CustomProperty5 = formValue;
                                    if (property == "CustomProperty6")
                                        cImageLanguage.CustomProperty6 = formValue;
                                    if (property == "CustomProperty7")
                                        cImageLanguage.CustomProperty7 = formValue;
                                    if (property == "CustomProperty8")
                                        cImageLanguage.CustomProperty8 = formValue;
                                    if (property == "CustomProperty9")
                                        cImageLanguage.CustomProperty9 = formValue;
                                    if (property == "CustomProperty10")
                                        cImageLanguage.CustomProperty10 = formValue;
                                }
                                else
                                {
                                    CCategoryLanguage ln = new CCategoryLanguage();
                                    if (property == "Name")
                                        ln.Name = formValue;
                                    if (property == "Description")
                                        ln.Description = formValue;
                                    if (property == "Content")
                                        ln.Content = formValue;
                                    if (property == "PageTitle")
                                        ln.PageTitle = formValue;
                                    if (property == "PageKeyword")
                                        ln.PageKeyword = formValue;
                                    if (property == "PageDescription")
                                        ln.PageDescription = formValue;
                                    if (property == "CustomProperty1")
                                        ln.CustomProperty1 = formValue;
                                    if (property == "CustomProperty2")
                                        ln.CustomProperty2 = formValue;
                                    if (property == "CustomProperty3")
                                        ln.CustomProperty3 = formValue;
                                    if (property == "CustomProperty4")
                                        ln.CustomProperty4 = formValue;
                                    if (property == "CustomProperty5")
                                        ln.CustomProperty5 = formValue;
                                    if (property == "CustomProperty6")
                                        ln.CustomProperty6 = formValue;
                                    if (property == "CustomProperty7")
                                        ln.CustomProperty7 = formValue;
                                    if (property == "CustomProperty8")
                                        ln.CustomProperty8 = formValue;
                                    if (property == "CustomProperty9")
                                        ln.CustomProperty9 = formValue;
                                    if (property == "CustomProperty10")
                                        ln.CustomProperty10 = formValue;
                                    ln.ActiveStatus = EActiveStatus.Active;
                                    ln.CreatedDate = DateTime.Now;
                                    ln.CreatedUserId = GetUserId();
                                    ln.OrganizationId = GetOrganizationId();
                                    ln.CategoryId = Dbcg.Id;
                                    db.CategoryLanguage.Add(ln);
                                    db.SaveChanges();
                                }
                            }
                        }

                        db.SaveChanges();
                        InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Başarılı bir şekilde düzenlenmiştir.", AlertType = EAlertType.Success });
                        return RedirectToAction("InsertCategory", "Category", new { Id = cat.Id, CatId = cat.SubCategoryId });
                    }
                    else
                    {
                        InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                        return RedirectToAction("Dashboard", "Home");
                    }
                }


                var topCategory = db.Categories.Where(item => item.Id == cat.SubCategoryId).FirstOrDefault();
                if (topCategory != null)
                {

                    cat.Level = topCategory.Level + 1;
                }
                else
                    cat.Level = 0;

                db.SaveChanges();

                return RedirectToAction("InsertCategory", "Category", new { CatId = cat.SubCategoryId });
            }
            if (cat.Id != 0)
            {
                List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                ViewBreadCrumb breadCrumb = new ViewBreadCrumb();
                breadCrumb.Name = "";
                breadCrumb.Url = "/blog/Admin/Category/InsertCategory";
                breadCrumb.Level = -1;
                breadCrumbList.Add(breadCrumb);

                ViewBreadCrumb breadCrumb1 = new ViewBreadCrumb();
                breadCrumb1.Name = "Kategori Düzenle";
                breadCrumb1.Url = "";
                breadCrumb1.Level = 1;
                breadCrumbList.Add(breadCrumb1);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            }
            else
            {
                List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                ViewBreadCrumb breadCrumb = new ViewBreadCrumb();
                breadCrumb.Name = "";
                breadCrumb.Url = "/blog/Admin/Category/InsertCategory";
                breadCrumb.Level = -1;
                breadCrumbList.Add(breadCrumb);

                ViewBreadCrumb breadCrumb1 = new ViewBreadCrumb();
                breadCrumb1.Name = "Kategori Ekle";
                breadCrumb1.Url = "";
                breadCrumb1.Level = 1;
                breadCrumbList.Add(breadCrumb1);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            }
            ViewBag.SubCatId = 0;
            ViewBag.ItemThemeId = 0;

            return View(cat);
        }
        private List<SelectListItem> DropDownListItem = new List<SelectListItem>();
        [HttpGet]
        public bool DeleteCategory(int id)
        {
            int organizationId = GetOrganizationId();
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var cat = db.Categories.Where(item => item.OrganizationId == organizationId && item.Id == id).FirstOrDefault();
            if (cat != null)
            {
                cat.ActiveStatus = EActiveStatus.Passive;
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Kategori başarılı bir şekilde silindi", Title = "Başarılı" });
                return true;
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Error, Title = "Hata", Desc = "Kategori Silerken Bir Hata Oluştu." });
                return false;
            }

        }
        private void InsertSubCategory(int catId, List<CCategory> fullCat)
        {
            foreach (var t in fullCat.Where(p => p.SubCategoryId == catId))
            {
                SelectListItem listItem1 = new SelectListItem();
                string level = "";
                if (t.Level != null)
                {
                    for (int i = 0; i < t.Level; i++)
                    {
                        level += " - ";
                    }
                }
                listItem1.Text = level + t.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR") != null ? t.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR").Name : "";
                listItem1.Value = t.Id.ToString();
                DropDownListItem.Add(listItem1);
                InsertSubCategory(t.Id, fullCat);
            }
        }
        [HttpGet]
        public JsonResult GetFullCategory()
        {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            DbDataContext dt = new DbDataContext("CloudAppWebSite");
            int organizationId = GetOrganizationId();
            var FullCat = dt.Categories.Where(item => item.OrganizationId == organizationId && item.ActiveStatus == EActiveStatus.Active).ToList();
            foreach (var item in FullCat.Where(item => item.SubCategoryId == null))
            {

                SelectListItem listItem = new SelectListItem();
                listItem.Text = item.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR") != null ? item.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR").Name : "";
                listItem.Value = item.Id.ToString();
                DropDownListItem.Add(listItem);
                foreach (var t in FullCat.Where(p => p.SubCategoryId == item.Id))
                {
                    SelectListItem listItem1 = new SelectListItem();
                    string level = "";
                    if (t.Level != null)
                    {
                        for (int i = 0; i < t.Level; i++)
                        {
                            level += " - ";
                        }
                    }
                    listItem1.Text = level + t.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR") != null ? t.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR").Name : "";
                    listItem1.Value = t.Id.ToString();
                    DropDownListItem.Add(listItem1);
                    if (t.SubCategoryId.HasValue == true)
                        InsertSubCategory(t.Id, FullCat);
                }
            }
            return Json(DropDownListItem, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string GetCategoryName(string id)
        {
            int orgId = GetOrganizationId();
            int Id = Convert.ToInt32(id);
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var nn = db.Categories.FirstOrDefault(s => s.Id == Id && s.ActiveStatus == EActiveStatus.Active && s.OrganizationId == orgId);
            if (nn != null)
            {
                return nn.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR") != null ? nn.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR").Name : "";
            }
            else
                return "";
        }

    }
}