using CloudApp.Admin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class HtmlConvertController : BaseController
    {
        // GET: HtmlConvert
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string id)
        {
            //HtmlSiteConverter convert = new HtmlSiteConverter();
            //var form = Request.Form["uploadFile"];
            //HttpPostedFileBase file = Request.Files[0];
            //convert.StartConvert(GetOrganizationId(), Server.MapPath("~/"), file,GetOrganizationId().ToString());
            return View();
        }
    }
}