using AuthDemo.App.Request;
using AuthDemo.App.Response;
using AuthDemo.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App
{
    /// <summary>
    /// 分类管理
    /// </summary>
    public class ResourceApp : BaseApp<Resource>
    {
        public RevelanceManagerApp RevelanceManagerApp { get; set; }

        public IEnumerable<Resource> Get(string type)
        {
            return Repository.Find(u => u.TypeId == type);
        }

        public void Add(Resource resource)
        {
            if (string.IsNullOrEmpty(resource.Id))
            {
                resource.Id = Guid.NewGuid().ToString();
            }
            Repository.Add(resource);
        }

        public void Update(Resource resource)
        {
            Repository.Update(u => u.Id, resource);
        }

        public IEnumerable<Resource> LoadForUser(string appId, string userId)
        {
            var elementIds = RevelanceManagerApp.Get(Define.USERRESOURCE, true, userId);
            return UnitWork.Find<Resource>(u => elementIds.Contains(u.Id) && (appId == "" || u.AppId == appId));
        }

        public IEnumerable<Resource> LoadForRole(string appId, string userId)
        {
            var elementIds = RevelanceManagerApp.Get(Define.ROLERESOURCE, true, userId);
            return UnitWork.Find<Resource>(u => elementIds.Contains(u.Id) && (appId == "" || u.AppId == appId));
        }



        public TableData Load(QueryResourcesReq request)
        {
            var result = new TableData();
            var resources = UnitWork.Find<Resource>(null);
            if (!string.IsNullOrEmpty(request.key))
            {
                resources = resources.Where(u => u.Name.Contains(request.key) || u.Id.Contains(request.key));
            }

            if (!string.IsNullOrEmpty(request.TypeId))
            {
                resources = resources.Where(u => u.TypeId == request.TypeId);
            }
            if (request.Status == 1)
            {
                //查已启用
                resources = resources.Where(x => x.Disable == false);
            }
            else if (request.Status == 2)
            {
                //查已禁用
                resources = resources.Where(x => x.Disable == true);
            }

            result.data = resources.OrderBy(u => u.TypeId)
                .Skip((request.page - 1) * request.limit)
                .Take(request.limit).ToList();
            result.count = resources.Count();
            return result;
        }

        /// <summary>
        /// 设置资源状态
        /// </summary>
        /// <param name="userIDs">用户ID</param>
        /// <param name="status">是否禁用</param>
        /// <returns></returns>
        public int SetResourcesStatus(string ids, bool status)
        {
            int result = 0;
            string[] arrays = ids.Split(',');
            foreach (var resID in arrays)
            {
                UnitWork.Update<Resource>(u => u.Id == resID, u => new Resource
                {
                    Disable = status
                });
                result++;
            }
            return result;
        }

    }
}
