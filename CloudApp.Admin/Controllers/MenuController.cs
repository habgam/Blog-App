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
    public class MenuController : BaseController
    {
        // GET: Menu
        private int PageSize = 15;
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertSubMenu(CMenuItem item)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int intId = Convert.ToInt32(Request.Form["QueryId"]);
            int intMenuId = Convert.ToInt32(Request.Form["QueryMenuId"]);
            int intSubMenuId = Convert.ToInt32(Request.Form["QuerySubMenuId"]);
            int UserId = GetUserId();
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            if (ModelState.IsValid)
            {
                if (intSubMenuId == 0)
                {
                    EMenuType type = ((EMenuType)Convert.ToInt32(Request.Form["MenuType"]));
                    if (type == EMenuType.Category || type == EMenuType.CategoryContent)
                    {
                        item.CategoryId = Convert.ToInt32(Request.Form["MenuConnect"]);
                    }
                    if (type == EMenuType.Text)
                    {
                        item.TextId = Convert.ToInt32(Request.Form["MenuConnect"]);
                    }
                    if (intMenuId != 0)
                    {
                        item.Level = 0;
                        item.MenuId = intMenuId;
                    }
                    if (intId != 0)
                    {
                        item.Level = db.MenuItems.Where(p => p.Id == intId && p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active).Select(m => m.Level).FirstOrDefault() + 1;
                        item.SubMenuId = intId;
                    }
                    item.OrganizationId = orgId;
                    item.CreatedUserId = UserId;
                    item.CreatedDate = DateTime.Now;
                    item.ActiveStatus = EActiveStatus.Active;
                    db.MenuItems.Add(item);
                    db.SaveChanges();

                    foreach (var formItem in Request.Form)
                    {
                        if (formItem.ToString().Contains("-"))
                        {
                            string lang = formItem.ToString().Split('|')[0];
                            string property = formItem.ToString().Split('|')[1];
                            CMenuItemLanguage cImageLanguage = null;
                            if (item != null && item.LanguageValues != null)
                                cImageLanguage = item.LanguageValues.FirstOrDefault(f => f.Lang == lang);

                            string formValue = Request.Form[formItem.ToString()];
                            if (cImageLanguage != null)
                            {
                                if (property == "Name")
                                    cImageLanguage.Name = formValue;
                                db.SaveChanges();
                            }
                            else
                            {
                                CMenuItemLanguage ln = new CMenuItemLanguage();
                                if (property == "Name")
                                    ln.Name = formValue;

                                ln.ActiveStatus = EActiveStatus.Active;
                                ln.CreatedDate = DateTime.Now;
                                ln.CreatedUserId = GetUserId();
                                ln.Lang = lang;
                                ln.OrganizationId = GetOrganizationId();
                                ln.MenuItemId = item.Id;
                                db.MenuItemLanguage.Add(ln);
                                db.SaveChanges();
                            }
                        }
                    }


                    InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Alt menü başarılı bir şekilde eklenmiştir.", AlertType = EAlertType.Success });
                    return Redirect("/blog/Admin/Menu/InsertSubMenu/?Id=" + intId + "&MenuId=" + intMenuId + "");
                    //return RedirectToAction("InsertSubMenu", "Menu", new { Id = intId, MenuId = intMenuId });

                }
                else
                {
                    CMenuItem cmm = db.MenuItems.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == intSubMenuId).FirstOrDefault();
                    if (cmm != null)
                    {
                        EMenuType type = ((EMenuType)Convert.ToInt32(Request.Form["MenuType"]));
                        if (type == EMenuType.Category || type == EMenuType.CategoryContent)
                        {
                            cmm.CategoryId = Convert.ToInt32(Request.Form["MenuConnect"]);
                        }
                        if (type == EMenuType.Text)
                        {
                            cmm.TextId = Convert.ToInt32(Request.Form["MenuConnect"]);
                        }
                        //cmm.Name = item.Name;
                        foreach (var formItem in Request.Form)
                        {
                            if (formItem.ToString().Contains("-"))
                            {
                                string lang = formItem.ToString().Split('|')[0];
                                string property = formItem.ToString().Split('|')[1];
                                CMenuItemLanguage cImageLanguage = null;
                                if (cmm != null && cmm.LanguageValues != null)
                                    cImageLanguage = cmm.LanguageValues.FirstOrDefault(f => f.Lang == lang);
                                string formValue = Request.Form[formItem.ToString()];
                                if (cImageLanguage != null)
                                {
                                    if (property == "Name")
                                        cImageLanguage.Name = formValue;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    CMenuItemLanguage ln = new CMenuItemLanguage();
                                    if (property == "Name")
                                        ln.Name = formValue;
                                   
                                    ln.ActiveStatus = EActiveStatus.Active;
                                    ln.CreatedDate = DateTime.Now;
                                    ln.CreatedUserId = GetUserId();
                                    ln.Lang = lang;
                                    ln.OrganizationId = GetOrganizationId();
                                    ln.MenuItemId = cmm.Id;
                                    db.MenuItemLanguage.Add(ln);
                                    db.SaveChanges();
                                }
                            }
                        }

                        cmm.Url = item.Url;
                        cmm.MenuType = type;
                        cmm.Order = item.Order;
                        db.SaveChanges();
                        InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Alt menü başarılı bir şekilde Düzenlenmiştir.", AlertType = EAlertType.Success });
                        return Redirect("/blog/Admin/Menu/InsertSubMenu/?Id=" + intId + "&MenuId=" + intMenuId + "&SubMenuId=" + cmm.Id + "");
                    }
                    else
                    {
                        InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
            }
            ViewBag.BreadCrumbList = GetBreadCrumbList(intId.ToString(), intMenuId.ToString()).OrderBy(p => p.Level).ToList();
            CMenuItem mm = new CMenuItem();
            return View(mm);
        }
        private List<ViewBreadCrumb> GetBreadCrumbList(string Id, string menuId)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            int orgId = GetOrganizationId();
            int intMenuId = Convert.ToInt32(menuId);
            int intId = Convert.ToInt32(Id);
            if (intMenuId != 0)
            {

                ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                breadCrumb3.Name = "";
                breadCrumb3.Url = "/blog/Admin/Menu/GetMenu/?id=0&page=1";
                breadCrumb3.Level = -1;
                breadCrumbList.Add(breadCrumb3);

                ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
                breadCrumbAll.Name = db.Menus.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == intMenuId).Select(s => s.Name).FirstOrDefault();
                breadCrumbAll.Url = "";
                breadCrumbAll.Level = 999999;
                breadCrumbList.Add(breadCrumbAll);
                //menuItemList = db.MenuItems.Where(item => item.OrganizationId == orgId && item.MenuId == intMenuId && item.ActiveStatus == EActiveStatus.Active).ToList();

                //ViewBag.ListCount = db.MenuItems.Where(item => item.OrganizationId == orgId && item.MenuId == intMenuId && item.ActiveStatus == EActiveStatus.Active).Count();
            }
            if (intId != 0)
            {
                ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                breadCrumb3.Name = "";
                breadCrumb3.Url = "/blog/Admin/Menu/GetMenu/?id=0&page=1";
                breadCrumb3.Level = -1;
                breadCrumbList.Add(breadCrumb3);

                CMenuItem cm = db.MenuItems.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == intId).FirstOrDefault();
                ViewBreadCrumb breadCrumb = new ViewBreadCrumb();
                breadCrumb.Name = cm.LanguageValues.FirstOrDefault(s=>s.Lang=="TR")!=null ? cm.LanguageValues.FirstOrDefault(s => s.Lang == "TR").Name:"" ;
                breadCrumb.Url = "/blog/Admin/Menu/GetSubMenuItem/?MenuId=0&id=" + cm.Id + "&page=1";
                breadCrumb.Level = 9999;
                breadCrumbList.Add(breadCrumb);

                if (cm.MenuId.HasValue == true)
                {
                    CMenu subMenu = db.Menus.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == cm.MenuId).FirstOrDefault();
                    ViewBreadCrumb breadCrumbSubm = new ViewBreadCrumb();
                    breadCrumbSubm.Name = subMenu.Name;
                    breadCrumbSubm.Url = "/blog/Admin/Menu/GetSubMenuItem/?MenuId=" + subMenu.Id + "&id=0&page=1";
                    breadCrumbSubm.Level = 0;
                    breadCrumbList.Add(breadCrumbSubm);
                }

                bool listContinue = false;
                int breadCrumbId = cm.SubMenuId.HasValue == true ? cm.SubMenuId.Value : 0;
                while (listContinue == false)
                {
                    CMenuItem subCat = db.MenuItems.Where(item => item.Id == breadCrumbId && item.ActiveStatus == EActiveStatus.Active && item.OrganizationId == orgId).FirstOrDefault();
                    if (subCat != null)
                    {
                        ViewBreadCrumb breadCrumbSub = new ViewBreadCrumb();
                        breadCrumbSub.Name = subCat.LanguageValues.FirstOrDefault(s => s.Lang == "TR") != null ? subCat.LanguageValues.FirstOrDefault(s => s.Lang == "TR").Name : "";
                        breadCrumbSub.Url = "/blog/Admin/Menu/GetSubMenuItem/?MenuId=0&id=" + subCat.Id + "&page=1";
                        breadCrumbSub.Level = (subCat.Level != 0 ? subCat.Level : 0) + 1;
                        breadCrumbList.Add(breadCrumbSub);
                        breadCrumbId = subCat.SubMenuId.HasValue == true ? subCat.SubMenuId.Value : 0;

                        if (subCat.MenuId.HasValue == true)
                        {
                            CMenu subMenu = db.Menus.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == subCat.MenuId).FirstOrDefault();
                            ViewBreadCrumb breadCrumbSubm = new ViewBreadCrumb();
                            breadCrumbSubm.Name = subMenu.Name;
                            breadCrumbSubm.Url = "/blog/Admin/Menu/GetSubMenuItem/?MenuId=" + subCat.Id + "&id=0&page=1";
                            breadCrumbSubm.Level = subCat.Level != 0 ? subCat.Level : 0;
                            breadCrumbList.Add(breadCrumbSubm);
                        }

                        if (breadCrumbId == 0)
                            listContinue = true;
                    }
                    else
                    {

                        listContinue = true;
                    }
                }
            }
            return breadCrumbList;
        }
        [HttpGet]
        public ActionResult InsertSubMenu(string menuId, string Id, string subMenuId)
        {
            ViewBag.Languages = ConfigurationManager.AppSettings["Languages"].Split(',').ToList();
            DbDataContext db = new DbDataContext("CloudAppWebSite");

            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int userId = GetUserId();
            int intMenuId = Convert.ToInt32(menuId);
            int intId = Convert.ToInt32(Id);
            int intSubMenuId = Convert.ToInt32(subMenuId);
            ViewBag.QueryMenuId = intMenuId;
            ViewBag.QueryId = intId;
            ViewBag.QuerySubMenuId = intSubMenuId;

            ViewBag.BreadCrumbList = GetBreadCrumbList(Id, menuId).OrderBy(item => item.Level).ToList();
            CMenuItem mMenuItem = new CMenuItem();
            if (intSubMenuId!=0)
            {
                int reelSubId = Convert.ToInt32(subMenuId);
                mMenuItem = db.MenuItems.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == reelSubId).FirstOrDefault();
                if (mMenuItem != null)
                {
                    ViewBag.MenuType =(int) mMenuItem.MenuType;
                    ViewBag.TextId = mMenuItem.TextId.HasValue == true ? mMenuItem.TextId.Value : 0;
                    ViewBag.CategoryId = mMenuItem.CategoryId.HasValue == true ? mMenuItem.CategoryId.Value : 0;
                    return View(mMenuItem);
                }
                else
                {
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                    return RedirectToAction("Dashboard", "Home");
                }

            }
            else
            {
                ViewBag.MenuType = 0;
                ViewBag.TextId =0;
                ViewBag.CategoryId =0;
                mMenuItem = new CMenuItem();
            }

            return View(mMenuItem);
        }
        public ActionResult GetSubMenuItem(string menuId, string Id, string page)
        {
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int userId = GetUserId();
            int intPage = Convert.ToInt32(page);
            int intMenuId = Convert.ToInt32(menuId);
            int intId = Convert.ToInt32(Id);

            List<CMenuItem> menuItemList = new List<CMenuItem>();
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            ViewBag.CurrentPage = page;
            List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
            if (intMenuId != 0)
            {
                menuItemList = db.MenuItems.Where(item => item.OrganizationId == orgId && item.MenuId == intMenuId && item.ActiveStatus == EActiveStatus.Active).ToList();
                ViewBag.ListCount = db.MenuItems.Where(item => item.OrganizationId == orgId && item.MenuId == intMenuId && item.ActiveStatus == EActiveStatus.Active).Count();
            }
            if (intId != 0)
            {


                menuItemList = db.MenuItems.Where(item => item.OrganizationId == orgId && item.SubMenuId == intId && item.ActiveStatus == EActiveStatus.Active).ToList();
                ViewBag.ListCount = db.MenuItems.Where(item => item.OrganizationId == orgId && item.SubMenuId == intId && item.ActiveStatus == EActiveStatus.Active).Count();
            }

            ViewBag.BreadCrumbList = GetBreadCrumbList(Id, menuId).OrderBy(item => item.Level).ToList();
            return View(menuItemList);
        }

        [HttpPost]
        public ActionResult InsertMenu(CMenu item)
        {
            if (ModelState.IsValid)
            {
                if (item.Id == 0)
                {
                    int orgId = GetOrganizationId();
                    if (orgId == 0)
                        return Redirect("/blog/Admin/Home/Index");
                    int userId = GetUserId();
                    item.OrganizationId = orgId;
                    item.CreatedUserId = userId;
                    item.ActiveStatus = EActiveStatus.Active;
                    item.CreatedDate = DateTime.Now;
                    item.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                    DbDataContext db = new DbDataContext("CloudAppWebSite");
                    db.Menus.Add(item);
                    db.SaveChanges();
                    InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Menü başarılı bir şekilde eklenmiştir.", AlertType = EAlertType.Success });
                    return RedirectToAction("InsertMenu", "Menu");
                }
                else
                {
                    int orgId = GetOrganizationId();
                    if (orgId == 0)
                        return Redirect("/blog/Admin/Home/Index");
                    DbDataContext db = new DbDataContext("CloudAppWebSite");
                    CMenu menuDB = db.Menus.Where(p => p.OrganizationId == orgId && p.Id == item.Id).FirstOrDefault();
                    if (menuDB != null)
                    {
                        menuDB.ItemThemeId = Convert.ToInt32(Request.Form["ThemeId"]);
                        menuDB.Name = item.Name;
                        menuDB.Desc = item.Desc;
                        db.SaveChanges();
                        InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Menü başarılı bir şekilde Düzenlenmiştir.", AlertType = EAlertType.Success });
                        return RedirectToAction("InsertMenu", "Menu", new { Id = item.Id });
                    }
                    else
                    {
                        InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                        return RedirectToAction("Dashboard", "Home");
                    }
                }
            }
            else
            {
                if (item.Id != 0)
                {
                    List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                    ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                    breadCrumb3.Name = "";
                    breadCrumb3.Url = "";
                    breadCrumb3.Level = -1;
                    breadCrumbList.Add(breadCrumb3);

                    ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
                    breadCrumbAll.Name = "Menü Düzenle";
                    breadCrumbAll.Url = "";
                    breadCrumbAll.Level = 999999;
                    breadCrumbList.Add(breadCrumbAll);
                    ViewBag.BreadCrumbList = breadCrumbList.OrderBy(p => p.Level).ToList();
                    return View(item);
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
                    breadCrumbAll.Name = "Menü Ekle";
                    breadCrumbAll.Url = "";
                    breadCrumbAll.Level = 999999;
                    breadCrumbList.Add(breadCrumbAll);
                    ViewBag.BreadCrumbList = breadCrumbList.OrderBy(p => p.Level).ToList();
                    return View(item);
                }
                return View(item);
            }
        }


        public bool DeleteMenu(int id)
        {
            int organizationId = GetOrganizationId();
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var cat = db.Menus.Where(item => item.OrganizationId == organizationId && item.Id == id).FirstOrDefault();
            if (cat != null)
            {
                cat.ActiveStatus = EActiveStatus.Passive;
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Menü başarılı bir şekilde silindi", Title = "Başarılı" });
                return true;
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Error, Title = "Hata", Desc = "Menü Silerken Bir Hata Oluştu." });
                return false;
            }

        }

        public bool DeleteSubMenu(int id)
        {
            int organizationId = GetOrganizationId();
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var cat = db.MenuItems.Where(item => item.OrganizationId == organizationId && item.Id == id).FirstOrDefault();
            if (cat != null)
            {
                cat.ActiveStatus = EActiveStatus.Passive;
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Alt menü başarılı bir şekilde silindi", Title = "Başarılı" });
                return true;
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Error, Title = "Hata", Desc = "Alt menü Silerken Bir Hata Oluştu." });
                return false;
            }

        }


        [HttpGet]
        public ActionResult InsertMenu(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                breadCrumb3.Name = "";
                breadCrumb3.Url = "";
                breadCrumb3.Level = -1;
                breadCrumbList.Add(breadCrumb3);

                ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
                breadCrumbAll.Name = "Menü Düzenle";
                breadCrumbAll.Url = "";
                breadCrumbAll.Level = 999999;
                breadCrumbList.Add(breadCrumbAll);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();
                int reelId = Convert.ToInt32(id);
                int orgId = GetOrganizationId();
                if (orgId == 0)
                    return Redirect("/blog/Admin/Home/Index");
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                CMenu menu = db.Menus.Where(p => p.Id == reelId && p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId).FirstOrDefault();
                if (menu != null)
                {
                    ViewBag.ItemThemeId = menu.ItemThemeId;
                    return View(menu);
                }
                else
                {
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                    return RedirectToAction("Dashboard", "Home");
                }
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
                breadCrumbAll.Name = "Menü Ekle";
                breadCrumbAll.Url = "";
                breadCrumbAll.Level = 999999;
                breadCrumbList.Add(breadCrumbAll);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();

                ViewBag.ItemThemeId = 0;
                CMenu menu = new CMenu();
                return View(menu);
            }

        }
        public ActionResult GetMenu(string Id, string page)
        {
            int intReel = Convert.ToInt32(Id);
            int intPage = Convert.ToInt32(page);
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            List<CMenu> MenuList;
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            ViewBag.CurrentPage = page;
            if (intReel == 0)
            {
                List<ViewBreadCrumb> breadCrumbList = new List<ViewBreadCrumb>();
                ViewBreadCrumb breadCrumb3 = new ViewBreadCrumb();
                breadCrumb3.Name = "";
                breadCrumb3.Url = "";
                breadCrumb3.Level = -1;
                breadCrumbList.Add(breadCrumb3);

                ViewBreadCrumb breadCrumbAll = new ViewBreadCrumb();
                breadCrumbAll.Name = "Tüm Menüler";
                breadCrumbAll.Url = "";
                breadCrumbAll.Level = 999999;
                breadCrumbList.Add(breadCrumbAll);
                ViewBag.BreadCrumbList = breadCrumbList.OrderBy(item => item.Level).ToList();

                MenuList = db.Menus.Where(item => item.OrganizationId == orgId && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(item => item.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                ViewBag.ListCount = db.Menus.Where(item => item.OrganizationId == orgId && item.ActiveStatus == EActiveStatus.Active).Count();
            }
            else
            {
                MenuList = null;
                //MenuList = db.Menus.Where(item => item.OrganizationId == orgId && item.SubCategoryId == intId && item.ActiveStatus == EActiveStatus.Active).OrderByDescending(item => item.Id).Skip((intPage - 1) * PageSize).Take(PageSize).ToList();
                //ViewBag.ListCount = db.Menus.Where(item => item.OrganizationId == orgId && item.SubCategoryId == intId && item.ActiveStatus == EActiveStatus.Active).Count();
            }

            return View(MenuList);
        }
    }
}