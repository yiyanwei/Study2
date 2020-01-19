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
                errMsg="没有访问权限"
                //errorCode = nameof(ErrorCode.E10004).GetCode().ToString(),
                //errorMessage = ErrorCode.E10004
            };
            var logger = LogManager.GetCurrentClassLogger();
            logger.Info(JsonConvert.SerializeObject(response));
            context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            context.HandleResponse();
            return base.Challenge(context);
        }
    }
}
