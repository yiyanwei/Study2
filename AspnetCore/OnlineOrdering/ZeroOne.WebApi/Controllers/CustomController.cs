using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using ZeroOne.Application;
using ZeroOne.Entity;

namespace ZeroOne.WebApi.Controllers
{

    public class CustomController<TEntity, TPrimaryKey, TAddRequest, TEditRequest> : ControllerBase
            where TEntity : BaseEntity<TPrimaryKey>
    where TAddRequest : IAddRequest, IEntity<TPrimaryKey>
    where TEditRequest : IEditRequest, IEntity<TPrimaryKey>
    {
        /// <summary>
        /// 服务注入
        /// </summary>
        protected IBaseService<TEntity, TPrimaryKey> service;
        public CustomController(IBaseService<TEntity, TPrimaryKey> service)
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
        /// 添加数据
        /// </summary>
        /// <param name="request">新增请求对象</param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<TEntity> Add(TAddRequest request)
        {
            request.CreatorUserId = UserId;
            return await service.AddAndReturnAsync(request);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="request">更新请求对象</param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task Edit(TEditRequest request)
        {
            request.LastModifierUserId = UserId;
            var result = await service.UpdateNotNullAsync(request);
            if (!result)
            {
                throw new Exception("");
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id">删除Id</param>
        /// <param name="rowVersion">版本号</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task Delete(TPrimaryKey id, Guid? rowVersion)
        {
            if (!rowVersion.HasValue)
            {

            }
            var result = await service.DeleteAsync(id, rowVersion.Value, UserId);
            if (!result)
            {

            }
        }

        /// <summary>
        /// 获取数据库实体对象
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("GetEntityById")]

        public async Task<TEntity> GetEntityById(TPrimaryKey id)
        {
            return await service.GetEntityByIdAsync(id);
        }

    }

    public class CustomController<TEntity, TPrimaryKey, TAddRequest, TEditRequest, TResult> : CustomController<TEntity, TPrimaryKey, TAddRequest, TEditRequest>
    where TEntity : BaseEntity<TPrimaryKey>
    where TAddRequest : IAddRequest, IEntity<TPrimaryKey>
    where TEditRequest : IEditRequest, IEntity<TPrimaryKey>
    where TResult : class, IResult, new()
    {
        /// <summary>
        /// 格式化单个结果格式函数
        /// </summary>
        private static Dictionary<string, Func<TResult, TResult>> ControllerFormatFuncs;

        static CustomController()
        {
            ControllerFormatFuncs = new Dictionary<string, Func<TResult, TResult>>();
        }


        public CustomController(IBaseService<TEntity, TPrimaryKey> service, Func<TResult, TResult> formatFunc = null) : base(service)
        {
            this.service = service;
            if (!ControllerFormatFuncs.ContainsKey(this.GetType().Name))
            {
                ControllerFormatFuncs.Add(this.GetType().Name, formatFunc);
            }
        }

        /// <summary>
        /// 根据Id获取单个结果对象（可以对结果格式化）
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpGet("GetResultById")]
        public async Task<TResult> GetResultById(TPrimaryKey id)
        {
            var result = await service.GetResultByIdAsync<TResult>(id);
            if (!ControllerFormatFuncs.ContainsKey(this.GetType().Name))
            {
                result = ControllerFormatFuncs[this.GetType().Name](result);
            }
            return result;
        }
    }
}
