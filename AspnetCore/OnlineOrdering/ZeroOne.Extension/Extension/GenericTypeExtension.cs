using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZeroOne.Extension
{
    public interface IHttpService
    {

    }

    public static class GenericTypeExtension
    {
        public async static Task<T> GetResponse<T>(this IHttpService service, string requestUrl, int expSeconds = 10, Func<string, string> CallBack = null)
            where T : class, new()
        {
            T retVal = null;
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            using (var client = new HttpClient())
            {
                //设置超时10秒
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(expSeconds);
                    httpResponseMessage = await client.GetAsync(requestUrl);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string contentStr = await httpResponseMessage.Content.ReadAsStringAsync();
                if (CallBack != null)
                {
                    contentStr = CallBack(contentStr);
                }
                retVal = JsonConvert.DeserializeObject<T>(contentStr);
            }
            return retVal;
        }
    }
}
