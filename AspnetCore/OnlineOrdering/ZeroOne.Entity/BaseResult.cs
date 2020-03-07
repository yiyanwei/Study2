using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    public interface IResult
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public class Result : IResult
    {

    }

    public interface IBaseResult<TPrimaryKey> : IEntity<TPrimaryKey>, IDeleted, IUpdated, IRowVersion
    {

    }

    public class BaseSearchResult<TResult>
    {
        public IList<TResult> Items { get; set; }
    }

    public class BaseResult<TPrimaryKey> : Result, IBaseResult<TPrimaryKey>
    {
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

        /// <summary>
        /// 版本号
        /// </summary>
        public Guid? RowVersion { get; set; }
    }
}
