using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZeroOne.Entity
{
    public enum EJoinType
    {
        /// <summary>
        /// 
        /// </summary>
        InnerJoin = 1,
        LeftJoin = 2,
        RightJoin = 3,
        FullJoin = 4
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
        Contains = 6,
        /// <summary>
        /// 等于
        /// </summary>
        [Description("不等于")]
        NotEqual = 7
    }

    /// <summary>
    /// 逻辑运算操作类型
    /// </summary>
    public enum ELogicalOperator
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

    /// <summary>
    /// 升序降序
    /// </summary>
    public enum EOrderRule
    {
        /// <summary>
        /// 升序
        /// </summary>
        [Description("升序")]
        Asc = 1,
        /// <summary>
        /// 降序
        /// </summary>
        [Description("降序")]
        Desc = 2
    }
}
