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
        public Task<User_Info> GetUserByAccount(string account, string pwd)
        {
            if (!string.IsNullOrWhiteSpace(account) && !string.IsNullOrWhiteSpace(pwd))
            {
                return this._rep.GetUserByAccount(account.Trim(), pwd);
            }
            else
            {
                throw new Exception("账号或密码为空");
            }
        }
    }
}
