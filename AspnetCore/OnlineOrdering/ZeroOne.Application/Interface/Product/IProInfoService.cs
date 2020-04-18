using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ZeroOne.Entity;

namespace ZeroOne.Application
{
    public interface IProInfoService : IBaseService<ProInfo, Guid, ProInfoSearch>
    {
        /// <summary>
        /// 获取单个产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProInfoSingleResult> GetSingleProInfoAsync(Guid id);

        Task<PageSearchResult<ProInfoResponse>> SearchPageListResponse(ProInfoPageSearch pageSearch);
    }
}