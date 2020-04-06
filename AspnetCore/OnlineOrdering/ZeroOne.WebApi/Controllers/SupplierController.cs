using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Application;
using ZeroOne.Entity;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}