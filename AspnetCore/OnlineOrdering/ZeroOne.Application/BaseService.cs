﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public abstract class BaseService<TEntity, TPrimaryKey, TSearch> : BaseService<TEntity, TPrimaryKey>, IBaseService<TEntity, TPrimaryKey, TSearch>
        where TEntity : BaseEntity<TPrimaryKey>, new()
        where TSearch : BaseSearch
    {
        /// <summary>
        /// 获取查询过滤数据库对象
        /// </summary>
        /// <param name="search">查询对象</param>
        /// <returns></returns>
        protected abstract IList<BaseRepModel> GetBaseRepBySearch(TSearch search);

        private IBaseRep<TEntity, TPrimaryKey, TSearch> Rep;
        public BaseService(IBaseRep<TEntity, TPrimaryKey, TSearch> rep) : base(rep)
        {
            this.Rep = rep;
        }

        public async Task<IList<TEntity>> GetEntitiesAsync(TSearch search)
        {
            var baseReps = this.GetBaseRepBySearch(search);
            if (baseReps?.Count > 0)
            {
                return await this.Rep.GetEntityListAsync(baseReps, search);
            }
            return new List<TEntity>();
        }
    }

    public abstract class BaseService<TEntity, TPrimaryKey> : IBaseService<TEntity, TPrimaryKey>
        where TEntity : BaseEntity<TPrimaryKey>, new()
    {
        public async virtual Task<TResult> FormatResult<TResult>(TResult result) where TResult : class, IResult, new()
        {
            return result;
        }

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
            var result = await this.Rep.GetResultByIdAsync<TResult>(id);
            return await this.FormatResult(result);
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
