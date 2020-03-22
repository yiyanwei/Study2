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
using AutoMapper;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProInfoController : CustomController<ProInfo, Guid?, ProInfoAddRequest, ProInfoEditRequest, ProInfoResult, ProInfoSearch>
    {
        /// <summary>
        /// 具体的服务接口
        /// </summary>
        private IProInfoService Service;

        public ProInfoController(IProInfoService service, IMapper mapper) : base(service)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSearch"></param>
        /// <returns></returns>
        [HttpGet("SearchPageList")]
        public async Task<PageSearchResult<ProInfoResponse>> SearchPageList(ProInfoPageSearch pageSearch)
        {
            var results = await this.Service.SearchPageResultAsync<ProInfoPageSearch, ProInfoSearchResult, PageSearchResult<ProInfoSearchResult>>(pageSearch);
            return Mapper.Map<PageSearchResult<ProInfoResponse>>(results);
        }

        /// <summary>
        /// 获取单个产品的信息，包含缩略图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProInfo")]
        public async Task<ProInfoSingleResult> GetProInfo(Guid? id)
        {
            return await this.Service.GetSingleProInfoAsync(id);
        }
    }
}