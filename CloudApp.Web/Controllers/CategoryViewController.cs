using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Web.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Web.Controllers
{
    public class CategoryViewController : BaseWebController
    {
        // GET: Category
        
        public ActionResult Index()
        {
            bool isOnline = GetOrganizationIsOnline(Request);
            if(isOnline==false)
            {
                return RedirectToAction("AppOffline", "HomePage");
            }
            SetTheme();
            return View();
        }
        [HttpGet]
        [Route("{lang}/Yazilar/{cat}")]
        [Route("{lang}/Yazilar/{cat1}/{cat}")]
        [Route("{lang}/Yazilar/{cat1}/{cat2}/{cat}")]
        [Route("{lang}/Yazilar/{cat1}/{cat2}/{cat3}/{cat}")]
        [Route("{lang}/Yazilar/{cat1}/{cat2}/{cat3}/{cat4}/{cat}")]
        [Route("{lang}/Yazilar/{cat1}/{cat2}/{cat3}/{cat4}/{cat5}/{cat}")]
        [Route("{lang}/Categories/{cat}")]
        [Route("{lang}/Categories/{cat1}/{cat}")]
        [Route("{lang}/Categories/{cat1}/{cat2}/{cat}")]
        [Route("{lang}/Categories/{cat1}/{cat2}/{cat3}/{cat}")]
        [Route("{lang}/Categories/{cat1}/{cat2}/{cat3}/{cat4}/{cat}")]
        [Route("{lang}/Categories/{cat1}/{cat2}/{cat3}/{cat4}/{cat5}/{cat}")]
        public ActionResult GetCategory(string cat)
        {
            bool isOnline = GetOrganizationIsOnline(Request);
            if (isOnline == false)
            {
                return RedirectToAction("AppOffline", "HomePage");
            }
            SetTheme();
            int orgId = GetOrganizationId(Request);
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            int id = Convert.ToInt32(StaticWebUtilities.GetQueryId(cat));
            CCategory text = db.Categories.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && p.Id == id).FirstOrDefault();
            if (text != null)
                return View(text);
            else
                return View();
        }
    }
}