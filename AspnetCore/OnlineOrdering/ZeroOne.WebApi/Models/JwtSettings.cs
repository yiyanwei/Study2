using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZeroOne.WebApi
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        /// <summary>
        /// token可以给那些客户端使用
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 加密key（SecretKey必须大于16个，是大于，不是大于等于）
        /// </summary>
        public string SecretKey { get; set; }
    }
}
