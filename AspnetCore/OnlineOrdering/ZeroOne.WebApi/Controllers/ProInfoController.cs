using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NLog.Web;
using ZeroOne.Entity;
using ZeroOne.Application;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProInfoController : CustomController<ProInfo, Guid, ProInfoAddRequest, ProInfoEditRequest, ProInfoResult, ProInfoSearch>
    {
        /// <summary>
        /// 具体的服务接口
        /// </summary>
        private IProInfoService Service;

        public ProInfoController(IProInfoService service) : base(service)
        {
            this.Service = service;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSearch"></param>
        /// <returns></returns>
        [HttpGet("SearchPageList")]
        public async Task<PageSearchResult<ProInfoSearchResult>> SearchPageList(ProInfoPageSearch pageSearch)
        {
            return await this.Service.SearchPageResultAsync<ProInfoPageSearch, ProInfoSearchResult, PageSearchResult<ProInfoSearchResult>>(pageSearch);
        }


    }
}