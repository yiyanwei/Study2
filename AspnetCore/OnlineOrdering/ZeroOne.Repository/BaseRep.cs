using System;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using SqlSugar;

using ZeroOne.Entity;
using ZeroOne.Extension;

namespace ZeroOne.Repository
{
    public abstract class BaseRep<TSearchModel, TModel>:IBaseRep<TSearchModel,TModel> where TSearchModel : BaseSearch where TModel : BaseEntity
    {
        private ISqlSugarClient _client;
        public BaseRep(ISqlSugarClient client)
        {
            this._client = client;
        }

        public async Task<TModel> GetModel(Guid id) 
        {
            return await  this._client.Queryable<TModel>().Where(t => t.Id == id).SingleAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="model"></param>
        /// <param name="client"></param>
        /// <typeparam name="TSearchModel"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public async Task<IList<TModel>> GetModelList(IList<BaseRepModel> items, TSearchModel model)
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
                    sbLambda = this.GetStrLambda(sbLambda,item.Key, item.LogicalOperatorType, item.CompareOperator, property, nameof(model));
                }
            }
            if (sbLambda.Length > 0)
            {
                string strLambda = sbLambda.ToString().TrimEnd(new char[] { '&', '|' });
                var lambdaExpression = strLambda.ToExpression<Func<TModel, bool>>();
                return await this._client.Queryable<TModel>().Where(lambdaExpression).ToListAsync();
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
        private  StringBuilder GetStrLambda(StringBuilder sbLambda, string key, ELogicalOperatorType logicalOperatorType,
        ECompareOperator compareOperator, PropertyInfo property, string whereModelName)
        {
            if (property != null)
            {
                string propTypeName;
                propTypeName = property.PropertyType.FullName;
                bool isAdd = true;
                if (property.PropertyType.BaseType is IEnumerable)
                {
                    sbLambda.Append($"{whereModelName}.{key}.Contains(x.{key})");
                }
                else
                {
                    if (propTypeName.Contains("Boolean"))
                    {
                        sbLambda.Append($"x.{key}=={whereModelName}.{key}");
                    }
                    else if (propTypeName.Contains("String"))
                    {
                        //比较运算
                        if (compareOperator == ECompareOperator.Contains)
                        {
                            sbLambda.Append($"x.{key}.Contains({whereModelName}.{key})");
                        }
                        else if (compareOperator == ECompareOperator.Equal)
                        {
                            sbLambda.Append($"x.{key}=={whereModelName}.{key}");
                        }
                        else
                        {
                            isAdd = false;
                        }
                    }
                    else if (propTypeName.Contains("Int32")|| propTypeName.Contains("Single") || propTypeName.Contains("Double") || propTypeName.Contains("Decimal"))
                    {
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
                        else
                        {
                            isAdd = false;
                        }
                    }
                    else if (propTypeName.Contains("DateTime"))
                    {
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
                        else
                        {
                            isAdd = false;
                        }
                    }
                    else
                    {
                        isAdd = false;
                    }
                }
                if (isAdd)
                {
                    if (logicalOperatorType == ELogicalOperatorType.And)
                    {
                        sbLambda.Append("&&");
                    }
                    else if (logicalOperatorType == ELogicalOperatorType.Or)
                    {
                        sbLambda.Append("||");
                    }
                }                
            }
            return sbLambda;
        }
    }
}