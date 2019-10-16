using AuthDemo.App.Request;
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
    public class AppManager : BaseApp<Application>
    {
        public void Add(Application Application)
        {
            if (string.IsNullOrEmpty(Application.Id))
            {
                Application.Id = Guid.NewGuid().ToString();
            }
            Repository.Add(Application);
        }

        public void Update(Application Application)
        {
            Repository.Update(u => u.Id, Application);
        }


        public List<Application> GetList(QueryAppListReq request)
        {
            var applications = UnitWork.Find<Application>(null);

            return applications.ToList();
        }
    }
}
