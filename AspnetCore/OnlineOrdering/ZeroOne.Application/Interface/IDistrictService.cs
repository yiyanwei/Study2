using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension.Model;

namespace ZeroOne.Application
{
    public interface IDistrictService : IBaseService<District, Guid>
    {
        /// <summary>
        /// 批量添加区域信息
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        Task<int> BulkAddAsync(List<District> models);

        /// <summary>
        /// 获取所有选项
        /// </summary>
        /// <returns></returns>
        Task<IList<SelectItem<string, Guid>>> GetSelectItems();
    }
}
