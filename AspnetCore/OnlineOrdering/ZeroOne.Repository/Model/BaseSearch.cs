using System;
using System.Collections.Generic;

namespace ZeroOne.Repository
{
    /// <summary>
    /// 查询基础类
    /// </summary>
    public class BaseSearch
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        /// <value></value>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }


        public IList<int> DataStatus { get; set; }
    }
}