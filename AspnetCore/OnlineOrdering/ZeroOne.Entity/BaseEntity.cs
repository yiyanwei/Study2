using System;
using SqlSugar;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 批量导入数据
    /// </summary>
    public interface IBulkModel
    {
        /// <summary>
        /// 批量标识，判断是什么时候导入的，数据格式 yyyyMMddHHmmssfff
        /// </summary>
        string BulkIdentity { get; set; }
    }

    /// <summary>
    /// 控制并发处理模型接口
    /// </summary>
    public interface IRowVersion
    {
        /// <summary>
        /// 行版本号
        /// </summary>
        Guid? RowVersion { get; set; }
    }

    public interface IDeleted
    {
        bool? IsDeleted { get; set; }

        /// <summary>
        /// 删除操作人
        /// </summary>
        string DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }

    }

    public interface IUpdated
    {
        /// <summary>
        /// 更新人
        /// </summary>
        string LastModifierUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }

    /// <summary>
    /// 实体基础类
    /// </summary>
    public class BaseEntity<TPrimaryKey>: IDeleted, IUpdated
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
        public string DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletionTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string LastModifierUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BaseEntity<TPrimaryKey>))
            {
                return false;
            }
            BaseEntity<TPrimaryKey> entity = (BaseEntity<TPrimaryKey>)obj;
            TPrimaryKey id = this.Id;
            return id.Equals(entity.Id);
        }
        public override int GetHashCode()
        {
            TPrimaryKey id = this.Id;
            return id.GetHashCode();
        }
        public override string ToString()
        {
            return string.Format("[{0} {1}]", base.GetType().Name, this.Id);
        }

        public static bool operator ==(BaseEntity<TPrimaryKey> left, BaseEntity<TPrimaryKey> right)
        {
            if (object.Equals(left, null))
            {
                return object.Equals(right, null);
            }
            return left.Equals(right);
        }
        public static bool operator !=(BaseEntity<TPrimaryKey> left, BaseEntity<TPrimaryKey> right)
        {
            return !(left == right);
        }
    }
}