using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Repository.Domain
{
    /// <summary>
	/// 分类类型
	/// </summary>
    public partial class CategoryType : Entity
    {
        public CategoryType()
        {
            this.Name = string.Empty;
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
	    /// 名称
	    /// </summary>
        public string Name { get; set; }
        /// <summary>
	    /// 创建时间
	    /// </summary>
        public System.DateTime CreateTime { get; set; }

    }
}
