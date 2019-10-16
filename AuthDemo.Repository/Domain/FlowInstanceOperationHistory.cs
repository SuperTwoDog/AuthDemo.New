using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Repository.Domain
{
    /// <summary>
	/// 工作流实例操作记录
	/// </summary>
    public partial class FlowInstanceOperationHistory : Entity
    {
        public FlowInstanceOperationHistory()
        {
            this.InstanceId = string.Empty;
            this.Content = string.Empty;
            this.CreateDate = DateTime.Now;
            this.CreateUserId = string.Empty;
            this.CreateUserName = string.Empty;
        }

        /// <summary>
	    /// 实例进程Id
	    /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
	    /// 操作内容
	    /// </summary>
        public string Content { get; set; }
        /// <summary>
	    /// 创建时间
	    /// </summary>
        public System.DateTime CreateDate { get; set; }
        /// <summary>
	    /// 创建用户主键
	    /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
	    /// 创建用户
	    /// </summary>
        public string CreateUserName { get; set; }

    }
}
