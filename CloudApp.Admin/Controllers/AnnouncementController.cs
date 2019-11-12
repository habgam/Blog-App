using CloudApp.Admin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class AnnouncementController : BaseController
    {
        // GET: Announcement
        public ActionResult Index()
        {
            return View();
        }
    }
}