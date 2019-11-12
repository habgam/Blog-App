using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using CloudApp.Web.Core;
using CloudApp.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CloudApp.Web.Controllers
{
    public class HomePageController : BaseWebController
    {
        // GET: Home
        public ActionResult Index()
        {
            bool isOnline = GetOrganizationIsOnline(Request);
            if (isOnline == false)
            {
                return RedirectToAction("AppOffline", "HomePage");
            }
            SetTheme();
            return Redirect("/blog/"+GetCulture() + "/Home");
        }
        [Route("{lang}/Home")]
        public ActionResult Home()
        {
            bool isOnline = GetOrganizationIsOnline(Request);
            if (isOnline == false)
            {
                return RedirectToAction("AppOffline", "HomePage");
            }
            SetCulture(RouteData.Values["lang"].ToString());
            SetTheme();
            return View();
        }
        //public bool SeedDataStart()
        //{
        //    DbDataContext context = new DbDataContext();
        //    COrganization org = new COrganization
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        IsOffline = false,
        //        Name = "Organization",
        //        CreatedDate = DateTime.Now
        //    };
        //    context.Organizations.Add(org);
        //    context.SaveChanges();

        //    context.AddressBindings.Add(new CAddressBindings
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        Address = "localhost",
        //        Port = "6565",
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();
        //    context.Users.Add(new CUser
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "ismail.erden",
        //        OrganizationId = org.OrganizationId,
        //        UserName = "ismail.erden",
        //        Password = "827CCB0EEA8A706C4C34A16891F84E7B"
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "Ana Menü",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_MainMenu.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Menu
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "Footer Menü",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_FooterMenu.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Menu
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "Ürün Kategorisi",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_ProductCategory.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Category
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "Yazı Kategorisi",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_TextCategory.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Category
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "Yazı",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_OnlyText.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Text
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "Galeri",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_MainPageSlider.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Slider
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "Ürün",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_ProductDetail.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Text
        //    });
        //    context.SaveChanges();
        //    context.ItemThemes.Add(new CItemTheme
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        Name = "İletişim",
        //        OrganizationId = org.OrganizationId,
        //        ThemePath = "_Contact.cshtml",
        //        ThemeType = Data.Enum.EItemTheme.Text
        //    });
        //    context.SaveChanges();
        //    context.Categories.Add(new CCategory
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_ProductCategory.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Category).Id,
        //        OrganizationId = org.OrganizationId,
        //        Name = "Ürün Kategorisi"
        //    });
        //    context.SaveChanges();
        //    context.Categories.Add(new CCategory
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_TextCategory.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Category).Id,
        //        OrganizationId = org.OrganizationId,
        //        Name = "Site Yazıları"
        //    });
        //    context.SaveChanges();
        //    context.Menus.Add(new CMenu
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_MainMenu.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Menu).Id,
        //        Name = "Ana Menü",
        //        OrganizationId = org.OrganizationId
        //    });
        //    context.SaveChanges();
        //    context.Menus.Add(new CMenu
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CreatedDate = DateTime.Now,
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_FooterMenu.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Menu).Id,
        //        Name = "Footer Menü",
        //        OrganizationId = org.OrganizationId
        //    });
        //    context.SaveChanges();
        //    context.Texts.Add(new CText
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CategoryId = context.Categories.FirstOrDefault(s => s.Name == "Site Yazıları").Id,
        //        Name = "Kurumsal",
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_OnlyText.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Text).Id,
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();
        //    context.Texts.Add(new CText
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CategoryId = context.Categories.FirstOrDefault(s => s.Name == "Site Yazıları").Id,
        //        Name = "Hakkımızda",
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_OnlyText.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Text).Id,
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();
        //    context.Texts.Add(new CText
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CategoryId = context.Categories.FirstOrDefault(s => s.Name == "Site Yazıları").Id,
        //        Name = "İletişim",
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_Contact.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Text).Id,
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();
        //    context.Texts.Add(new CText
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CategoryId = context.Categories.FirstOrDefault(s => s.Name == "Ürün Kategorisi").Id,
        //        Name = "Ürün 1",
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_ProductDetail.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Text).Id,
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();
        //    context.Texts.Add(new CText
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CategoryId = context.Categories.FirstOrDefault(s => s.Name == "Ürün Kategorisi").Id,
        //        Name = "Ürün 2",
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_ProductDetail.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Text).Id,
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();
        //    context.Texts.Add(new CText
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        CategoryId = context.Categories.FirstOrDefault(s => s.Name == "Ürün Kategorisi").Id,
        //        Name = "Ürün 3",
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_ProductDetail.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Text).Id,
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();
        //    context.Sliders.Add(new CSlider
        //    {
        //        ActiveStatus = Data.Enum.EActiveStatus.Active,
        //        Name = "Galeri",
        //        ItemThemeId = context.ItemThemes.FirstOrDefault(f => f.ThemePath == "_MainPageSlider.cshtml" && f.ThemeType == Data.Enum.EItemTheme.Slider).Id,
        //        OrganizationId = org.OrganizationId,
        //        CreatedDate = DateTime.Now
        //    });
        //    context.SaveChanges();

        //    return true;
        //}
        public ActionResult SalesPoint(string lang)
        {

            SeoUrlController sc = new SeoUrlController();
            return Redirect("/"+sc.GetUrlString(Convert.ToInt32(Request.Form["frmWhere"]), CloudApp.Data.Enum.EMenuType.Category,1, lang));
        }
        public ViewPartialObject GetSliderPartial(int id, HttpRequestBase req,string lang)
        {
            string rootPah = "~/blog/Theme/";
            int orgId = GetOrganizationId(req);
            rootPah += orgId.ToString() + "/Views/SliderPartial/";
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");

            ViewPartialObject view = new ViewPartialObject();
            CSlider slider = db.Sliders.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && p.Id == id).FirstOrDefault();
            if (slider != null)
            {
                view.ViewName = rootPah + slider.ItemThemes.ThemePath;
                view.ViewModel = slider.Images.Where(s => s.ActiveStatus == EActiveStatus.Active && s.Language.ToLower() == lang.ToLower()).ToList();
                return view;
            }
            else
                return null;
        }
        public ActionResult Iletisim()
        {
            return View();
        }
        public ActionResult Kurumsal()
        {
            return View();
        }
        public ActionResult Arsa()
        {
            return View();
        }
        public ActionResult Kariyer()
        {
            return View();
        }
        public ActionResult AppOffline()
        {
            return View();
        }
        public ViewPartialObject GetCategoryPartial(int id, HttpRequestBase req)
        {
            string rootPah = "~/blog/Theme/";
            int orgId = GetOrganizationId(req);
            rootPah += orgId.ToString() + "/Views/CategoryPartial/";
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            ViewPartialObject view = new ViewPartialObject();
            CCategory slider = db.Categories.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && p.Id == id).FirstOrDefault();
            if (slider != null)
            {
                view.ViewName = rootPah + slider.ItemTheme.ThemePath;
                view.ViewModel = slider;
                return view;
            }
            else
                return null;
        }

        public ViewPartialObject GetMenuPartial(int id, HttpRequestBase req,string lang)
        {
            string rootPath = "~/blog/Theme/";
            int orgId = GetOrganizationId(req);
            rootPath += orgId.ToString() + "/Views/MenuPartial/";
            SeoUrlController sc = new SeoUrlController();
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            CMenu mm = db.Menus.Where(p => p.Id == id && p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active).FirstOrDefault();
            if (mm == null)
                return null;
            else
            {
                ViewPartialObject menu = new ViewPartialObject();
                menu.ViewName = rootPath + mm.ItemTheme.ThemePath;
                menu.ViewModel = sc.GetViewMenu(mm,lang);
                return menu;
            }

        }
        public ViewPartialObject GetTextPartial(int id, HttpRequestBase req)
        {
            string rootPah = "~/blog/Theme/";
            int orgId = GetOrganizationId(req);
            rootPah += orgId.ToString() + "/Views/TextPartial/";
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            ViewPartialObject view = new ViewPartialObject();
            CText slider = db.Texts.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && p.Id == id).FirstOrDefault();
            if (slider != null)
            {
                view.ViewName = rootPah + slider.ItemTheme.ThemePath;
                view.ViewModel = slider;
                return view;
            }
            else
                return null;
        }


        [HttpGet]
        [Route("SiteMap")]
        public ActionResult SiteMap(string lang)
        {
            int orgId = GetOrganizationId(Request);
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            SeoUrlController sc = new SeoUrlController();
            COrganization cg = db.Organizations.Where(k => k.OrganizationId == orgId).FirstOrDefault();
            string adress = cg.AdressBindings.OrderByDescending(s => s.Address.Length).FirstOrDefault().Address;
            if (cg != null)
            {
                Response.Clear();
                Response.ContentType = "text/xml";
                XmlTextWriter xr = new XmlTextWriter(Response.OutputStream, Encoding.UTF8);
                xr.WriteStartDocument();
                xr.WriteStartElement("urlset");//urlset etiketi açıyoruz 
                xr.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
                xr.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xr.WriteAttributeString("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");
                /* sitemap dosyamızın olmazsa olmazını ekledik. Şeması bu dedik buraya kadar.  */

                //--------1-----------//
                xr.WriteStartElement("url");
                xr.WriteElementString("loc", "http://" + adress);
                xr.WriteElementString("lastmod", DateTime.Now.ToString("yyyy-MM-dd"));
                xr.WriteElementString("changefreq", "daily");
                xr.WriteElementString("priority", "1");
                xr.WriteEndElement();
                //------2---------//

                foreach (var p in cg.Texts.Where(p => p.ActiveStatus == EActiveStatus.Active && p.Category.ActiveStatus == EActiveStatus.Active))
                {
                    xr.WriteStartElement("url");
                    xr.WriteElementString("loc", "http://" + adress + "/" + sc.GetUrlString(p.Id, EMenuType.Text, orgId,lang));
                    xr.WriteElementString("lastmod", p.CreatedDate != null ? p.CreatedDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"));
                    xr.WriteElementString("priority", "0.5");
                    xr.WriteElementString("changefreq", "monthly");
                    xr.WriteEndElement();
                }

                foreach (var p in cg.Categories.Where(p => p.ActiveStatus == EActiveStatus.Active))
                {
                    xr.WriteStartElement("url");
                    xr.WriteElementString("loc", "http://" + adress + "/" + sc.GetUrlString(p.Id, EMenuType.Category, orgId,lang));
                    xr.WriteElementString("lastmod", p.CreatedDate != null ? p.CreatedDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd"));
                    xr.WriteElementString("priority", "0.5");
                    xr.WriteElementString("changefreq", "monthly");
                    xr.WriteEndElement();
                }


                xr.WriteEndDocument();
                //urlset etiketini kapattık
                xr.Flush();
                xr.Close();
                Response.End();

            }
            return View();

        }
        public bool CheckFile(HttpPostedFileBase ff)
        {

            if (ff == null)
            {

                return false;
            }
            if (ff.ContentType.ToLower() != "image/jpg" && ff.ContentType.ToLower() != "image/jpeg" && ff.ContentType.ToLower() != "image/image/pjpeg" && ff.ContentType.ToLower() != "image/gif" && ff.ContentType.ToLower() != "image/x-png" && ff.ContentType.ToLower() != "image/png" && ff.ContentType.ToLower() != "application/pdf" && ff.ContentType.ToLower() != "application/msword"&& ff.ContentType.ToLower() != "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
            {

                return false;
            }
            return true;
        }

        [HttpPost]
        public ActionResult InsertForm(string header, string subject, string url,string whoSendMail)
        {

            if (!CheckReCapcha(Request))
            {
                TempData["Message"] = "<script> alert( 'Lütfen güvenliği doğrulayınız.')</script>";
                return Redirect(Request.UrlReferrer.ToString());
            }


            string body = "<div style='margin:10px;font-weight:bold'>" + header + "</div>";
            foreach (var item in Request.Form)
            {
                if (item.ToString().Contains("ExternalFormElement") == true)
                {
                    body += "<div> <span style='font-weight:bold;'>" + Request.Form["ExternalFormHeader_" + item.ToString().Split('_')[1].ToString()] + " : </span> " + Request.Form[item.ToString()] + " </div>";
                }

            }
            HttpPostedFileBase ExternalFormFile = null;
            if (Request.Files.Count > 0)
                ExternalFormFile = Request.Files[0];
            MailService msc = new MailService();
            bool result = false;
            if (CheckFile(ExternalFormFile))
                result = msc.SendMail(subject, body, GetOrganizationId(Request), ExternalFormFile,!String.IsNullOrEmpty(whoSendMail) ? whoSendMail.Split(',').ToList() : null);
            else
                result = msc.SendMail(subject, body, GetOrganizationId(Request), null, !String.IsNullOrEmpty(whoSendMail) ? whoSendMail.Split(',').ToList() : null);
            if (result == true)
            {
                TempData["Message"] = "<script> alert( 'Bilgiler gönderildi.')</script>";
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                TempData["Message"] = "<script> alert( 'Hata Oluştu')</script>";
                return Redirect(Request.UrlReferrer.ToString() );
            }
        }
        private bool CheckReCapcha(HttpRequestBase request)
        {
            var response = request["g-recaptcha-response"];
            string secret = ConfigurationManager.AppSettings["ReCapchaSecretKey"].ToString();
            //Kendi Secret keyinizle değiştirin.

            var client = new WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponseViewModel>(reply);

            return captchaResponse.Success;
        }
        [HttpPost]
        public ActionResult InsertFormDb(string formId)
        {

            if (!CheckReCapcha(Request))
            {
                TempData["Message"] = "<script> alert( 'Lütfen güvenliği doğrulayınız.')</script>";
                return Redirect(Request.UrlReferrer.ToString());
            }


            int id = 0;
            int.TryParse(formId, out id);

            string jsonData = "";
            foreach (var item in Request.Form)
            {
                if (item.ToString().Contains("ExternalFormElement") == true)
                {
                    string header = Request.Form["ExternalFormHeader_" + item.ToString().Split('_')[1].ToString()];
                    string data = Request.Form[item.ToString()];
                    jsonData += "\"" + header + "\": \"" + data + "\",";
                }
            }
            jsonData = jsonData.Substring(0, jsonData.Length - 1);
            jsonData = "{ " + jsonData + " }";
            if (id == 0)
            {
                TempData["Message"] = "<script> alert( 'Hatalı Giriş')</script>";
                return Redirect(Request.UrlReferrer.ToString());
            }
            CFormList lst = new CFormList();
            lst.ActiveStatus = EActiveStatus.Active;
            lst.CreatedDate = DateTime.Now;
            lst.CreatedUserId = 1;
            lst.FormId = id;
            lst.OrganizationId = GetOrganizationId(Request);
            lst.JsonData = jsonData;
            DbDataContext db = new DbDataContext();
            db.FormLists.Add(lst);
            db.SaveChanges();
            TempData["Message"] = "<script> alert( 'Form başarılı bir şekilde gönderilmiştir.')</script>";
            return Redirect(Request.UrlReferrer.ToString());

        }

        //[HttpPost]
        //public List<CText> TextSearch(List<ViewTextSearch> textSearch, HttpRequestBase req)
        //{
        //    DbDataContext db = new DbDataContext("CloudAppWebSiteView");
        //    int orgId = GetOrganizationId(req);
        //    var query = db.Texts.Where(s => s.ActiveStatus == EActiveStatus.Active && s.OrganizationId == orgId);
        //    foreach (var item in textSearch)
        //    {
        //        if (item.SearchProperty == "Name")
        //            query = query.Where(s => s.Name.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "Description")
        //            query = query.Where(s => s.Description.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "Content")
        //            query = query.Where(s => s.Content.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty1")
        //            query = query.Where(s => s.CustomProperty1.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty2")
        //            query = query.Where(s => s.CustomProperty2.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty3")
        //            query = query.Where(s => s.CustomProperty3.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty4")
        //            query = query.Where(s => s.CustomProperty4.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty5")
        //            query = query.Where(s => s.CustomProperty5.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty6")
        //            query = query.Where(s => s.CustomProperty6.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty7")
        //            query = query.Where(s => s.CustomProperty7.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty8")
        //            query = query.Where(s => s.CustomProperty8.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty9")
        //            query = query.Where(s => s.CustomProperty9.Contains(item.SearchValue));
        //        else if (item.SearchProperty == "CustomProperty10")
        //            query = query.Where(s => s.CustomProperty10.Contains(item.SearchValue));
        //    }
        //    return query.ToList();
        //}
    }
}