using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public interface IUserInfoRep : IBaseRep<BaseUserSearch, User_Info>
    {
        Task<User_Info> GetUserByAccount(string account, string pwd);
    }
}
