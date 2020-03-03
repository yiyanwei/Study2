using System;
using System.ComponentModel;
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
            this.LogicalOperatorType = ELogicalOperatorType.And;
        }

        public BaseRepModel(string key, ECompareOperator compareOperator)
        {
            this.Key = key;
            this.CompareOperator = compareOperator;
            this.LogicalOperatorType = ELogicalOperatorType.And;
        }

        public BaseRepModel(string key, ECompareOperator compareOperator, ELogicalOperatorType logicalOperatorType)
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
        public ELogicalOperatorType LogicalOperatorType { get; set; }
    }

    /// <summary>
    /// 查询数据比较类型
    /// </summary>
    public enum ECompareOperator
    {
        /// <summary>
        /// 等于
        /// </summary>
        [Description("等于")]
        Equal = 1,
        /// <summary>
        /// 大于
        /// </summary>
        [Description("大于")]
        GreaterThan = 2,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        LessThan = 3,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        GreaterThanOrEqual = 4,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        LessThanOrEqual = 5,
        /// <summary>
        /// 包含
        /// </summary>
        [Description("包含")]
        Contains = 6
    }

    /// <summary>
    /// 逻辑运算操作类型
    /// </summary>
    public enum ELogicalOperatorType
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        [Description("None")]
        None = 0,
        /// <summary>
        /// 并运算
        /// </summary>
        [Description("And")]
        And = 1,
        /// <summary>
        /// 或运算
        /// </summary>
        [Description("Or")]
        Or = 2
    }
}