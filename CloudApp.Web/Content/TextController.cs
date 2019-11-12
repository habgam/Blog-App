using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Web.Content
{
    public class TextController : Controller
    {
        // GET: Text
        public ActionResult GetDetail()
        {
            return View();
        }
    }
}