using System;
using System.Collections.Generic;
using System.Linq;
using ZeroOne.Extension.Model;

namespace ZeroOne.Extension
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 递归获取父子节点数据
        /// </summary>
        /// <typeparam name="TLabel">文本类型</typeparam>
        /// <typeparam name="TValue">值类型</typeparam>
        /// <param name="source">源数据（所有）</param>
        /// <param name="parentItem">父节点</param>
        /// <param name="target">目标数据集</param>
        public static void SelectRecursionCall<TLabel, TValue>(this IList<SelectItem<TLabel, TValue>> source, SelectItem<TLabel, TValue> parentItem, IList<SelectItem<TLabel, TValue>> target)
            //where TValue : IComparable, IComparable<TValue>, IEquatable<TValue>
        {
            if (source == null || source.Count() <= 0)
            {
                throw new Exception("源数据是空数据集");
            }
            //获取所有父对象值为当前对象的值
            var result = source.Where(t => t.ParentValue.Equals(parentItem.Value)).ToList();
            foreach (var item in result)
            {
                var count = source.Where(t => t.ParentValue.Equals(item.Value))?.Count();
                if (count.HasValue && count.Value > 0)
                {
                    var temp = new SelectItemParent<TLabel, TValue>(item.Label, item.Value, item.ParentValue);
                    temp.Children = new List<SelectItem<TLabel, TValue>>();
                    target.Add(temp);
                    SelectRecursionCall(source, item, temp.Children);
                }
                else
                {
                    var temp = new SelectItem<TLabel, TValue>(item.Label, item.Value, item.ParentValue);
                    target.Add(temp);
                }
            }
        }

        /// <summary>
        /// 将集合对象转换成IEnumerable<object>对象
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="collection">源集合对象</param>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public static IEnumerable<object> ChangeCollectionDataType<T>(this IEnumerable<T> collection, Type targetType)
        {
            return collection.Select(t => t.ChangeDataType(targetType));
        }
    }
}
