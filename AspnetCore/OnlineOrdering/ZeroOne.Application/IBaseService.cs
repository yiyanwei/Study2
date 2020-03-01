using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;

namespace ZeroOne.Application
{
    public interface IBaseService<TEntity,TPrimaryKey>
        where TEntity : BaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 更新不为空的字段
        /// </summary>
        /// <typeparam name="TRequest">更新请求对象类型</typeparam>
        /// <param name="request">更新对象</param>
        /// <returns></returns>
        Task<bool> UpdateNotNullAsync<TRequest>(TRequest request) where TRequest:IUpdated,IRowVersion;

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <param name="rowVersion">行版本号</param>
        /// <param name="userId">当前操作人Id</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(TPrimaryKey id,Guid rowVersion, string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TEntity> AddAndReturnAsync<TRequest>(TRequest request) where TRequest : IAdd;

        /// <summary>
        /// 根据主键Id获取对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        Task<TEntity> GetByIdAsync(TPrimaryKey id);
    }
}
