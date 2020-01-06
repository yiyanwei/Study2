using System;
using System.ComponentModel;
namespace ZeroOne.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepModel<T>
    {
        public BaseRepModel(string key, EBaseRepOperator _operator, T val)
        { 
            this.Key = key;
            this.Operator = _operator;
            this.Value = val;
        }
        /// <summary>
        /// key值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 比较操作类型
        /// </summary>
        public EBaseRepOperator Operator { get; set; }
        /// <summary>
        /// value值
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// 查询数据比较类型
    /// </summary>
    public enum EBaseRepOperator
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
        Great = 2,
        /// <summary>
        /// 小于
        /// </summary>
        [Description("小于")]
        Less = 3,
        /// <summary>
        /// 大于等于
        /// </summary>
        [Description("大于等于")]
        GreatEqual = 4,
        /// <summary>
        /// 小于等于
        /// </summary>
        [Description("小于等于")]
        LessEqual = 5,
        /// <summary>
        /// 包含
        /// </summary>
        [Description("包含")]
        Contains = 6
    }
}