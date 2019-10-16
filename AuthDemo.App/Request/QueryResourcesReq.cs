using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App.Request
{
    public class QueryResourcesReq : PageReq
    {
        /// <summary>
        /// TypeID
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
