using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AuthDemo.App.SSO
{
    /// <summary>
    /// 基础控制器
    /// 继承该控制器可以防止未登录查看
    /// 继承该控制器后，如果想访问控制器中存在，但模块配置里面没有的Action（如：Home/Git），请使用AnonymousAttribute
    /// </summary>
    public class SSOController : Controller
    {
        public const string Token = "Token";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = "";

            //Token by QueryString
            var request = filterContext.HttpContext.Request;
            if (request.Cookies[Token] != null)  //从Cookie读取Token
            {
                token = request.Cookies[Token].Value;
            }

            if (string.IsNullOrEmpty(token))
            {
                //直接登录
                filterContext.Result = LoginResult("");
                return;
            }
            //验证
            if (AuthUtil.CheckLogin(token, request.RawUrl) == false)
            {
                //会话丢失，跳转到登录页面
                filterContext.Result = LoginResult("");
                return;
            }

            base.OnActionExecuting(filterContext);
        }

        public virtual ActionResult LoginResult(string username)
        {
            return new RedirectResult("/Login");
        }
    }
}
