using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudApp.Data.Enum
{
    public enum EActiveStatus : int
    {
        Active=1,
        Passive = 0,
        Deleted = -1 ,
        Blocked = -2 

    }
}
