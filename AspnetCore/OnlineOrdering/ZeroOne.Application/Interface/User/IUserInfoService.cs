using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ZeroOne.Entity;
namespace ZeroOne.Application
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface IUserInfoService
    {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserInfo> UserLogin(UserLoginRequest request);
    }
}
