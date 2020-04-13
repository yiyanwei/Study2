using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Application;
using ZeroOne.Extension.Model;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        protected IDistrictService Service { get; set; }
        public DistrictController(IDistrictService service)
        {
            this.Service = service;
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
    }
}