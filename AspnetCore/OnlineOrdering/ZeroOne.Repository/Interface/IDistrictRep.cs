using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Extension.Model;

namespace ZeroOne.Repository
{
    public interface IDistrictRep : IBaseRep<District, Guid>
    {
        /// <summary>
        /// 获取所有的省份
        /// </summary>
        /// <returns></returns>
        Task<IList<SelectItem<string, Guid>>> GetSelectItems();
    }
}
