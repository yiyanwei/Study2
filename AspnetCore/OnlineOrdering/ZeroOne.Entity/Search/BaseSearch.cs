using System;
using System.Collections.Generic;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 查询基础类
    /// </summary>
    public abstract class BaseSearch
    {
           
    }

    /// <summary>
    /// 基本数据查询
    /// </summary>
    public class BaseInfoSearch: BaseSearch
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