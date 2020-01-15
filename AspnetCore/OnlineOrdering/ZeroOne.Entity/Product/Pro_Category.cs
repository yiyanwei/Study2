using System;
using SqlSugar;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Pro_Category:BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public Pro_Category()
        {
        }

        /// <summary>
        /// 产品类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级分类Id
        /// </summary>
        public Guid ParentId { get; set; }

        ///// <summary>
        ///// 数据状态 0：正常，1：删除
        ///// </summary>
        //public int? DataStatus { get; set; }

    }
}