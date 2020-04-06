using System;
using SqlSugar;


namespace ZeroOne.Entity
{
    /// <summary>
    /// 供应商
    /// </summary>
    public class Supplier : IEntity<Guid>, IDeleted
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier()
        {

        }

        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SupplierCode { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// 供应商联系人
        /// </summary>
        public string ContactMan { get; set; }

        /// <summary>
        /// 供应商联系电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 县市区
        /// </summary>
        public string Prefecture { get; set; }

        /// <summary>
        /// 供应商详细地址（不包括省市区）
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public int? DataStatus { get; set; }

        /// <summary>
        /// 营业执照
        /// </summary>
        public Guid? BusinessLicense { get; set; }

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