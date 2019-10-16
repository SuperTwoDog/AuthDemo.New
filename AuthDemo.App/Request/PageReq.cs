using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App.Request
{
    public class PageReq
    {
        public int page { get; set; }
        public int limit { get; set; }

        public string key { get; set; }

        public PageReq()
        {
            page = 1;
            limit = 10;
        }
    }
}
