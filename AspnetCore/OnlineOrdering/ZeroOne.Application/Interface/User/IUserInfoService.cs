using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using ZeroOne.Entity;
namespace ZeroOne.Application
{
    public interface IUserInfoService
    {
        Task<User_Info> GetUserByAccount(string account, string pwd);
    }
}
