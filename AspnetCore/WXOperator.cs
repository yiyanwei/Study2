using Newtonsoft.Json;
using Sino.CustomerService.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sino.CustomerService.Web
{
    /// <summary>
    /// 微信返回编号
    /// </summary>
    public enum EWXCode
    {
        /// <summary>
        /// 获取access_token时Secret错误，或者access_token无效
        /// </summary>
        [Description("access_token无效")]
        WX40001 = 40001,
        /// <summary>
        /// 不合法的access_token
        /// </summary>
        [Description("不合法的access_token")]
        WX40014 = 40014,
        /// <summary>
        /// access_token过期
        /// </summary>
        [Description("access_token过期")]
        WX42001 = 42001
    }

    /// <summary>
    /// 微信操作
    /// </summary>
    public class WXOperator
    {
        private static ClientCredentialTokenResult _token;
        private static DateTime _lastGetTime;
        private static string _appid;
        private static int expires_in;
        private static string _appsecret;
        /// <summary>
        /// 
        /// </summary>
        public static IList<int> InvalidTokenCode;

        static WXOperator()
        {
            InvalidTokenCode = new List<int>();
            InvalidTokenCode.Add((int)EWXCode.WX40001);
            InvalidTokenCode.Add((int)EWXCode.WX40014);
            InvalidTokenCode.Add((int)EWXCode.WX42001);

            //默认7200秒
            expires_in = 7200;
        }

        /// <summary>
        /// 注册微信公众号
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="appsecret"></param>
        public static void RegisterAppId(string appid, string appsecret)
        {
            _appid = appid;
            _appsecret = appsecret;
        }

        /// <summary>
        /// 获取微信token值，不能一直调用微信的token接口
        /// </summary>
        /// <returns></returns>
        public async static Task<ClientCredentialTokenResult> GetToken()
        {
            //判断微信appid和appsecret是否注册
            if (string.IsNullOrWhiteSpace(_appid) || string.IsNullOrWhiteSpace(_appsecret))
            {
                throw new SinoException("未注册微信appid和appsecret");
            }

            bool isPassExpire = false;
            //判断是否为初始值
            if (_lastGetTime <= DateTime.MinValue)
            {
                isPassExpire = true;
            }
            else
            {
                //微信token两小时失效
                if (_lastGetTime.AddSeconds(expires_in) <= DateTime.Now)
                {
                    isPassExpire = true;
                }
                else
                {
                    isPassExpire = false;
                }
            }
            //获取token值
            if (_token == null || isPassExpire)
            {
                ClientCredentialTokenResult response = new ClientCredentialTokenResult();
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _appid, _appsecret);
                using (var _client = new HttpClient())
                {
                    httpResponseMessage = await _client.GetAsync(url);
                }
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string contentStr = await httpResponseMessage.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ClientCredentialTokenResult>(contentStr);
                    _token = response;
                    if (_token != null)
                    {
                        _lastGetTime = DateTime.Now;
                        expires_in = _token.expires_in;
                    }
                }
            }
            return _token;
        }

        /// <summary>
        /// 刷新token，重新再获取一次
        /// </summary>
        /// <returns></returns>
        public async static Task<ClientCredentialTokenResult> RefreshToken()
        {
            ClientCredentialTokenResult response = new ClientCredentialTokenResult();
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", _appid, _appsecret);
            using (var _client = new HttpClient())
            {
                httpResponseMessage = await _client.GetAsync(url);
            }
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string contentStr = await httpResponseMessage.Content.ReadAsStringAsync();
                response = JsonConvert.DeserializeObject<ClientCredentialTokenResult>(contentStr);
                _token = response;
                if (_token != null)
                {
                    _lastGetTime = DateTime.Now;
                    expires_in = _token.expires_in;
                }
            }
            return _token;
        }
    }
}
