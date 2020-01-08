using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using SqlSugar;

using ZeroOne.Extension;

namespace ZeroOne.Repository
{
    public static class RepExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="model"></param>
        /// <param name="client"></param>
        /// <typeparam name="TWhereModel"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IList<TModel> GetList<TWhereModel, TModel>(this IList<BaseRepModel> items, TWhereModel model, SqlSugarClient client)
        {
            //获取model类型
            var type = model.GetType();
            StringBuilder sbLambda = new StringBuilder();
            PropertyInfo property;
            if (items.Count > 0)
            {
                sbLambda.Append("x=>");
                foreach (var item in items)
                {
                    property = type.GetProperty(item.Key);
                    //判断值是否存在
                    var value = property.GetValue(model);
                    if (value == null)
                    {
                        continue;
                    }
                    sbLambda = sbLambda.GetStrLambda(item.Key, item.LogicalOperatorType, item.CompareOperator, property, nameof(model));
                }
            }
            if (sbLambda.Length > 0)
            {
                var lambdaExpression = sbLambda.ToString().ToExpression<Func<TModel, bool>>();
                return client.Queryable<TModel>().Where(lambdaExpression).ToList();
            }
            else
            {
                return new List<TModel>();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbLambda"></param>
        /// <param name="key"></param>
        /// <param name="logicalOperatorType"></param>
        /// <param name="compareOperator"></param>
        /// <param name="property"></param>
        /// <param name="whereModelName"></param>
        /// <returns></returns>
        private static StringBuilder GetStrLambda(this StringBuilder sbLambda, string key, ELogicalOperatorType logicalOperatorType,
        ECompareOperator compareOperator, PropertyInfo property, string whereModelName)
        {
            if (property != null)
            {
                string propTypeName;
                propTypeName = property.Name;
                if (property.PropertyType.BaseType is IEnumerable)
                {
                    sbLambda.Append($"{whereModelName}.{key}.Contains(x.{key})");
                }
                else
                {
                    switch (propTypeName)
                    {
                        case "Boolean":
                            sbLambda.Append($"x.{key}=={whereModelName}.{key}");
                            break;
                        case "String":
                            //比较运算
                            if (compareOperator == ECompareOperator.Contains)
                            {
                                sbLambda.Append($"x.{key}.Contains({whereModelName}.{key})");
                            }
                            else if (compareOperator == ECompareOperator.Equal)
                            {
                                sbLambda.Append($"x.{key}=={whereModelName}.{key}");
                            }
                            //sbLambda.Append($"")
                            break;
                        #region 数值类型生成字符串lambda表达式
                        case "Int32":
                        case "Single":
                        case "Double":
                        case "Decimal":
                            //比较运算
                            if (compareOperator == ECompareOperator.Equal)
                            {
                                sbLambda.Append($"x.{key}=={whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.Great)
                            {
                                sbLambda.Append($"x.{key}>{whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.GreatEqual)
                            {
                                sbLambda.Append($"x.{key}>={whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.Less)
                            {
                                sbLambda.Append($"x.{key}<{whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.LessEqual)
                            {
                                sbLambda.Append($"x.{key}<={whereModelName}.{key}");
                            }
                            break;
                        #endregion
                        #region  DateTime 生成字符串lambda表达式
                        case "DateTime":
                            //比较运算
                            if (compareOperator == ECompareOperator.Equal)
                            {
                                sbLambda.Append($"x.{key}=={whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.Great)
                            {
                                sbLambda.Append($"x.{key}>{whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.GreatEqual)
                            {
                                sbLambda.Append($"x.{key}>={whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.Less)
                            {
                                sbLambda.Append($"x.{key}<{whereModelName}.{key}");
                            }
                            else if (compareOperator == ECompareOperator.LessEqual)
                            {
                                sbLambda.Append($"x.{key}<={whereModelName}.{key}");
                            }
                            break;
                        #endregion
                        default:
                            break;
                    }
                }
                if (logicalOperatorType == ELogicalOperatorType.And)
                {
                    sbLambda.Append(" && ");
                }
                else if (logicalOperatorType == ELogicalOperatorType.Or)
                {
                    sbLambda.Append(" || ");
                }
            }
            return sbLambda;
        }
    }
}