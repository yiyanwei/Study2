using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品分类响应对象
    /// </summary>
    public class ProCategoryResponse : IResult
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级分类名称
        /// </summary>
        public string ParentCategoryName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreationTime { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string RealName { get; set; }
    }
}
