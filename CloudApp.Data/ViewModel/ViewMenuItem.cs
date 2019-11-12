using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.ViewModel
{
    public class ViewMenuItem
    {
        public string Name{get;set;}
        public string Url { get; set; }
        public List<ViewMenuItem> SubMenu { get; set; }

    }
}
