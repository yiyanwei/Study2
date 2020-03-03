using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public interface IProInfoRep : IBaseRep<ProInfo,Guid, ProInfoSearch>, IBulkAddOrUpdate
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<ProInfo> GetProByName(string name);
    }
}