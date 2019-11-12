
using CloudApp.Data;
using CloudApp.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Web.Core
{
    public class BaseWebController : Controller
    {
        public string GetCulture()
        {
            HttpCookie Cookie = null;
            if (HttpContext.Response.Cookies["Culture"] != null)
                //Cookie varsa devam.
                Cookie = HttpContext.Response.Cookies["Culture"];
            else
            { 
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("Culture");
                Cookie.Value = "tr-TR";
            }
            if (String.IsNullOrEmpty(Cookie.Value))
                Cookie.Value = "tr-TR";
            Cookie.Expires = DateTime.Now.AddYears(1); // Süre(1 yıl)
            HttpContext.Response.Cookies.Add(Cookie);
            return Cookie.Value;
            //}
        }
        public void SetCulture(string lang)
        {
            HttpCookie Cookie = null;
            if (HttpContext.Response.Cookies["Culture"] != null)
                //Cookie varsa devam.
                Cookie = HttpContext.Response.Cookies["Culture"];
            else
                //Yoksa oluşturuyoruz.
                Cookie = new HttpCookie("Culture");
            Cookie.Value = lang;
            Cookie.Expires = DateTime.Now.AddYears(1); // Süre(1 yıl)
            HttpContext.Response.Cookies.Add(Cookie);

        }
        public int GetOrganizationId(HttpRequestBase req)
        {
            DbDataContext dt = new DbDataContext("CloudAppWebSiteView");
            string prt = req.Url.Port.ToString();
            return dt.AddressBindings.Where(a => a.Address.ToLower().Equals(req.Url.Host.ToLower()) && a.Port == prt && a.ActiveStatus == Data.Enum.EActiveStatus.Active).Select(t => t.OrganizationId).FirstOrDefault().Value;
        }
        public bool GetOrganizationIsOnline(HttpRequestBase req)
        {


            DbDataContext dt = new DbDataContext("CloudAppWebSiteView");
            string prt = req.Url.Port.ToString();
            bool ReqAuth = false;
            if (Request.QueryString["uId"] != null)
            {
                if (Session["isOrganizationOnline"] == null)
                {
                    string urr = Request.QueryString["uId"].ToString().Substring(37, 6);
                    if (urr == "a6g9e4")
                    {
                        Session["isOrganizationOnline"] = "true";
                        ReqAuth = true;
                    }
                }
                else if (Session["isOrganizationOnline"] != null && Session["isOrganizationOnline"].ToString() == "true")
                {
                    ReqAuth = true;
                }
            }
            else if (Session["isOrganizationOnline"] != null && Session["isOrganizationOnline"].ToString() == "true")
            {
                ReqAuth = true;
            }
            //HomeController ct = new HomeController();
            //bool isauth = ct.isAuthe();
            //WebRequest request = WebRequest.Create("http://" + req.Url.Host + ":" + req.Url.Port + "blog/admin/Home/isAuthe");
            //WebResponse response = request.GetResponse();
            //Stream ResponseStream = response.GetResponseStream();

            //using (StreamReader streamReader = new StreamReader(ResponseStream))       // Load the stream reader to read the response
            //{
            //    var test = streamReader.ReadToEnd(); // Read the entire response and store it in the siteContent variable
            //}

            bool isAuth = dt.AddressBindings.Where(a => a.Address.ToLower().Equals(req.Url.Host.ToLower()) && a.Port == prt && a.ActiveStatus == Data.Enum.EActiveStatus.Active && a.Organization.IsOffline == true).FirstOrDefault() != null ? true : false;
            if (isAuth == false && ReqAuth == true)
                return true;
            else if (isAuth == true)
                return true;
            else
                return false;


        }
        public void SetTheme()
        {
            //ViewEngines.Engines.Clear();

            //ViewEngines.Engines.Add(new CloudAppViewEngine(GetOrganizationId(Request)));
            //ViewEngines.Engines.
        }
    }
}