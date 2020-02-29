using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public interface IUserInfoRep : IBaseRep<BaseUserSearch, UserInfo,Guid>
    {
        Task<UserInfo> UserLogin(UserLoginRequest request);
    }
}
