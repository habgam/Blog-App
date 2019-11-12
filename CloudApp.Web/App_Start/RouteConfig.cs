using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CloudApp.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "HomePage", action = "Index", id = UrlParameter.Optional }

            );
          //  routes.MapRoute(
          //    name: "TextDetail111",
          //    url: "Yazi/111",
          //    defaults: new { controller = "CategoryView", action = "Index"  }
          //);
      //      routes.MapRoute(
      //         name: "TextDetail1",
      //         url: "Yazi/{cat1}/{tex}",
      //         defaults: new { controller = "Text", action = "GetTextDetail", cat1 = "", tex ="" }
      //     );

      //      routes.MapRoute(
      //        name: "TextDetail2",
      //        url: "Yazi/{cat1}/{cat2}/{tex}",
      //        defaults: new { controller = "Text", action = "GetTextDetail", cat1 = UrlParameter.Optional, cat2 = UrlParameter.Optional, tex = UrlParameter.Optional }
      //    );

      //      routes.MapRoute(
      //      name: "TextDetail3",
      //      url: "Yazi/{cat1}/{cat2}/{cat3}/{tex}",
      //      defaults: new { controller = "Text", action = "GetTextDetail", cat1 = UrlParameter.Optional, cat2 = UrlParameter.Optional, cat3 = UrlParameter.Optional, tex = UrlParameter.Optional }
      //  );
      //      routes.MapRoute(
      //    name: "TextDetail4",
      //    url: "Yazi/{cat1}/{cat2}/{cat3}/{cat4}/{tex}",
      //    defaults: new { controller = "Text", action = "GetTextDetail", cat1 = UrlParameter.Optional, cat2 = UrlParameter.Optional, cat3 = UrlParameter.Optional, cat4 = UrlParameter.Optional, tex = UrlParameter.Optional }
      //);

        }
    }
}
