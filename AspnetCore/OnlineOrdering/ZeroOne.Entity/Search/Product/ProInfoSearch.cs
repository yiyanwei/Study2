using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品分类查询类
    /// </summary>
    public class ProInfoSearch : BaseSearch
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName{get;set;}

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProCode { get; set; }

        /// <summary>
        /// 产品分类Id
        /// </summary>
        public Guid? CategoryId { get; set; }

        [DbOperation()]
        public string CategoryName { get; set; }

    }

    /// <summary>
    /// 产品分页查询对象
    /// </summary>
    public class ProInfoPageSearch : ProInfoSearch, IPageSearch
    {
        /// <summary>
        /// 页索引
        /// </summary>
        public int PageIndex { get;set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get;set; }
    }
}