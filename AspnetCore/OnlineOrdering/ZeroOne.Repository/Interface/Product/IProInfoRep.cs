using System;
using System.Collections.Generic;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public interface IProInfoRep:IBaseRep<Pro_Info>
    {
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <returns></returns>
        IList<Pro_Info> GetProducts();

        /// <summary>
        /// 根据Id获取产品信息
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns>产品对象</returns>
        Pro_Info GetModel(Guid id);
    }
}