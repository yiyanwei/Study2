using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ZeroOne.Extension
{
    /// <summary>
    /// 选择项
    /// </summary>
    public class SelectItem<TLabel, TValue>
    {
        public SelectItem()
        {

        }

        public SelectItem(TLabel label, TValue val, TValue parentVal)
        {
            Label = label;
            Value = val;
            ParentValue = parentVal;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public TLabel Label { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// 父级值
        /// </summary>
        [JsonIgnore]
        
        public TValue ParentValue { get; set; }
    }

    /// <summary>
    /// 选择项包含孩子节点
    /// </summary>
    /// <typeparam name="TLabel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SelectItemParent<TLabel, TValue> : SelectItem<TLabel, TValue>
    {
        public SelectItemParent()
        {

        }

        public SelectItemParent(TLabel label, TValue val, TValue parentVal) : base(label, val, parentVal)
        {

        }
        /// <summary>
        /// 孩子节点
        /// </summary>
        public IList<SelectItem<TLabel, TValue>> Children { get; set; }
    }
}
