using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App.Request
{
    public class QueryCommonReq : PageReq
    {
        /// <summary>
        /// 机构ID
        /// </summary>
        public string orgId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
