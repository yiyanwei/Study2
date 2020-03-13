using System;
using SqlSugar;

namespace ZeroOne.Entity
{
    
    /// <summary>
    /// 实体基础类
    /// </summary>
    public class BaseEntity<TPrimaryKey>: IEntity<TPrimaryKey>, IDeleted, IUpdated, IRowVersion
    {
        
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public TPrimaryKey Id { get; set; }

        /// <summary>
        /// 数据状态 0：正常，1：删除
        /// </summary>
        public int? DataStatus { get; set; }

        /// <summary>
        /// 是否已经删除 0：未删除，1：已删除
        /// </summary>
        /// <value></value>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// 删除操作人
        /// </summary>
        public Guid? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }

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
        /// 版本号
        /// </summary>
        public Guid? RowVersion { get; set; }

        //public override bool Equals(object obj)
        //{
        //    if (obj == null || !(obj is BaseEntity<TPrimaryKey>))
        //    {
        //        return false;
        //    }
        //    BaseEntity<TPrimaryKey> entity = (BaseEntity<TPrimaryKey>)obj;
        //    TPrimaryKey id = this.Id;
        //    return id.Equals(entity.Id);
        //}
        //public override int GetHashCode()
        //{
        //    TPrimaryKey id = this.Id;
        //    return id.GetHashCode();
        //}
        //public override string ToString()
        //{
        //    return string.Format("[{0} {1}]", base.GetType().Name, this.Id);
        //}

        //public static bool operator ==(BaseEntity<TPrimaryKey> left, BaseEntity<TPrimaryKey> right)
        //{
        //    if (object.Equals(left, null))
        //    {
        //        return object.Equals(right, null);
        //    }
        //    return left.Equals(right);
        //}
        //public static bool operator !=(BaseEntity<TPrimaryKey> left, BaseEntity<TPrimaryKey> right)
        //{
        //    return !(left == right);
        //}
    }
}