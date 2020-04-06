using System;
using SqlSugar;


namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品供应商关联表
    /// </summary>
    public class ProSupplier : IEntity<Guid>
    {
        /// <summary>
        /// 产品供应商关联表
        /// </summary>
        public ProSupplier()
        {
        }

        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public Guid? ProId { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public Guid? SupplierId { get; set; }

        /// <summary>
        /// 该供应商该产品的星级，0-5级，5星最高
        /// </summary>
        public bool? StarLevel { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public Guid? BusinessLicense { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public int? DataStatus { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public Guid? LastModifierUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// 是否删除 false(0):未删除，true(1):已删除
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 删除操作人Id
        /// </summary>
        public Guid? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public Guid? RowVersion { get; set; }
    }
}