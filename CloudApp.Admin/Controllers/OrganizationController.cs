using CloudApp.Admin.Core;
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class OrganizationController : BaseController
    {
        // GET: Organization
        [HttpGet]
        public ActionResult EditOrganization()
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            COrganization org = db.Organizations.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId).FirstOrDefault() ;
            if(org!=null)
            {
                return View( org);
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }
        [HttpGet]
        public ActionResult ResetPassword()
        {
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            return View();
        }
        [HttpPost]
        public ActionResult ResetPasswordPost()
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int UserId = GetUserId();
            string oldPassword = Request.Form["oldPassword"];
            string newPassword = Request.Form["newPassword"];
            string newPasswordRetry = Request.Form["newPasswordRety"];
            UtilitiesControl ut = new UtilitiesControl();
            CUser usr = db.Users.Where(p => p.UserId == UserId).FirstOrDefault();
            if(usr.Password==ut.CalculateMD5Hash( oldPassword))
            {
                if(newPassword == newPasswordRetry)
                {
                    if(newPassword.Length<6)
                    {
                        InsertAlert(new ViewAlert { Title = "Başarısız", Desc = "Yeni şifreniz minimum 6 karakter olmalıdır.", AlertType = EAlertType.Error });
                        return Redirect("/blog/Admin/Organization/ResetPassword");
                    }
                    else
                    { 
                    usr.Password = ut.CalculateMD5Hash( newPassword);
                    db.SaveChanges();
                    InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Şifre Başarılı Bir Şekilde Değiştirilmiştir.", AlertType = EAlertType.Success });
                    return Redirect("/blog/Admin/Organization/ResetPassword");
                    }
                }
                else
                {
                    InsertAlert(new ViewAlert { Title = "Başarısız", Desc = "Yeni şifre ile şifre tekrar uyuşmuyor.", AlertType = EAlertType.Error });
                    return Redirect("/blog/Admin/Organization/ResetPassword");
                }
            }
            else
            {
                InsertAlert(new ViewAlert { Title = "Başarısız", Desc = "Girdiğiniz Şifre Yanlış", AlertType = EAlertType.Error });
                return Redirect("/blog/Admin/Organization/ResetPassword");
            }

            return View();
        }
        [HttpPost]
        public ActionResult EditOrganization(COrganization item)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            COrganization dbOrganization = db.Organizations.Where(p=>p.ActiveStatus == EActiveStatus.Active && p.OrganizationId==orgId).FirstOrDefault();
            dbOrganization.Name = item.Name;
            dbOrganization.Title = item.Title;
            dbOrganization.SmtpPassword = item.SmtpPassword;
            dbOrganization.SmtpPort = item.SmtpPort;
            dbOrganization.SmtpServer = item.SmtpServer;
            dbOrganization.SmtpUserName = item.SmtpUserName;
            dbOrganization.SmtpUseSSL = item.SmtpUseSSL;
            dbOrganization.SmtpWhoMail = item.SmtpWhoMail;
            dbOrganization.IsOffline = Convert.ToBoolean(Request.Form["isOffline"]);
            dbOrganization.SmtpUseSSL = Convert.ToBoolean(Request.Form["useSSL"]);
            dbOrganization.GoogleAnalyticsProfileId = item.GoogleAnalyticsProfileId;
            //var ff = Request.Files[0];
            //ImageController ic = new ImageController();
            //string uploadResult = ic.UploadImage(ff, Server.MapPath("~/"),GetOrganizationId());
            //if (uploadResult == "")
            //{

            //}
            //else
            //    dbOrganization.ImageUrl = uploadResult;
            db.SaveChanges();
            InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Organizasyon Ayarları Güncellendi.", AlertType = EAlertType.Success });
            return Redirect("/blog/Admin/Organization/EditOrganization");
        }
    }
}