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
    }
}