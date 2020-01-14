using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Entity;
using ZeroOne.Application;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProInfoController : ControllerBase
    {
        private IProInfoService _service;
        public ProInfoController(IProInfoService service)
        {
            this._service = service;
        }

        [HttpGet("GetProByName/{name}")]
        public async Task<Pro_Info> GetProByName(string name)
        {
            return await this._service.GetProByName(name);
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<Pro_Info> GetProduct(string id)
        {
            return await this._service.GetProductInfo(Guid.Parse(id));
        }

        [HttpGet]
        public async Task<IList<Pro_Info>> GetProducts()
        {
            return await this._service.GetProducts();
        }

        [HttpPost]
        public async Task<ActionResult<Pro_Info>> AddProduct(Pro_Info product)
        {
            var proId = await this._service.AddProductInfo(product);
            return CreatedAtAction(nameof(GetProduct), new { id = proId.ToString() }, product);
        }
    }
}