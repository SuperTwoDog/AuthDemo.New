using AuthDemo.App;
using AuthDemo.App.Request;
using AuthDemo.App.Response;
using AuthDemo.Mvc.Models;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace AuthDemo.Mvc.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleManagerController : BaseController
    {
        public RoleApp App { get; set; }
        public RevelanceManagerApp RevelanceManagerApp { get; set; }

        // GET: RoleManager
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        #region 添加角色

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public ActionResult AddRole()
        {
            return View("RoleEdit");
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public ActionResult AddRole(RoleView model)
        {
            try
            {
                App.Add(model);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return View("RoleEdit");
        }

        #endregion

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Add(RoleView obj)
        {
            try
            {
                App.Add(obj);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Update(RoleView obj)
        {
            try
            {
                App.Update(obj);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 加载用户的角色
        /// </summary>
        public string LoadForUser(string userId)
        {
            try
            {
                var result = new Response<List<string>>
                {
                    Result = RevelanceManagerApp.Get(Define.USERROLE, true, userId)
                };
                return JsonHelper.Instance.Serialize(result);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        /// <summary>
        /// 加载组织下面的所有角色
        /// </summary>
        public string Load([FromUri]QueryCommonReq request)
        {
            return JsonHelper.Instance.Serialize(App.Load(request));
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Delete(string[] ids)
        {
            try
            {
                App.Delete(ids);
            }
            catch (Exception e)
            {
                Result.Code = 500;
                Result.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }
    }
}