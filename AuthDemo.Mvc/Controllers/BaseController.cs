using AuthDemo.App.SSO;
using AuthDemo.Mvc.Models;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Configuration;

namespace AuthDemo.Mvc.Controllers
{
    /// <summary>
    /// 基础控制器
    /// <para>用于控制登录用户是否有权限访问指定的Action</para>
    /// </summary>
    public class BaseController : SSOController
    {
        protected Response Result = new Response();
        protected string controllerName;   //当前控制器小写名称
        protected string actionName;        //当前Action小写名称

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (!AuthUtil.CheckLogin()) return;

            controllerName = Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();//获取当前控制器名称
            actionName = filterContext.ActionDescriptor.ActionName.ToLower();//获取action名称

            var function = this.GetType().GetMethods().FirstOrDefault(u => u.Name.ToLower() == actionName);
            if (function == null)
            {
                throw new Exception("未能找到Action");
            }

            //权限验证标识
            var authorize = function.GetCustomAttribute(typeof(AuthenticateAttribute));
            if (authorize == null)
            {
                return;
            }

            var currentModule = AuthUtil.GetCurrentUser().Modules.FirstOrDefault(u => u.Url.ToLower().Contains(controllerName));
            //当前登录用户没有Action记录&&Action有authenticate标识
            if (currentModule == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
                return;
            }

            var version = ConfigurationManager.AppSettings["version"];
            if (version == "demo" && Request.HttpMethod == "POST")
            {
                throw new HttpException(400, "演示版本，不能进行该操作，当前模块:" + controllerName + "/" + actionName);
            }
        }
    }
}