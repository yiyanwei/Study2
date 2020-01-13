using System;
using SqlSugar;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 实体基础类
    /// </summary>
    public class BaseEntity
    {
        
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid Id { get; set; }

        /// <summary>
        /// 数据状态 0：正常，1：删除
        /// </summary>
        public int? DataStatus { get; set; }
    }
}