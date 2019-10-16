using AuthDemo.App;
using AuthDemo.App.Request;
using AuthDemo.Repository.Domain;
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
    /// 资源管理
    /// </summary>
    public class ResourcesController : BaseController
    {
        public ResourceApp App { get; set; }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 加载资源信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Load([FromUri]QueryResourcesReq request)
        {
            return JsonHelper.Instance.Serialize(App.Load(request));
        }

        /// <summary>
        /// 新增资源信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Add(Resource obj)
        {
            Response resp = new Response();
            try
            {
                App.Add(obj);
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        /// <summary>
        /// 修改资源信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Update(Resource obj)
        {
            Response resp = new Response();
            try
            {
                App.Update(obj);
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        /// <summary>
        /// 删除资源信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string Delete(string[] ids)
        {
            Response resp = new Response();
            try
            {
                App.Delete(ids);
            }
            catch (Exception e)
            {
                resp.Code = 500;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        /// <summary>
        /// 加载角色资源
        /// </summary>
        /// <param name="appId">应用ID</param>
        /// <param name="firstId">角色ID</param>
        public string LoadForRole(string appId, string firstId)
        {
            try
            {
                var result = new Response<List<string>>
                {
                    Result = App.LoadForRole(appId, firstId).Select(u => u.Id).ToList()
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
        /// 分配资源页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Assign()
        {
            return View();
        }

        /// <summary>
        /// 加载特定用户的资源
        /// </summary>
        /// <param name="appId">应用appId</param>
        /// <param name="firstId">用户ID</param>
        /// <returns>System.String.</returns>
        public string LoadForUser(string appId, string firstId)
        {

            try
            {
                var result = new Response<List<string>>
                {
                    Result = App.LoadForUser(appId, firstId).Select(u => u.Id).ToList()
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
        ///设置资源状态
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="status">状态：0删除 1启用</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public string SetResourcesStatus(string ids, int status = 0)
        {
            try
            {
                bool bStatus = status == 0 ? true : false;
                //App.Delete(ids);
                //逻辑删除
                App.SetResourcesStatus(ids, bStatus);
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