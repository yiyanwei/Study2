using System;
using SqlSugar;


namespace ZeroOne.Entity
{
    /// <summary>
    /// 供应商
    /// </summary>
    public class supplier : IEntity<Guid>
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public supplier()
        {

        }

        private System.Guid _Id;
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public System.Guid Id { get { return this._Id; } set { this._Id = value; } }

        private System.String _SupplierCode;
        /// <summary>
        /// 供应商编号
        /// </summary>
        public System.String SupplierCode { get { return this._SupplierCode; } set { this._SupplierCode = value; } }

        private System.String _SupplierName;
        /// <summary>
        /// 供应商名称
        /// </summary>
        public System.String SupplierName { get { return this._SupplierName; } set { this._SupplierName = value; } }

        private System.String _ContactMan;
        /// <summary>
        /// 供应商联系人
        /// </summary>
        public System.String ContactMan { get { return this._ContactMan; } set { this._ContactMan = value; } }

        private System.String _ContactPhone;
        /// <summary>
        /// 供应商联系电话
        /// </summary>
        public System.String ContactPhone { get { return this._ContactPhone; } set { this._ContactPhone = value; } }

        private System.String _Province;
        /// <summary>
        /// 省
        /// </summary>
        public System.String Province { get { return this._Province; } set { this._Province = value; } }

        private System.String _City;
        /// <summary>
        /// 市
        /// </summary>
        public System.String City { get { return this._City; } set { this._City = value; } }

        private System.String _Prefecture;
        /// <summary>
        /// 区
        /// </summary>
        public System.String Prefecture { get { return this._Prefecture; } set { this._Prefecture = value; } }

        private System.String _Address;
        /// <summary>
        /// 供应商详细地址（不包括省市区）
        /// </summary>
        public System.String Address { get { return this._Address; } set { this._Address = value; } }

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