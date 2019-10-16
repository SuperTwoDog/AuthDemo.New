using Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App.SSO
{
    public class AppInfoService : CacheProvider
    {
        public AppInfo Get(string appKey)
        {
            //可以从数据库读取
            return _applist.SingleOrDefault(u => u.AppKey == appKey);
        }

        private AppInfo[] _applist = new[]
        {
            new AppInfo
            {
                AppKey = "authdemo",
                Icon = "/Areas/SSO/Content/images/logo.png",
                IsEnable = true,
                Remark = "芒果计量管理系统",
                ReturnUrl = "http://localhost:60620",
                Title = "OpenAuth.Net",
                CreateTime = DateTime.Now,
            },
            new AppInfo
            {
                AppKey = "openauthtest",
                Icon = "/Areas/SSO/Content/images/logo.png",
                IsEnable = true,
                Remark = "这只是个模拟的测试站点",
                ReturnUrl = "http://localhost:53050",
                Title = "OpenAuth.Net测试站点",
                CreateTime = DateTime.Now,
            }
        };
    }
}
