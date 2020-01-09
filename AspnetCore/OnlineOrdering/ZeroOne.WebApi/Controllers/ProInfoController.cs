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

        [HttpGet]
        public async Task<IList<Pro_Info>> GetProducts()
        {
            return await this._service.GetProducts();
        }
    }
}