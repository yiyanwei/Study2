using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroOne.Entity
{
    /// <summary>
    /// 用户登录对象
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 登录源  1：小程序，2：PC，3：App
        /// </summary>
        public int? LoginOrigin { get; set; }
    }
}
