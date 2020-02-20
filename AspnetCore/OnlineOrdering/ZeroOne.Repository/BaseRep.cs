using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using SqlSugar;

using ZeroOne.Entity;
using ZeroOne.Extension;


namespace ZeroOne.Repository
{
    public abstract class BaseRep<TSearchModel, TModel, TPrimaryKey> : IBaseRep<TSearchModel, TModel, TPrimaryKey> where TSearchModel : BaseSearch where TModel : BaseEntity<TPrimaryKey>, IRowVersion, new()
    {

        private static readonly object lockObj = new object();
        private ISqlSugarClient _client;
        public BaseRep(ISqlSugarClient client)
        {
            this._client = client;
        }

        public async Task<TModel> GetModel(TPrimaryKey id)
        {
            var query = this._client.Queryable<TModel>();
            return await query.Where(GetBaseWhereExpression(id)).Where(t => SqlFunc.IsNull(t.IsDeleted, false) == false).SingleAsync();
        }

        public T IFNULL<T>(T thisValue, T ifNullValue)
        {
            throw new NotSupportedException("Can only be used in expressions");
        }

        private Expression<Func<TModel, bool>> GetBaseWhereExpression(TPrimaryKey id)
        {

            ParameterExpression paramExp = Expression.Parameter(typeof(TModel), "t");
            #region 主键值相等
            //属性表达式
            Expression propExp = Expression.Property(paramExp, nameof(BaseEntity<TPrimaryKey>.Id));
            //值表达式
            Expression valExp = Expression.Constant(id);
            //相等表达式
            Expression equalExp = Expression.Equal(propExp, valExp);
            #endregion
            #region 未删除
            //SqlFunc.IsNull<bool>()
            Expression delPropExp = Expression.Property(paramExp, nameof(IDeleted.IsDeleted));
            Expression delValExp = Expression.Constant(false);
            var method = this.GetType().BaseType.GetMethod(nameof(this.IFNULL), BindingFlags.Instance | BindingFlags.Public);
            ////Expression callExp = Expression.Call(method, delPropExp, delValExp);
            //var configurationType = typeof(Func<>).MakeGenericType(typeof(bool));
            //var configurationInstance = Activator.CreateInstance(configurationType);
            //var configParameter = Expression.Parameter(configurationType, "config");

            //var registrarParameter = Expression.Parameter(typeof(BaseRep<TSearchModel, TModel, TPrimaryKey>), "registrar");

            //var callExp = Expression.Call(method, delPropExp, delValExp);
            //var callExp = Expression.Call(this.GetType(), "IFNULL", new[] { typeof(bool) }, delPropExp, delValExp);
            //Expression delEqualExp = Expression.Equal(callExp, delValExp);

            //SqlFunc.IsNull()

            #endregion
            Expression<Func<TModel, bool>> lambda = Expression.Lambda<Func<TModel, bool>>(equalExp, paramExp);
            return lambda;
        }



        /// <summary>
        /// 添加数据实体
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
        public async Task<bool> AddModel(TModel model)
        {
            if (!model.RowVersion.HasValue)
            {
                model.RowVersion = Guid.Empty;
            }
            int affectedRows = await this._client.Insertable<TModel>(model).ExecuteCommandAsync();
            return affectedRows > 0;
        }

