using System;
using SqlSugar;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品类别
    /// </summary>
    [SugarTable("pro_category")]
    public class ProCategory: BaseEntity<Guid?>
    {
        /// <summary>
        /// 
        /// </summary>
        public ProCategory()
        {

        }

        /// <summary>
        /// 产品类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级分类Id
        /// </summary>
        public Guid? ParentId { get; set; }
    }
}