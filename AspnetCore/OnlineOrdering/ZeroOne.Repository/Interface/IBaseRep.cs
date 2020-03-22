using System;
using ZeroOne.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZeroOne.Repository
{
    public interface IBaseRep<TEntity, TPrimaryKey> where TEntity : IEntity<TPrimaryKey>
    {
        Task<List<TEntity>> GetEntityListAsync(string propName, object value, ECompareOperator compareOperator = ECompareOperator.Equal);
        ///// <summary>
        ///// 获取结果对象
        ///// </summary>
        ///// <param name="id">主键Id</param>
        ///// <returns></returns>
        //Task<TResponse> GetResultByIdAsync<TResponse>(TPrimaryKey id) where TResponse : class, IResult, new();

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="id">对象id</param>
        /// <returns></returns>
        Task<TEntity> GetEntityByIdAsync(TPrimaryKey id);

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <returns></returns>
        TEntity GetEntityById(TPrimaryKey id);

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="model">对象实例</param>
        /// <returns></returns>
        Task<TEntity> AddEntityAsync(TEntity model);

        /// <summary>
        /// 添加对象集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<int> AddEntityListAsync(List<TEntity> list);
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="rowVersion">对象的版本号</param>
        /// <param name="userId">删除操作操作人id</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(TPrimaryKey id, Guid rowVersion, Guid? userId = null);

        /// <summary>
        /// 更新对象（不为空的数据）
        /// </summary>
        /// <param name="entity">对象实例</param>
        /// <returns></returns>
        Task<bool> UpdateEntityNotNullAsync(TEntity entity);

        /// <summary>
        /// 更新所有数据
        /// </summary>
        /// <param name="entity">更新对象</param>
        /// <returns></returns>
        Task<bool> UpdateEntityAsync(TEntity entity);
    }

    public interface IBaseRep<TEntity, TPrimaryKey, TSearch> : IBaseRep<TEntity, TPrimaryKey>
        where TSearch : BaseSearch
        where TEntity : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取TModel对象的列表
        /// </summary>
        /// <param name="items">查询运算操作，比较运算和逻辑运算</param>
        /// <param name="search">查询的数据</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetEntityListAsync(IList<BaseRepModel> items, TSearch search);

        /// <summary>
        /// 获取最终结果
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <typeparam name="TSearchResult">查询结果类型</typeparam>
        /// <param name="search">查询对象</param>
        /// <returns></returns>
        Task<TSearchResult> SearchResultAsync<TResult, TSearchResult>(TSearch search)
                where TResult : IResult, new()
                where TSearchResult : BaseSearchResult<TResult>, new();

        /// <summary>
        /// 获取TResult对象的List集合
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<TResult>> GetResultListAsync<TResult>(TSearch search) where TResult : IResult, new();

        /// <summary>
        /// 获取最终分页结果
        /// </summary>
        /// <typeparam name="TPageSearch">分页查询类型参数</typeparam>
        /// <typeparam name="TResult">集合里的成员对象类型</typeparam>
        /// <typeparam name="TPageSearchResult">包括总页数以及分页的结果对象集合</typeparam>
        /// <param name="pageSearch">查询对象</param>
        /// <returns></returns>
        Task<TPageSearchResult> SearchPageResultAsync<TPageSearch, TResult, TPageSearchResult>(TPageSearch pageSearch)
            where TPageSearch : BaseSearch, IPageSearch
            where TResult : IResult, new()
            where TPageSearchResult : PageSearchResult<TResult>, new();
    }

    //public interface IBaseRep<TEntity, TPrimaryKey, TSearch,TResult> : IBaseRep<TEntity, TPrimaryKey, TSearch>
    //    where TEntity : BaseEntity<TPrimaryKey>
    //    where TSearch : BaseSearch
    //    where 
    //    where TSearchResult : BaseResult<>
    //{

    //}
}