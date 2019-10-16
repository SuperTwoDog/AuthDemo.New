using AuthDemo.App.Request;
using AuthDemo.App.Response;
using AuthDemo.App.SSO;
using AuthDemo.Repository;
using AuthDemo.Repository.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App
{
    public class UserManagerApp : BaseApp<User>
    {
        public RevelanceManagerApp ReleManagerApp { get; set; }

        public User Get(string account)
        {
            return Repository.FindSingle(u => u.Account == account);
        }

        public User GetUserInfo(string userID)
        {
            return Repository.FindSingle(u => u.Id == userID);
        }

        /// <summary>
        /// 加载当前登录用户可访问的一个部门及子部门全部用户
        /// </summary>
        public TableData Load(QueryUserListReq request)
        {
            var loginUser = AuthUtil.GetCurrentUser();

            string cascadeId = ".0.";
            if (!string.IsNullOrEmpty(request.orgId))
            {
                var org = loginUser.Orgs.SingleOrDefault(u => u.Id == request.orgId);
                cascadeId = org.CascadeId;
            }

            var ids = loginUser.Orgs.Where(u => u.CascadeId.Contains(cascadeId)).Select(u => u.Id).ToArray();
            var userIds = ReleManagerApp.Get(Define.USERORG, false, ids);

            Expression<Func<User, bool>> where = p => true;
            if (!string.IsNullOrEmpty(request.key))
            {
                where = i => i.Name.Contains(request.key);
            }
            Expression<Func<User, bool>> where1 = p => true;
            Expression<Func<User, bool>> where2 = p => true;
            if (request.UserStatus > 0)
            {
                if (request.UserStatus == 2)
                {
                    where1 = i => i.Status == 0;
                    where2 = i => i.Status == 0 && userIds.Contains(i.Id);
                }
                else if (request.UserStatus == 3)
                {
                    where1 = i => i.Status == 0 || i.Status == 1;
                    where2 = i => (i.Status == 0 || i.Status == 1) && userIds.Contains(i.Id);
                }
                else
                {
                    where1 = i => i.Status == 1;
                    where2 = i => i.Status == 1 && userIds.Contains(i.Id);
                }
            }
            var users = UnitWork.Find<User>(u => userIds.Contains(u.Id)).Where(where).Where(where1)
                   .OrderBy(u => u.Name)
                   .Skip((request.page - 1) * request.limit)
                   .Take(request.limit);

            var records = Repository.GetCount(where2);


            var userviews = new List<UserView>();
            foreach (var user in users.ToList())
            {
                UserView uv = user;
                var orgs = LoadByUser(user.Id);
                uv.Organizations = string.Join(",", orgs.Select(u => u.Name).ToList());
                uv.OrganizationIds = string.Join(",", orgs.Select(u => u.Id).ToList());
                userviews.Add(uv);
            }

            return new TableData
            {
                count = records,
                data = userviews,
            };
        }

        public void AddOrUpdate(UserView view)
        {
            if (string.IsNullOrEmpty(view.OrganizationIds))
                throw new Exception("请为用户分配机构");
            User user = view;
            if (string.IsNullOrEmpty(view.Id))
            {
                if (UnitWork.IsExist<User>(u => u.Account == view.Account))
                {
                    throw new Exception("用户账号已存在");
                }
                user.CreateTime = DateTime.Now;
                user.Password = user.Account; //初始密码与账号相同
                Repository.Add(user);
                view.Id = user.Id;   //要把保存后的ID存入view
            }
            else
            {
                UnitWork.Update<User>(u => u.Id == view.Id, u => new User
                {
                    Account = user.Account,
                    BizCode = user.BizCode,
                    Name = user.Name,
                    Sex = user.Sex,
                    Status = user.Status,
                    Description = user.Description
                });
            }
            string[] orgIds = view.OrganizationIds.Split(',').ToArray();

            ReleManagerApp.DeleteBy(Define.USERORG, user.Id);
            ReleManagerApp.AddRelevance(Define.USERORG, orgIds.ToLookup(u => user.Id));
        }

        /// <summary>
        /// 加载用户的所有机构
        /// </summary>
        public IEnumerable<Org> LoadByUser(string userId)
        {
            var result = from userorg in UnitWork.Find<Relevance>(null)
                         join org in UnitWork.Find<Org>(null) on userorg.SecondId equals org.Id
                         where userorg.FirstId == userId && userorg.Key == Define.USERORG
                         select org;
            return result;
        }

        /// <summary>
        /// 设置用户状态
        /// </summary>
        /// <param name="userIDs">用户ID</param>
        /// <param name="status">状态：0禁用 1启用</param>
        /// <returns></returns>
        public int SetUserStatus(string userIDs, int status = 0)
        {
            int result = 0;
            string[] userArray = userIDs.Split(',');
            foreach (var userID in userArray)
            {
                UnitWork.Update<User>(u => u.Id == userID, u => new User
                {
                    Status = status
                });
                result++;
            }
            return result;
        }

    }
}
