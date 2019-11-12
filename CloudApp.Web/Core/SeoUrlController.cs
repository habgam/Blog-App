using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.Model;
using CloudApp.Data.ViewModel;
using CloudApp.Web.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudApp.Web.Core
{
    public class SeoUrlController
    {
        private string CreateSeoUrl(string url)
        {
            string temp = "";
            temp = url.ToLower();
            temp = temp.Replace(" ", "-");
            temp = temp.Replace("ı", "i");
            temp = temp.Replace("ö", "o");
            temp = temp.Replace("ü", "u");
            temp = temp.Replace("ç", "c");
            temp = temp.Replace("ğ", "g");
            temp = temp.Replace("'", "");
            temp = temp.Replace("&", "-and-");
            temp = temp.Replace("ş", "s");
            temp = temp.Replace("#", "sharp");
            temp = temp.Replace("?", "-");
            temp = temp.Replace("%", "-");
            return temp;
        }
        public string GetUrlString(int id, EMenuType type, int orgId,string lang)
        {
            lang = lang.ToLower();
            //if (lang.ToLower() == "ar-sa")
            //    lang = "en-US";
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            if (type == EMenuType.Text)
            {
                string url = lang + "/" + LocalizationHelper.Localize("r_url_menu_texts",culture:lang) + "/";
                if (lang.ToLower() == "ar-sa")
                    lang = "en-us";
                List<Tuple<string, int>> menuList = new List<Tuple<string, int>>();
                int level = 0;
                CText ct = db.Texts.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == id).FirstOrDefault();
                if (ct != null)
                {
                    menuList.Add(new Tuple<string, int>(CreateSeoUrl((ct.LanguageValues.FirstOrDefault(f=>f.Lang.ToLower()==lang) != null ? ct.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-" )+ "-" + ct.Id), level));
                    level++;
                    if (ct.Category != null)
                    {
                        CCategory item1 = ct.Category;
                        menuList.Add(new Tuple<string, int>(CreateSeoUrl((item1.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item1.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item1.Id), level));
                        level++;
                        if (item1.TopCategory != null)
                        {
                            CCategory item2 = item1.TopCategory;
                            menuList.Add(new Tuple<string, int>(CreateSeoUrl((item2.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item2.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item2.Id), level));
                            level++;
                            if (item2.TopCategory != null)
                            {
                                CCategory item3 = item2.TopCategory;
                                menuList.Add(new Tuple<string, int>(CreateSeoUrl((item3.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item3.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item3.Id), level));
                                level++;
                                if (item3.TopCategory != null)
                                {
                                    CCategory item4 = item3.TopCategory;
                                    menuList.Add(new Tuple<string, int>(CreateSeoUrl((item4.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item4.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item4.Id), level));
                                    level++;
                                    if (item4.TopCategory != null)
                                    {
                                        CCategory item5 = item4.TopCategory;
                                        menuList.Add(new Tuple<string, int>(CreateSeoUrl((item5.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item5.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item5.Id), level));
                                        level++;
                                        if (item5.TopCategory != null)
                                        {
                                            CCategory item6 = item5.TopCategory;
                                            menuList.Add(new Tuple<string, int>(CreateSeoUrl((item6.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item6.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item6.Id), level));
                                            level++;
                                        }
                                    }
                                }
                            }

                        }

                    }
                    foreach (var item in menuList.OrderByDescending(t => t.Item2))
                    {
                        url += item.Item1 + "/";
                    }
                    return CreateSeoUrl(url);
                }
                else
                {
                    return "#";
                }
            }
            else if (type == EMenuType.Category)
            {
                string url = lang + "/" + LocalizationHelper.Localize("r_url_menu_categories", culture: lang) + "/";
                if (lang.ToLower() == "ar-sa")
                    lang = "en-us";
                List<Tuple<string, int>> menuList = new List<Tuple<string, int>>();
                int level = 0;
                CCategory ct = db.Categories.Where(p => p.OrganizationId == orgId && p.ActiveStatus == EActiveStatus.Active && p.Id == id).FirstOrDefault();
                if (ct != null)
                {
                    menuList.Add(new Tuple<string, int>(CreateSeoUrl((ct.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? ct.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + ct.Id), level));
                    level++;
                    if (ct.TopCategory != null)
                    {
                        CCategory item1 = ct.TopCategory;
                        menuList.Add(new Tuple<string, int>(CreateSeoUrl((item1.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item1.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item1.Id), level));
                        level++;
                        if (item1.TopCategory != null)
                        {
                            CCategory item2 = item1.TopCategory;
                            menuList.Add(new Tuple<string, int>(CreateSeoUrl((item2.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item2.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item2.Id), level));
                            level++;
                            if (item2.TopCategory != null)
                            {
                                CCategory item3 = item2.TopCategory;
                                menuList.Add(new Tuple<string, int>(CreateSeoUrl((item3.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item3.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item3.Id), level));
                                level++;
                                if (item3.TopCategory != null)
                                {
                                    CCategory item4 = item3.TopCategory;
                                    menuList.Add(new Tuple<string, int>(CreateSeoUrl((item4.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item4.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item4.Id), level));
                                    level++;
                                    if (item4.TopCategory != null)
                                    {
                                        CCategory item5 = item4.TopCategory;
                                        menuList.Add(new Tuple<string, int>(CreateSeoUrl((item5.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item5.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item5.Id), level));
                                        level++;
                                        if (item5.TopCategory != null)
                                        {
                                            CCategory item6 = item5.TopCategory;
                                            menuList.Add(new Tuple<string, int>(CreateSeoUrl((item6.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? item6.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-") + "-" + item6.Id), level));
                                            level++;
                                        }
                                    }
                                }
                            }

                        }

                    }
                    foreach (var item in menuList.OrderByDescending(t => t.Item2))
                    {
                        url += item.Item1 + "/";
                    }
                    return CreateSeoUrl(url);
                }
                else
                {
                    return "#";
                }
            }
            else
            {
                return "#";
            }
        }
        private List<ViewMenuItem> MenuList = new List<ViewMenuItem>();
        private List<ViewMenuItem> InsertViewSubMenu(List<CMenuItem> list,string lang)
        {
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            List<ViewMenuItem> mm = new List<ViewMenuItem>();
            foreach (var p in list.Where(k=>k.ActiveStatus==EActiveStatus.Active).OrderBy(p=>p.Order))
            {
                if (p.MenuType == EMenuType.Category)
                {
                    mm.Add(new ViewMenuItem { Name = p.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = GetUrlString(p.CategoryId.Value, EMenuType.Category, p.OrganizationId.Value,lang) });
                }
                else if (p.MenuType == EMenuType.CategoryContent)
                {
                    CCategory cg = db.Categories.Where(t => t.OrganizationId == p.OrganizationId && t.ActiveStatus == EActiveStatus.Active && t.Id == p.CategoryId.Value).FirstOrDefault();
                    InsertCotegoryContent(cg,lang);
                }
                else if (p.MenuType == EMenuType.Root)
                {
                    ViewMenuItem ii = new ViewMenuItem();
                    ii.Name = p.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-";
                    ii.Url = p.Url;
                    if (p.SubMenu != null)
                    {
                       ii.SubMenu= InsertViewSubMenu(p.SubMenu.ToList(),lang);
                    }
                    mm.Add(new ViewMenuItem { Name = p.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = "#" });

                }
                else if (p.MenuType == EMenuType.Text)
                {
                    mm.Add(new ViewMenuItem { Name = p.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = GetUrlString(p.TextId.Value, EMenuType.Text, p.OrganizationId.Value,lang) });
                }
                else if (p.MenuType == EMenuType.Url)
                {
                    mm.Add(new ViewMenuItem { Name = p.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = p.Url });
                }

            }
            return mm;
        }
        private void InsertCotegoryContent (CCategory cg,string lang)
        {
            ViewMenuItem mItem = new ViewMenuItem();
            mItem.Name = cg.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? cg.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-";
            mItem.Url = "#";
            mItem.SubMenu = new List<ViewMenuItem>();
            if (cg.SubCategory != null && cg.SubCategory.Count > 0)
            {
                foreach (var item1 in cg.SubCategory.Where(p=>p.ActiveStatus==EActiveStatus.Active))
                {
                    ViewMenuItem subItem1 = new ViewMenuItem();
                    subItem1.Name = item1.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item1.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-";
                    subItem1.Url = GetUrlString(item1.Id, EMenuType.Category, cg.OrganizationId.Value,lang);
                    subItem1.SubMenu = new List<ViewMenuItem>();
                    if (item1.SubCategory != null && item1.SubCategory.Count > 0)
                    {

                        foreach (var item11 in item1.SubCategory.Where(p => p.ActiveStatus == EActiveStatus.Active))
                        {
                            ViewMenuItem subItem11 = new ViewMenuItem();
                            subItem11.Name = item11.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item11.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-";
                            subItem11.Url = GetUrlString(item11.Id, EMenuType.Category, cg.OrganizationId.Value,lang);
                            subItem1.SubMenu.Add(new ViewMenuItem { Name = item11.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item11.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-", Url = GetUrlString(item11.Id, EMenuType.Category, cg.OrganizationId.Value,lang) });
                        }

                    }
                    else
                    {
                        List<ViewMenuItem> ssMenuList1 = new List<ViewMenuItem>();

                        foreach (var item11 in item1.Texts.Where(p => p.ActiveStatus == EActiveStatus.Active))
                        {
                            ViewMenuItem subItem11 = new ViewMenuItem();
                            subItem11.Name = item11.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item11.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-";
                            subItem11.Url = GetUrlString(item11.Id, EMenuType.Text, cg.OrganizationId.Value,lang);
                            subItem1.SubMenu.Add(subItem11);
                        }
                        //mItem.SubMenu = ssMenuList1;
                    }
                    mItem.SubMenu.Add(subItem1);

                }
                MenuList.Add(mItem);
                //mItem.SubMenu = ssMenuList;

            }
            else
            {
                List<ViewMenuItem> ssMenuList = new List<ViewMenuItem>();
                foreach (var item1 in cg.Texts.Where(p => p.ActiveStatus == EActiveStatus.Active))
                {
                    ViewMenuItem subItem1 = new ViewMenuItem();
                    subItem1.Name = item1.LanguageValues.FirstOrDefault(f => f.Lang == lang) != null ? item1.LanguageValues.FirstOrDefault(f => f.Lang == lang).Name : "-";
                    subItem1.Url = GetUrlString(item1.Id, EMenuType.Text, cg.OrganizationId.Value,lang);
                    ssMenuList.Add(subItem1);
                }
                mItem.SubMenu = ssMenuList;
                MenuList.Add(mItem);
            }
        }
        public List<ViewMenuItem> GetViewMenu(CMenu item,string lang)
        {
            //List<ViewMenuItem> itemList = new List<ViewMenuItem>();
            lang = lang.ToLower();
            DbDataContext db = new DbDataContext("CloudAppWebSiteView");
            foreach (var p in item.MenuItem.Where(k=>k.ActiveStatus==EActiveStatus.Active).OrderBy(p=>p.Order).ToList())
            {
                if (p.MenuType == EMenuType.Category)
                {
                    MenuList.Add(new ViewMenuItem { Name = p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-", Url = GetUrlString(p.CategoryId.Value, EMenuType.Category, p.OrganizationId.Value,lang) });
                }
                else if (p.MenuType == EMenuType.CategoryContent)
                {
                    CCategory cg = db.Categories.Where(t => t.OrganizationId == item.OrganizationId && t.ActiveStatus == EActiveStatus.Active && t.Id == p.CategoryId.Value).FirstOrDefault();
                    InsertCotegoryContent(cg,lang);
                }
                else if (p.MenuType == EMenuType.Root)
                {
                    ViewMenuItem ii = new ViewMenuItem();
                    ii.Name = p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-";
                    ii.Url = "#";
                    if (p.SubMenu != null)
                    {
                       ii.SubMenu= InsertViewSubMenu(p.SubMenu.ToList(),lang);
                    }
                    MenuList.Add(ii);
                }
                else if (p.MenuType == EMenuType.Text)
                {
                    MenuList.Add(new ViewMenuItem { Name = p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-", Url = GetUrlString(p.TextId.Value, EMenuType.Text, p.OrganizationId.Value,lang) });
                }
                else if (p.MenuType == EMenuType.Url)
                {
                    MenuList.Add(new ViewMenuItem { Name = p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang) != null ? p.LanguageValues.FirstOrDefault(f => f.Lang.ToLower() == lang).Name : "-", Url = p.Url });
                }

            }
            return MenuList;
        }
    }
}