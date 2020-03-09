using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Application;
using ZeroOne.Entity;
using ZeroOne.Extension.Model;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProCategoryController : CustomController<ProCategory, Guid, ProCategoryAddRequest, ProCategoryEditRequest, ProCategoryResult, ProCategorySearch>
    {
        private IProCategoryService Service
        {
            get
            {
                return (IProCategoryService)this.service;
            }
        }
        /// <summary>
        /// 服务注入
        /// </summary>
        public ProCategoryController(IProCategoryService service) : base(service)
        {

        }

        /// <summary>
        /// 获取产品分类下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDropDownListAsync")]
        public async Task<IList<SelectItem<string, Guid>>> GetDropDownListAsync()
        {
            var result = await Service.GetSelectItems();
            return result;
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageSearch"></param>
        /// <returns></returns>
        [HttpGet("SearchPageList")]
        public async Task<PageSearchResult<ProCategorySearchResult>> SearchPageList(ProCategoryPageSearch pageSearch)
        {
            var claim = User.Claims.FirstOrDefault(t => t.Type == JwtClaimTypes.Id);
            return await this.Service.SearchPageResultAsync<ProCategoryPageSearch, ProCategorySearchResult, PageSearchResult<ProCategorySearchResult>>(pageSearch);
        }
    }
}