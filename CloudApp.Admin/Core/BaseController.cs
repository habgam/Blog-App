using CloudApp.Data.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudApp.Admin.Controllers;
using CloudApp.Data.Model;
using CloudApp.Data;
using CloudApp.Admin.Membership;

namespace CloudApp.Admin.Core
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
            var userManager =
           new UserManager<CUser>(new UserStoreService(
               new DbDataContext("CloudAppWebSite")) { });
            userManager.PasswordHasher =
                      new MyPasswordHasher();
            UserManager = userManager;

           
        }
        public void InsertAlert(ViewAlert alert)
        {

            if (Session["SSAlert"] != null)
                ((List<ViewAlert>)Session["SSAlert"]).Add(alert);
            else
            {
                List<ViewAlert> alertList = new List<ViewAlert>();
                alertList.Add(alert);
                Session.Add("SSAlert", alertList);
                //Session["SS:Alert"] = alertList;
            }
        }
        private UserManager<CUser> UserManager;
        private IAuthenticationManager GetAuthenticationManager()
        {
            var ctx = Request.GetOwinContext();
            return ctx.Authentication;
        }
        public int GetOrganizationId()
        {
            if (((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "OrganizationId") != null)
            {
                string adress = Request.Url.Host;
                string port = Request.Url.Port.ToString();
                string[] identityAdress = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Address").Value.Split(',');
                string[] identityPort = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Port").Value.Split(',');
                if (!identityAdress.Contains( adress) && !identityPort.Contains(port) )
                {
                    GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return 0;
                }
                else
                {
                    return ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "OrganizationId") != null ? Convert.ToInt32(((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "OrganizationId").Value) : 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public int GetUserId()
        {

            return ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "UserId") != null ? Convert.ToInt32(((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "UserId").Value) : 0;
        }
        public string GetUserName()
        {
            if (((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "UserName") != null)
            {
                string adress = Request.Url.Host;
                string port = Request.Url.Port.ToString();
                string[] identityAdress = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Address").Value.Split(',');
                string[] identityPort = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Port").Value.Split(',');
                if (!identityAdress.Contains(adress) && !identityPort.Contains(port))
                {
                    GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return "";
                }
                else
                {
                    return ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "UserName") != null ? ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "UserName").ToString() : "-";
                }
            }
            else
            {
                return "";
            }
        }
        public string GetName()
        {
            if (((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Name") != null)
            {
                string adress = Request.Url.Host;
                string port = Request.Url.Port.ToString();
                string[] identityAdress = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Address").Value.Split(',');
                string[] identityPort = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Port").Value.Split(',');
                if (!identityAdress.Contains(adress) && !identityPort.Contains(port))
                {
                    GetAuthenticationManager().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    return "";
                }
                else
                {
                    return ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Name") != null ? ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(item => item.Type.ToString() == "Name").ToString() : "-";
                }

            }
            else
            {
                return "";
            }
        }
    }
}