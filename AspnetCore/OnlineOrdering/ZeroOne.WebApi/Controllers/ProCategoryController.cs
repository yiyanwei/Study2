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
    public class ProCategoryController : CustomController
    {
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
            var result = await service.GetSelectItems();
            return result;
        }

        /// <summary>
        /// 添加产品分类
        /// </summary>
        /// <param name="request">产品分类请求对象</param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<ProCategory> Add(ProCategoryAddRequest request)
        {
            request.CreatorUserId = UserId;
            return await service.AddEntityAsync(request);
        }

        /// <summary>
        /// 更新产品分类
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task Edit(ProCategoryEditRequest request)
        {
            request.LastModifierUserId = UserId;
            var result = await service.UpdateEntityAsync(request);
            if (!result)
            {
                throw new Exception("");
            }
        }
    }
}