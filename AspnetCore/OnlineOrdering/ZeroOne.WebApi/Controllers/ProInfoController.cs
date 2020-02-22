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
    public class ProInfoController : ControllerBase
    {
        private IProInfoService _service;
        public ProInfoController(IProInfoService service)
        {
            
            this._service = service;
        }

        /// <summary>
        /// 根据产品名称获取产品信息（模糊查询）
        /// </summary>
        /// <param name="name">产品名称</param>
        /// <returns></returns>
        [HttpGet("GetProByName/{name}")]
        public async Task<ProInfo> GetProByName(string name)
        {
            return await this._service.GetProByName(name);
        }

        /// <summary>
        /// 根据产品Id获取产品信息
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns></returns>
        [HttpGet("GetProduct/{id}")]
        public async Task<ProInfo> GetProduct(string id)
        {
            return await this._service.GetProductInfo(Guid.Parse(id));
        }

        /// <summary>
        /// 根据查询条件获取符合的产品信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IList<ProInfo>> GetProducts(ProInfoSearch search)
        {
            return await this._service.GetProducts();
        }

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="product">产品对象</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProInfo>> AddProduct(ProInfo product)
        {
            var proId = await this._service.AddProductInfo(product);
            return CreatedAtAction(nameof(GetProduct), new { id = proId.ToString() }, product);
        }

        [HttpGet("ImportData")]
        public void ImportData()
        {
            this._service.ImportData();
        }
    }
}