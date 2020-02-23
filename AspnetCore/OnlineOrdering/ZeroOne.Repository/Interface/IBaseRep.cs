using System;
using ZeroOne.Entity;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ZeroOne.Repository
{
    public interface IBaseRep<TModel, TPrimaryKey> where TModel : BaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        Task<TModel> GetModel(TPrimaryKey id);
        /// <summary>
        /// 添加对象
        /// </summary>
        /// <param name="model">对象实例</param>
        /// <returns></returns>
        Task<bool> AddModel(TModel model);
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">对象Id</param>
        /// <param name="rowVersion">对象的版本号</param>
        /// <returns></returns>
        Task<bool> DeleteModel(TPrimaryKey id, Guid rowVersion);
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="model">对象实例</param>
        /// <returns></returns>
        Task<bool> UpdateModel(TModel model);
    }

    public interface IBaseRep<TSearchModel, TModel, TPrimaryKey> : IBaseRep<TModel, TPrimaryKey> where TSearchModel : BaseSearch where TModel : BaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取TModel对象的列表
        /// </summary>
        /// <param name="items">查询运算操作，比较运算和逻辑运算</param>
        /// <param name="search">查询的数据</param>
        /// <returns></returns>
        Task<IList<TModel>> GetListAsync(IList<BaseRepModel> items, TSearchModel search);
    }
}