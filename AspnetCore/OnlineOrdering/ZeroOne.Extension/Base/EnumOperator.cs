using System;
using System.ComponentModel;
using System.Reflection;

namespace ZeroOne.Extension
{
    /// <summary>
    /// 枚举相关的扩展方法
    /// </summary>
    public static class EnumOperator
    {
        /// <summary>
        /// 根据值得到中文备注
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="t"></param>
        /// <param name="value">枚举值</param>
        /// <returns></returns>
        public static string GetEnumDesc<T>(this T t) where T : struct
        {
            var e = t.GetType();
            FieldInfo[] fields = e.GetFields();
            for (int i = 1, count = fields.Length; i < count; i++)
            {
                if ((int)System.Enum.Parse(e, fields[i].Name) == Convert.ToInt32(t))
                {
                    DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])fields[i].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (EnumAttributes.Length > 0)
                    {
                        return EnumAttributes[0].Description;
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 将string值转换成Enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string source) where T : struct
        {
            T temp;
            if (Enum.TryParse<T>(source, out temp))
            {
                return temp;
            }
            else
            {
                throw new Exception("转换枚举类型失败");
            }
        }

        /// <summary>
        /// 将数值转换成枚举
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this int source) where T : struct
        {
            T temp;
            if (Enum.TryParse<T>(source.ToString(), out temp))
            {
                return temp;
            }
            else
            {
                throw new Exception("转换枚举类型失败");
            }
        }
    }
}
