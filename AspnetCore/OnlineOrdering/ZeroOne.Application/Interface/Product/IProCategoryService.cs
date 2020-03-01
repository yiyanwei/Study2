using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension.Model;

namespace ZeroOne.Application
{
    public interface IProCategoryService : IBaseService<ProCategory, Guid>
    {
        /// <summary>
        /// 获取产品分类下拉列表
        /// </summary>
        /// <returns></returns>
        Task<IList<SelectItem<string, Guid>>> GetSelectItems();

        /// <summary>
        /// 添加产品分类
        /// </summary>
        /// <param name="request">产品分类请求对象</param>
        /// <returns></returns>
        Task<ProCategory> AddEntityAsync(ProCategoryAddRequest request);

        /// <summary>
        /// 更新产品分类
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        Task<bool> UpdateEntityAsync(ProCategoryEditRequest request);
    }
}