using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public interface IProInfoRep : IBaseRep<ProInfoSearch, ProInfo,Guid>, IBulkAddOrUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ProInfo> GetProByName(string name);
    }
}