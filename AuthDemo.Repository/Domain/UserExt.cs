using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Repository.Domain
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public static class UserExt
    {
        public static void CheckPassword(this User user, string password)
        {
            if (user.Password != password)
            {
                throw new Exception("密码错误");
            }
        }
    }
}
