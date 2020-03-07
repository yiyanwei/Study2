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

    public abstract class BaseRep<TEntity, TPrimaryKey> : IBaseRep<TEntity, TPrimaryKey>
        where TEntity : BaseEntity<TPrimaryKey>, IRowVersion, new()
    {


        protected ISugarQueryable<TEntity> Queryable { get; set; }

        private static readonly object lockObj = new object();
        protected ISqlSugarClient _client;
        public BaseRep(ISqlSugarClient client)
        {
            this._client = client;
            this.Queryable = _client.Queryable<TEntity>();
        }

        /// <summary>
        /// 如果需要格式化结果，则重写该方法
        /// </summary>
        /// <typeparam name="TResult">类型参数</typeparam>
        /// <param name="result">格式化的结果</param>
        /// <returns></returns>
        public virtual TResult FormatResult<TResult>(TResult result) where TResult : class, IResult, new()
        {
            //result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(t => t.PropertyType.IsClass);
            return result;
        }

        /// <summary>
        /// 获取所有未删除的数据
        /// </summary>
        /// <returns></returns>
        protected async Task<IList<TEntity>> GetListAsync()
        {
            return await this.Queryable.Where(t => (bool)SqlFunc.IsNull(t.IsDeleted, false) == false).ToListAsync();
        }

        /// <summary>
        /// 根据Id获取对应的结果对象
        /// </summary>
        /// <typeparam name="TResult">结果对象类型</typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TResult> GetResultByIdAsync<TResult>(TPrimaryKey id) where TResult : class, IResult, new()
        {
            var query = this._client.Queryable<TEntity>();
            var result = await query.Where(t => t.Id.Equals(id) && SqlFunc.IsNull(t.IsDeleted, false) == false).Select(t => t.Map<TResult>()).FirstAsync();
            return this.FormatResult(result);
        }

        /// <summary>
        /// 根据id返回数据库对象
        /// </summary>
        /// <param name="id">对象id</param>
        /// <returns></returns>
        public async Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            var query = this._client.Queryable<TEntity>();
            return await query.Where(t => t.Id.Equals(id) && SqlFunc.IsNull(t.IsDeleted, false) == false).FirstAsync();
        }

        public TEntity GetEntityById(TPrimaryKey id)
        {
            var query = this._client.Queryable<TEntity>();
            return query.Where(t => t.Id.Equals(id) && SqlFunc.IsNull(t.IsDeleted, false) == false).First();
        }

        private Expression<Func<TEntity, bool>> GetBaseWhereExpression(TPrimaryKey id)
        {

            ParameterExpression paramExp = Expression.Parameter(typeof(TEntity), "t");
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
            //Expression delPropExp = Expression.Property(paramExp, nameof(IDeleted.IsDeleted));
            //Expression delValExp = Expression.Constant(false);

            //var genenicType = typeof(int);
            //var methodExp = Expression.Call(Expression.Constant(this), nameof(IFNULL), new Type[] { genenicType }, Expression.Constant(1), Expression.Constant(2));
            //Expression<Func<int>> expression = Expression.Lambda<Func<int>>(methodExp);
            //var result = expression.Compile()();

            #endregion
            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(equalExp, paramExp);
            return lambda;
        }



        /// <summary>
        /// 添加数据实体
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        public async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            if (!entity.RowVersion.HasValue)
            {
                entity.RowVersion = Guid.Empty;
            }
            return await this._client.Insertable<TEntity>(entity).ExecuteReturnEntityAsync();
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="rowVersion">版本号</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(TPrimaryKey id, Guid rowVersion, string userId = null)
        {
            ConcurrentProcess(new TEntity() { Id = id, RowVersion = rowVersion });
            Guid newGuid = Guid.NewGuid();
            int affecedRows = await this._client.Updateable<TEntity>()
                .SetColumns(t => new TEntity { IsDeleted = true, DeleterUserId = userId, DeletionTime = DateTime.Now, RowVersion = newGuid })
                .Where(t => t.Id.Equals(id) && SqlFunc.IsNull(t.IsDeleted, false) == false).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        /// <summary>
        /// 更新对象（不为空）
        /// </summary>
        /// <param name="entity">更新对象</param>
        /// <returns></returns>
        public async Task<bool> UpdateEntityNotNullAsync(TEntity entity)
        {
            ConcurrentProcess(entity);
            entity.RowVersion = Guid.NewGuid();
            int affecedRows = await this._client.Updateable(entity).IgnoreColumns(true).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        /// <summary>
        /// 更新对象（所有）
        /// </summary>
        /// <param name="entity">更新对象</param>
        /// <returns></returns>
        public async Task<bool> UpdateEntityAsync(TEntity entity)
        {
            ConcurrentProcess(entity);
            entity.RowVersion = Guid.NewGuid();
            int affecedRows = await this._client.Updateable(entity).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        /// <summary>
        /// 判断是否并发
        /// </summary>
        private async void ConcurrentProcess(TEntity entity)
        {
            //GetBaseWhereExpression(model.Id)
            var searchModel = await this._client.Queryable<TEntity>().Where(t => t.Id.Equals(entity.Id) && SqlFunc.IsNull(t.IsDeleted, false) == false).FirstAsync();
            if (searchModel != null)
            {
                if (searchModel.RowVersion != entity.RowVersion)
                {
                    throw new DBConcurrencyException($"{nameof(entity.Id)}:{entity.Id} 数据已更新，请刷新后重试！");
                }
            }
            else
            {
                throw new Exception($"{nameof(entity.Id)}:{entity.Id},未查询到该数据或已被删除");
            }
        }

        /// <summary>
        /// 根据不同的IEnumerable的泛型类型参数的类型调用不同的Contains方法
        /// </summary>
        /// <param name="type">泛型类型参数的类型</param>
        /// <returns></returns>
        protected MethodInfo GetMethodByGenericArgType(Type type)
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
    }

    public abstract class BaseRep<TEntity, TPrimaryKey, TSearch> : BaseRep<TEntity, TPrimaryKey>, IBaseRep<TEntity, TPrimaryKey, TSearch>
        where TSearch : BaseSearch
        where TEntity : BaseEntity<TPrimaryKey>, IRowVersion, new()
    {
        /// <summary>
        /// 如果需要格式化结果，则重写该方法
        /// </summary>
        /// <typeparam name="TResult">类型参数</typeparam>
        /// <param name="result">格式化的结果</param>
        /// <returns></returns>

        public BaseRep(ISqlSugarClient client) : base(client)
        {

        }

        /// <summary>
        /// 获取查询最终表达式
        /// </summary>
        /// <param name="items">数据库查询的运算对象集合</param>
        /// <param name="model"></param>
        /// <returns></returns>
        private Expression GetExpressionResult(IList<BaseRepModel> items, TSearch model, ParameterExpression paramExpr)
        {

            //返回model的类型
            var modelType = typeof(TEntity);
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
                //判断是否实现了IEnumerable<>的集合对象
                if (property.PropertyType != typeof(string) && property.PropertyType.GetInterfaces().Any(x => typeof(IEnumerable<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                {
                    PropertyInfo compareProp = modelType.GetProperty(item.Key);

                    MethodInfo containsMethod = this.GetMethodByGenericArgType(property.PropertyType.GetGenericArguments()[0]);
                    //判断属性值是否为Nullable类型值
                    if (compareProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var valProp = compareProp.PropertyType.GetProperty(nameof(Nullable<int>.Value));
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
                        if (item.LogicalOperatorType == ELogicalOperator.And)
                        {
                            tempExpression = Expression.AndAlso(tempExpression, childExpression);
                        }
                        else if (item.LogicalOperatorType == ELogicalOperator.Or)
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
        /// <param name="items">数据库基础查询对象集合</param>
        /// <param name="model"></param>
        /// <param name="client"></param>
        /// <typeparam name="TSearchModel">查询对象</typeparam>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetEntityListAsync(IList<BaseRepModel> items, TSearch model)
        {

            //返回model的类型
            var modelType = typeof(TEntity);

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
                    var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(tempExpression, new ParameterExpression[] { paramExpr });
                    var query = this._client.Queryable<TEntity>().Where(lambdaExpression);
                    var keyValues = query.ToSql();
                    return await query.ToListAsync();
                }
            }
            return new List<TEntity>();
        }

        /// <summary>
        /// 获取数据对象集合
        /// </summary>
        /// <param name="groups">分组查询条件</param>
        /// <param name="model">查询对象</param>
        /// <returns></returns>
        public async Task<IList<TEntity>> GetEntityListByWhereGroupAsync(List<Tuple<IList<BaseRepModel>, ELogicalOperator>> groups, TSearch model)
        {
            if (groups != null && groups.Count > 0)
            {
                //返回model的类型
                var modelType = typeof(TEntity);

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
                                if (group.Item2 == ELogicalOperator.And)
                                {
                                    totalExpression = Expression.AndAlso(totalExpression, tempExpression);
                                }
                                //判断是否或运算
                                else if (group.Item2 == ELogicalOperator.Or)
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
                    var lambdaExpression = Expression.Lambda<Func<TEntity, bool>>(totalExpression, new ParameterExpression[] { paramExpr });
                    return await this._client.Queryable<TEntity>().Where(lambdaExpression).ToListAsync();
                }
            }
            return new List<TEntity>();
        }


        private object GetSelectResult<TResult, TSearchResult>(BaseSearch search)
            where TResult : IResult, new()
            where TSearchResult : BaseSearchResult<TResult>
        {
            Dictionary<Type, IList<PropertyInfo>> dicTypeProps = new Dictionary<Type, IList<PropertyInfo>>();
            //当前对象的类型，也就是主表对象类型
            var entityType = typeof(TEntity);
            //获取所有配置了关联信息的类型对象
            var resultType = typeof(TResult);
            //配置未关联属性
            var totalProps = resultType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var myProps = totalProps.Where(t => t.GetCustomAttribute<MainTableRelationAttribute>() == null).ToList();
            dicTypeProps.Add(entityType, myProps);

            //配置关联表属性
            var joinTableProps = totalProps.Where(t => t.GetCustomAttribute<MainTableRelationAttribute>() != null).OrderBy(t => t.GetCustomAttribute<MainTableRelationAttribute>().JoinType);

            //Tuple<PropertyInfo, PropertyInfo>  Item1:TSearchResult的当前TEntity的属性，Item2：目标类型的属性
            //Dictionary<Tuple<Type,Type>, Tuple<EJoinType, IList<Tuple<PropertyInfo, PropertyInfo>>>> dicJoinTables = new Dictionary<Tuple<Type, Type>, Tuple<EJoinType, IList<Tuple<PropertyInfo, PropertyInfo>>>>();

            //Dictionary<Type, ParameterExpression> dicTypeParams = new Dictionary<Type, ParameterExpression>();
            IList<Type> types = new List<Type>();
            //添加默认的当前Entity
            types.Add(entityType);

            Dictionary<Type, Tuple<EJoinType, IList<KeyValuePair<PropertyInfo, PropertyInfo>>, IList<JoinTableRelationAttribute>>> dicTypePropMappings =
                new Dictionary<Type, Tuple<EJoinType, IList<KeyValuePair<PropertyInfo, PropertyInfo>>, IList<JoinTableRelationAttribute>>>();
            foreach (var prop in joinTableProps)
            {

                var mainTableRelation = prop.GetCustomAttribute<MainTableRelationAttribute>();
                if (!types.Contains(mainTableRelation.EntityType))
                {
                    types.Add(mainTableRelation.EntityType);
                }
                if (!dicTypePropMappings.Keys.Contains(mainTableRelation.EntityType))
                {
                    dicTypePropMappings.Add(mainTableRelation.EntityType, new Tuple<EJoinType, IList<KeyValuePair<PropertyInfo, PropertyInfo>>, IList<JoinTableRelationAttribute>>(
                    mainTableRelation.JoinType, new List<KeyValuePair<PropertyInfo, PropertyInfo>>(), new List<JoinTableRelationAttribute>()));
                }

                IList<KeyValuePair<PropertyInfo, PropertyInfo>> destPropList = dicTypePropMappings[mainTableRelation.EntityType].Item2;
                IList<JoinTableRelationAttribute> joinTableRelList = dicTypePropMappings[mainTableRelation.EntityType].Item3;
                string destPropName = prop.Name;
                if (!string.IsNullOrEmpty(mainTableRelation.DestPropName))
                {
                    destPropName = mainTableRelation.DestPropName;
                }
                //var currentProp = prop;
                var destProp = mainTableRelation.EntityType.GetProperty(destPropName, BindingFlags.Instance | BindingFlags.Public);
                if (!(destPropList.Where(t => t.Value == destProp)?.Count() > 0))
                {
                    destPropList.Add(new KeyValuePair<PropertyInfo, PropertyInfo>(prop, destProp));
                }


                //获取当前属性所有的关联关系
                var joinTableRels = prop.GetCustomAttributes<JoinTableRelationAttribute>();
                foreach (var item in joinTableRels)
                {
                    if (!types.Contains(item.DestEntityType))
                    {
                        types.Add(item.DestEntityType);
                    }

                    var currProp = mainTableRelation.EntityType.GetProperty(item.PropName, BindingFlags.Instance | BindingFlags.Public);
                    var destRelProp = item.DestEntityType.GetProperty(item.DestRelPropName, BindingFlags.Instance | BindingFlags.Public);
                    //必须要保证有一个类型的属性存在
                    if (currProp != null || destRelProp != null)
                    {
                        if (!(joinTableRelList.Where(t => t.Property == currProp && t.DestEntityType == item.DestEntityType && t.DestProperty == destRelProp)?.Count() > 0))
                        {
                            item.Property = currProp;
                            item.DestProperty = destProp;
                            joinTableRelList.Add(item);
                        }
                        else if (currProp != null && item.PropValue != null)
                        {
                            item.Property = currProp;
                            joinTableRelList.Add(item);
                        }
                        else if (destRelProp != null && item.DestPropValue != null)
                        {
                            item.DestProperty = destProp;
                            joinTableRelList.Add(item);
                        }
                    }

                }
            }

            //生成类型对象参数表达式
            var paramExps = types.Select((t, i) => new KeyValuePair<Type, ParameterExpression>(t, Expression.Parameter(t, $"t{i + 1}"))).ToArray();
            //结果类型表达式参数
            var rExpParam = Expression.Parameter(resultType, "r");

            //IList<Type> joinTypes = new List<Type>();
            //IList<Expression> joinExpList = new List<Expression>();

            IList<MemberBinding> memberBindings = new List<MemberBinding>();
            Expression bindingExp = null;

            IList<KeyValuePair<Type, Expression>> joinList = new List<KeyValuePair<Type, Expression>>();
            foreach (var item in dicTypePropMappings)
            {

                Expression joinFieldExp = null;
                if (item.Value.Item1 == EJoinType.InnerJoin)
                {
                    //joinExpList.Add(Expression.Constant(JoinType.Inner));
                    joinFieldExp = Expression.Constant(JoinType.Inner);
                }
                else if (item.Value.Item1 == EJoinType.LeftJoin)
                {
                    //joinExpList.Add(Expression.Constant(JoinType.Left));
                    joinFieldExp = Expression.Constant(JoinType.Left);
                }
                else if (item.Value.Item1 == EJoinType.RightJoin)
                {
                    //joinExpList.Add(Expression.Constant(JoinType.Right));
                    joinFieldExp = Expression.Constant(JoinType.Right);
                }
                else
                {
                    throw new Exception("");
                }
                //join类型
                joinList.Add(new KeyValuePair<Type, Expression>(typeof(JoinType), joinFieldExp));

                Expression tempTotal = null;
                foreach (var joinItem in item.Value.Item3)
                {

                    Expression left = null;
                    Expression right = null;

                    //两个属性都不为空
                    if (joinItem.Property != null && joinItem.DestProperty != null)
                    {
                        left = Expression.Property(paramExps.FirstOrDefault(t => t.Key == item.Key).Value, joinItem.Property);
                        right = Expression.Property(paramExps.FirstOrDefault(t => t.Key == joinItem.DestEntityType).Value, joinItem.DestProperty);
                    }
                    else if (joinItem.Property != null && joinItem.PropValue != null)
                    {
                        left = Expression.Property(paramExps.FirstOrDefault(t => t.Key == item.Key).Value, joinItem.Property);
                        right = Expression.Constant(joinItem.PropValue);
                    }
                    else if (joinItem.DestProperty != null && joinItem.DestPropValue != null)
                    {
                        left = Expression.Property(paramExps.FirstOrDefault(t => t.Key == joinItem.DestEntityType).Value, joinItem.DestProperty);
                        right = Expression.Constant(joinItem.PropValue);
                    }
                    else
                    {
                        throw new Exception("");
                    }

                    Expression compareExp = null;
                    //比较运算
                    if (joinItem.CompareOperator == ECompareOperator.Equal)
                    {
                        compareExp = Expression.Equal(left, right);
                    }
                    else if (joinItem.CompareOperator == ECompareOperator.NotEqual)
                    {
                        compareExp = Expression.NotEqual(left, right);
                    }
                    else if (joinItem.CompareOperator == ECompareOperator.GreaterThan)
                    {
                        compareExp = Expression.GreaterThan(left, right);
                    }
                    else if (joinItem.CompareOperator == ECompareOperator.GreaterThanOrEqual)
                    {
                        compareExp = Expression.GreaterThanOrEqual(left, right);
                    }
                    else if (joinItem.CompareOperator == ECompareOperator.LessThan)
                    {
                        compareExp = Expression.LessThan(left, right);
                    }
                    else if (joinItem.CompareOperator == ECompareOperator.LessThanOrEqual)
                    {
                        compareExp = Expression.LessThanOrEqual(left, right);
                    }
                    else
                    {
                        throw new Exception("");
                    }

                    //逻辑运算
                    if (joinItem.LogicalOperator == ELogicalOperator.None)
                    {
                        if (tempTotal == null)
                        {
                            tempTotal = compareExp;
                        }
                        else
                        {
                            throw new Exception("");
                        }
                    }
                    else if (joinItem.LogicalOperator == ELogicalOperator.And)
                    {
                        if (tempTotal != null)
                        {
                            tempTotal = Expression.AndAlso(tempTotal, compareExp);
                        }
                        else
                        {
                            throw new Exception("");
                        }
                    }
                    else if (joinItem.LogicalOperator == ELogicalOperator.Or)
                    {
                        if (tempTotal != null)
                        {
                            tempTotal = Expression.OrElse(tempTotal, compareExp);
                        }
                        else
                        {
                            throw new Exception("");
                        }
                    }
                    else
                    {
                        throw new Exception("");
                    }
                }
                //表达式最终true
                joinList.Add(new KeyValuePair<Type, Expression>(typeof(bool), tempTotal));

                //处理查询结果属性绑定
                foreach (var keyValue in item.Value.Item2)
                {
                    bindingExp = Expression.Property(paramExps.First(t => t.Key == item.Key).Value, keyValue.Value);
                    memberBindings.Add(Expression.Bind(keyValue.Key, bindingExp));
                }
            }

            //设置了当前EntityPropNameAttribute的属性 或者啥特性都没有设置
            var entityProps = totalProps.Where(t => t.GetCustomAttribute<MainTableRelationAttribute>() == null);
            foreach (var prop in entityProps)
            {
                var attr = prop.GetCustomAttribute<EntityPropNameAttribute>();
                string propName = prop.Name;
                if (attr != null && !string.IsNullOrEmpty(attr.PropName))
                {
                    propName = attr.PropName;
                }
                var entityProp = entityType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                bindingExp = Expression.Property(paramExps.First(t => t.Key == entityType).Value, entityProp);
                memberBindings.Add(Expression.Bind(prop, bindingExp));
            }



            //根据参数获取JoinQueryInfos的构造函数
            var constructor = typeof(JoinQueryInfos).GetConstructor(joinList.Select(t => t.Key).ToArray());
            //最终的Queryable的参数
            Expression joinExp = Expression.Lambda(Expression.New(constructor, joinList.Select(t => t.Value)), paramExps.Select(t => t.Value));
            //查询方法
            var queryableMethod = this._client.GetType().GetMethod(nameof(this._client.Queryable)).MakeGenericMethod(types.ToArray());
            //执行返回查询对象
            var queryObject = queryableMethod.Invoke(this._client, new object[] { joinExp });
            //判断查询对象是否为空
            object whereObject = null;
            if (search != null)
            {
                IList<DbOperationAttribute> dbOperations = new List<DbOperationAttribute>();
                var searchProps = search.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                object val = null;
                DbOperationAttribute attribute = null;
                foreach (var prop in searchProps)
                {
                    val = prop.GetValue(search);
                    if (val != null)
                    {
                        attribute = prop.GetCustomAttribute<DbOperationAttribute>();
                        if (attribute == null)
                        {
                            attribute = new DbOperationAttribute(typeof(TEntity), prop.Name);
                        }
                        else
                        {
                            if (attribute.EntityType == null) { attribute.EntityType = typeof(TEntity); }
                            if (string.IsNullOrEmpty(attribute.PropName)) { attribute.PropName = prop.Name; }
                        }
                        attribute.Prop = attribute.EntityType.GetProperty(attribute.PropName, BindingFlags.Public | BindingFlags.Instance);
                        attribute.Value = val;
                        dbOperations.Add(attribute);
                    }
                }
                var whereExp = GetWhereExpression(dbOperations, paramExps);
                if (whereExp != null)
                {
                    var whereMethod = queryObject.GetType().GetMethod("Where", new Type[] { whereExp.GetType() });
                    if (whereMethod != null)
                    {
                        whereObject = whereMethod.Invoke(queryObject, new object[] { whereExp });
                    }
                }
            }

            var memerInitExp = Expression.MemberInit(Expression.New(resultType), memberBindings);
            var selectLambda = Expression.Lambda(memerInitExp, paramExps.Select(t => t.Value));
            MethodInfo selectMethod = null;
            object queryResult = null;
            //int totalCount = 0;
            if (selectLambda != null)
            {
                if (whereObject != null)
                {
                    selectMethod = whereObject.GetType().GetMethod("Select", new Type[] { selectLambda.GetType() });
                    queryResult = selectMethod.Invoke(whereObject, new object[] { selectLambda });
                }
                else
                {
                    selectMethod = queryObject.GetType().GetMethod("Select", new Type[] { selectLambda.GetType() });
                    queryResult = selectMethod.Invoke(queryObject, new object[] { selectLambda });
                }
            }
            //return new KeyValuePair<object, MethodInfo>(whereObject != null ? whereObject : queryObject, selectMethod);
            return queryResult;
        }

        /// <summary>
        /// 获取最终结果
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <typeparam name="TSearchResult">查询结果类型</typeparam>
        /// <param name="search">查询对象</param>
        /// <returns></returns>
        public async Task<TSearchResult> SearchResultAsync<TResult, TSearchResult>(TSearch search)
            where TResult : IResult, new()
            where TSearchResult : BaseSearchResult<TResult>, new()
        {
            //获取最终查询对象
            var selectMethodResult = this.GetSelectResult<TResult, TSearchResult>(search);
            var listMethod = selectMethodResult.GetType().GetMethod("ToListAsync");
            //获取最终结果对象
            object resultList = listMethod.Invoke(selectMethodResult, null);

            //返回分页查询对象
            TSearchResult searchResult = new TSearchResult();
            if (resultList is Task<List<TResult>>)
            {
                var taskResult = resultList as Task<List<TResult>>;
                searchResult.Items = await taskResult;
            }
            return searchResult;
        }

        /// <summary>
        /// 获取最终分页结果
        /// </summary>
        /// <typeparam name="TPageSearch">分页查询类型参数</typeparam>
        /// <typeparam name="TResult">集合里的成员对象类型</typeparam>
        /// <typeparam name="TPageSearchResult">包括总页数以及分页的结果对象集合</typeparam>
        /// <param name="pageSearch">查询对象</param>
        /// <returns></returns>
        public async Task<TPageSearchResult> SearchPageResultAsync<TPageSearch, TResult, TPageSearchResult>(TPageSearch pageSearch)
            where TPageSearch : BaseSearch, IPageSearch
            where TResult : IResult, new()
            where TPageSearchResult : PageSearchResult<TResult>, new()
        {
            //获取最终查询对象
            var selectMethodResult = this.GetSelectResult<TResult, TPageSearchResult>(pageSearch);

            object resultList = null;
            int totalCount = 0;
            var pagetListMethod = selectMethodResult.GetType().GetMethod("ToPageListAsync", new Type[] { typeof(int), typeof(int), typeof(int) });
            //获取最终结果对象
            resultList = pagetListMethod.Invoke(selectMethodResult, new object[] { pageSearch.PageIndex, pageSearch.PageSize, totalCount });

            //返回分页查询对象
            TPageSearchResult pageSearchResult = new TPageSearchResult();
            if (resultList is Task<List<TResult>>)
            {
                var taskResult = resultList as Task<List<TResult>>;
                pageSearchResult.Items = await taskResult;
                pageSearchResult.TotalCount = totalCount;
            }
            return pageSearchResult;


            //if (selectLambda != null)
            //{
            //    if (whereObject != null)
            //    {
            //        var selectMethod = whereObject.GetType().GetMethod("Select", new Type[] { selectLambda.GetType() });
            //        if (selectMethod != null)
            //        {
            //            var result = selectMethod.Invoke(whereObject, new object[] { selectLambda });
            //            var pagetListMethod = result.GetType().GetMethod("ToPageList", new Type[] { typeof(int), typeof(int), typeof(int) });
            //            resultList = pagetListMethod.Invoke(result, new object[] { pageSearch.PageIndex, pageSearch.PageSize, totalCount });
            //        }
            //    }
            //    else
            //    {
            //        var selectMethod = queryObject.GetType().GetMethod("Select", new Type[] { selectLambda.GetType() });
            //        if (selectMethod != null)
            //        {
            //            var result = selectMethod.Invoke(queryObject, new object[] { selectLambda });
            //            var pagetListMethod = result.GetType().GetMethod("ToPageListAsync", new Type[] { typeof(int), typeof(int), typeof(int) });
            //            resultList = pagetListMethod.Invoke(result, new object[] { pageSearch.PageIndex, pageSearch.PageSize, totalCount });
            //        }
            //    }
            //}
            //最后执行select方法
            //int totalCount = 0;
            //this._client.Queryable<ProInfo, ProCategory, UserInfo>((t1, t2, t3) =>
            //new JoinQueryInfos(JoinType.Inner, t1.CategoryId == t2.Id, JoinType.Left, t2.CreatorUserId == t3.Id.ToString()))
            //    .Where((t1, t2, t3) => t1.IsDeleted == false && t2.IsDeleted == false).Select<TResult>((t1, t2, t3) => new TResult() { }).ToListAsync()

            //return null;
        }


        private Expression GetWhereExpression(IList<DbOperationAttribute> operAttrs, IList<KeyValuePair<Type, ParameterExpression>> keyValues)
        {
            var groups = operAttrs.GroupBy(t => t.GroupKey);
            //Dictionary<int?, KeyValuePair<int?, Expression>> dicGroupExps = new Dictionary<int?, KeyValuePair<int?, Expression>>();
            IList<DbOperationGroupExpression> expList = new List<DbOperationGroupExpression>();
            foreach (var groupItem in groups)
            {
                int? parentGroupKey = null;
                var parentGroupKeys = groupItem.Select(t => t.ParentGroupKey).Distinct().ToList();
                if (parentGroupKeys.Count > 1)
                {
                    parentGroupKey = parentGroupKeys.First(t => t != null);
                }
                else
                {
                    parentGroupKey = parentGroupKeys[0];
                }

                //默认And
                ELogicalOperator parentLogicalOperator = ELogicalOperator.And;
                var parentLogicalOperators = groupItem.Select(t => t.ParentLogicalOperator).Distinct().ToList();
                //存在取第一个
                if (parentLogicalOperators.Count > 0)
                {
                    parentLogicalOperator = parentLogicalOperators[0];
                }

                Expression totalExp = null;
                int count = groupItem.Count();
                if (count > 0)
                {
                    var items = groupItem.OrderBy(t => t.LogicalOperator).ToList();
                    for (var i = 0; i < count; i++)
                    {
                        Expression compareExp = null;
                        var current = items[i];
                        //比较运算
                        Expression left = Expression.Property(keyValues.First(t => t.Key == current.EntityType).Value, current.Prop);
                        Expression right = Expression.Constant(current.Value);
                        if (current.CompareOperator == ECompareOperator.Equal)
                        {
                            compareExp = Expression.Equal(left, right);
                        }
                        else if (current.CompareOperator == ECompareOperator.NotEqual)
                        {
                            compareExp = Expression.NotEqual(left, right);
                        }
                        else if (current.CompareOperator == ECompareOperator.GreaterThan)
                        {
                            compareExp = Expression.GreaterThan(left, right);
                        }
                        else if (current.CompareOperator == ECompareOperator.GreaterThanOrEqual)
                        {
                            compareExp = Expression.GreaterThanOrEqual(left, right);
                        }
                        else if (current.CompareOperator == ECompareOperator.LessThan)
                        {
                            compareExp = Expression.LessThan(left, right);
                        }
                        else if (current.CompareOperator == ECompareOperator.LessThanOrEqual)
                        {
                            compareExp = Expression.LessThanOrEqual(left, right);
                        }
                        else
                        {
                            throw new Exception("");
                        }
                        if (i == 0)
                        {
                            totalExp = compareExp;
                        }
                        else
                        {
                            if (current.LogicalOperator == ELogicalOperator.None || current.LogicalOperator == ELogicalOperator.And)
                            {
                                totalExp = Expression.AndAlso(totalExp, compareExp);
                            }
                            else
                            {
                                totalExp = Expression.OrElse(totalExp, compareExp);
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("");
                }
                expList.Add(new DbOperationGroupExpression(groupItem.Key, parentLogicalOperator, parentGroupKey, totalExp));
            }
            int? paramParentGroupKey = null;
            //递归调用所有的分组
            this.GetRecursion(expList, paramParentGroupKey);
            var endExp = expList.First(t => t.GroupKey == paramParentGroupKey);
            if (endExp == null || endExp.TotalExpression == null)
            {
                throw new Exception("");
            }
            return Expression.Lambda(endExp.TotalExpression, keyValues.Select(t => t.Value));
        }

        private void GetRecursion(IList<DbOperationGroupExpression> keyValues, int? parentGroupKey)
        {
            var groupExps = keyValues.Where(t => t.ParentGroupKey == parentGroupKey);
            foreach (var exp in groupExps)
            {
                if (exp.GroupKey == parentGroupKey)
                {
                    continue;
                }
                else
                {
                    var childCount = keyValues.Where(t => t.ParentGroupKey == exp.GroupKey).Count();
                    if (childCount > 0)
                    {
                        this.GetRecursion(keyValues, exp.GroupKey);
                    }
                    else
                    {
                        var parentItem = keyValues.First(t => t.GroupKey == parentGroupKey);
                        if (exp.ParentLogicalOperator == ELogicalOperator.And || exp.ParentLogicalOperator == ELogicalOperator.None)
                        {
                            parentItem.TotalExpression = Expression.AndAlso(parentItem.TotalExpression, exp.TotalExpression);
                        }
                        else
                        {
                            parentItem.TotalExpression = Expression.OrElse(parentItem.TotalExpression, exp.TotalExpression);
                        }
                    }
                }
            }
        }
    }



    public abstract class BaseRep<TEntity, TPrimaryKey, TSearch, TSearchResult> : BaseRep<TEntity, TPrimaryKey, TSearch>
                where TSearch : BaseSearch
        where TEntity : BaseEntity<TPrimaryKey>, IRowVersion, new()
    {
        public BaseRep(ISqlSugarClient client) : base(client)
        { }
    }
}