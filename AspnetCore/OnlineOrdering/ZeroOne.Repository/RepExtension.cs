using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace ZeroOne.Repository
{
    public static class RepExtension
    {
        public static IList<BaseRepModel> GetList<TModel>(this IList<BaseRepModel> items, TModel model)
        {
            //获取model类型
            var type = model.GetType();
            //获取model所有公共属性
            var propertys = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            PropertyInfo property;
            foreach (var item in items)
            {
                property = propertys.First(t => t.Name.ToLower().Equals(item.Key.ToLower().Trim()));
                if (property != null)
                {
                    item.Value = property.GetValue(model);
                }
            }
            return items;
        }
    }
}