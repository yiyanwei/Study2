using System; 
using SqlSugar;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class Pro_Info
    {
        /// <summary>
        /// 
        /// </summary>
        public Pro_Info()
        {
        }

        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProDesc { get; set; }

        /// <summary>
        /// 产品图片地址
        /// </summary>
        public string ProImg { get; set; }

        /// <summary>
        /// 产品的基本单位
        /// </summary>
        public string ProBaseUnit { get; set; }

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
        public bool IsDeleted{get;set;}
    }
}