using CloudApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Web.Core
{
    public class CloudAppViewEngine : RazorViewEngine
    {
        private string BrandName = "";
        private string[] _newAreaViewLocations = new string[] {
        "~/Areas/{2}/%1Views/{1}/{0}.cshtml",
        "~/Areas/{2}/%1Views/{1}/{0}.vbhtml",
        "~/Areas/{2}/%1Views//Shared/{0}.cshtml",
        "~/Areas/{2}/%1Views//Shared/{0}.vbhtml"
    };

        private string[] _newAreaMasterLocations = new string[] {
        "~/Areas/{2}/%1Views/{1}/{0}.cshtml",
        "~/Areas/{2}/%1Views/{1}/{0}.vbhtml",
        "~/Areas/{2}/%1Views/Shared/{0}.cshtml",
        "~/Areas/{2}/%1Views/Shared/{0}.vbhtml"
    };

        private string[] _newAreaPartialViewLocations = new string[] {
        "~/Areas/{2}/%1Views/{1}/{0}.cshtml",
        "~/Areas/{2}/%1Views/{1}/{0}.vbhtml",
        "~/Areas/{2}/%1Views/Shared/{0}.cshtml",
        "~/Areas/{2}/%1Views/Shared/{0}.vbhtml"
    };

        private string[] _newViewLocations = new string[] {
        "~/%1Views/{1}/{0}.cshtml",
        "~/%1Views/{1}/{0}.vbhtml",
        "~/%1Views/Shared/{0}.cshtml",
        "~/%1Views/Shared/{0}.vbhtml"
    };

        private string[] _newMasterLocations = new string[] {
        "~/%1Views/{1}/{0}.cshtml",
        "~/%1Views/{1}/{0}.vbhtml",
        "~/%1Views/Shared/{0}.cshtml",
        "~/%1Views/Shared/{0}.vbhtml"
    };

        private string[] _newPartialViewLocations = new string[] {
        "~/%1Views/{1}/{0}.cshtml",
        "~/%1Views/{1}/{0}.vbhtml",
        "~/%1Views/Shared/{0}.cshtml",
        "~/%1Views/Shared/{0}.vbhtml"
    };

        public CloudAppViewEngine()
            : base()
        {


            AreaViewLocationFormats = AppendLocationFormats(_newAreaViewLocations, AreaViewLocationFormats);

            AreaMasterLocationFormats = AppendLocationFormats(_newAreaMasterLocations, AreaMasterLocationFormats);

            AreaPartialViewLocationFormats = AppendLocationFormats(_newAreaPartialViewLocations, AreaPartialViewLocationFormats);

            ViewLocationFormats = AppendLocationFormats(_newViewLocations, ViewLocationFormats);

            MasterLocationFormats = AppendLocationFormats(_newMasterLocations, MasterLocationFormats);

            PartialViewLocationFormats = AppendLocationFormats(_newPartialViewLocations, PartialViewLocationFormats);
        }

        private string[] AppendLocationFormats(string[] newLocations, string[] defaultLocations)
        {
            List<string> viewLocations = new List<string>();
            viewLocations.AddRange(newLocations);
            viewLocations.AddRange(defaultLocations);
            return viewLocations.ToArray();
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string customCshtml = "";
            if (controllerContext.RouteData.Values["lang"].ToString().Trim().ToLower() == "ar-sa")
                customCshtml = "-ar-sa.cshtml";
            BaseWebController wb = new BaseWebController();
            int orgId = wb.GetOrganizationId(controllerContext.HttpContext.Request);
            if (String.IsNullOrEmpty(customCshtml))
                return base.CreateView(controllerContext, viewPath.Replace("%1", "Theme/" + orgId + "/"), masterPath);
            else
            {
                string path = viewPath.Replace("%1", "Theme/" + orgId + "/");
                if (!path.EndsWith("ar-sa.cshtml"))
                    path = path.Substring(0, path.Length - 7) + customCshtml;
                return base.CreateView(controllerContext, path, masterPath);
            }
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string customCshtml = "";
            if (controllerContext.RouteData.Values["lang"].ToString().Trim().ToLower() == "ar-sa")
                customCshtml = "-ar-sa.cshtml";
            BaseWebController wb = new BaseWebController();
            int orgId = wb.GetOrganizationId(controllerContext.HttpContext.Request);
            if (String.IsNullOrEmpty(customCshtml))
                return base.CreatePartialView(controllerContext, partialPath.Replace("%1", "Theme/" + orgId + "/"));
            else
            {
                string path = partialPath.Replace("%1", "Theme/" + orgId + "/");
                if (!path.EndsWith("ar-sa.cshtml"))
                    path = path.Substring(0, path.Length - 7) + customCshtml;
                return base.CreatePartialView(controllerContext, path);
            }
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            BaseWebController wb = new BaseWebController();
            int orgId = wb.GetOrganizationId(controllerContext.HttpContext.Request);
            return base.FileExists(controllerContext, virtualPath.Replace("%1", "Theme/" + orgId + "/"));
        }
    }
}