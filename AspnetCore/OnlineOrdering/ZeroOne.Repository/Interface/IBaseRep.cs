using System;
using ZeroOne.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZeroOne.Repository
{
    public interface IBaseRep<TModel> where TModel : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> GetModel(Guid id);

        Task<bool> AddModel(TModel model);

        Task<bool> DeleteModel(Guid id);

        Task<bool> UpdateModel(TModel model);
    }

    public interface IBaseRep<TSearchModel, TModel> : IBaseRep<TModel> where TSearchModel : BaseSearch where TModel : BaseEntity
    {
        /// <summary>
        /// 获取TModel对象的列表
        /// </summary>
        /// <param name="items">查询运算操作，比较运算和逻辑运算</param>
        /// <param name="search">查询的数据</param>
        /// <returns></returns>
        Task<IList<TModel>> GetModelList(IList<BaseRepModel> items, TSearchModel search);
    }
}