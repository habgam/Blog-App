using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.ViewModel
{
    public class FormViewListModel
    {
        public int FormListId { get; set; }
        public List<KeyValuePair<string, string>> Parameters { get; set; }
    }
}
