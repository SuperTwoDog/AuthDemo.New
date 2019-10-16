using AuthDemo.App;
using AuthDemo.App.Request;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AuthDemo.Mvc.Controllers
{
    public class ApplicationsController : BaseController
    {
        public AppManager App { get; set; }

        public string GetList([FromUri]QueryAppListReq request)
        {
            return JsonHelper.Instance.Serialize(App.GetList(request));
        }

    }
}