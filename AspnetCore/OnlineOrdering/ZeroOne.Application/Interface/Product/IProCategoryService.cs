using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Extension;

namespace ZeroOne.Application
{
    public interface IProCategoryService
    {
        /// <summary>
        /// 获取产品分类下拉列表
        /// </summary>
        /// <returns></returns>
        Task<IList<SelectItem<string, Guid>>> GetSelectItems();
    }
}