using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Application;

namespace ZeroOne.WebApi.Controllers
{
    public class CustomController : ControllerBase
    {
        /// <summary>
        /// 服务注入
        /// </summary>
        protected IProCategoryService service;
        public CustomController(IProCategoryService service)
        {
            this.service = service;
        }
        /// <summary>
        /// 当前用户Id
        /// </summary>
        protected string UserId { get; set; }
        public CustomController()
        {
            var claim = User.Claims.FirstOrDefault(t => t.Type == JwtClaimTypes.Id);
            if (claim != null)
            {
                UserId = claim.Value;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">删除Id</param>
        /// <param name="rowVersion">版本号</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task Delete(Guid? id, Guid? rowVersion)
        {
            if (!id.HasValue)
            {

            }
            if (!rowVersion.HasValue)
            {

            }
            var result = await service.DeleteAsync(id.Value, rowVersion.Value, UserId);
            if (!result)
            {

            }
        }
    }
}
