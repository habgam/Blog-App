using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace CloudApp.Admin.Core
{
    public class ImageController
    {
        private string guid = "";
        public ImageController()
        {
            guid = Guid.NewGuid().ToString();
        }
        public string UploadImage(HttpPostedFileBase ff, string serverMappath , int orgId)
        {

            ResizeThumbAndSave(150, 300, serverMappath + "Image/150X300/"+ orgId  , ff);
              ResizeThumbAndSave(300, 150, serverMappath + "Image/300X150/" + orgId, ff);
              ResizeThumbAndSave(360, 300, serverMappath + "Image/360X300/"+orgId, ff);
              ResizeThumbAndSave(1140, 400, serverMappath + "Image/1140X400/"+orgId, ff);
              SaveOrjinal(ff, serverMappath + "Image/Orji/" + orgId);
             string result = ResizeThumbAndSave(800, 800, serverMappath + "Image/800X800/" + orgId, ff);
           
                return result;
        }
        private string ResizeThumbAndSave(int kwidth, int kheight, string FileName, HttpPostedFileBase ff)
        {
            Image imgFile;
            if (ff == null)
            {

                return "";
            }
            if (ff.ContentType.ToLower() != "image/jpg" && ff.ContentType.ToLower() != "image/jpeg" && ff.ContentType.ToLower() != "image/image/pjpeg" && ff.ContentType.ToLower() != "image/gif" && ff.ContentType.ToLower() != "image/x-png" && ff.ContentType.ToLower() != "image/png")
            {

                return "";
            }
            imgFile = System.Drawing.Image.FromStream(ff.InputStream);
            if (imgFile.Width == 0 || imgFile.Height == 0)
            {

                return "";
            }
            int sw = imgFile.Width;
            int sh = imgFile.Height;
            while (sh > kheight || sw > kwidth)
            {
                sh = sh * 90 / 100;
                sw = sw * 90 / 100;
            }
            Size ss = new Size(sw, sh);
            Bitmap bmp = new Bitmap(imgFile, ss);
           
            bmp.Save(FileName + "\\" + guid + ".png");
            bmp.Dispose();
            return guid + ".png";
        }
        private string SaveOrjinal(HttpPostedFileBase ff,string fileName)
        {
            Image imgFile;
            if (ff == null)
            {

                return "";
            }
            if (ff.ContentType.ToLower() != "image/jpg" && ff.ContentType.ToLower() != "image/jpeg" && ff.ContentType.ToLower() != "image/image/pjpeg" && ff.ContentType.ToLower() != "image/gif" && ff.ContentType.ToLower() != "image/x-png" && ff.ContentType.ToLower() != "image/png")
            {

                return "";
            }
            imgFile = System.Drawing.Image.FromStream(ff.InputStream);
            if (imgFile.Width == 0 || imgFile.Height == 0)
            {

                return "";
            }
            Bitmap bmp = new Bitmap(imgFile);

            bmp.Save(fileName + "\\" + guid + ".png");
            bmp.Dispose();
            return guid + ".png";
        }


    }
}