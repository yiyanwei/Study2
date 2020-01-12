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

        /// <summary>
        /// 数据状态 0：正常，1：删除
        /// </summary>
        public int? DataStatus { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 是否已经删除 0：未删除，1：已删除
        /// </summary>
        /// <value></value>
        public bool? IsDeleted { get; set; }
    }
}