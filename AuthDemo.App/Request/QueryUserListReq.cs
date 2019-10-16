using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App.Request
{
    public class QueryUserListReq : PageReq
    {
        public string orgId { get; set; }

        public int UserStatus { get; set; }
    }
}
