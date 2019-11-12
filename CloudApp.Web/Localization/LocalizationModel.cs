using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudApp.Web.Localization
{
    [Serializable]
    public class LocalizationModel
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValue = new Dictionary<string, string>();
        public bool? UseInJS { get; set; }
    }
}