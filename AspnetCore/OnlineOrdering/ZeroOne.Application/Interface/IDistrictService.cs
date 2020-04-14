using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension;
using ZeroOne.Extension.Model;

namespace ZeroOne.Application
{
    public interface IDistrictService : IBaseService<District, Guid>, IHttpService
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

        /// <summary>
        /// 同步高德地图的区域接口数据到数据库
        /// </summary>
        /// <param name="settings">高德地图区域接口配置信息</param>
        /// <returns>受影响的行数</returns>
        Task<int> SyncDistrictAsync(DistrictSettings settings);
    }
}
