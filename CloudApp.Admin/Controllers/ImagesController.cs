using CloudApp.Admin.Core;
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CloudApp.Admin.Controllers
{
    public class ImagesController : BaseController
    {
        // GET: Images
        [HttpGet]
        public ActionResult Index(string type, string Id)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            List<CImage> imageList = new List<CImage>();
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int intId = Convert.ToInt32(Id);
            EImageType enumType = ((EImageType)Convert.ToInt32(type));
            if (enumType == EImageType.Slider)
            {
                //ViewBag.SliderName = db.sl
                imageList = db.Images.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.SliderId == intId).ToList();
            }
            else if (enumType == EImageType.Text)
            {
                var texts = db.Texts.Where(p => p.Id == intId && p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active).FirstOrDefault();
                if (texts.LanguageValues.FirstOrDefault(h => h.Lang == "TR") != null)
                    ViewBag.TextName = texts.LanguageValues.FirstOrDefault(h => h.Lang == "TR").Name;
                else
                    ViewBag.TextName = "";
                imageList = db.Images.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.TextId == intId).ToList();
            }
            //if(enumType.)
            int intType = Convert.ToInt32(type);
            ViewBag.type = intType;
            ViewBag.id = intId;

            return View(imageList);
        }
        [HttpGet]
        public ActionResult GetImage(string Id, string type, string pictureId)
        {
            ViewBag.Languages = ConfigurationManager.AppSettings["Languages"].Split(',').ToList();
            int intId = Convert.ToInt32(Id);
            int intType = Convert.ToInt32(type);
            int intPictureId = 0;
            if (!String.IsNullOrEmpty(pictureId))
            {
                intPictureId = Convert.ToInt32(pictureId);
            }
            ViewBag.type = intType;
            ViewBag.id = intId;
            ViewBag.PictureId = intPictureId;
            if (intPictureId == 0)
                return View();
            else
            {
                int orgId = GetOrganizationId();
                if (orgId == 0)
                    return Redirect("/blog/Admin/Home/Index");
                DbDataContext db = new DbDataContext("CloudAppWebSite");
                CImage cimg = db.Images.Where(p => p.Id == intPictureId && p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active).FirstOrDefault();
                if (cimg != null)
                {
                    return View(cimg);
                }
                else
                {
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });
                    if (Request.Form["queryType"] == "1")
                    {
                        return Redirect("/blog/Admin/Images/Index/?Id=" + intId + "&type=" + type + "");
                    }
                    else if (Request.Form["queryType"] == "2")
                    {
                        return Redirect("/blog/Admin/Images/Index/?Id=" + intId + "&type=" + type + "");
                    }
                }
            }
            return View();
        }
        public bool DeleteImage(int id)
        {
            int organizationId = GetOrganizationId();

            DbDataContext db = new DbDataContext("CloudAppWebSite");
            var cat = db.Images.Where(item => item.OrganizationId == organizationId && item.Id == id).FirstOrDefault();
            if (cat != null)
            {
                cat.ActiveStatus = EActiveStatus.Passive;
                db.SaveChanges();
                InsertAlert(new ViewAlert { AlertType = EAlertType.Success, Desc = "Resim başarılı bir şekilde silindi", Title = "Başarılı" });
                return true;
            }
            else
            {
                InsertAlert(new ViewAlert { AlertType = EAlertType.Error, Title = "Hata", Desc = "Resim Silerken Bir Hata Oluştu." });
                return false;
            }

        }
        [HttpPost]
        public ActionResult GetImage(CImage item)
        {
            List<string> languages = ConfigurationManager.AppSettings["Languages"].Split(',').ToList();
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            int userId = GetUserId();
            int orgId = GetOrganizationId();
            if (orgId == 0)
                return Redirect("/blog/Admin/Home/Index");
            int pictureId = 0;
            if (!String.IsNullOrEmpty(Request.Form["queryPictureId"]))
            {
                pictureId = Convert.ToInt32(Request.Form["queryPictureId"]);
            }
            if (pictureId != 0)
            {
                CImage img = db.Images.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == pictureId).FirstOrDefault();
                if (img != null)
                {
                    img.AltText = item.AltText;
                    img.Description = item.Description;
                    img.Name = item.Name;
                    var ff = Request.Files[0];
                    ImageController ic = new ImageController();
                    if (!String.IsNullOrEmpty(ff.FileName))
                    {
                        string uploadResult = ic.UploadImage(ff, Server.MapPath("~/"), GetOrganizationId());
                        if (uploadResult == "")
                            img.ImageUrl = "ERROR.png";
                        else
                            img.ImageUrl = uploadResult;
                    }
                    db.SaveChanges();
                    InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Resim başarılı bir şekilde düzenlenmiştir.", AlertType = EAlertType.Success });
                }
                else
                {
                    InsertAlert(new ViewAlert { AlertType = EAlertType.Warning, Title = "Uyarı", Desc = "Yetkisiz Giriş" });

                }
            }
            else
            {
                item.CreatedUserId = userId;
                item.OrganizationId = orgId;
                item.CreatedDate = DateTime.Now;
                var ff = Request.Files[0];
                ImageController ic = new ImageController();
                string uploadResult = ic.UploadImage(ff, Server.MapPath("~/"), GetOrganizationId());
                if (uploadResult == "")
                    item.ImageUrl = "ERROR.png";
                else
                    item.ImageUrl = uploadResult;
                if (Request.Form["queryType"] == "1")
                {
                    item.Type = EImageType.Text;
                    item.TextId = Convert.ToInt32(Request.Form["queryId"]);
                }
                else if (Request.Form["queryType"] == "2")
                {
                    item.Type = EImageType.Slider;
                    item.SliderId = Convert.ToInt32(Request.Form["queryId"]);
                }
                item.ActiveStatus = EActiveStatus.Active;
                db.Images.Add(item);
                db.SaveChanges();

             

                InsertAlert(new ViewAlert { Title = "Başarılı", Desc = "Resim başarılı bir şekilde eklenmiştir.", AlertType = EAlertType.Success });
            }
            if (Request.Form["queryType"] == "1")
            {
                return Redirect("/blog/Admin/Images/Index/?Id=" + Request.Form["queryId"] + "&type=" + Request.Form["queryType"] + "");
            }
            else if (Request.Form["queryType"] == "2")
            {
                return Redirect("/blog/Admin/Images/Index/?Id=" + Request.Form["queryId"] + "&type=" + Request.Form["queryType"] + "");
            }

            return View();
        }
    }
}