using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Web.Core;
using CloudApp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Web.Controllers
{
    public class TextViewController : BaseWebController
    {
        // GET: Text
        [HttpGet]
        [Route("{lang}/Pages/{cat1}/{tex}")]
        [Route("{lang}/Pages/{cat1}/{cat2}/{tex}")]
        [Route("{lang}/Pages/{cat1}/{cat2}/{cat3}/{tex}")]
        [Route("{lang}/Pages/{cat1}/{cat2}/{cat3}/{cat4}/{tex}")]
        [Route("{lang}/Pages/{cat1}/{cat2}/{cat3}/{cat4}/{cat5}/{tex}")]
        [Route("{lang}/Sayfalar/{cat1}/{tex}")]
        [Route("{lang}/Sayfalar/{cat1}/{cat2}/{tex}")]
        [Route("{lang}/Sayfalar/{cat1}/{cat2}/{cat3}/{tex}")]
        [Route("{lang}/Sayfalar/{cat1}/{cat2}/{cat3}/{cat4}/{tex}")]
        [Route("{lang}/Sayfalar/{cat1}/{cat2}/{cat3}/{cat4}/{cat5}/{tex}")]
        public ActionResult GetTextDetail(string cat1, string tex,string lang)
        {
            bool isOnline = GetOrganizationIsOnline(Request);
            if (isOnline == false)
            {
                return RedirectToAction("AppOffline", "HomePage");
            }
            SetTheme();
            int orgId = GetOrganizationId(Request);
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            int id = Convert.ToInt32(StaticWebUtilities.GetQueryId(tex));
            CText text = db.Texts.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && p.Id == id).FirstOrDefault();
            if (text != null)
                return View(text);
            else
                return View();

        }
        public List<ViewNavigation> GetSimilarCategory(CText text, HttpRequestBase req,string lang, string catId = "")
        {
            List<ViewNavigation> nList = new List<ViewNavigation>();
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            SeoUrlController sc = new SeoUrlController();
            int orgId = GetOrganizationId(req);
            if (catId == "")
            {
                if (text.Category.TopCategory != null)
                    foreach (var item in text.Category.TopCategory.SubCategory.Where(p=>p.ActiveStatus==EActiveStatus.Active).ToList())
                    {
                        nList.Add(new ViewNavigation { Name = item.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = sc.GetUrlString(item.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                    }
                else
                {
                    foreach (var item in db.Categories.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && (p.Level == 0 || p.Level == null)).ToList())
                    {
                        nList.Add(new ViewNavigation { Name = item.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = sc.GetUrlString(item.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                    }
                }
            }
            else
            {
                List<int> intList = new List<int>();
                string[] arrCategory = catId.Split(',');
                foreach(var k in arrCategory)
                {
                    intList.Add(Convert.ToInt32(k));
                }
                List<CCategory> cg = db.Categories.Where(p => p.OrganizationId == orgId && intList.Contains(p.Id) && p.ActiveStatus==EActiveStatus.Active).ToList();
                foreach (var item in cg)
                {
                    if (item.TopCategory != null)
                        foreach (var item1 in item.TopCategory.SubCategory.Where(p => p.ActiveStatus == EActiveStatus.Active).ToList())
                        {
                            nList.Add(new ViewNavigation { Name = item1.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item1.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = sc.GetUrlString(item1.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                        }
                    else
                    {
                        foreach (var item1 in db.Categories.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && (p.Level == 0 || p.Level == null)).ToList())
                        {
                            nList.Add(new ViewNavigation { Name = item1.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item1.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = sc.GetUrlString(item1.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                        }
                    }
                }
            }
            return nList;
        }
        public List<ViewNavigation> GetTextNavigation(CText text, HttpRequestBase req,string lang)
        {
            List<ViewNavigation> nList = new List<ViewNavigation>();
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            SeoUrlController sc = new SeoUrlController();
            nList.Add(new ViewNavigation { Name = "Anasayfa", Level = -1, Url = "#" });
            nList.Add(new ViewNavigation { Name = text.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 999, Url = sc.GetUrlString(text.Id, EMenuType.Text, text.OrganizationId.Value,lang) });
            if (text.Category != null)
            {
                nList.Add(new ViewNavigation { Name = text.Category.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.Category.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 4, Url = sc.GetUrlString(text.Category.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                if (text.Category.TopCategory != null)
                {
                    nList.Add(new ViewNavigation { Name = text.Category.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.Category.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 3, Url = sc.GetUrlString(text.Category.TopCategory.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                    if (text.Category.TopCategory.TopCategory != null)
                    {
                        nList.Add(new ViewNavigation { Name = text.Category.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.Category.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 2, Url = sc.GetUrlString(text.Category.TopCategory.TopCategory.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                        if (text.Category.TopCategory.TopCategory.TopCategory != null)
                        {
                            nList.Add(new ViewNavigation { Name = text.Category.TopCategory.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.Category.TopCategory.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 1, Url = sc.GetUrlString(text.Category.TopCategory.TopCategory.TopCategory.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                        }
                    }
                }
            }
            return nList;
        }
        public List<ViewNavigation> GetCategoryNavigation(CCategory text, HttpRequestBase req,string lang)
        {
            List<ViewNavigation> nList = new List<ViewNavigation>();
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            SeoUrlController sc = new SeoUrlController();
            nList.Add(new ViewNavigation { Name = "Anasayfa", Level = -1, Url = "#" });
            nList.Add(new ViewNavigation { Name = text.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 999, Url = sc.GetUrlString(text.Id,EMenuType.Category,text.OrganizationId.Value,lang )});
            if (text.TopCategory != null)
            {
                nList.Add(new ViewNavigation { Name = text.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 4, Url = sc.GetUrlString(text.TopCategory.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                if (text.TopCategory.TopCategory != null)
                {
                    nList.Add(new ViewNavigation { Name = text.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 3, Url = sc.GetUrlString(text.TopCategory.TopCategory.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                    if (text.TopCategory.TopCategory.TopCategory != null)
                    {
                        nList.Add(new ViewNavigation { Name = text.TopCategory.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.TopCategory.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 2, Url = sc.GetUrlString(text.TopCategory.TopCategory.TopCategory.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                        if (text.TopCategory.TopCategory.TopCategory.TopCategory != null)
                        {
                            nList.Add(new ViewNavigation { Name = text.TopCategory.TopCategory.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? text.TopCategory.TopCategory.TopCategory.TopCategory.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Level = 1, Url = sc.GetUrlString(text.TopCategory.TopCategory.TopCategory.TopCategory.Id, EMenuType.Category, GetOrganizationId(req),lang) });
                        }
                    }
                }
            }
            return nList;
        }
        public ActionResult Index()
        {
            bool isOnline = GetOrganizationIsOnline(Request);
            if (isOnline == false)
            {
                return RedirectToAction("AppOffline", "HomePage");
            }
            return View();
        }
    }
}