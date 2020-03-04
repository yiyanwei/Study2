using System;
using System.ComponentModel;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    /// <summary>
    /// 数据库基础查询类型
    /// </summary>
    public class BaseRepModel
    {
        public BaseRepModel(string key)
        {
            this.Key = key;
            this.CompareOperator = ECompareOperator.Equal;
            this.LogicalOperatorType = ELogicalOperator.And;
        }

        public BaseRepModel(string key, ECompareOperator compareOperator)
        {
            this.Key = key;
            this.CompareOperator = compareOperator;
            this.LogicalOperatorType = ELogicalOperator.And;
        }

        public BaseRepModel(string key, ECompareOperator compareOperator, ELogicalOperator logicalOperatorType)
        {
            this.Key = key;
            this.CompareOperator = compareOperator;
            this.LogicalOperatorType = logicalOperatorType;
        }
        
        /// <summary>
        /// key值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 比较操作类型
        /// </summary>
        public ECompareOperator CompareOperator { get; set; }
        /// <summary>
        /// value值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 逻辑运算符
        /// </summary>
        public ELogicalOperator LogicalOperatorType { get; set; }
    }

    
}