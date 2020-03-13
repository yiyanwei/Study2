using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ZeroOne.Entity;
using ZeroOne.Extension.Model;

namespace ZeroOne.Repository
{
    /// <summary>
    /// 产品类别仓储接口
    /// </summary>
    public interface IProCategoryRep:IBaseRep<ProCategory,Guid?, ProCategorySearch>
    {
        /// <summary>
        /// 获取产品分类下拉列表
        /// </summary>
        /// <returns></returns>
        Task<IList<SelectItem<string, Guid?>>> GetSelectItems();
    }
}