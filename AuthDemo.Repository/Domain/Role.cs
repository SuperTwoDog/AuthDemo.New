﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Repository.Domain
{
    /// <summary>
    /// 角色表
    /// </summary>
    public partial class Role : Entity
    {
        public Role()
        {
            this.Name = string.Empty;
            this.Status = 0;
            this.CreateTime = DateTime.Now;
            this.CreateId = string.Empty;
            this.TypeName = string.Empty;
            this.TypeId = string.Empty;
            this.Remark = string.Empty;
        }

        /// <summary>
	    /// 角色名称
	    /// </summary>
        public string Name { get; set; }
        /// <summary>
	    /// 当前状态
	    /// </summary>
        public int Status { get; set; }
        /// <summary>
	    /// 创建时间
	    /// </summary>
        public System.DateTime CreateTime { get; set; }
        /// <summary>
	    /// 创建人ID
	    /// </summary>
        public string CreateId { get; set; }
        /// <summary>
	    /// 分类名称
	    /// </summary>
        public string TypeName { get; set; }
        /// <summary>
	    /// 分类ID
	    /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
