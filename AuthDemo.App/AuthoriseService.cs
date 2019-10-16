using AuthDemo.Repository;
using AuthDemo.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App
{
    /// <summary>
    /// 领域服务
    /// <para>用户授权服务</para>
    /// </summary>
    public class AuthoriseService : BaseApp<User>
    {
        protected User _user;

        private List<string> _userRoleIds;    //用户角色GUID

        /// <summary>
        /// 用户拥有的模块
        /// </summary>
        public List<Module> Modules
        {
            get { return GetModulesQuery().ToList(); }
        }

        /// <summary>
        /// 用户角色
        /// </summary>
        public List<Role> Roles
        {
            get { return GetRolesQuery().ToList(); }
        }

        /// <summary>
        /// 用户模块元素
        /// </summary>
        public List<ModuleElement> ModuleElements
        {
            get { return GetModuleElementsQuery().ToList(); }
        }

        /// <summary>
        /// 用户资源
        /// </summary>
        public List<Resource> Resources
        {
            get { return GetResourcesQuery().ToList(); }
        }

        /// <summary>
        /// 机构列表
        /// </summary>
        public List<Org> Orgs
        {
            get { return GetOrgsQuery().ToList(); }
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                _userRoleIds = UnitWork.Find<Relevance>(u => u.FirstId == _user.Id && u.Key == Define.USERROLE).Select(u => u.SecondId).ToList();
            }
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void Check(string userName, string password)
        {
            var _user = Repository.FindSingle(u => u.Account == userName);
            if (_user == null)
            {
                throw new Exception("用户帐号不存在");
            }
            _user.CheckPassword(password);
        }

        /// <summary>
        /// 用户可访问的机构
        /// </summary>
        /// <returns>IQueryable&lt;Org&gt;.</returns>
        public virtual IQueryable<Org> GetOrgsQuery()
        {
            var orgids = UnitWork.Find<Relevance>(
                u =>
                    (u.FirstId == _user.Id && u.Key == Define.USERORG) ||
                    (u.Key == Define.ROLEORG && _userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId);
            return UnitWork.Find<Org>(u => orgids.Contains(u.Id));
        }

        /// <summary>
        /// 获取用户可访问的资源
        /// </summary>
        /// <returns>IQueryable&lt;Resource&gt;.</returns>
        public virtual IQueryable<Resource> GetResourcesQuery()
        {
            var resourceIds = UnitWork.Find<Relevance>(
                u =>
                    (u.FirstId == _user.Id && u.Key == Define.USERRESOURCE) ||
                    (u.Key == Define.ROLERESOURCE && _userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId);
            return UnitWork.Find<Resource>(u => resourceIds.Contains(u.Id));
        }

        /// <summary>
        /// 模块菜单权限
        /// </summary>
        public virtual IQueryable<ModuleElement> GetModuleElementsQuery()
        {
            var elementIds = UnitWork.Find<Relevance>(
                u =>
                    (u.FirstId == _user.Id && u.Key == Define.USERELEMENT) ||
                    (u.Key == Define.ROLEELEMENT && _userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId);
            return UnitWork.Find<ModuleElement>(u => elementIds.Contains(u.Id));
        }

        /// <summary>
        /// 得出最终用户拥有的模块
        /// </summary>
        public virtual IQueryable<Module> GetModulesQuery()
        {
            var moduleIds = UnitWork.Find<Relevance>(
                u =>
                    (u.FirstId == _user.Id && u.Key == Define.USERMODULE) ||
                    (u.Key == Define.ROLEMODULE && _userRoleIds.Contains(u.FirstId))).Select(u => u.SecondId);
            return UnitWork.Find<Module>(u => moduleIds.Contains(u.Id)).OrderBy(u => u.SortNo);
        }

        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<Role> GetRolesQuery()
        {
            return UnitWork.Find<Role>(u => _userRoleIds.Contains(u.Id));
        }
    }
}
