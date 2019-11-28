using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace TodoApi.Common
{
    public class TextPlainOutputFormatter : TextOutputFormatter
    {
        //构造函数，在构造函数指定有效的类型和编码
        public TextPlainOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            return true;
        }


        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            //IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            //var logger = serviceProvider.GetService(typeof(ILogger<TextPlainOutputFormatter>)) as ILogger;
            var response = context.HttpContext.Response;

            //定义输出的对象
            string buffers = string.Empty;
            if (context.Object != null)
            {
                buffers = JsonSerializer.Serialize(context.Object);
            }
            await response.WriteAsync(buffers);
        }
    }
}