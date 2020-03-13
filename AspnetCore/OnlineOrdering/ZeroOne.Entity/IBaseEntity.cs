using System;
using System.Collections.Generic;
using System.Text;

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
        Guid? DeleterUserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        DateTime? DeletionTime { get; set; }

    }

    /// <summary>
    /// 更新数据接口
    /// </summary>
    public interface IUpdated : IRowVersion
    {
        /// <summary>
        /// 更新人
        /// </summary>
        Guid? LastModifierUserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        DateTime? LastModificationTime { get; set; }
    }

    /// <summary>
    /// 新增数据结构
    /// </summary>
    public interface IAdd
    {
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }
    }

    /// <summary>
    /// 主键数据接口
    /// </summary>
    /// <typeparam name="TPrimarykey"></typeparam>
    public interface IEntity<TPrimarykey>
    {
        TPrimarykey Id { get; set; }
    }

    /// <summary>
    /// 更新对象
    /// </summary>
    public class UpdateEntity : IUpdated
    {
        public UpdateEntity()
        {
            this.LastModificationTime = DateTime.Now;
        }
        /// <summary>
        /// 更新操作人
        /// </summary>
        public Guid? LastModifierUserId { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? LastModificationTime { get; set; }
        /// <summary>
        /// 操作版本号
        /// </summary>
        public Guid? RowVersion { get; set; }
    }

    /// <summary>
    /// 添加对象
    /// </summary>
    public class AddEntity : IAdd
    {
        public AddEntity()
        {
            this.CreationTime = DateTime.Now;
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public Guid? CreatorUserId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreationTime { get; set; }
    }

    public interface IAddRequest : IAdd
    {

    }

    public interface IEditRequest : IUpdated
    {

    }

}
