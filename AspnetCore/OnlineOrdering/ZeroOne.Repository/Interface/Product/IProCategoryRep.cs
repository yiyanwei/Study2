using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    /// <summary>
    /// 产品类别仓储接口
    /// </summary>
    public interface IProCategoryRep:IBaseRep<Pro_Category>
    {
        /// <summary>
        /// 获取所有产品类别数据
        /// </summary>
        /// <returns></returns>
        Task<IList<Pro_Category>> GetCategoryList(bool isdeleted = true);

        /// <summary>
        /// 根据类别Id获取类别信息
        /// </summary>
        /// <param name="id">产品类别Id，type:Guid</param>
        /// <returns>返回产品类别信息</returns>
        Task<Pro_Category> GetModel(Guid id);
    }
}