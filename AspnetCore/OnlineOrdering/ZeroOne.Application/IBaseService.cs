using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;

namespace ZeroOne.Application
{

    public interface IBaseService<TEntity, TPrimaryKey>
        where TEntity : IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 更新不为空的字段
        /// </summary>
        /// <typeparam name="TRequest">更新请求对象类型</typeparam>
        /// <param name="request">更新对象</param>
        /// <returns></returns>
        Task<bool> UpdateNotNullAsync<TRequest>(TRequest request) where TRequest : IEditRequest;

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="rowVersion">行版本号</param>
        /// <param name="userId">当前操作人Id</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TPrimaryKey id, Guid rowVersion, Guid? userId);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TEntity> AddAndReturnAsync<TRequest>(TRequest request) where TRequest : IAddRequest;

        /// <summary>
        /// 根据主键Id结果对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        Task<TResult> GetResultByIdAsync<TResult>(TPrimaryKey id) where TResult : class, IResult, new();

        /// <summary>
        /// 获取数据库实体对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetEntityByIdAsync(TPrimaryKey id);
    }

    public interface IBaseService<TEntity, TPrimaryKey, TSearch> : IBaseService<TEntity, TPrimaryKey>
        where TEntity : IEntity<TPrimaryKey>
        where TSearch : BaseSearch
    {
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<IList<TEntity>> GetEntitiesAsync(TSearch search);

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
}
