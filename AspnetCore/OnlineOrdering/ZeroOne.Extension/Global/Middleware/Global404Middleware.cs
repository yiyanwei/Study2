using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZeroOne.Extension.Global
{
    public class Global404Middleware
    {
        private readonly RequestDelegate next;
        private ILogger<Global404Middleware> logger;
        public Global404Middleware(RequestDelegate next, ILogger<Global404Middleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            
        }
    }
}
