using CloudApp.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.ViewModel
{
    public class ViewAlert
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public EAlertType AlertType { get; set; }
    }
}
