using AuthDemo.App.Response;
using AuthDemo.Repository.Domain;
using AuthDemo.Repository.Interface;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App
{
    /// <summary>
    ///  加载用户所有可访问的资源/机构/模块
    /// </summary>
    public class AuthorizeApp
    {
        public SystemAuthService AuthService { get; set; }

        public AuthoriseService AuthoriseService { get; set; }

        public IUnitWork _unitWork { get; set; }

        public AuthoriseService Create(string loginuser)
        {
            if (loginuser == "System")
            {
                return AuthService;
            }
            else
            {
                AuthoriseService.User = _unitWork.FindSingle<User>(u => u.Account == loginuser);
                return AuthoriseService;
            }
        }

        public UserWithAccessedCtrls GetAccessedControls(string username)
        {
            var service = Create(username);
            var user = new UserWithAccessedCtrls
            {
                User = service.User,
                Orgs = service.Orgs,
                Modules = service.Modules.OrderBy(u => u.SortNo).ToList().MapToList<ModuleView>(),
                Resources = service.Resources,
                Roles = service.Roles
            };

            var ModuleElements = service.ModuleElements;

            foreach (var moduleView in user.Modules)
            {
                if (moduleView.Code == "User")
                {
                    var list = ModuleElements.Where(u => u.ModuleId == moduleView.Id).OrderBy(u => u.Sort).ToList();
                }
                moduleView.Elements = ModuleElements.Where(u => u.ModuleId == moduleView.Id).OrderBy(u => u.Sort).ToList();
            }

            return user;
        }
    }
}
