using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZeroOne.Entity;
using ZeroOne.Repository;

namespace ZeroOne.Application
{
    public class UserInfoService : IUserInfoService
    {
        private IUserInfoRep _rep;
        public UserInfoService(IUserInfoRep rep)
        {
            this._rep = rep;
        }
        public Task<UserInfo> UserLogin(UserLoginRequest request)
        {
            return this._rep.UserLogin(request);
        }
    }
}
