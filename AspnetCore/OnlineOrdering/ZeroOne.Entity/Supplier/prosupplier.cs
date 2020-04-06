using System;
using SqlSugar;


namespace ZeroOne.Entity
{
    /// <summary>
    /// 产品供应商关联表
    /// </summary>
    public class prosupplier : IEntity<Guid>
    {
        /// <summary>
        /// 产品供应商关联表
        /// </summary>
        public prosupplier()
        {
        }

        private System.Guid _Id;
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public System.Guid Id { get { return this._Id; } set { this._Id = value; } }

        private System.Guid? _ProId;
        /// <summary>
        /// 产品Id
        /// </summary>
        public System.Guid? ProId { get { return this._ProId; } set { this._ProId = value; } }

        private System.Guid? _SupplierId;
        /// <summary>
        /// 供应商Id
        /// </summary>
        public System.Guid? SupplierId { get { return this._SupplierId; } set { this._SupplierId = value; } }

        private System.Boolean? _StarLevel;
        /// <summary>
        /// 该供应商该产品的星级，0-5级，5星最高
        /// </summary>
        public System.Boolean? StarLevel { get { return this._StarLevel; } set { this._StarLevel = value; } }

        private System.Guid? _BusinessLicense;
        /// <summary>
        /// 营业执照
        /// </summary>
        public System.Guid? BusinessLicense { get { return this._BusinessLicense; } set { this._BusinessLicense = value; } }

        private System.Int32? _DataStatus;
        /// <summary>
        /// 数据状态
        /// </summary>
        public System.Int32? DataStatus { get { return this._DataStatus; } set { this._DataStatus = value; } }

        private System.Guid? _CreatorUserId;
        /// <summary>
        /// 创建人
        /// </summary>
        public System.Guid? CreatorUserId { get { return this._CreatorUserId; } set { this._CreatorUserId = value; } }

        private System.DateTime? _CreationTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime? CreationTime { get { return this._CreationTime; } set { this._CreationTime = value; } }

        private System.Guid? _LastModifierUserId;
        /// <summary>
        /// 更新人
        /// </summary>
        public System.Guid? LastModifierUserId { get { return this._LastModifierUserId; } set { this._LastModifierUserId = value; } }

        private System.DateTime? _LastModificationTime;
        /// <summary>
        /// 更新时间
        /// </summary>
        public System.DateTime? LastModificationTime { get { return this._LastModificationTime; } set { this._LastModificationTime = value; } }

        private System.UInt64? _IsDeleted;
        /// <summary>
        /// 是否删除 false(0):未删除，true(1):已删除
        /// </summary>
        public System.UInt64? IsDeleted { get { return this._IsDeleted; } set { this._IsDeleted = value; } }

        private System.Guid? _DeleterUserId;
        /// <summary>
        /// 删除操作人Id
        /// </summary>
        public System.Guid? DeleterUserId { get { return this._DeleterUserId; } set { this._DeleterUserId = value; } }

        private System.DateTime? _DeletionTime;
        /// <summary>
        /// 删除时间
        /// </summary>
        public System.DateTime? DeletionTime { get { return this._DeletionTime; } set { this._DeletionTime = value; } }

        private System.Guid? _RowVersion;
        /// <summary>
        /// 版本号
        /// </summary>
        public System.Guid? RowVersion { get { return this._RowVersion; } set { this._RowVersion = value; } }
    }
}