        public async Task<bool> DeleteModel(TPrimaryKey id, Guid rowVersion)
        {
            //var searchModel = await this._client.Queryable<TModel>().Where(it => (BaseEntity<TPrimaryKey>)it == new BaseEntity<TPrimaryKey>() { Id = id } && it.RowVersion == rowVersion).FirstAsync();
            //if (searchModel == null)
            //{
            //    throw new DBConcurrencyException($"id:{id} 数据已更新，请刷新后重试！");
            //}

            ConcurrentProcess(new TModel() { Id = id, RowVersion = rowVersion });
            Guid newGuid = Guid.NewGuid();
            int affecedRows = await this._client.Updateable<TModel>()
                .SetColumns(t => new TModel { IsDeleted = true, RowVersion = newGuid })
                .Where(t => (BaseEntity<TPrimaryKey>)t == new BaseEntity<TPrimaryKey>() { Id = id }).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        public async Task<bool> UpdateModel(TModel model)
        {
            //var searchModel = await this._client.Queryable<TModel>().Where(it => (BaseEntity<TPrimaryKey>)it == (BaseEntity<TPrimaryKey>)model && it.RowVersion == model.RowVersion).FirstAsync();
            //if (searchModel == null)
            //{
            //    throw new DBConcurrencyException($"id:{model.Id} 数据已更新，请刷新后重试！");
            //}
            ConcurrentProcess(model);
            model.RowVersion = Guid.NewGuid();
            int affecedRows = await this._client.Updateable(model).IgnoreColumns(true).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        /// <summary>
        /// 判断是否并发
        /// </summary>
        private async void ConcurrentProcess(TModel model)
        {

            var searchModel = await this._client.Queryable<TModel>().Where(it => (BaseEntity<TPrimaryKey>)it == (BaseEntity<TPrimaryKey>)model && it.IsDeleted == false).FirstAsync();
            if (searchModel != null)
            {
                if (searchModel.RowVersion != model.RowVersion)
                {
                    throw new DBConcurrencyException($"{nameof(model.Id)}:{model.Id} 数据已更新，请刷新后重试！");
                }
            }
            else
            {
                throw new Exception($"{nameof(model.Id)}:{model.Id},未查询到该数据或已被删除");
            }
        }

        /// <summary>
        /// 根据不同的IEnumerable的泛型类型参数的类型调用不同的Contains方法
        /// </summary>
        /// <param name="type">泛型类型参数的类型</param>
        /// <returns></returns>
        private MethodInfo GetMethodByGenericArgType(Type type)
        {
            MethodInfo method = null;
            if (type != null)
            {
                if (type == typeof(int))
                {
                    method = (methodof<Func<IEnumerable<int>, int, bool>>)Enumerable.Contains;
                }
                else if (type == typeof(decimal))
                {
                    method = (methodof<Func<IEnumerable<decimal>, decimal, bool>>)Enumerable.Contains;
                }
                else if (type == typeof(float))
                {
                    method = (methodof<Func<IEnumerable<float>, float, bool>>)Enumerable.Contains;
                }
                else if (type == typeof(long))
                {
                    method = (methodof<Func<IEnumerable<long>, long, bool>>)Enumerable.Contains;
                }
                else if (type == typeof(double))
                {
                    method = (methodof<Func<IEnumerable<double>, double, bool>>)Enumerable.Contains;
                }
                else if (type == typeof(DateTime))
                {
                    method = (methodof<Func<IEnumerable<DateTime>, DateTime, bool>>)Enumerable.Contains;
                }
                else if (type == typeof(string))
                {
                    method = (methodof<Func<IEnumerable<string>, string, bool>>)Enumerable.Contains;
                }
            }
            return method;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private Expression GetExpressionResult(IList<BaseRepModel> items, TSearchModel model, ParameterExpression paramExpr)
        {

            //返回model的类型
            var modelType = typeof(TModel);
            //获取model类型
            var searchModelType = model.GetType();

            //lambda类型参数别名
            //ParameterExpression paramExpr = Expression.Parameter(modelType, "it");

            PropertyInfo property;
            string propTypeName = string.Empty;

            Expression left = null;
            Expression right = null;
            Expression childExpression = null;
            Expression tempExpression = null;
            //ConditionalExpression 
            foreach (var item in items)
            {
                property = searchModelType.GetProperty(item.Key);
                //判断值是否存在
                var value = property.GetValue(model);
                if (value == null)
                {
                    continue;
                }
                propTypeName = property.PropertyType.FullName;
                //判断是泛型IEnumerable<>
                if (property.PropertyType != typeof(string) && property.PropertyType.GetInterfaces().Any(x => typeof(IEnumerable<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                {
                    PropertyInfo compareProp = modelType.GetProperty(item.Key);

                    MethodInfo containsMethod = this.GetMethodByGenericArgType(property.PropertyType.GetGenericArguments()[0]);
                    //判断属性值是否为Nullable类型值
                    //var parentTypes = compareProp.PropertyType.GetGenericTypeDefinition();
                    if (compareProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var valProp = compareProp.PropertyType.GetProperty("Value");
                        var valExpression = Expression.Property(Expression.Property(paramExpr, compareProp), valProp);
                        var searchPropertyExpression = Expression.Property(Expression.Constant(model, searchModelType), property);
                        childExpression = Expression.Call(containsMethod, searchPropertyExpression, valExpression);
                    }
                    else
                    {
                        var searchPropertyExpression = Expression.Property(Expression.Constant(model, searchModelType), property);
                        childExpression = Expression.Call(containsMethod, searchPropertyExpression, Expression.Property(paramExpr, compareProp));
                    }
                }
                else
                {
                    //等于运算各种基础类型一致
                    if (item.CompareOperator == ECompareOperator.Equal)
                    {
                        left = Expression.Property(paramExpr, modelType.GetProperty(item.Key));
                        right = Expression.Constant(value, property.PropertyType);
                        childExpression = Expression.Equal(left, right);
                    }
                    else
                    {
                        if (propTypeName.Contains("String"))
                        {
                            if (item.CompareOperator == ECompareOperator.Contains)
                            {
                                string strVal = ((string)value).Trim();
                                //常量表达式
                                Expression valExpression = Expression.Constant(strVal, strVal.GetType());
                                //调用的函数
                                MethodInfo contains = (methodof<Func<string, bool>>)strVal.Contains;
                                childExpression = Expression.Call(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), contains, valExpression);
                            }
                        }
                        else if (propTypeName.Contains("Int32") || propTypeName.Contains("Single")
                                || propTypeName.Contains("Double") || propTypeName.Contains("Decimal")
                                || propTypeName.Contains("DateTime"))
                        {
                            if (item.CompareOperator == ECompareOperator.GreaterThan)
                            {
                                childExpression = Expression.GreaterThan(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                            }
                            else if (item.CompareOperator == ECompareOperator.GreaterThanOrEqual)
                            {
                                childExpression = Expression.GreaterThanOrEqual(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                            }
                            else if (item.CompareOperator == ECompareOperator.LessThan)
                            {
                                childExpression = Expression.LessThan(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                            }
                            else if (item.CompareOperator == ECompareOperator.LessThanOrEqual)
                            {
                                childExpression = Expression.LessThanOrEqual(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                            }
                        }
                    }
                }

                //表达式是否为空
                if (childExpression != null)
                {
                    if (tempExpression == null)
                    {
                        tempExpression = childExpression;
                    }
                    else
                    {
                        if (item.LogicalOperatorType == ELogicalOperatorType.And)
                        {
                            tempExpression = Expression.AndAlso(tempExpression, childExpression);
                        }
                        else if (item.LogicalOperatorType == ELogicalOperatorType.Or)
                        {
                            tempExpression = Expression.OrElse(tempExpression, childExpression);
                        }
                    }
                }
            }
            return tempExpression;
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

            //返回model的类型
            var modelType = typeof(TModel);

            //lambda类型参数别名
            ParameterExpression paramExpr = Expression.Parameter(modelType, "it");

            //PropertyInfo property;


            if (items.Count > 0)
            {
                Expression tempExpression = this.GetExpressionResult(items, model, paramExpr);
                #region 注释代码
                //Expression left;
                //Expression right;
                //Expression childExpression = null;
                //Expression totalExpression = null;

                //string propTypeName = string.Empty;
                ////ConditionalExpression 
                //foreach (var item in items)
                //{
                //    property = searchModelType.GetProperty(item.Key);
                //    //判断值是否存在
                //    var value = property.GetValue(model);
                //    if (value == null)
                //    {
                //        continue;
                //    }
                //    propTypeName = property.PropertyType.FullName;
                //    //判断是泛型IEnumerable<>
                //    if (property.PropertyType != typeof(string) && property.PropertyType.GetInterfaces().Any(x => typeof(IEnumerable<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                //    {
                //        PropertyInfo compareProp = modelType.GetProperty(item.Key);

                //        MethodInfo containsMethod = this.GetMethodByGenericArgType(property.PropertyType.GetGenericArguments()[0]);
                //        //判断属性值是否为Nullable类型值
                //        var parentTypes = compareProp.PropertyType.GetGenericTypeDefinition();
                //        if (compareProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                //        {
                //            var valProp = compareProp.PropertyType.GetProperty("Value");
                //            var valExpression = Expression.Property(Expression.Property(paramExpr, compareProp), valProp);
                //            var searchPropertyExpression = Expression.Property(Expression.Constant(model, searchModelType), property);
                //            childExpression = Expression.Call(containsMethod, searchPropertyExpression, valExpression);
                //        }
                //        else
                //        {
                //            var searchPropertyExpression = Expression.Property(Expression.Constant(model, searchModelType), property);
                //            childExpression = Expression.Call(containsMethod, searchPropertyExpression, Expression.Property(paramExpr, compareProp));
                //        }
                //    }
                //    else
                //    {
                //        //等于运算各种基础类型一致
                //        if (item.CompareOperator == ECompareOperator.Equal)
                //        {
                //            left = Expression.Property(paramExpr, modelType.GetProperty(item.Key));
                //            right = Expression.Constant(value, property.PropertyType);
                //            childExpression = Expression.Equal(left, right);
                //        }
                //        else
                //        {
                //            if (propTypeName.Contains("String"))
                //            {
                //                if (item.CompareOperator == ECompareOperator.Contains)
                //                {
                //                    string strVal = ((string)value).Trim();
                //                    //常量表达式
                //                    Expression valExpression = Expression.Constant(strVal, strVal.GetType());
                //                    //调用的函数
                //                    MethodInfo contains = (methodof<Func<string, bool>>)strVal.Contains;
                //                    childExpression = Expression.Call(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), contains, valExpression);
                //                }
                //            }
                //            else if (propTypeName.Contains("Int32") || propTypeName.Contains("Single")
                //                    || propTypeName.Contains("Double") || propTypeName.Contains("Decimal")
                //                    || propTypeName.Contains("DateTime"))
                //            {
                //                if (item.CompareOperator == ECompareOperator.GreaterThan)
                //                {
                //                    childExpression = Expression.GreaterThan(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                //                }
                //                else if (item.CompareOperator == ECompareOperator.GreaterThanOrEqual)
                //                {
                //                    childExpression = Expression.GreaterThanOrEqual(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                //                }
                //                else if (item.CompareOperator == ECompareOperator.LessThan)
                //                {
                //                    childExpression = Expression.LessThan(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                //                }
                //                else if (item.CompareOperator == ECompareOperator.LessThanOrEqual)
                //                {
                //                    childExpression = Expression.LessThanOrEqual(Expression.Property(paramExpr, modelType.GetProperty(item.Key)), Expression.Constant(value, value.GetType()));
                //                }
                //            }
                //        }
                //    }

                //    //表达式是否为空
                //    if (childExpression != null)
                //    {
                //        if (totalExpression == null)
                //        {
                //            totalExpression = childExpression;
                //        }
                //        else
                //        {
                //            if (item.LogicalOperatorType == ELogicalOperatorType.And)
                //            {
                //                totalExpression = Expression.AndAlso(totalExpression, childExpression);
                //            }
                //            else if (item.LogicalOperatorType == ELogicalOperatorType.Or)
                //            {
                //                totalExpression = Expression.OrElse(totalExpression, childExpression);
                //            }
                //        }
                //    }                
                //}
                #endregion
                if (tempExpression != null)
                {
                    var lambdaExpression = Expression.Lambda<Func<TModel, bool>>(tempExpression, new ParameterExpression[] { paramExpr });
                    var query = this._client.Queryable<TModel>().Where(lambdaExpression);
                    var keyValues = query.ToSql();
                    return await query.ToListAsync();
                }
            }
            return new List<TModel>();
        }

        public async Task<IList<TModel>> GetModelList(List<Tuple<IList<BaseRepModel>, ELogicalOperatorType>> groups, TSearchModel model)
        {
            if (groups != null && groups.Count > 0)
            {
                //返回model的类型
                var modelType = typeof(TModel);

                //lambda类型参数别名
                ParameterExpression paramExpr = Expression.Parameter(modelType, "it");

                //所有表达式合并
                Expression totalExpression = null;
                foreach (var group in groups)
                {
                    //判断List的组成员是否为空并且判断组成员下面的List集合是否为空
                    if (group != null && group.Item1 != null && group.Item1.Count > 0)
                    {
                        Expression tempExpression = this.GetExpressionResult(group.Item1, model, paramExpr);

                        if (tempExpression != null)
                        {
                            if (totalExpression == null)
                            {
                                totalExpression = tempExpression;
                            }
                            else
                            {
                                //判断是否与运算
                                if (group.Item2 == ELogicalOperatorType.And)
                                {
                                    totalExpression = Expression.AndAlso(totalExpression, tempExpression);
                                }
                                //判断是否或运算
                                else if (group.Item2 == ELogicalOperatorType.Or)
                                {
                                    totalExpression = Expression.OrElse(totalExpression, tempExpression);
                                }
                            }
                        }
                    }
                }
                //判断最终的表达式是否为空
                if (totalExpression != null)
                {
                    var lambdaExpression = Expression.Lambda<Func<TModel, bool>>(totalExpression, new ParameterExpression[] { paramExpr });
                    return await this._client.Queryable<TModel>().Where(lambdaExpression).ToListAsync();
                }
            }
            return new List<TModel>();
        }
    }
}