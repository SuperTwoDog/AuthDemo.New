using AuthDemo.Repository.Domain;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.App.Response
{
    public partial class RoleView
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <returns></returns>
        public string Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
	    /// 角色类型
	    /// </summary>
        public int Type { get; set; }


        /// <summary>
        /// 所属组织名称，多个可用，分隔
        /// </summary>
        public string Organizations { get; set; }

        /// <summary>
        /// 所属组织ID，多个可用，分隔
        /// </summary>
        public string OrganizationIds { get; set; }

        /// <summary>
        ///是否属于某用户 
        /// </summary>
        public bool Checked { get; set; }

        public static implicit operator RoleView(Role role)
        {
            return role.MapTo<RoleView>();
        }

        public static implicit operator Role(RoleView rolevm)
        {
            return rolevm.MapTo<Role>();
        }

    }
}
