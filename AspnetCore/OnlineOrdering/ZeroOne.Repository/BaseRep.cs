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
        where TEntity : class, IEntity<TPrimaryKey>, new()
    {
        public async Task<List<TEntity>> GetEntityListAsync(string propName, object value, ECompareOperator compareOperator = ECompareOperator.Equal)
        {
            //比较运算
            Type entityType = typeof(TEntity);
            PropertyInfo prop = entityType.GetProperty(propName.Trim());
            if (prop == null)
            {
                throw new Exception("");
            }
            ParameterExpression paramExp = Expression.Parameter(entityType, "t");
            Expression left = null;
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                left = Expression.Convert(Expression.Property(paramExp, prop), prop.PropertyType.GenericTypeArguments[0]);
            }
            else
            {
                left = Expression.Property(paramExp, prop);
            }

            Expression right = null;
            var valType = value.GetType();
            if (valType.IsGenericType && value.GetType().GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                right = Expression.Convert(Expression.Constant(value), value.GetType().GenericTypeArguments[0]);
            }
            else
            {
                right = Expression.Constant(value);
            }

            Expression compareExp = null;
            if (compareOperator == ECompareOperator.Equal)
            {
                compareExp = Expression.Equal(left, right);
            }
            else if (compareOperator == ECompareOperator.NotEqual)
            {
                compareExp = Expression.NotEqual(left, right);
            }
            else if (compareOperator == ECompareOperator.GreaterThan)
            {
                compareExp = Expression.GreaterThan(left, right);
            }
            else if (compareOperator == ECompareOperator.GreaterThanOrEqual)
            {
                compareExp = Expression.GreaterThanOrEqual(left, right);
            }
            else if (compareOperator == ECompareOperator.LessThan)
            {
                compareExp = Expression.LessThan(left, right);
            }
            else if (compareOperator == ECompareOperator.LessThanOrEqual)
            {
                compareExp = Expression.LessThanOrEqual(left, right);
            }
            else if (compareOperator == ECompareOperator.Contains)
            {
                if (typeof(string) == prop.PropertyType)
                {
                    var strType = typeof(string);
                    var containsMethod = strType.GetMethod(nameof(string.Contains), new Type[] { strType });
                    compareExp = Expression.Call(left, containsMethod, right);
                }
                else if (value.GetType().GetInterfaces().Any(x => typeof(ICollection<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                {
                    var containsMethod = this.GetContainsMethodByGenericArgType(prop.PropertyType);
                    compareExp = Expression.Call(right, containsMethod, left);
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
            Expression<Func<TEntity, bool>> lambdaExp = Expression.Lambda<Func<TEntity, bool>>(compareExp, paramExp);
            return await this.Queryable.Where(lambdaExp).ToListAsync();
        }

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
            return result;
        }

        /// <summary>
        /// 获取所有未删除的数据
        /// </summary>
        /// <returns></returns>
        protected async Task<IList<TEntity>> GetListAsync()
        {
            var entityType = typeof(TEntity);
            if (entityType.GetInterface(nameof(IDeleted)) != null)
            {
                var paramExp = Expression.Parameter(entityType, "t");
                Expression delPropExp = Expression.Property(paramExp, entityType.GetProperty(nameof(IDeleted.IsDeleted)));
                //执行SqlFunc.IsNull方法
                var sqlFuncType = typeof(SqlFunc);
                var isnull = sqlFuncType.GetMethod(nameof(SqlFunc.IsNull));
                isnull = isnull.MakeGenericMethod(typeof(bool));
                Expression constantExp = Expression.Constant(false);
                Expression callExp = Expression.Call(isnull, Expression.Convert(delPropExp, typeof(bool)), constantExp);
                Expression isnullEqualExp = Expression.Equal(callExp, constantExp);
                Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(isnullEqualExp, paramExp);
                return await this.Queryable.Where(lambda).ToListAsync();

            }
            else
            {
                return await this.Queryable.ToListAsync();
            }
        }

        ///// <summary>
        ///// 根据Id获取对应的结果对象
        ///// </summary>
        ///// <typeparam name="TResult">结果对象类型</typeparam>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<TResult> GetResultByIdAsync<TResult>(TPrimaryKey id) where TResult : class, IResult, new()
        //{
        //    var query = this._client.Queryable<TEntity>();
        //    var result = await query.Where(t => t.Id.Equals(id) && SqlFunc.IsNull(t.IsDeleted, false) == false).Select(t => t.Map<TResult>()).FirstAsync();
        //    return this.FormatResult(result);
        //}

        /// <summary>
        /// 根据id返回数据库对象
        /// </summary>
        /// <param name="id">对象id</param>
        /// <returns></returns>
        public async Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            var query = this._client.Queryable<TEntity>();
            var entityType = typeof(TEntity);
            var paramExp = Expression.Parameter(entityType, "t");
            Expression isnullEqualExp = null;
            if (entityType.GetInterface(nameof(IDeleted)) != null)
            {
                Expression delPropExp = Expression.Property(paramExp, entityType.GetProperty(nameof(IDeleted.IsDeleted)));
                //执行SqlFunc.IsNull方法
                var sqlFuncType = typeof(SqlFunc);
                var isnull = sqlFuncType.GetMethod(nameof(SqlFunc.IsNull));
                isnull = isnull.MakeGenericMethod(typeof(bool));
                Expression constantExp = Expression.Constant(false);
                Expression callExp = Expression.Call(isnull, Expression.Convert(delPropExp, typeof(bool)), constantExp);
                isnullEqualExp = Expression.Equal(callExp, constantExp);
            }
            Expression tempExp = null;
            var idprop = entityType.GetProperty(nameof(IEntity<Guid>.Id));
            Expression idEqlExp = Expression.Equal(Expression.Property(paramExp, idprop), Expression.Constant(id));
            if (isnullEqualExp == null)
            {
                tempExp = Expression.AndAlso(idEqlExp, isnullEqualExp);
            }
            else
            {
                tempExp = idEqlExp;
            }
            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(tempExp, paramExp);
            return await query.Where(lambda).FirstAsync();
        }

        public TEntity GetEntityById(TPrimaryKey id)
        {
            var query = this._client.Queryable<TEntity>();
            var entityType = typeof(TEntity);
            var paramExp = Expression.Parameter(entityType, "t");
            Expression isnullEqualExp = null;
            if (entityType.GetInterface(nameof(IDeleted)) != null)
            {
                Expression delPropExp = Expression.Property(paramExp, entityType.GetProperty(nameof(IDeleted.IsDeleted)));
                //执行SqlFunc.IsNull方法
                var sqlFuncType = typeof(SqlFunc);
                var isnull = sqlFuncType.GetMethod(nameof(SqlFunc.IsNull));
                isnull = isnull.MakeGenericMethod(typeof(bool));
                Expression constantExp = Expression.Constant(false);
                Expression callExp = Expression.Call(isnull, Expression.Convert(delPropExp, typeof(bool)), constantExp);
                isnullEqualExp = Expression.Equal(callExp, constantExp);
            }
            Expression tempExp = null;
            var idprop = entityType.GetProperty(nameof(IEntity<Guid>.Id));
            Expression idEqlExp = Expression.Equal(Expression.Property(paramExp, idprop), Expression.Constant(id));
            if (isnullEqualExp == null)
            {
                tempExp = Expression.AndAlso(idEqlExp, isnullEqualExp);
            }
            else
            {
                tempExp = idEqlExp;
            }
            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(tempExp, paramExp);
            return query.Where(lambda).First();
        }


        /// <summary>
        /// 添加数据实体
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        public async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            var entityType = entity.GetType();
            if (entityType.GetInterfaces().Where(t => t == typeof(IRowVersion)).Count() > 0)
            {
                PropertyInfo rowVersionProp = entityType.GetProperty(nameof(IRowVersion.RowVersion));
                rowVersionProp.SetValue(entity, Guid.Empty);
            }
            return await this._client.Insertable<TEntity>(entity).ExecuteReturnEntityAsync();
        }

        /// <summary>
        /// 添加对象集合
        /// </summary>
        /// <param name="list">list集合</param>
        /// <returns></returns>
        public async Task<int> AddEntityListAsync(List<TEntity> list)
        {
            return await this._client.Insertable<TEntity>(list).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="rowVersion">版本号</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(TPrimaryKey id, Guid rowVersion, Guid? userId = null)
        {
            var entity = new TEntity();
            entity.Id = id;
            var entityType = entity.GetType();
            if (entityType.GetInterface(nameof(IDeleted)) == null)
            {
                throw new Exception($"使用此方法，请将类型{entityType.FullName}实现接口{nameof(IDeleted)}");
            }
            else
            {
                PropertyInfo property = entityType.GetProperty(nameof(IDeleted.IsDeleted));
                if (property != null)
                {
                    property.SetValue(entity, true);
                }
                property = entityType.GetProperty(nameof(IDeleted.DeleterUserId));
                if (property != null)
                {
                    property.SetValue(entity, userId);
                }
                property = entityType.GetProperty(nameof(IDeleted.DeletionTime));
                if (property != null)
                {
                    property.SetValue(entity, DateTime.Now);
                }
            }
            PropertyInfo rowVersionProp = null;

            if (entityType.GetInterfaces().Where(t => t == typeof(IRowVersion)).Count() > 0)
            {
                rowVersionProp = entityType.GetProperty(nameof(IRowVersion.RowVersion));
                rowVersionProp.SetValue(entity, rowVersion);
                ConcurrentProcess(entity);
            }
            int affecedRows = await this._client.Updateable(entity).IgnoreColumns(true)
                .Where(t => t.Id.Equals(id)).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        /// <summary>
        /// 删除对象数据
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<bool> DeleteByIdAsync(TPrimaryKey id, Guid? userId = null)
        {
            //Expression.Bind(targetProp, sourcePropExp)
            var entityType = typeof(TEntity);
            if (entityType.GetInterface(nameof(IDeleted)) == null)
            {
                throw new Exception($"使用此方法，请将类型{entityType.FullName}实现接口{nameof(IDeleted)}");
            }
            ParameterExpression paramExp = Expression.Parameter(entityType, "t");
            var memberBindings = new List<MemberBinding>();
            memberBindings.Add(Expression.Bind(entityType.GetProperty(nameof(IDeleted.IsDeleted)), Expression.Constant(true)));
            memberBindings.Add(Expression.Bind(entityType.GetProperty(nameof(IDeleted.DeleterUserId)), Expression.Constant(userId)));
            memberBindings.Add(Expression.Bind(entityType.GetProperty(nameof(IDeleted.DeletionTime)), Expression.Constant(DateTime.Now)));
            var memerInitExp = Expression.MemberInit(Expression.New(entityType), memberBindings);
            var lambdaExp = Expression.Lambda<Func<TEntity, TEntity>>(memerInitExp, paramExp);
            int affecedRows = await this._client.Updateable<TEntity>()
            .SetColumns(lambdaExp)
            .IgnoreColumns(true)
            .Where(t => t.Id.Equals(id)).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        /// <summary>
        /// 更新对象（不为空）
        /// </summary>
        /// <param name="entity">更新对象</param>
        /// <returns></returns>
        public async Task<bool> UpdateEntityNotNullAsync(TEntity entity)
        {
            var entityType = entity.GetType();
            if (entity.GetType().GetInterfaces().Where(t => t == typeof(IRowVersion)).Count() > 0)
            {
                ConcurrentProcess(entity);
                var rowVersionProp = entityType.GetProperty(nameof(IRowVersion.RowVersion));
                if (rowVersionProp != null)
                {
                    rowVersionProp.SetValue(entity, Guid.NewGuid());
                }
            }
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
            var entityType = entity.GetType();
            if (entityType.GetInterfaces().Where(t => t == typeof(IRowVersion)).Count() > 0)
            {
                ConcurrentProcess(entity);
                var rowVersionProp = entityType.GetProperty(nameof(IRowVersion.RowVersion));
                if (rowVersionProp != null)
                {
                    rowVersionProp.SetValue(entity, Guid.NewGuid());
                }
            }
            int affecedRows = await this._client.Updateable(entity).ExecuteCommandAsync();
            return affecedRows > 0;
        }

        /// <summary>
        /// 判断是否并发
        /// </summary>
        private async void ConcurrentProcess(TEntity entity)
        {
            //GetBaseWhereExpression(model.Id)
            var entityType = typeof(TEntity);
            var paramExp = Expression.Parameter(entityType, "t");
            Expression isnullEqualExp = null;
            if (entityType.GetInterface(nameof(IDeleted)) != null)
            {
                Expression delPropExp = Expression.Property(paramExp, entityType.GetProperty(nameof(IDeleted.IsDeleted)));
                //执行SqlFunc.IsNull方法
                var sqlFuncType = typeof(SqlFunc);
                var isnull = sqlFuncType.GetMethod(nameof(SqlFunc.IsNull));
                Expression callExp = Expression.Call(isnull, delPropExp, Expression.Constant(false));
                Expression typeAsExp = Expression.TypeAs(callExp, typeof(bool?));
                isnullEqualExp = Expression.Equal(typeAsExp, Expression.Constant(false));
            }
            Expression tempExp = null;
            var idprop = entityType.GetProperty(nameof(IEntity<Guid>.Id));
            Expression idEqlExp = Expression.Equal(Expression.Property(paramExp, idprop), Expression.Constant(entity.Id));
            if (isnullEqualExp == null)
            {
                tempExp = Expression.AndAlso(idEqlExp, isnullEqualExp);
            }
            else
            {
                tempExp = idEqlExp;
            }
            Expression<Func<TEntity, bool>> lambda = Expression.Lambda<Func<TEntity, bool>>(tempExp, paramExp);

            var searchModel = await this._client.Queryable<TEntity>().Where(lambda).FirstAsync();
            if (searchModel != null)
            {
                var searchProp = searchModel.GetType().GetProperty(nameof(IRowVersion.RowVersion));
                var entityProp = entity.GetType().GetProperty(nameof(IRowVersion.RowVersion));
                if (searchProp.GetValue(searchModel) != entityProp.GetValue(entity))
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
        protected MethodInfo GetContainsMethodByGenericArgType(Type type)
        {
            Type typeArgument = null;
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeArgument = type.GetGenericArguments()[0];
            }
            else
            {
                typeArgument = type;
            }

            var enumerableType = typeof(ICollection<>).MakeGenericType(typeArgument);
            var containMethod = enumerableType.GetMethod(nameof(string.Contains));
            return containMethod;

            //MethodInfo method = null;
            //if (type != null)
            //{
            //    if (type == typeof(int))
            //    {
            //        method = (methodof<Func<IEnumerable<int>, int, bool>>)Enumerable.Contains;
            //    }
            //    else if (type == typeof(decimal))
            //    {
            //        method = (methodof<Func<IEnumerable<decimal>, decimal, bool>>)Enumerable.Contains;
            //    }
            //    else if (type == typeof(float))
            //    {
            //        method = (methodof<Func<IEnumerable<float>, float, bool>>)Enumerable.Contains;
            //    }
            //    else if (type == typeof(long))
            //    {
            //        method = (methodof<Func<IEnumerable<long>, long, bool>>)Enumerable.Contains;
            //    }
            //    else if (type == typeof(double))
            //    {
            //        method = (methodof<Func<IEnumerable<double>, double, bool>>)Enumerable.Contains;
            //    }
            //    else if (type == typeof(DateTime))
            //    {
            //        method = (methodof<Func<IEnumerable<DateTime>, DateTime, bool>>)Enumerable.Contains;
            //    }
            //    else if (type == typeof(string))
            //    {
            //        method = (methodof<Func<IEnumerable<string>, string, bool>>)Enumerable.Contains;
            //    }
            //}
            //return method;
        }

        protected object GetSelectResult<TResult>(BaseSearch search)
        where TResult : IResult, new()
        {
            //Dictionary<Type, IList<PropertyInfo>> dicTypeProps = new Dictionary<Type, IList<PropertyInfo>>();
            //当前对象的类型，也就是主表对象类型
            var entityType = typeof(TEntity);
            //获取所有配置了关联信息的类型对象
            var resultType = typeof(TResult);
            //配置未关联属性
            var totalProps = resultType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //配置关联表属性
            var mainTableProps = totalProps.Where(t => t.GetCustomAttribute<MainTableRelationAttribute>() != null).OrderBy(t => t.GetCustomAttribute<MainTableRelationAttribute>().JoinType);

            //序号、类型以及是否共用同类型参数集合
            IList<Tuple<int, Type, bool>> orderEntityTypeAndIsSamples = new List<Tuple<int, Type, bool>>();


            Dictionary<KeyValuePair<int, Type>, Tuple<EJoinType, IList<KeyValuePair<PropertyInfo, PropertyInfo>>, IList<JoinTableRelationAttribute>>> dicTypePropMappings =
                new Dictionary<KeyValuePair<int, Type>, Tuple<EJoinType, IList<KeyValuePair<PropertyInfo, PropertyInfo>>, IList<JoinTableRelationAttribute>>>();
            int i = 2;
            int start = i;

            //用来属性查找Join表
            Dictionary<string, int> dicMainTablePropMapOrder = new Dictionary<string, int>();

            foreach (var prop in mainTableProps)
            {

                var mainTableRelation = prop.GetCustomAttribute<MainTableRelationAttribute>();
                var notExist = mainTableRelation.IsTogetherSampleType == false || orderEntityTypeAndIsSamples.Where(t => t.Item2 == mainTableRelation.EntityType && t.Item3 == true).Count() <= 0;
                if (notExist)
                {
                    //types.Add(mainTableRelation.EntityType);
                    orderEntityTypeAndIsSamples.Add(new Tuple<int, Type, bool>(i, mainTableRelation.EntityType, mainTableRelation.IsTogetherSampleType));
                }

                IList<KeyValuePair<PropertyInfo, PropertyInfo>> destPropList = null;
                IList<JoinTableRelationAttribute> joinTableRelList = null;
                if (notExist)
                {
                    destPropList = new List<KeyValuePair<PropertyInfo, PropertyInfo>>();
                    joinTableRelList = new List<JoinTableRelationAttribute>();
                    dicTypePropMappings.Add(new KeyValuePair<int, Type>(i, mainTableRelation.EntityType), new Tuple<EJoinType, IList<KeyValuePair<PropertyInfo, PropertyInfo>>, IList<JoinTableRelationAttribute>>(
                    mainTableRelation.JoinType, destPropList, joinTableRelList));
                    //用来属性查找Join表
                    dicMainTablePropMapOrder.Add(prop.Name, i);
                }
                else
                {
                    var first = orderEntityTypeAndIsSamples.First(t => t.Item3 == true && t.Item2 == mainTableRelation.EntityType);
                    var itemKey = dicTypePropMappings.Keys.First(t => t.Key == first.Item1 && t.Value == first.Item2);
                    destPropList = dicTypePropMappings[itemKey].Item2;
                    joinTableRelList = dicTypePropMappings[itemKey].Item3;
                    //用来属性查找Join表
                    dicMainTablePropMapOrder.Add(prop.Name, first.Item1);
                }

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
                    var currProp = mainTableRelation.EntityType.GetProperty(item.PropName, BindingFlags.Instance | BindingFlags.Public);
                    var destRelProp = item.DestEntityType.GetProperty(item.DestRelPropName, BindingFlags.Instance | BindingFlags.Public);
                    //两个表的属性存在
                    if (currProp != null && destRelProp != null && item.DestEntityType != null)
                    {
                        if (!(joinTableRelList.Where(t => t.Property == currProp && t.DestEntityType == item.DestEntityType && t.DestProperty == destRelProp)?.Count() > 0))
                        {
                            item.Property = currProp;
                            item.DestProperty = destRelProp;
                            joinTableRelList.Add(item);
                        }
                    }
                    else if (currProp != null && item.PropValue != null)
                    {
                        item.Property = currProp;
                        joinTableRelList.Add(item);
                    }
                    else if (destRelProp != null && item.DestPropValue != null && item.DestEntityType != null)
                    {
                        item.DestProperty = destRelProp;
                        joinTableRelList.Add(item);
                    }
                }
                i++;
            }

            //添加第一个当前TEntity
            orderEntityTypeAndIsSamples.Insert(0, new Tuple<int, Type, bool>(start - 1, entityType, false));

            //生成类型对象参数表达式
            //var paramExps = types.Select((t, i) => new KeyValuePair<Type, ParameterExpression>(t, Expression.Parameter(t, $"t{i + 1}"))).ToArray();
            var paramExps = orderEntityTypeAndIsSamples.Select(t => new KeyValuePair<int, ParameterExpression>(t.Item1, Expression.Parameter(t.Item2, $"t{t.Item1}"))).ToArray();
            //结果类型表达式参数
            var rExpParam = Expression.Parameter(resultType, "r");

            //获取排序特性
            //int orderPropAttrCount = 0;
            //IEnumerable<DbOrderingAttribute> orderPropAttrs = null;
            //int orderTypeAttrCount = 0;
            //IEnumerable<DbOrderingAttribute> orderTypeAttrs = null;
            IList<KeyValuePair<Expression, EOrderRule>> typeOrderRuleList = new List<KeyValuePair<Expression, EOrderRule>>();
            var orderAttrs = resultType.GetCustomAttributes<DbOrderingAttribute>();
            if (orderAttrs.Count() > 0)
            {
                //orderPropAttrs = orderAttrs.Where(t => !string.IsNullOrEmpty(t.MainTablePropName));
                //orderPropAttrCount = orderPropAttrs.Count();
                //orderTypeAttrs = orderAttrs.Where(t => t.EntityType != null);
                //orderTypeAttrCount = orderTypeAttrs.Count();
                foreach (var orderAttr in orderAttrs)
                {
                    if (!string.IsNullOrEmpty(orderAttr.MainTablePropName) && dicMainTablePropMapOrder.Keys.Contains(orderAttr.MainTablePropName))
                    {
                        int sortNum = dicMainTablePropMapOrder[orderAttr.MainTablePropName];
                        var first = paramExps.First(t => t.Key == sortNum);
                        //排序属性
                        var orderProp = orderEntityTypeAndIsSamples[sortNum].Item2.GetProperty(orderAttr.PropName, BindingFlags.Public | BindingFlags.Instance);
                        typeOrderRuleList.Add(new KeyValuePair<Expression, EOrderRule>(Expression.TypeAs(Expression.Property(first.Value, orderProp), typeof(object)), orderAttr.OrderRule));
                    }
                    else if (orderAttr.EntityType != null && orderEntityTypeAndIsSamples.Where(t => t.Item2 == orderAttr.EntityType).Count() > 0)
                    {
                        int sortNum = orderEntityTypeAndIsSamples.First(t => t.Item2 == orderAttr.EntityType).Item1;
                        var first = paramExps.First(t => t.Key == sortNum);
                        //排序属性
                        var orderProp = orderAttr.EntityType.GetProperty(orderAttr.PropName, BindingFlags.Public | BindingFlags.Instance);
                        typeOrderRuleList.Add(new KeyValuePair<Expression, EOrderRule>(Expression.TypeAs(Expression.Property(first.Value, orderProp), typeof(object)), orderAttr.OrderRule));
                    }
                }
            }

            //if (orderPropAttrCount > 0)
            //{
            //    foreach (var orderPropAttr in orderPropAttrs)
            //    {
            //        if (dicMainTablePropMapOrder.Keys.Contains(orderPropAttr.MainTablePropName))
            //        {
            //            int sortNum = dicMainTablePropMapOrder[orderPropAttr.MainTablePropName];
            //            var first = paramExps.First(t => t.Key == sortNum);
            //            //排序属性
            //            var orderProp = orderEntityTypeAndIsSamples[sortNum].Item2.GetProperty(orderPropAttr.PropName, BindingFlags.Public | BindingFlags.Instance);
            //            typeOrderRuleList.Add(new KeyValuePair<Expression, EOrderRule>(Expression.TypeAs(Expression.Property(first.Value, orderProp), typeof(object)), orderPropAttr.OrderRule));
            //        }
            //    }
            //}
            ////排序属性大小
            //if (orderTypeAttrCount > 0)
            //{
            //    foreach (var orderAttr in orderTypeAttrs)
            //    {
            //        if (orderEntityTypeAndIsSamples.Where(t => t.Item2 == orderAttr.EntityType).Count() > 0)
            //        {
            //            int sortNum = orderEntityTypeAndIsSamples.First(t => t.Item2 == orderAttr.EntityType).Item1;
            //            var first = paramExps.First(t => t.Key == sortNum);
            //            //排序属性
            //            var orderProp = orderAttr.EntityType.GetProperty(orderAttr.PropName, BindingFlags.Public | BindingFlags.Instance);
            //            typeOrderRuleList.Add(new KeyValuePair<Expression, EOrderRule>(Expression.Property(first.Value, orderProp), orderAttr.OrderRule));
            //        }
            //    }
            //}

            //属性绑定表达式
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
                        left = Expression.Property(paramExps.FirstOrDefault(t => t.Key == item.Key.Key).Value, joinItem.Property);
                        if (!string.IsNullOrEmpty(joinItem.MainTableAttrPropName) && dicMainTablePropMapOrder.Keys.Contains(joinItem.MainTableAttrPropName))
                        {
                            var first = paramExps.First(t => t.Key == dicMainTablePropMapOrder[joinItem.MainTableAttrPropName]);
                            right = Expression.Property(first.Value, joinItem.DestProperty);
                        }
                        else
                        {
                            var first = orderEntityTypeAndIsSamples.First(t => t.Item2 == joinItem.DestEntityType);
                            right = Expression.Property(paramExps.FirstOrDefault(t => t.Key == first.Item1).Value, joinItem.DestProperty);
                        }
                        //right = Expression.Property(paramExps.FirstOrDefault(t => t.Key == joinItem.DestEntityType).Value, joinItem.DestProperty);
                    }
                    else if (joinItem.Property != null && joinItem.PropValue != null)
                    {
                        left = Expression.Property(paramExps.FirstOrDefault(t => t.Key == item.Key.Key).Value, joinItem.Property);
                        right = Expression.Constant(joinItem.PropValue);
                    }
                    else if (joinItem.DestProperty != null && joinItem.DestPropValue != null)
                    {
                        if (!string.IsNullOrEmpty(joinItem.MainTableAttrPropName) && dicMainTablePropMapOrder.Keys.Contains(joinItem.MainTableAttrPropName))
                        {
                            int sortNum = dicMainTablePropMapOrder[joinItem.MainTableAttrPropName];
                            var first = paramExps.First(t => t.Key == sortNum);
                            left = Expression.Property(first.Value, joinItem.DestProperty);
                        }
                        else
                        {
                            var myType = orderEntityTypeAndIsSamples.First(t => t.Item2 == joinItem.DestEntityType);
                            var myExpParam = paramExps.FirstOrDefault(t => t.Key == myType.Item1);
                            left = Expression.Property(myExpParam.Value, joinItem.DestProperty);
                        }
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
                    //字符串就是like，ICollection<>就是in
                    else if (joinItem.CompareOperator == ECompareOperator.Contains)
                    {
                        //目标值不为空并且目标值是string类型
                        //当前属性值不为空并且值类型是string类型
                        //当前属性不为空目标属性不为空并且字符串类型
                        if (
                                 (joinItem.DestPropValue != null && joinItem.DestPropValue.GetType() == typeof(string))
                                 || (joinItem.PropValue != null && joinItem.PropValue.GetType() == typeof(string))
                                 || (joinItem.Property != null && joinItem.DestProperty != null && joinItem.Property.PropertyType == typeof(string) && joinItem.DestProperty.PropertyType == typeof(string))
                             )
                        {
                            var strType = typeof(string);
                            var containsMethod = strType.GetMethod(nameof(string.Contains), new Type[] { strType });
                            compareExp = Expression.Call(left, containsMethod, right);
                        }
                        //当前对象表属性不为空值不为空，并且目标类型为ICollection<>类型
                        else if (joinItem.Property != null && joinItem.PropValue != null && joinItem.PropValue.GetType().GetInterfaces().Any(x => typeof(ICollection<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                        {
                            var property = joinItem.Property;
                            var containsMethod = this.GetContainsMethodByGenericArgType(property.PropertyType);
                            compareExp = Expression.Call(right, containsMethod, left);
                        }
                        //目标属性不为空目标值不为空，并且目标值类型为ICollection<>类型
                        else if (joinItem.DestProperty != null && joinItem.DestPropValue != null && joinItem.DestPropValue.GetType().GetInterfaces().Any(x => typeof(ICollection<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                        {
                            var property = joinItem.DestProperty;
                            var containsMethod = this.GetContainsMethodByGenericArgType(property.PropertyType);
                            compareExp = Expression.Call(right, containsMethod, left);
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
                    bindingExp = Expression.Property(paramExps.First(t => t.Key == item.Key.Key).Value, keyValue.Value);
                    memberBindings.Add(Expression.Bind(keyValue.Key, bindingExp));
                }
            }

            //设置了当前EntityPropNameAttribute的属性 或者啥特性都没有设置
            var entityProps = totalProps.Where(t => t.GetCustomAttribute<MainTableRelationAttribute>() == null && t.GetCustomAttribute<ResultPropIgnoreAttribute>() == null);
            var firstType = orderEntityTypeAndIsSamples.First(t => t.Item2 == entityType);
            foreach (var prop in entityProps)
            {
                var attr = prop.GetCustomAttribute<EntityPropNameAttribute>();
                string propName = prop.Name;
                if (attr != null && !string.IsNullOrEmpty(attr.PropName))
                {
                    propName = attr.PropName;
                }
                var entityProp = entityType.GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                bindingExp = Expression.Property(paramExps.First(t => t.Key == firstType.Item1).Value, entityProp);
                memberBindings.Add(Expression.Bind(prop, bindingExp));
            }

            //执行返回查询对象
            object queryObject = null;
            if (joinList?.Count > 0)
            {
                //根据参数获取JoinQueryInfos的构造函数
                var constructor = typeof(JoinQueryInfos).GetConstructor(joinList.Select(t => t.Key).ToArray());
                //最终的Queryable的参数
                Expression joinExp = Expression.Lambda(Expression.New(constructor, joinList.Select(t => t.Value)), paramExps.Select(t => t.Value));
                //查询方法
                //var typeList = dicTypePropMappings.Select(t => t.Key.Value).ToList();
                //typeList.Insert(0,entityType);
                var joinExpType = joinExp.GetType();
                var queryableMethods = this._client.GetType().GetMethods().Where(
                    t => t.Name == nameof(this._client.Queryable)
                    && t.GetGenericArguments().Count() == orderEntityTypeAndIsSamples.Count
                    && t.ToString().Contains(nameof(JoinQueryInfos)));
                if (queryableMethods.Count() <= 0)
                {
                    throw new Exception("");
                }
                var queryableMethod = queryableMethods.ToList()[0].MakeGenericMethod(orderEntityTypeAndIsSamples.Select(t => t.Item2).ToArray());
                queryObject = queryableMethod.Invoke(this._client, new object[] { joinExp });
            }
            else
            {
                var queryableMethods = this._client.GetType().GetMethods().Where(
                    t => t.Name == nameof(this._client.Queryable)
                    && t.GetGenericArguments().Count() == orderEntityTypeAndIsSamples.Count
                    && t.GetParameters().Length == 0).ToList();
                var queryableMethod = queryableMethods[0].MakeGenericMethod(orderEntityTypeAndIsSamples.Select(t => t.Item2).ToArray());
                queryObject = queryableMethod.Invoke(this._client, null);
            }
            //this._client.Queryable<FileInfo>()
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
                    if (prop.Name == nameof(IPageSearch.PageIndex) || prop.Name == nameof(IPageSearch.PageSize))
                    {
                        continue;
                    }
                    val = prop.GetValue(search);
                    if (val.IsNotNullAndEmpty())
                    {
                        attribute = prop.GetCustomAttribute<DbOperationAttribute>();
                        if (attribute == null)
                        {
                            attribute = new DbOperationAttribute(entityType, prop.Name);
                        }
                        else
                        {
                            if (attribute.EntityType == null)
                            {
                                attribute.EntityType = entityType;
                            }
                            if (string.IsNullOrEmpty(attribute.PropName))
                            {
                                attribute.PropName = prop.Name;
                            }
                        }

                        if (!string.IsNullOrWhiteSpace(attribute.MainTablePropName) && dicMainTablePropMapOrder.ContainsKey(attribute.MainTablePropName))
                        {
                            var keyEntityType = orderEntityTypeAndIsSamples.First(t => t.Item1 == dicMainTablePropMapOrder[attribute.MainTablePropName]);
                            attribute.Prop = keyEntityType.Item2.GetProperty(attribute.PropName, BindingFlags.Public | BindingFlags.Instance);
                        }
                        else if (attribute.EntityType != null)
                        {
                            attribute.Prop = attribute.EntityType.GetProperty(attribute.PropName, BindingFlags.Public | BindingFlags.Instance);
                        }
                        else
                        {

                            throw new Exception("");
                        }
                        attribute.Value = val;
                        dbOperations.Add(attribute);
                    }
                }
                if (dbOperations.Count > 0)
                {
                    var whereExp = GetWhereExpression(dbOperations, paramExps, orderEntityTypeAndIsSamples.Select(t => new KeyValuePair<int, Type>(t.Item1, t.Item2)).ToList(), dicMainTablePropMapOrder);
                    if (whereExp != null)
                    {
                        var whereMethod = queryObject.GetType().GetMethod("Where", new Type[] { whereExp.GetType() });
                        if (whereMethod != null)
                        {
                            whereObject = whereMethod.Invoke(queryObject, new object[] { whereExp });
                        }
                    }
                }
            }

            //排序对象
            object orderObject = queryObject;
            if (whereObject != null)
            {
                orderObject = whereObject;
            }
            LambdaExpression orderLambda = null;
            MethodInfo orderMethod = null;
            object queryResult = null;
            if (typeOrderRuleList.Count > 0)
            {
                var orderMethods = orderObject.GetType().GetMethods().Where(t => t.Name == "OrderBy" && t.GetParameters().Where(x => x.Name == "expression" && x.Member?.DeclaringType?.GenericTypeArguments?.Count() == orderEntityTypeAndIsSamples.Count).Count() > 0).ToList();
                if (orderMethods.Count > 0)
                {
                    orderMethod = orderMethods[0];
                    foreach (var orderRule in typeOrderRuleList)
                    {
                        OrderByType orderByType = OrderByType.Asc;
                        if (orderRule.Value == EOrderRule.Desc)
                        {
                            orderByType = OrderByType.Desc;
                        }
                        orderLambda = Expression.Lambda(orderRule.Key, paramExps.Select(t => t.Value));
                        orderObject = orderMethod.Invoke(orderObject, new object[] { orderLambda, orderByType });
                    }
                }
            }

            //select对象
            var memerInitExp = Expression.MemberInit(Expression.New(resultType), memberBindings);
            var selectLambda = Expression.Lambda(memerInitExp, paramExps.Select(t => t.Value));
            //int totalCount = 0;
            if (selectLambda != null)
            {
                //if (whereObject != null)
                //{
                //this._client.Queryable<ProCategory, ProCategory>((t1, t2) => t1.Id == t2.ParentId).Where((t1, t2) => t1.CategoryName == "")
                //    .OrderBy((t1, t2) => t1.CategoryName, OrderByType.Asc)
                //    .OrderBy(())
                //    .Select<ProCategorySearchResult>((t1, t2) => new ProCategorySearchResult())
                var selectMethods = orderObject.GetType().GetMethods().Where(t => t.Name == "Select" && t.GetParameters().Where(x => x.Name == "expression" && x.Member?.DeclaringType?.GenericTypeArguments?.Count() == orderEntityTypeAndIsSamples.Count).Count() > 0).ToList();
                //var b = whereObject.GetType().GetMethod("Select");
                if (selectMethods.Count > 0)
                {
                    MethodInfo selectMethod = selectMethods[0].MakeGenericMethod(new[] { resultType });
                    queryResult = selectMethod.Invoke(orderObject, new object[] { selectLambda });
                }
                //}
                //else
                //{
                //    var selectMethods = queryObject.GetType().GetMethods().Where(t => t.Name == "Select" && t.GetParameters().Where(x => x.Name == "expression" && x.Member?.DeclaringType?.GenericTypeArguments?.Count() == orderEntityTypeAndIsSamples.Count).Count() > 0).ToList();
                //    //selectMethod = queryObject.GetType().GetMethod("Select", new Type[] { selectLambda.GetType() });
                //    if (selectMethods.Count > 0)
                //    {
                //        selectMethod = selectMethods[0].MakeGenericMethod(new[] { resultType });
                //        queryResult = selectMethod.Invoke(queryObject, new object[] { selectLambda });
                //    }
                //}
            }
            return queryResult;
        }

        /// <summary>
        /// 获取结果集
        /// </summary>
        /// <typeparam name="TResult">结果对象</typeparam>
        /// <typeparam name="TChildSearch">查询对象</typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<TResult>> GetResultListAsync<TResult, TChildSearch>(TChildSearch search)
           where TResult : IResult, new()
           where TChildSearch : BaseSearch
        {
            //获取最终查询对象
            var selectMethodResult = this.GetSelectResult<TResult>(search);
            var listMethod = selectMethodResult.GetType().GetMethod("ToListAsync");
            //获取最终结果对象
            object resultList = listMethod.Invoke(selectMethodResult, null);
            if (resultList is Task<List<TResult>>)
            {
                var taskResult = resultList as Task<List<TResult>>;
                return await taskResult;
            }
            return new List<TResult>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operAttrs"></param>
        /// <param name="keyValues"></param>
        /// <param name="numTypes"></param>
        /// <param name="dicPropNums"></param>
        /// <returns></returns>

        protected Expression GetWhereExpression(IList<DbOperationAttribute> operAttrs, IList<KeyValuePair<int, ParameterExpression>> keyValues, IList<KeyValuePair<int, Type>> numTypes, Dictionary<string, int> dicPropNums)
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
                var parentLogicalOperators = groupItem.Select(t => t.ParGroupLogicalOperator).Distinct().ToList();
                //存在取第一个
                if (parentLogicalOperators.Count > 0)
                {
                    parentLogicalOperator = parentLogicalOperators[0];
                }

                Expression totalExp = null;
                //int count = groupItem.Count();
                var items = groupItem.OrderBy(t => t.LogicalOperator).ToList();
                //items = items.Where(t => t.PropName != nameof(IPageSearch.PageIndex) && t.PropName != nameof(IPageSearch.PageSize)).ToList();
                int count = items.Count;
                if (count > 0)
                {
                    for (var i = 0; i < count; i++)
                    {
                        var current = items[i];
                        Expression compareExp = null;
                        //比较运算
                        Expression left = null;
                        if (!string.IsNullOrEmpty(current.MainTablePropName) && dicPropNums.Keys.Contains(current.MainTablePropName))
                        {
                            left = Expression.Property(keyValues.First(t => t.Key == dicPropNums[current.MainTablePropName]).Value, current.Prop);
                        }
                        else
                        {
                            var first = numTypes.First(t => t.Value == current.EntityType);
                            left = Expression.Property(keyValues.First(t => t.Key == first.Key).Value, current.Prop);
                        }
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
                        else if (current.CompareOperator == ECompareOperator.Contains)
                        {
                            if (typeof(string) == current.Prop.PropertyType)
                            {
                                var strType = typeof(string);
                                var containsMethod = strType.GetMethod(nameof(string.Contains), new Type[] { strType });
                                compareExp = Expression.Call(left, containsMethod, right);
                            }
                            else if (current.Value.GetType().GetInterfaces().Any(x => typeof(ICollection<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                            {
                                var property = current.Prop;
                                if (property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    var valProp = property.PropertyType.GetProperty(nameof(Nullable<int>.Value));
                                    left = Expression.Property(left, valProp);
                                }
                                var containsMethod = this.GetContainsMethodByGenericArgType(property.PropertyType);
                                compareExp = Expression.Call(right, containsMethod, left);
                            }
                            else
                            {
                                throw new Exception("");
                            }
                            //if(current.Prop.PropertyType)
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
            var endExp = expList.Single(t => t.GroupKey == paramParentGroupKey);
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
            var selectMethodResult = this.GetSelectResult<TResult>(pageSearch);
            if (selectMethodResult == null)
            {
                throw new Exception("");
            }

            object resultList = null;
            int intTotalCount = 0;
            var totalCount = new RefAsync<int>(intTotalCount);
            var pagetListMethod = selectMethodResult.GetType().GetMethod("ToPageListAsync", new Type[] { typeof(int), typeof(int), totalCount.GetType() });
            //获取最终结果对象
            object[] parameters = new object[] { pageSearch.PageIndex, pageSearch.PageSize, totalCount };
            resultList = pagetListMethod.Invoke(selectMethodResult, parameters);

            //返回分页查询对象
            TPageSearchResult pageSearchResult = new TPageSearchResult();
            if (resultList is Task<List<TResult>>)
            {
                var taskResult = resultList as Task<List<TResult>>;
                pageSearchResult.Items = await taskResult;
                pageSearchResult.TotalCount = totalCount;
            }
            return pageSearchResult;
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
                //判断是否实现了ICollection<>的集合对象
                if (property.PropertyType != typeof(string) && property.PropertyType.GetInterfaces().Any(x => typeof(ICollection<>) == (x.IsGenericType ? x.GetGenericTypeDefinition() : x)))
                {
                    PropertyInfo compareProp = modelType.GetProperty(item.Key);

                    Type typeArgument = null;
                    if (property.PropertyType.GetGenericArguments()[0].GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        typeArgument = property.PropertyType.GetGenericArguments()[0].GetGenericArguments()[0];
                    }
                    else
                    {
                        typeArgument = property.PropertyType.GetGenericArguments()[0];
                    }
                    MethodInfo containsMethod = this.GetContainsMethodByGenericArgType(typeArgument);
                    //判断属性值是否为Nullable类型值
                    if (compareProp.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        var valProp = compareProp.PropertyType.GetProperty(nameof(Nullable<int>.Value));
                        var valExpression = Expression.Property(Expression.Property(paramExpr, compareProp), valProp);
                        var searchPropertyExpression = Expression.Property(Expression.Constant(model, searchModelType), property);
                        childExpression = Expression.Call(searchPropertyExpression, containsMethod, valExpression);
                    }
                    else
                    {
                        var searchPropertyExpression = Expression.Property(Expression.Constant(model, searchModelType), property);
                        childExpression = Expression.Call(searchPropertyExpression, containsMethod, Expression.Property(paramExpr, compareProp));
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


        ///// <summary>
        ///// 获取最终结果
        ///// </summary>
        ///// <typeparam name="TResult">结果类型</typeparam>
        ///// <typeparam name="TSearchResult">查询结果类型</typeparam>
        ///// <param name="search">查询对象</param>
        ///// <returns></returns>
        //public async Task<TSearchResult> SearchResultAsync<TResult, TSearchResult>(TSearch search)
        //    where TResult : IResult, new()
        //    where TSearchResult : BaseSearchResult<TResult>, new()
        //{
        //    //获取最终查询对象
        //    var selectMethodResult = this.GetSelectResult<TResult>(search);
        //    var listMethod = selectMethodResult.GetType().GetMethod("ToListAsync");
        //    //获取最终结果对象
        //    object resultList = listMethod.Invoke(selectMethodResult, null);

        //    //返回分页查询对象
        //    TSearchResult searchResult = new TSearchResult();
        //    if (resultList is Task<List<TResult>>)
        //    {
        //        var taskResult = resultList as Task<List<TResult>>;
        //        searchResult.Items = await taskResult;
        //    }
        //    return searchResult;
        //}

        public async Task<List<TResult>> GetResultListAsync<TResult>(TSearch search)
            where TResult : IResult, new()
        {
            //获取最终查询对象
            var selectMethodResult = this.GetSelectResult<TResult>(search);
            var listMethod = selectMethodResult.GetType().GetMethod("ToListAsync");
            //获取最终结果对象
            object resultList = listMethod.Invoke(selectMethodResult, null);
            if (resultList is Task<List<TResult>>)
            {
                var taskResult = resultList as Task<List<TResult>>;
                return await taskResult;
            }
            return new List<TResult>();
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