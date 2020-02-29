using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using ZeroOne.Extension;
using NLog.Web;
using NLog;
using ZeroOne.Extension.Model;

namespace ZeroOne.WebApi
{
    public class JwtBearerOverrideEvents:JwtBearerEvents
    {
        /// <summary>
        /// JwtToken失效触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 没有JwtToken时触发 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task Challenge(JwtBearerChallengeContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            BaseResponse<object> response = new BaseResponse<object>()
            {
                success = false,
                errCode = nameof(ErrorCode.E10001).GetCode(),
                errMsg = ErrorCode.E10001
            };
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info(JsonConvert.SerializeObject(response));
            context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            context.HandleResponse();
            return base.Challenge(context);
        }
    }
}
