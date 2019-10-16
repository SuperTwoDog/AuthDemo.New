using AuthDemo.Repository.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Repository.Domain
{
    /// <summary>
	/// 功能模块表
	/// </summary>
    public partial class Module : TreeEntity
    {
        public Module()
        {
            this.Url = string.Empty;
            this.HotKey = string.Empty;
            this.IconName = string.Empty;
            this.Status = 0;
            this.Vector = string.Empty;
            this.SortNo = 0;
            this.Code = string.Empty;
        }

        /// <summary>
	    /// 主页面URL
	    /// </summary>
        public string Url { get; set; }
        /// <summary>
	    /// 热键
	    /// </summary>
        public string HotKey { get; set; }
        /// <summary>
	    /// 是否叶子节点
	    /// </summary>
        public bool IsLeaf { get; set; }
        /// <summary>
	    /// 是否自动展开
	    /// </summary>
        public bool IsAutoExpand { get; set; }
        /// <summary>
	    /// 节点图标文件名称
	    /// </summary>
        public string IconName { get; set; }
        /// <summary>
	    /// 当前状态
	    /// </summary>
        public int Status { get; set; }

        /// <summary>
	    /// 矢量图标
	    /// </summary>
        public string Vector { get; set; }
        /// <summary>
	    /// 排序号
	    /// </summary>
        public int SortNo { get; set; }

        /// <summary>
        /// 模块标识
        /// </summary>
        /// <value>The code.</value>
        public string Code { get; set; }

    }
}
