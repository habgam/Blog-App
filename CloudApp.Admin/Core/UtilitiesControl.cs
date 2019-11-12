using CloudApp.Data;
using CloudApp.Data.Enum;
using CloudApp.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CloudApp.Admin.Core
{
    public class UtilitiesControl
    {
        public List<ViewMenuItem> GetAdminMenu(string sOrgId)
        {
            int orgId = Convert.ToInt32(sOrgId);
            DbDataContext db = new DbDataContext("CloudAppWebSite");
            List<ViewMenuItem> MenuList = new List<ViewMenuItem>();
            var query = db.AdminMenus.Where(p => p.ActiveStatus == EActiveStatus.Active && p.OrganizationId == orgId && p.SubMenuId==null).OrderBy(s => s.Order).ToList();
            foreach (var item in query)
            {
                ViewMenuItem mm = new ViewMenuItem();
                mm.Name = item.MenuName;
                mm.Url = item.MenuUrl;
                //var subQuery = db.AdminMenus.Where(s => s.SubMenuId == item.Id && s.ActiveStatus == EActiveStatus.Active && s.OrganizationId == orgId).ToList();
                if (item.SubMenu != null)
                {
                    mm.SubMenu = new List<ViewMenuItem>();
                   var subQuery = item.SubMenu.Where(s => s.ActiveStatus == EActiveStatus.Active && s.OrganizationId == orgId).OrderBy(s => s.Order).ToList();
                    foreach (var subItem in subQuery)
                    {
                        ViewMenuItem mms = new ViewMenuItem();
                        mms.Name = subItem.MenuName;
                        mms.Url = subItem.MenuUrl;
                        mm.SubMenu.Add(mms);
                    }
                }
                MenuList.Add(mm);
            }

            return MenuList;
        }

        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}