using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudApp.Web.Models
{
    public class ViewPartialObject
    {
        public string ViewName { get; set; }
        public object ViewModel { get; set; }
    }
}