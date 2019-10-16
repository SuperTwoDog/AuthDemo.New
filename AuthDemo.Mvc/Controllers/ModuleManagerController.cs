using AuthDemo.App.Response;
using AuthDemo.App.SSO;
using AuthDemo.Mvc.Models;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthDemo.Mvc.Controllers
{
    public class ModuleManagerController : BaseController
    {
        // GET: ModuleManager
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        #region 加载用户模块操作菜单

        /// <summary>
        /// 加载当前用户可访问模块的菜单
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <returns>System.String.</returns>
        public string LoadMenus(string moduleId)
        {
            var user = AuthUtil.GetCurrentUser();

            var module = user.Modules.Single(u => u.Id == moduleId);

            var data = new TableData
            {
                data = module.Elements,
                count = module.Elements.Count(),
            };
            return JsonHelper.Instance.Serialize(data);
        }

        /// <summary>
        /// 获取发起页面的菜单权限
        /// </summary>
        /// <returns>System.String.</returns>
        public string LoadAuthorizedMenus(string modulecode)
        {
            var user = AuthUtil.GetCurrentUser();
            var module = user.Modules.First(u => u.Code == modulecode);
            if (module != null)
            {
                return JsonHelper.Instance.Serialize(module.Elements);

            }
            return "";
        }

        #endregion
    }
}