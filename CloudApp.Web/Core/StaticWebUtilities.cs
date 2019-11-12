using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudApp.Web.Core
{
    public static class StaticWebUtilities
    {
        public static string GetQueryId(string request)
        {
            return request.Split('-')[request.Split('-').Length - 1].ToString() ;
        }
    }
}