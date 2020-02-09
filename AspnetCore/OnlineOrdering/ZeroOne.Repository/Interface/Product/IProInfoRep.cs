using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public interface IProInfoRep : IBaseRep<ProInfoSearch, Pro_Info>, IBulkAddOrUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Pro_Info> GetProByName(string name);
    }
}