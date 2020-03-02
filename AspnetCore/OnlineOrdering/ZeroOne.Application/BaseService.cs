using System;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class BaseService<TEntity, TPrimaryKey> : IBaseService<TEntity, TPrimaryKey>
        where TEntity : BaseEntity<TPrimaryKey>, new()
    {
        private IBaseRep<TEntity, TPrimaryKey> Rep;
        public BaseService(IBaseRep<TEntity, TPrimaryKey> rep)
        {
            this.Rep = rep;
        }

        public async Task<TEntity> AddAndReturnAsync<TRequest>(TRequest request) where TRequest : IAddRequest
        {
            var entity = request.Map<TEntity>();
            return await this.Rep.AddEntityAsync(entity);
        }

        public async Task<bool> DeleteAsync(TPrimaryKey id, Guid rowVersion, string userId)
        {
            return await this.Rep.DeleteByIdAsync(id, rowVersion, userId);
        }

        public async Task<TResult> GetResultByIdAsync<TResult>(TPrimaryKey id) where TResult : class, IResult, new()
        {
            return await this.Rep.GetResultByIdAsync<TResult>(id);
        }

        public async Task<TEntity> GetEntityByIdAsync(TPrimaryKey id)
        {
            return await this.Rep.GetEntityByIdAsync(id);
        }

        public async Task<bool> UpdateNotNullAsync<TRequest>(TRequest request) where TRequest : IEditRequest
        {
            var entity = request.Map<TEntity>();
            return await this.Rep.UpdateEntityNotNullAsync(entity);
        }
    }
}
