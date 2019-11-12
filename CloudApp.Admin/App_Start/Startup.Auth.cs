using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using CloudApp.Admin.Membership;
using CloudApp.Data.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using CloudApp.Data;
[assembly: OwinStartup(typeof(CloudApp.Admin.Startup))]
namespace CloudApp.Admin
{

    public partial class Startup
    {
        public static Func<UserManager<ApplicationUser>> UserManagerFactory { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
        {
            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            LoginPath = new PathString("/Home/Index")
        });

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "404148299409-5u56vfsc09vi0p1j7dhtohulvr8heuor.apps.googleusercontent.com",
            //    ClientSecret = "59uIkUcZV0zpvUkBIpxw_s1k"
            //});

        
        }

    }
}