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

    public interface IPageSearch
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    /// <summary>
    /// 分页基础查询对象
    /// </summary>
    public class BasePageSearch : BaseSearch, IPageSearch
    {
        private int pageIndex { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (pageIndex < 0)
                {
                    pageIndex = 0;
                }
                return pageIndex;
            }
            set
            {
                this.pageIndex = value;
            }
        }
        private int pageSize;
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                if (pageSize <= 0)
                {
                    pageSize = 10;
                }
                return pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }
    }


    /// <summary>
    /// 基本数据查询
    /// </summary>
    public class BaseInfoSearch : BaseSearch
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

    public class BasePageInfoSearch : BasePageSearch
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