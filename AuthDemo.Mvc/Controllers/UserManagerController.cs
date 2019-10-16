using AuthDemo.App;
using AuthDemo.App.Request;
using AuthDemo.App.Response;
using AuthDemo.App.SSO;
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
    /// 用户管理控制器
    /// </summary>
    public class UserManagerController : BaseController
    {
        public UserManagerApp App { get; set; }

        // GET: UserManager
        [Authenticate]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载组织下面的所有用户
        /// </summary>
        public string Load([FromUri]QueryUserListReq request)
        {
            var data = App.Load(request);
            return JsonHelper.Instance.Serialize(data);
        }

        #region 新增用户

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <returns></returns>
        public ActionResult AddUser()
        {
            UserView model = new UserView();
            return View("UserEdit", model);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string AddUser(UserView view)
        {
            try
            {
                App.AddOrUpdate(view);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
        }

        #endregion

        #region 修改用户

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public ActionResult EditUser(string userID)
        {
            UserView model = App.GetUserInfo(userID);
            return View("UserEdit", model);
        }

        #endregion

        /// <summary>
        /// 添加或修改用户
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string AddOrUpdate(UserView view)
        {
            try
            {
                App.AddOrUpdate(view);
            }
            catch (Exception ex)
            {
                Result.Code = 500;
                Result.Message = ex.Message;
            }
            return JsonHelper.Instance.Serialize(Result);
            //return this.Content("");
        }

        /// <summary>
        /// 删除用户(设置用户状态)
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">状态：0删除 1启用</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string SetUserStatus(string ids,int status = 0)
        {
            try
            {
                //App.Delete(ids);
                //逻辑删除
                App.SetUserStatus(ids, status);
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