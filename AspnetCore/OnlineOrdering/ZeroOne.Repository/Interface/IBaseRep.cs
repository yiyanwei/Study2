using System;
using ZeroOne.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZeroOne.Repository
{
    public interface IBaseRep<TEntity, TPrimaryKey> where TEntity : BaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取结果对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        Task<TResponse> GetResultByIdAsync<TResponse>(TPrimaryKey id) where TResponse : class, IResult, new();

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetEntityByIdAsync(TPrimaryKey id);

        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="model">对象实例</param>
        /// <returns></returns>
        Task<TEntity> AddEntityAsync(TEntity model);
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="rowVersion">对象的版本号</param>
        /// <param name="userId">删除操作操作人id</param>
        /// <returns></returns>
        Task<bool> DeleteByIdAsync(TPrimaryKey id, Guid rowVersion, string userId = null);

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
        where TEntity : BaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取TModel对象的列表
        /// </summary>
        /// <param name="items">查询运算操作，比较运算和逻辑运算</param>
        /// <param name="search">查询的数据</param>
        /// <returns></returns>
        Task<IList<TEntity>> GetEntityListAsync(IList<BaseRepModel> items, TSearch search);
    }

    //public interface IBaseRep<TEntity, TPrimaryKey, TSearch,TResult> : IBaseRep<TEntity, TPrimaryKey, TSearch>
    //    where TEntity : BaseEntity<TPrimaryKey>
    //    where TSearch : BaseSearch
    //    where 
    //    where TSearchResult : BaseResult<>
    //{

    //}
}