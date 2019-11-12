
using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CloudApp.Admin.Core
{
    public class HtmlSiteConverter
    {
        public const string _CategoryThemeString = "_CategoryAuto.cshtml";
        public const string _ZipFilePath = "ZipFile";
        public const string _ExtrachFilePath = "ExtrachFile";
        public const string _HtmlConvertPath = "HtmlConvert";
        public const string _TemporaryPath = "Temporary";
        private List<int> _AllowedMenuItemId = new List<int>();
        public string _WebDirectoryPath = ConfigurationManager.AppSettings["WebDirectoryPath"].ToString();
        public string _ServerMappath = "";
        public string _OrganizationId = "0";
        public string _OutputDirectory = "";
        public string _RealThemePath = "";
        public List<string> _PageNameList = new List<string>();
        public string _FileName { get; set; }
        public Guid _GuidPath { get; set; }
        public void ClearFolder(int orgId, string mapPath)
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(mapPath + _TemporaryPath + "/" + orgId + "/" + _HtmlConvertPath);

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }
        public void SaveFile(HttpPostedFileBase file, int orgId, string mapPath)
        {
            _GuidPath = Guid.NewGuid();
            Directory.CreateDirectory(mapPath + _TemporaryPath + "/" + orgId + "/" + _HtmlConvertPath + "/" + _GuidPath);
            Directory.CreateDirectory(mapPath + _TemporaryPath + "/" + orgId + "/" + _HtmlConvertPath + "/" + _GuidPath + "/" + _ZipFilePath);
            Directory.CreateDirectory(mapPath + _TemporaryPath + "/" + orgId + "/" + _HtmlConvertPath + "/" + _GuidPath + "/" + _ExtrachFilePath);
            _FileName = mapPath + _TemporaryPath + "/" + orgId + "/" + _HtmlConvertPath + "/" + _GuidPath + "/" + _ZipFilePath + "/" + file.FileName;
            file.SaveAs(_FileName);
        }
        private static void EnsureDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        public void ExtrachFolder(HttpPostedFileBase file, int orgId, string mapPath)
        {


            SaveFile(file, orgId, mapPath);
            string outputDirectory = mapPath + _TemporaryPath + "/" + orgId + "/" + _HtmlConvertPath + "/" + _GuidPath + "/" + _ExtrachFilePath;
            _OutputDirectory = outputDirectory;
            _RealThemePath = mapPath + "../" + _WebDirectoryPath + "/Theme/" + _OrganizationId + "/Content/";
            ZipInputStream s = new ZipInputStream(File.OpenRead(_FileName));
            ZipEntry theEntry;



            while ((theEntry = s.GetNextEntry()) != null)
            {
                string directoryName = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);



                FileInfo fi = new FileInfo(Path.Combine(outputDirectory, theEntry.Name));
                EnsureDirectory(fi.Directory.FullName);

                if (fileName != String.Empty)
                {
                    FileStream streamWriter = File.Create((Path.Combine(outputDirectory, theEntry.Name)));

                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (size > 0)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            streamWriter.Write(data, 0, size);
                        }
                    }
                    streamWriter.Close();
                }
            }
            s.Close();




        }

        private static void DirectoryCopy(
        string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, true);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        private HtmlDocument ConvertStaticLink(HtmlDocument doc, DbDataContext db)
        {
            SeoUrlController seo = new SeoUrlController();
            int orgId = Convert.ToInt32(_OrganizationId);
            foreach (var item in doc.DocumentNode.SelectNodes("//*[@data-staticmenu='true']"))
            {
                string dataType = item.Attributes["data-type"] != null ? item.Attributes["data-type"].Value : "";
                string dataName = item.Attributes["data-name"] != null ? item.Attributes["data-name"].Value : "";
                string dataCategoryName = item.Attributes["data-categoryname"] != null ? item.Attributes["data-categoryname"].Value : "";
                string dataItemTheme = item.Attributes["data-itemtheme"] != null ? item.Attributes["data-itemtheme"].Value : "";
                if (dataType == "category")
                {
                    CCategory cat;
                    cat = db.Categories.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                         k.OrganizationId == orgId &&
                         k.Name == dataCategoryName).FirstOrDefault();
                    if (cat == null)
                    {
                        CItemTheme itemTheme = new CItemTheme();
                        itemTheme = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                            k.Name == _CategoryThemeString &&
                            k.ActiveStatus == EActiveStatus.Active &&
                            k.ThemeType == EItemTheme.Category).FirstOrDefault();

                        if (itemTheme == null)
                        {
                            itemTheme = new CItemTheme();
                            itemTheme.Name = _CategoryThemeString;
                            itemTheme.OrganizationId = orgId;
                            itemTheme.ThemeType = EItemTheme.Category;
                            itemTheme.ThemePath = _CategoryThemeString;
                            itemTheme.ActiveStatus = EActiveStatus.Active;
                            db.ItemThemes.Add(itemTheme);
                            db.SaveChanges();
                        }

                        cat = new CCategory();
                        cat.Name = dataCategoryName;
                        cat.ActiveStatus = EActiveStatus.Active;
                        cat.CreatedDate = DateTime.Now;
                        cat.CreatedUserId = 1;
                        cat.OrganizationId = orgId;
                        cat.ItemThemeId = itemTheme.Id;
                        db.Categories.Add(cat);
                        db.SaveChanges();
                    }

                    item.Attributes["href"].Value = seo.GetUrlString(cat.Id, EMenuType.Category, orgId);

                }
                else if (dataType == "text")
                {
                    CText text;
                    CCategory cat;
                    text = db.Texts.Where(k => k.OrganizationId == orgId &&
                        k.Name == dataName &&
                        k.Category.Name == dataCategoryName &&
                        k.ActiveStatus == EActiveStatus.Active).FirstOrDefault();

                    cat = db.Categories.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                           k.OrganizationId == orgId &&
                           k.Name == dataCategoryName).FirstOrDefault();

                    if (cat == null)
                    {
                        CItemTheme itemTheme;
                        itemTheme = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                            k.Name == _CategoryThemeString &&
                            k.ActiveStatus == EActiveStatus.Active &&
                            k.ThemeType == EItemTheme.Category).FirstOrDefault();

                        if (itemTheme == null)
                        {
                            itemTheme = new CItemTheme();
                            itemTheme.Name = _CategoryThemeString;
                            itemTheme.OrganizationId = orgId;
                            itemTheme.ThemeType = EItemTheme.Category;
                            itemTheme.ThemePath = _CategoryThemeString;
                            itemTheme.ActiveStatus = EActiveStatus.Active;
                            itemTheme.CreatedUserId = 1;
                            itemTheme.CreatedDate = DateTime.Now;
                            db.ItemThemes.Add(itemTheme);
                            db.SaveChanges();
                        }

                        cat = new CCategory();
                        cat.Name = dataCategoryName;
                        cat.ActiveStatus = EActiveStatus.Active;
                        cat.CreatedDate = DateTime.Now;
                        cat.CreatedUserId = 1;
                        cat.ItemThemeId = itemTheme.Id;
                        cat.OrganizationId = orgId;
                        db.Categories.Add(cat);
                        db.SaveChanges();
                    }

                    if (text == null)
                    {

                        if (String.IsNullOrEmpty(dataItemTheme))
                            throw new Exception("Yazının 'data-itemtheme' değeri boş geçilemez.");
                        CItemTheme itemThemeText;
                        itemThemeText = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                            k.Name == dataItemTheme &&
                            k.ActiveStatus == EActiveStatus.Active &&
                            k.ThemeType == EItemTheme.Text).FirstOrDefault();

                        if (itemThemeText == null)
                        {
                            itemThemeText = new CItemTheme();
                            itemThemeText.Name = dataItemTheme;
                            itemThemeText.OrganizationId = orgId;
                            itemThemeText.ThemeType = EItemTheme.Text;
                            itemThemeText.CreatedDate = DateTime.Now;
                            itemThemeText.CreatedUserId = 1;
                            itemThemeText.ThemePath = dataItemTheme + ".cshtml";
                            itemThemeText.ActiveStatus = EActiveStatus.Active;
                            db.ItemThemes.Add(itemThemeText);
                            db.SaveChanges();
                        }


                        text = new CText();
                        text.Name = dataName;
                        text.OrganizationId = orgId;
                        text.PageTitle = dataName;
                        text.CategoryId = cat.Id;
                        text.ItemThemeId = itemThemeText.Id;
                        text.CreatedDate = DateTime.Now;
                        text.ActiveStatus = EActiveStatus.Active;
                        text.CreatedUserId = 1;
                        db.Texts.Add(text);
                        db.SaveChanges();
                    }
                    item.Attributes.Remove("data-staticmenu");
                    item.Attributes.Remove("data-name");
                    item.Attributes.Remove("data-type");
                    item.Attributes.Remove("data-categoryname");
                    item.Attributes.Remove("data-itemtheme");
                    item.Attributes["href"].Value = seo.GetUrlString(text.Id, EMenuType.Text, orgId);
                }
            }
            return doc;
        }

        private HtmlDocument OuterMenuCshtml(HtmlDocument doc, DbDataContext db, string menuName)
        {
            int orgId = Convert.ToInt32(_OrganizationId);

            HtmlNode nodeMenu = doc.DocumentNode.SelectSingleNode("//*[@data-menu='" + menuName + "']");

            CMenu menu = null;
            if (nodeMenu != null)
            {
                menu = HtmlNodeToCMenu(nodeMenu, db, orgId, menuName);

                foreach (var menuItem in nodeMenu.SelectNodes(".//*[@data-mainmenu-item='True']"))
                {
                    HtmlNodeToCMenuItem(menuItem, db, orgId, menu.Id);
                }
            }

            if (nodeMenu != null)
            {

                foreach (var menuItem in nodeMenu.SelectNodes(".//*[@data-mainmenu-item='True']//a"))
                {

                    string InsertBefore = @" @foreach (var item in Model)  {";
                    string InsertAfter = @"}";
                    menuItem.Attributes["href"].Value = "/@item.Url";
                    menuItem.InnerHtml = "@item.Name";
                    if (menuItem.Attributes["data-type"] != null)
                        menuItem.Attributes.Remove(menuItem.Attributes["data-type"]);
                    if (menuItem.Attributes["data-url"] != null)
                        menuItem.Attributes.Remove(menuItem.Attributes["data-url"]);
                    if (menuItem.Attributes["data-name"] != null)
                        menuItem.Attributes.Remove(menuItem.Attributes["data-name"]);
                    if (menuItem.Attributes["data-categoryname"] != null)
                        menuItem.Attributes.Remove(menuItem.Attributes["data-categoryname"]);
                    menuItem.ParentNode.ParentNode.InnerHtml = InsertBefore + " " + menuItem.ParentNode.OuterHtml + " " + InsertAfter;
                    break;

                }
            }





            if (nodeMenu != null)
            {

                string MenuPartial = _ServerMappath + "../" + _WebDirectoryPath + "/Theme/" + _OrganizationId + "/Views/MenuPartial/" + menuName + ".cshtml";

                byte[] buffer = Encoding.UTF8.GetBytes(nodeMenu.OuterHtml);
                MemoryStream ms = new MemoryStream(buffer);
                //write to file
                FileStream file = new FileStream(MenuPartial, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                ms.Close();

                if (menu != null)
                {
                    string replaceMenu = @"@{
                  CloudApp.Web.Models.ViewPartialObject Menu_" + menu.Id + " = pg.GetMenuPartial(" + menu.Id + @", Request);
                  if (Menu_" + menu.Id + @" != null)
                  {
                      Html.RenderPartial(Menu_" + menu.Id + @".ViewName, Menu_" + menu.Id + @".ViewModel);
                  }

            }";
                    nodeMenu.ParentNode.InnerHtml = replaceMenu;
                }
            }
            return doc;

        }
        public void ConvertLayoutPage()
        {
            _PageNameList = Directory.GetFiles(_OutputDirectory).ToList();

            string orjinalText = "";
            int orgId = Convert.ToInt32(_OrganizationId);
            DbDataContext db = new DbDataContext();
            foreach (var item in _PageNameList.Where(s => s.Contains("index.html")))
            {
                string text = "";
                using (var streamReader = new StreamReader(item, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }

                HtmlDocument doc = new HtmlDocument();
                text = @"@inherits System.Web.Mvc.WebViewPage
@using System.Web.Mvc.Html

@{
    CloudApp.Web.Controllers.HomePageController pg = new CloudApp.Web.Controllers.HomePageController();

}" +text;

                doc.LoadHtml(text);
                doc = ConvertStaticLink(doc, db);
                #region FindRenderBodyContent;
                HtmlNode node = doc.DocumentNode.SelectSingleNode("//*[@data-renderbody='True']");
                //node.ParentNode.RemoveChild(node);
                #endregion

                HtmlNode nodeHead = doc.DocumentNode.SelectSingleNode("//head");
                HtmlNode nodeBody = doc.DocumentNode.SelectSingleNode("//body");
                string headElementString = "@RenderSection($head$, required: false)";
                string scriptElementString = "@RenderSection($scripts$, required: false)";
                HtmlNode headElement = doc.CreateTextNode(headElementString.Replace('$','"'));
                HtmlNode scriptElement = doc.CreateTextNode(scriptElementString.Replace('$','"'));
                nodeHead.AppendChild(headElement);
                nodeBody.AppendChild(scriptElement);

                #region ReplaceRenderBody
                var renderBody = "@RenderBody()";
                var newNode = HtmlNode.CreateNode(renderBody);
                node.ParentNode.ReplaceChild(newNode, node);
                #endregion



                doc = OuterMenuCshtml(doc, db, "MainMenu");
                doc = OuterMenuCshtml(doc, db, "FooterMenu");
                doc = OuterMenuCshtml(doc, db, "CustomMenu1");
                doc = OuterMenuCshtml(doc, db, "CustomMenu2");
                doc = OuterMenuCshtml(doc, db, "CustomMenu3");
                doc = OuterMenuCshtml(doc, db, "CustomMenu4");



                foreach (var k in doc.DocumentNode.SelectNodes("//script"))
                {
                    if (k.Attributes["href"] != null)
                        k.Attributes["href"].Value = "~/Theme/" + _OrganizationId + "/Content/" + k.Attributes["href"].Value;
                }

                foreach (var k in doc.DocumentNode.SelectNodes("//link"))
                {
                    if (k.Attributes["href"] != null)
                        k.Attributes["href"].Value = "~/Theme/" + _OrganizationId + "/Content/" + k.Attributes["href"].Value;
                }

                DirectoryCopy(_OutputDirectory, _RealThemePath, true);

                string path = _ServerMappath + "../" + _WebDirectoryPath + "/Theme/" + _OrganizationId + "/Views/Shared/_Layout.cshtml";


                // convert string to stream
                byte[] bufferL = Encoding.UTF8.GetBytes(doc.DocumentNode.OuterHtml);
                MemoryStream msL = new MemoryStream(bufferL);
                //write to file
                FileStream fileL = new FileStream(path, FileMode.Create, FileAccess.Write);
                msL.WriteTo(fileL);
                fileL.Close();
                msL.Close();

            }


        }
        public void ConvertIndexPage()
        {
            _PageNameList = Directory.GetFiles(_OutputDirectory).ToList();
            int orgId = Convert.ToInt32(_OrganizationId);
            DbDataContext db = new DbDataContext();
            foreach (var item in _PageNameList.Where(s => s.Contains("index.html")))
            {
                string text = "";
                using (var streamReader = new StreamReader(item, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(text);

                HtmlNode indexNode = doc.DocumentNode.SelectSingleNode("//*[@data-renderbody='True']");
                string layoutText = @"@{
    Layout = $~/Theme/" + _OrganizationId + @"/Views/Shared/_Layout.cshtml$;
    CloudApp.Web.Controllers.HomePageController pg = new CloudApp.Web.Controllers.HomePageController();
}";
                layoutText = layoutText.Replace('$','"');

                indexNode.InnerHtml = layoutText + indexNode.OuterHtml;
                string path = _ServerMappath + "../" + _WebDirectoryPath + "/Theme/" + _OrganizationId + "/Views/HomePage/index.cshtml";

                // convert string to stream
                byte[] bufferL = Encoding.UTF8.GetBytes(indexNode.OuterHtml);
                MemoryStream msL = new MemoryStream(bufferL);
                //write to file
                FileStream fileL = new FileStream(path, FileMode.Create, FileAccess.Write);
                msL.WriteTo(fileL);
                fileL.Close();
                msL.Close();

            }
        }
        private void HtmlNodeToCMenuItem(HtmlNode nodeMenu, DbDataContext db, int orgId, int menuId)
        {
            if (nodeMenu.FirstChild.Attributes["data-type"] != null && nodeMenu.FirstChild.Attributes["data-type"].Value == "text")
            {
                string textName = nodeMenu.FirstChild.Attributes["data-name"].Value;
                string categoryName = nodeMenu.FirstChild.Attributes["data-categoryname"].Value;
                CText text;
                CCategory cat;
                text = db.Texts.Where(k => k.OrganizationId == orgId &&
                    k.Name == textName &&
                    k.Category.Name == categoryName &&
                    k.ActiveStatus == EActiveStatus.Active).FirstOrDefault();

                cat = db.Categories.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                       k.OrganizationId == orgId &&
                       k.Name == categoryName).FirstOrDefault();

                if (cat == null)
                {
                    CItemTheme itemTheme;
                    itemTheme = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                        k.Name == _CategoryThemeString &&
                        k.ActiveStatus == EActiveStatus.Active &&
                        k.ThemeType == EItemTheme.Category).FirstOrDefault();

                    if (itemTheme == null)
                    {
                        itemTheme = new CItemTheme();
                        itemTheme.Name = _CategoryThemeString;
                        itemTheme.OrganizationId = orgId;
                        itemTheme.ThemeType = EItemTheme.Category;
                        itemTheme.ThemePath = _CategoryThemeString;
                        itemTheme.ActiveStatus = EActiveStatus.Active;
                        itemTheme.CreatedUserId = 1;
                        itemTheme.CreatedDate = DateTime.Now;
                        db.ItemThemes.Add(itemTheme);
                        db.SaveChanges();
                    }

                    cat = new CCategory();
                    cat.Name = categoryName;
                    cat.ActiveStatus = EActiveStatus.Active;
                    cat.CreatedDate = DateTime.Now;
                    cat.CreatedUserId = 1;
                    cat.ItemThemeId = itemTheme.Id;
                    cat.OrganizationId = orgId;
                    db.Categories.Add(cat);
                    db.SaveChanges();
                }

                if (text == null)
                {

                    if (String.IsNullOrEmpty(nodeMenu.FirstChild.Attributes["data-itemtheme"].Value))
                        throw new Exception("Yazının 'data-itemtheme' değeri boş geçilemez.");
                    string themeName = nodeMenu.FirstChild.Attributes["data-itemtheme"].Value;
                    CItemTheme itemThemeText;
                    itemThemeText = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                        k.Name == themeName &&
                        k.ActiveStatus == EActiveStatus.Active &&
                        k.ThemeType == EItemTheme.Text).FirstOrDefault();

                    if (itemThemeText == null)
                    {
                        itemThemeText = new CItemTheme();
                        itemThemeText.Name = themeName;
                        itemThemeText.OrganizationId = orgId;
                        itemThemeText.ThemeType = EItemTheme.Text;
                        itemThemeText.CreatedDate = DateTime.Now;
                        itemThemeText.CreatedUserId = 1;
                        itemThemeText.ThemePath = themeName + ".cshtml";
                        itemThemeText.ActiveStatus = EActiveStatus.Active;
                        db.ItemThemes.Add(itemThemeText);
                        db.SaveChanges();
                    }


                    text = new CText();
                    text.Name = nodeMenu.FirstChild.Attributes["data-name"].Value;
                    text.OrganizationId = orgId;
                    text.PageTitle = nodeMenu.FirstChild.Attributes["data-name"].Value;
                    text.CategoryId = cat.Id;
                    text.ItemThemeId = itemThemeText.Id;
                    text.CreatedDate = DateTime.Now;
                    text.ActiveStatus = EActiveStatus.Active;
                    text.CreatedUserId = 1;
                    db.Texts.Add(text);
                    db.SaveChanges();
                }
                string menuName = nodeMenu.FirstChild.InnerHtml;
                CMenuItem cm = db.MenuItems.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                    k.OrganizationId == orgId &&
                    k.Name == menuName &&
                    k.MenuId == menuId).FirstOrDefault();
                if (cm != null)
                {
                    cm.MenuType = EMenuType.Text;
                    cm.TextId = text.Id;
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cm.Id);
                }
                else
                {
                    CMenuItem cmNew = new CMenuItem();
                    cmNew.TextId = text.Id;
                    cmNew.OrganizationId = orgId;
                    cmNew.Name = menuName;
                    cmNew.MenuId = menuId;
                    cmNew.MenuType = EMenuType.Text;
                    cmNew.CreatedDate = DateTime.Now;
                    cmNew.ActiveStatus = EActiveStatus.Active;
                    cmNew.CreatedUserId = 1;
                    db.MenuItems.Add(cmNew);
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cmNew.Id);
                }

            }
            else if (nodeMenu.FirstChild.Attributes["data-type"] != null && nodeMenu.FirstChild.Attributes["data-type"].Value == "url")
            {
                string menuName = nodeMenu.FirstChild.InnerHtml;
                CMenuItem cm = db.MenuItems.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                    k.OrganizationId == orgId &&
                    k.Name == menuName &&
                    k.MenuId == menuId).FirstOrDefault();
                if (cm != null)
                {
                    cm.MenuType = EMenuType.Url;
                    cm.Url = nodeMenu.FirstChild.Attributes["data-url"].Value;
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cm.Id);
                }
                else
                {
                    CMenuItem cmNew = new CMenuItem();
                    cmNew.Url = nodeMenu.FirstChild.Attributes["data-url"].Value;
                    cmNew.OrganizationId = orgId;
                    cmNew.Name = menuName;
                    cmNew.MenuId = menuId;
                    cmNew.MenuType = EMenuType.Url;
                    cmNew.CreatedDate = DateTime.Now;
                    cmNew.CreatedUserId = 1;
                    cmNew.ActiveStatus = EActiveStatus.Active;
                    db.MenuItems.Add(cmNew);
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cmNew.Id);
                }
            }
            else if (nodeMenu.FirstChild.Attributes["data-type"] != null && nodeMenu.FirstChild.Attributes["data-type"].Value == "category")
            {
                string categoryName = nodeMenu.FirstChild.Attributes["data-name"].Value;
                string menuName = nodeMenu.FirstChild.InnerHtml;
                CMenuItem cm = db.MenuItems.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                    k.OrganizationId == orgId &&
                    k.Name == menuName &&
                    k.MenuId == menuId).FirstOrDefault();
                CCategory cat;
                cat = db.Categories.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                     k.OrganizationId == orgId &&
                     k.Name == categoryName).FirstOrDefault();
                if (cat == null)
                {
                    CItemTheme itemTheme = new CItemTheme();
                    itemTheme = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                        k.Name == _CategoryThemeString &&
                        k.ActiveStatus == EActiveStatus.Active &&
                        k.ThemeType == EItemTheme.Category).FirstOrDefault();

                    if (itemTheme == null)
                    {
                        itemTheme = new CItemTheme();
                        itemTheme.Name = _CategoryThemeString;
                        itemTheme.OrganizationId = orgId;
                        itemTheme.ThemeType = EItemTheme.Category;
                        itemTheme.ThemePath = _CategoryThemeString;
                        itemTheme.ActiveStatus = EActiveStatus.Active;
                        db.ItemThemes.Add(itemTheme);
                        db.SaveChanges();
                    }

                    cat = new CCategory();
                    cat.Name = categoryName;
                    cat.ActiveStatus = EActiveStatus.Active;
                    cat.CreatedDate = DateTime.Now;
                    cat.CreatedUserId = 1;
                    cat.OrganizationId = orgId;
                    cat.ItemThemeId = itemTheme.Id;
                    db.Categories.Add(cat);
                    db.SaveChanges();
                }

                if (cm != null)
                {
                    cm.MenuType = EMenuType.Category;
                    cm.CategoryId = cat.Id;
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cm.Id);
                }
                else
                {
                    CMenuItem cmNew = new CMenuItem();
                    cmNew.CategoryId = cat.Id;
                    cmNew.OrganizationId = orgId;
                    cmNew.Name = menuName;
                    cmNew.MenuId = menuId;
                    cmNew.MenuType = EMenuType.Category;
                    cmNew.CreatedDate = DateTime.Now;
                    cmNew.CreatedUserId = 1;
                    cmNew.ActiveStatus = EActiveStatus.Active;
                    db.MenuItems.Add(cmNew);
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cmNew.Id);
                }
            }

            else if (nodeMenu.FirstChild.Attributes["data-type"] != null && nodeMenu.FirstChild.Attributes["data-type"].Value == "categorycontent")
            {
                string categoryName = nodeMenu.FirstChild.Attributes["data-name"].Value;
                string menuName = nodeMenu.FirstChild.InnerHtml;
                CMenuItem cm = db.MenuItems.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                    k.OrganizationId == orgId &&
                    k.Name == menuName &&
                    k.MenuId == menuId).FirstOrDefault();
                CCategory cat;
                cat = db.Categories.Where(k => k.ActiveStatus == EActiveStatus.Active &&
                     k.OrganizationId == orgId &&
                     k.Name == categoryName).FirstOrDefault();
                if (cat == null)
                {
                    CItemTheme itemTheme = new CItemTheme();
                    itemTheme = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                        k.Name == _CategoryThemeString &&
                        k.ActiveStatus == EActiveStatus.Active &&
                        k.ThemeType == EItemTheme.Category).FirstOrDefault();

                    if (itemTheme == null)
                    {
                        itemTheme = new CItemTheme();
                        itemTheme.Name = _CategoryThemeString;
                        itemTheme.OrganizationId = orgId;
                        itemTheme.ThemeType = EItemTheme.Category;
                        itemTheme.ThemePath = _CategoryThemeString;
                        itemTheme.ActiveStatus = EActiveStatus.Active;
                        db.ItemThemes.Add(itemTheme);
                        db.SaveChanges();
                    }

                    cat = new CCategory();
                    cat.Name = categoryName;
                    cat.ActiveStatus = EActiveStatus.Active;
                    cat.CreatedDate = DateTime.Now;
                    cat.CreatedUserId = 1;
                    cat.OrganizationId = orgId;
                    cat.ItemThemeId = itemTheme.Id;
                    db.Categories.Add(cat);
                    db.SaveChanges();
                }

                if (cm != null)
                {
                    cm.MenuType = EMenuType.Category;
                    cm.CategoryId = cat.Id;
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cm.Id);
                }
                else
                {
                    CMenuItem cmNew = new CMenuItem();
                    cmNew.CategoryId = cat.Id;
                    cmNew.OrganizationId = orgId;
                    cmNew.Name = menuName;
                    cmNew.MenuId = menuId;
                    cmNew.MenuType = EMenuType.Category;
                    cmNew.CreatedDate = DateTime.Now;
                    cmNew.CreatedUserId = 1;
                    cmNew.ActiveStatus = EActiveStatus.Active;
                    db.MenuItems.Add(cmNew);
                    db.SaveChanges();
                    _AllowedMenuItemId.Add(cmNew.Id);
                }
            }

        }
        private CMenu HtmlNodeToCMenu(HtmlNode nodeMenu, DbDataContext db, int orgId, string themePath)
        {
            CMenu menu;
            if (nodeMenu != null)
            {
                if (nodeMenu.Attributes["data-id"] != null && !String.IsNullOrEmpty(nodeMenu.Attributes["data-id"].Value))
                {
                    int menuId = Convert.ToInt32(nodeMenu.Attributes["data-id"].Value);
                    menu = db.Menus.Where(k => k.Id == menuId && k.ActiveStatus == EActiveStatus.Active &&
                        k.OrganizationId == orgId).FirstOrDefault();
                    if (menu == null)
                        throw new Exception("Geçersiz Menu Id'si girdiniz");
                }
                else
                {
                    string menuname = nodeMenu.Attributes["data-name"].Value;
                    menu = db.Menus.Where(k => k.Name == menuname && k.ActiveStatus == EActiveStatus.Active &&
                        k.OrganizationId == orgId).FirstOrDefault();
                    if (String.IsNullOrEmpty(menuname))
                        throw new Exception("Menu Id'si yok ise isim boş geçilemez.");
                    if (menu == null)
                    {
                        CItemTheme itemTheme = new CItemTheme();
                        itemTheme = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                            k.Name == themePath &&
                            k.ActiveStatus == EActiveStatus.Active &&
                            k.ThemeType == EItemTheme.Menu).FirstOrDefault();

                        if (itemTheme == null)
                        {
                            itemTheme = new CItemTheme();
                            itemTheme.Name = themePath;
                            itemTheme.OrganizationId = orgId;
                            itemTheme.ThemeType = EItemTheme.Menu;
                            itemTheme.ThemePath = themePath + ".cshtml";
                            itemTheme.CreatedDate = DateTime.Now;
                            itemTheme.CreatedUserId = 1;
                            itemTheme.ActiveStatus = EActiveStatus.Active;
                            db.ItemThemes.Add(itemTheme);
                            db.SaveChanges();
                        }
                        else
                        {
                            itemTheme.Name = themePath;
                            itemTheme.ThemePath = themePath + ".cshtml";
                            db.SaveChanges();
                        }
                        menu = new CMenu();
                        menu.Name = menuname;
                        menu.OrganizationId = Convert.ToInt32(_OrganizationId);
                        menu.ActiveStatus = EActiveStatus.Active;
                        menu.CreatedDate = DateTime.Now;
                        menu.CreatedUserId = 1;
                        menu.ItemThemeId = itemTheme.Id;
                        db.Menus.Add(menu);
                        db.SaveChanges();
                    }
                    else
                    {
                        CItemTheme itemTheme = new CItemTheme();
                        itemTheme = db.ItemThemes.Where(k => k.OrganizationId == orgId &&
                            k.Name == themePath &&
                            k.ActiveStatus == EActiveStatus.Active &&
                            k.ThemeType == EItemTheme.Menu).FirstOrDefault();
                        if (itemTheme == null)
                        {
                            itemTheme = new CItemTheme();
                            itemTheme.Name = themePath;
                            itemTheme.OrganizationId = orgId;
                            itemTheme.ThemeType = EItemTheme.Menu;
                            itemTheme.ThemePath = themePath + ".cshtml";
                            itemTheme.CreatedDate = DateTime.Now;
                            itemTheme.CreatedUserId = 1;
                            itemTheme.ActiveStatus = EActiveStatus.Active;
                            db.ItemThemes.Add(itemTheme);
                            db.SaveChanges();
                            menu.ItemThemeId = itemTheme.Id;
                            db.SaveChanges();
                        }
                        else
                        {
                            itemTheme.Name = themePath;
                            itemTheme.ThemePath = themePath + ".cshtml";
                            db.SaveChanges();
                        }
                    }
                }
                return menu;
            }
            else
            {
                return null;
            }
        }

        public void StartConvert(int orgId, string mapPath, HttpPostedFileBase file, string organizationId)
        {
            _ServerMappath = mapPath;
            _OrganizationId = organizationId;
            //ClearFolder(orgId, mapPath);
            ExtrachFolder(file, orgId, mapPath);
            ConvertLayoutPage();
            ConvertIndexPage();
        }
    }
}