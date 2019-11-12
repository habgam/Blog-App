using CloudApp.Admin.Api.GoogleAnalytics;
using CloudApp.Admin.Core;
using CloudApp.Admin.Membership;
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        [AllowAnonymous]
        public bool isAuthe()
        {
            return User.Identity.IsAuthenticated;
        }
        public ActionResult ViewSite()
        {
            return Redirect("Http://" + Request.Url.Host + "?uId=" + Guid.NewGuid() + "-a6g9e4-" + Guid.NewGuid());
        }
        private UserManager<CUser> UserManager;
        public HomeController()
        {
            var userManager =
            new UserManager<CUser>(new UserStoreService(
                new DbDataContext("CloudAppWebSite")) { });
            userManager.PasswordHasher =
                      new MyPasswordHasher();
            UserManager = userManager;
        }



        public ActionResult Dashboard()
        {
            //var identity = ((ClaimsIdentity)User.Identity).Claims;
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {

            DbDataContext dt = new DbDataContext("CloudAppWebSite");
            string prt = Request.Url.Port.ToString();
            CAddressBindings address = dt.AddressBindings.FirstOrDefault(a => a.Address.ToLower().Equals(Request.Url.Host.ToLower()) && a.Port == prt && a.ActiveStatus == Data.Enum.EActiveStatus.Active);
            if (address == null)
                return RedirectToAction("UnauthorizedAccess", "Account");
            else
            {

                if (User != null)
                {
                    if (address.OrganizationId == GetOrganizationId())
                        return RedirectToAction("Dashboard", "Home");
                }
            }
            return View(address.Organization);
        }
        [HttpGet]
        [AllowAnonymous]
        public JsonResult ThrowError()
        {
            if (Session["SSAlert"] != null)
            {
                List<ViewAlert> alertList = (List<ViewAlert>)Session["SSAlert"];
                Session.Remove("SSAlert");
                return Json(alertList, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(null, JsonRequestBehavior.AllowGet);
        }
        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Index(string username, string password)
        {
            UtilitiesControl ut = new UtilitiesControl();
            var usr = await UserManager.FindAsync(username,ut.CalculateMD5Hash( password));
            if (usr != null)
            {
                DbDataContext dt = new DbDataContext("CloudAppWebSite");
                string prt = Request.Url.Port.ToString();
                if (dt.AddressBindings.FirstOrDefault(a => a.Address.ToLower().Equals(Request.Url.Host.ToLower()) && a.Port == prt && a.ActiveStatus == Data.Enum.EActiveStatus.Active).OrganizationId == usr.OrganizationId && usr.ActiveStatus == EActiveStatus.Active)
                {
                    var identity = await UserManager.CreateIdentityAsync(usr, DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim("OrganizationId", usr.OrganizationId.ToString()));
                    identity.AddClaim(new System.Security.Claims.Claim("UserId", usr.UserId.ToString()));
                    identity.AddClaim(new System.Security.Claims.Claim("UserName", usr.UserName.ToString()));
                    identity.AddClaim(new System.Security.Claims.Claim("FullName", usr.Name.ToString()));
                    identity.AddClaim(new System.Security.Claims.Claim("ImageUrl", usr.Organization.ImageUrl != null ? usr.Organization.ImageUrl.ToString() : ""));
                    identity.AddClaim(new System.Security.Claims.Claim("Address", String.Join(",", usr.Organization.AdressBindings.Select(f => f.Address).ToList())));
                    identity.AddClaim(new System.Security.Claims.Claim("Port", String.Join(",", usr.Organization.AdressBindings.Select(f => f.Port).ToList())));
                    GetAuthenticationManager().SignIn(identity);
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            //SignInManager.PasswordSignIn("33","22",false,false);
            //LoginViewModel Identity = userManager.Find("123", "321");

            return RedirectToAction("Index", "Home");
        }
        public ActionResult LogOut()
        {
            Session.Remove("isOrganizationOnline");
            GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
            //GetAuthenticationManager().SignOut((ClaimsIdentity)User.Identity);
        }


        [HttpGet]
        public async Task<ActionResult> OuthGoogle()
        {

            
            //GoogleAnalyticsApi api = new GoogleAnalyticsApi(Server.MapPath("~/"),GetOrganizationId());

            //api.GetMonthVisitorsAndPageviews();
            //Response.Write(api.GetMonthVisitors());
            return View();

            //Operations opt = new Operations(this, new CancellationToken(), "App", "404148299409-5u56vfsc09vi0p1j7dhtohulvr8heuor.apps.googleusercontent.com", "59uIkUcZV0zpvUkBIpxw_s1k");
            //string test = await opt.GetAnalyticsService();
            //Response.Write(test);
            //return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetAnayticsMonthVSU()
        {
            GoogleAnalyticsApi api = new GoogleAnalyticsApi(Server.MapPath("~/"), GetOrganizationId());
            var list = await api.GetMonthVisitorsAndPageviews();
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetAnayticsSourceVSU()
        {
            GoogleAnalyticsApi api = new GoogleAnalyticsApi(Server.MapPath("~/"), GetOrganizationId());
            var list = await api.GetSource();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<string> GetAnayticsRealTimeS()
        {
            GoogleAnalyticsApi api = new GoogleAnalyticsApi(Server.MapPath("~/"), GetOrganizationId());
            return await api.RealTimeVisitors();
        }

    }
}