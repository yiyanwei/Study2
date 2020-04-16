using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.OpenApi.Models;

namespace ZeroOne.Extension.Model.Swagger
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplyTagDescriptions : IDocumentFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                //添加对应的控制器描述 这个是好不容易在issues里面翻到的
                new OpenApiTag { Name = "XuGong", Description = "徐工调用接口" },
                new OpenApiTag { Name = "User", Description = "用户接口"}
            };
        }
    }
}
