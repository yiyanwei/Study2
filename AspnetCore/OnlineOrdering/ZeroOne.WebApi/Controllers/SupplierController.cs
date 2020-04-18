using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Application;
using ZeroOne.Entity;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SupplierController : CustomController<Supplier, Guid, SupplierAddRequest, SupplierEditRequest>
    {
        protected ISupplierService Service { get; set; }
        public SupplierController(ISupplierService service) : base(service)
        {
            this.Service = (ISupplierService)this.service;
        }

        /// <summary>
        /// 获取供应商分页列表
        /// </summary>
        /// <param name="pageSearch">分页查询对象</param>
        /// <returns></returns>
        [HttpGet("SearchPageList")]
        public async Task<PageSearchResult<SupplierSearchResult>> SearchPageList(SupplierPageSearch pageSearch)
        {
            return await this.Service.SearchPageListResponse(pageSearch);
        }

        /// <summary>
        /// 获取供应商数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetResultById")]
        public async Task<SupplierDetailResult> GetResultById([Required]Guid id)
        {
            return await this.Service.GetResultById(id);
        }
    }
}