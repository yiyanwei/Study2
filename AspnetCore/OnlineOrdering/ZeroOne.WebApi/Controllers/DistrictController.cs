using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ZeroOne.Application;
using ZeroOne.Extension.Model;

namespace ZeroOne.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        protected IDistrictService Service { get; set; }
        protected DistrictSettings DistrictSettings { get; set; }
        public DistrictController(IDistrictService service,IOptions<DistrictSettings> options)
        {
            this.Service = service;
            this.DistrictSettings = options.Value;
        }

        ///// <summary>
        ///// 同步区域接口（中国）
        ///// </summary>
        ///// <returns></returns>        
        //public async Task<int> SyncDistrict()
        //{
        //    var result = await Service.SyncDistrictAsync(this.DistrictSettings);
        //    return result;
        //}

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