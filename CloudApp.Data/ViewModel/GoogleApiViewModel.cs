using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.ViewModel
{
    public class GoogleVisitorsAndPageView
    {
        public string Time { get; set; }
        public string Session { get; set; }
        public string Users { get; set; }
        public string PageViews { get; set; }
        public string MaxView { get; set; }
    }
    public class GoogleSource
    {
        public string Refferal { get; set; }
        public string SessionCount { get; set; }
        public string Users { get; set; }
    }
}
