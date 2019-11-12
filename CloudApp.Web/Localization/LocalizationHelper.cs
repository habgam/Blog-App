using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CloudApp.Web.Localization
{
    public static class LocalizationHelper
    {

        public static List<LocalizationModel> GetLocalizationModels()
        {
            List<LocalizationModel> resources = null;
            if (HttpContext.Current.Application["CultureLanguageItems"] == null)
            {

                HttpContext.Current.Application.Lock();
                if (HttpContext.Current.Application["CultureLanguageItems"] == null)
                {
                    resources = JsonConvert.DeserializeObject<List<LocalizationModel>>(LocalizeFileResource(null));
                    HttpContext.Current.Application["CultureLanguageItems"] = resources;
                }
                HttpContext.Current.Application.UnLock();
                return resources;
            }
            else
                return (List<LocalizationModel>)HttpContext.Current.Application["CultureLanguageItems"];

        }

        public static string Localize<T>(T key, bool jsFriendly = false, string culture = null)
        {
            string keyValue = null;

            if (typeof(T).IsClass && (object)key == null) return "";

            if (typeof(T) == typeof(String))
            {
                keyValue = key as String;
            }
            else
            {
                keyValue = key.ToString();
            }

            if (!keyValue.StartsWith("r_"))
            {
                return keyValue;
            }
            List<LocalizationModel> resources = GetLocalizationModels();  // null;

            if (HttpContext.Current.Application["CultureLanguageItems"] == null)
            {
                // InitCache();
                HttpContext.Current.Application.Lock();
                if (HttpContext.Current.Application["CultureLanguageItems"] == null)
                {
                    resources = JsonConvert.DeserializeObject<List<LocalizationModel>>(LocalizeFileResource(null));
                    HttpContext.Current.Application["CultureLanguageItems"] = resources;
                }
                HttpContext.Current.Application.UnLock();
            }

            if (resources == null)
            {
                resources = (List<LocalizationModel>)HttpContext.Current.Application["CultureLanguageItems"];
            }

            var uiCulture = new CultureInfo( "tr-TR");

            if (!String.IsNullOrEmpty(culture))
            {
                try
                {
                    uiCulture = new CultureInfo(culture);
                }
                finally
                {

                }
            }
            if (uiCulture == null)
            {
                uiCulture = new CultureInfo("tr-TR");
            }
            string res;
            try
            {
                res = (from result in resources
                       where result.Key == keyValue
                       select result.LocalizedValue[uiCulture.Name]).FirstOrDefault().ToString();
            }
            catch
            {
                res = keyValue;
            }
            if (String.IsNullOrEmpty(res)) res = keyValue;
            return res;
        }

        public static string LocalizeFileResource(string organizationId)
        {
            string filePath;

            if (!String.IsNullOrEmpty(organizationId))
                filePath = HttpContext.Current.Server.MapPath(@"~/App_Data/" + organizationId + ".json");
            else
                filePath = HttpContext.Current.Server.MapPath(@"~/App_Data/BaseLocalize.json");

            string fileResources = "";

            if (File.Exists(filePath))
                fileResources = File.ReadAllText(filePath);

            return fileResources;
        }

        public static string GetResourcesJS(string culture = null)
        {
            var uiCulture = new CultureInfo("tr-TR");
            if (!String.IsNullOrEmpty(culture))
            {
                try
                {
                    uiCulture = new CultureInfo(culture);
                }
                finally
                {

                }
            }
            if (uiCulture == null)
            {
                uiCulture = new CultureInfo("tr-TR");
            }
            var resourceObj = from result in (List<LocalizationModel>)HttpContext.Current.Application["CultureLanguageItems"]
                              where
                              result.UseInJS == true
                              select result;

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var item in resourceObj)
            {
                dictionary.Add(item.Key, item.LocalizedValue[uiCulture.Name]);
            }
            string serializedJson = JsonConvert.SerializeObject(dictionary, Formatting.Indented);
            string test = serializedJson;
            return serializedJson;
        }

        public static bool SearchByKey(string langKey)
        {
            var resources = (List<LocalizationModel>)HttpContext.Current.Application["CultureLanguageItems"];
            var find = (from result in resources
                        where result.Key.Equals(langKey)
                        select result).FirstOrDefault();
            if (find != null)
                return true;

            return false;
        }

      
    }
}
