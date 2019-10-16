using AuthDemo.App.SSO;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AuthDemo.Mvc.Controllers
{
    public class LoginController : Controller
    {
        private string _appKey = ConfigurationManager.AppSettings["SSOAppKey"];
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.AppKey = _appKey;
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string test()
        {
            return null;
        }

        ///// <summary>
        ///// 表单提交登录
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public string Index(string username, string password)
        //{
        //    var resp = new LoginResult();
        //    try
        //    {
        //        var result = AuthUtil.Login(_appKey, username, password);
        //        if (result.Code == 200)
        //        {

        //            var cookie = new HttpCookie("Token", result.Token)
        //            {
        //                Expires = DateTime.Now.AddDays(10)
        //            };
        //            Response.Cookies.Add(cookie);
        //            resp.Result = "/home/index";
        //        }
        //        else
        //        {
        //            resp.Message = "登录失败";
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        resp.Code = 500;
        //        resp.Message = e.Message;
        //    }
        //    return JsonHelper.Instance.Serialize(resp);
        //}

        /// <summary>
        /// 异步提交登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <returns></returns>
        public string Login(string username, string password)
        {
            var resp = new Response();
            try
            {
                var result = AuthUtil.Login(_appKey, username, password);
                if (result.Code == 200)
                {
                    var cookie = new HttpCookie("Token", result.Token)
                    {
                        Expires = DateTime.Now.AddDays(10)
                    };
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    resp.Code = 500;
                    resp.Message = result.Message;
                }
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            AuthUtil.Logout();
            return RedirectToAction("Index", "Login");
        }
    }
}