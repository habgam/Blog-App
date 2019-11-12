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
    public class TextController : BaseController
    {
        // GET: Text
        private int PageSize = 15;
        public ActionResult Index(string Id, string page)
        {
            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
            breadCrumb3.Name = "";
            breadCrumb3.Url = "";
            breadCrumb3.Level = -1;
            breadCrumbList.Add(breadCrumb3);

            ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
            breadCrumbAll.Name = "Tüm Yazılar";
            breadCrumbAll.Url = "";
            breadCrumbAll.Level = 999999;
            breadCrumbList.Add(breadCrumbAll);
            ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();

            DbDataContext db = new DbDataContext("CloudAppWebSite");
            List<CText> textList = new List<CText>();
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int intPage = Convert.ToInt32(page);
            int catId = Convert.ToInt32(Id);
            if (catId != 0)
            {
                textList = db.Texts.Where(item => item.OrganizationId == orgId && item.CategoryId == catId && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(p => p.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.ListCount = db.Texts.Where(item => item.OrganizationId == orgId && item.CategoryId == catId).Count();
            }
            else
            {
                textList = db.Texts.Where(item => item.OrganizationId == orgId && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(p => p.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.ListCount = db.Texts.Where(item => item.OrganizationId == orgId).Count();
            }
            return View(textList);
        }
        [HttpGet]
        public ActionResult Insert(string id)
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
                breadCrumb1.Name = "Yazı Düzenle";
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
                breadCrumb1.Name = "Yazı Ekle";
                breadCrumb1.Url = "";
                breadCrumb1.Level = 1;
                breadCrumbList.Add(breadCrumb1);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
            }

            if (!String.IsNullOrEmpty(id))
            {
                int orgId = GetOrganizationId();
                if (orgId == 0)
                    return Redirect("/blog/Admin/Home/Index");
                int reelId = Convert.ToInt32(id);
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                CText cte = db.Texts.Where(item => item.OrganizationId == orgId && item.ActiveStatus == EActiveStatus.Active && item.Id == reelId).FirstOrDefault();
                if (cte != null)
                {
                    ViewBag.ItemThemeId = cte.ItemThemeId;
                    ViewBag.CatId = cte.CategoryId;
                    return View(cte);
                }
                else
                {
                    ViewBag.ItemThemeId = 0;
                    ViewBag.CatId = 0;
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            else
            {
                ViewBag.ItemThemeId = 0;
                ViewBag.CatId = 0;
                CText ct = new CText();
                return View(ct);
            }
        }
        [HttpGet]
        public bool DeleteText(int id)
        {
            int organizationId = GetOrganizationId();
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var cat = db.Texts.Where(item => item.OrganizationId == organizationId && item.Id == id).FirstOrDefault();
            if (cat != null)
            {
                cat.ActiveStatus = EActiveStatus.Passive;
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Yazı başarılı bir şekilde silindi", Title = "Başarılı" });
                return true;
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Error, Title = "Hata", Desc = "Yazı Silerken Bir Hata Oluştu." });
                return false;
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(CText item)
        {
            if (ModelState.IsValid)
            {
                var ff = Request.Files[0];
                ImageController ic = new ImageController();
                string uploadResult = ic.UploadImage(ff, Server.MapPath("~/"), GetOrganizationId());
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                int orgId = GetOrganizationId();
                if (orgId == 0)
                    return Redirect("/blog/Admin/Home/Index");
                if (item.Id != 0)
                {
                    CText cte = db.Texts.Where(itemdb => itemdb.OrganizationId == orgId && itemdb.ActiveStatus == EActiveStatus.Active && itemdb.Id == item.Id).FirstOrDefault();
                    if (cte != null)
                    {
                        if (uploadResult != "")
                        {
                            cte.ImageUrl = uploadResult;
                        }
                        cte.CategoryId = Convert.ToInt32(Request.Form["SubCategoryId"]);


                        foreach (var formItem in Request.Unvalidated.Form)
                        {
                            if (formItem.ToString().Contains("|"))
                            {
                                string lang = formItem.ToString().Split('|')[0];
                                string property = formItem.ToString().Split('|')[1];
                                CTextLanguage cImageLanguage = null;
                                if (cte != null && cte.LanguageValues != null)
                                    cImageLanguage = cte.LanguageValues.FirstOrDefault(f => f.Lang == lang);
                                string formValue = Request.Unvalidated.Form[formItem.ToString()];
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
                                    db.SaveChanges();
                                }
                                else
                                {
                                    CTextLanguage ln = new CTextLanguage();

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
                                    ln.Lang = lang;
                                    ln.OrganizationId = GetOrganizationId();
                                    ln.TextId = item.Id;
                                    db.TextLanguage.Add(ln);
                                    db.SaveChanges();
                                }
                            }
                        }


                        cte.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                        db.SaveChanges();
                        InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Başarılı bir şekilde Düzenlenmiştir.", AlertType = EAlertType.Success });

                        return Redirect(String.Format("/blog/Admin/Text/Index/?Id={0}&page={1}", cte.CategoryId, 1));


                    }
                    else
                    {
                        InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
                else
                {
                    if (uploadResult == "")
                        item.ImageUrl = "ERROR.png";
                    else
                        item.ImageUrl = uploadResult;
                    item.CategoryId = Convert.ToInt32(Request.Form["SubCategoryId"]);
                    item.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                    item.ActiveStatus = Data.Enum.EActiveStatus.Active;
                    item.CreatedDate = DateTime.Now;
                    item.CreatedUserId = GetUserId();
                    item.OrganizationId = orgId;
                    db.Texts.Add(item);
                    db.SaveChanges();
                    InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Başarılı bir şekilde eklenmiştir.", AlertType = EAlertType.Success });

                    foreach (var formItem in Request.Unvalidated.Form)
                    {
                        if (formItem.ToString().Contains("|"))
                        {
                            string lang = formItem.ToString().Split('|')[0];
                            string property = formItem.ToString().Split('|')[1];
                            CTextLanguage cImageLanguage = null;
                            if (item != null && item.LanguageValues != null)
                                cImageLanguage = item.LanguageValues.FirstOrDefault(f => f.Lang == lang);
                            string formValue = Request.Unvalidated.Form[formItem.ToString()];
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
                                db.SaveChanges();
                            }
                            else
                            {
                                CTextLanguage ln = new CTextLanguage();

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
                                ln.Lang = lang;
                                ln.OrganizationId = GetOrganizationId();
                                ln.TextId = item.Id;
                                db.TextLanguage.Add(ln);
                                db.SaveChanges();
                            }
                        }
                    }



                    return Redirect(String.Format("/blog/Admin/Text/Index/?Id={0}&page={1}", item.CategoryId, 1));


                }
            }
            else
            {
                if (item.Id != 0)
                {
                    List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                    ViewBreadCrumb breadCrumb = new ViewBreadCrumb();
                    breadCrumb.Name = "";
                    breadCrumb.Url = "";
                    breadCrumb.Level = -1;
                    breadCrumbList.Add(breadCrumb);

                    ViewBreadCrumb breadCrumb1 = new ViewBreadCrumb();
                    breadCrumb1.Name = "Yazı Düzenle";
                    breadCrumb1.Url = "";
                    breadCrumb1.Level = 1;
                    breadCrumbList.Add(breadCrumb1);
                    ViewBag.BreadCrumbList = breadCrumbList.OrderBy(brd => brd.Level).ToList();
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
                    breadCrumb1.Name = "Yazı Ekle";
                    breadCrumb1.Url = "";
                    breadCrumb1.Level = 1;
                    breadCrumbList.Add(breadCrumb1);
                    ViewBag.BreadCrumbList = breadCrumbList.OrderBy(brd => brd.Level).ToList();
                }
                ViewBag.ItemThemeId = 0;
                ViewBag.CatId = 0;
                return View(item);
            }

            return View();
        }
        private List<SelectListItem> DropDownListItem = new List<SelectListItem>();
        [HttpGet]
        public JsonResult GetFullText()
        {
            List<SelectListItem> ListItem = new List<SelectListItem>();
            DbDataContext dt = new DbDataContext("CloudAppWebSite");
            int organizationId = GetOrganizationId();
            var FullCat = dt.Categories.Where(item => item.OrganizationId == organizationId && item.ActiveStatus == EActiveStatus.Active).ToList();
            foreach (var item in FullCat.Where(item => item.SubCategoryId == null && item.ActiveStatus == EActiveStatus.Active))
            {
                foreach (var subItem in item.Texts)
                {
                    SelectListItem listItem = new SelectListItem();
                    listItem.Text =( item.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR") != null ? item.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR").Name : "") + "/" + (subItem.LanguageValues.FirstOrDefault(s => s.Lang == "tr-TR") != null ? subItem.LanguageValues.FirstOrDefault(s => s.Lang == "tr-TR").Name : "");
                    listItem.Value = subItem.Id.ToString();
                    DropDownListItem.Add(listItem);
                }
                foreach (var t in FullCat.Where(p => p.SubCategoryId == item.Id && p.ActiveStatus == EActiveStatus.Active))
                {
                    foreach (var p in t.Texts.Where(f=>f.ActiveStatus == EActiveStatus.Active))
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
                        listItem1.Text = level + (t.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR") != null ? t.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR").Name : "") + "/" + (p.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR") != null ? p.LanguageValues.FirstOrDefault(f => f.Lang == "tr-TR").Name : "");
                        listItem1.Value = p.Id.ToString();
                        DropDownListItem.Add(listItem1);
                        if (t.SubCategoryId.HasValue == true)
                            InsertSubText(t.Id, FullCat);
                    }
                }
            }
            return Json(DropDownListItem, JsonRequestBehavior.AllowGet);
        }
        private void InsertSubText(int catId, List<CCategory> fullCat)
        {
            foreach (var t in fullCat.Where(p => p.SubCategoryId == catId))
            {
                foreach (var p in t.Texts)
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
                    listItem1.Text = level + t.LanguageValues.FirstOrDefault(f => f.Lang == "TR") != null ? t.LanguageValues.FirstOrDefault(f => f.Lang == "TR").Name : "" + "/" + p.LanguageValues.FirstOrDefault(f => f.Lang == "TR") != null ? p.LanguageValues.FirstOrDefault(f => f.Lang == "TR").Name : "";
                    listItem1.Value = t.Id.ToString();
                    DropDownListItem.Add(listItem1);
                    InsertSubText(t.Id, fullCat);
                }
            }
        }




    }
}