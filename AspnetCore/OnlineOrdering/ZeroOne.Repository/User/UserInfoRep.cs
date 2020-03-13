using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using ZeroOne.Entity;
namespace ZeroOne.Repository
{
    public class UserInfoRep : BaseRep<UserInfo, Guid?, BaseUserSearch>, IUserInfoRep
    {
        public UserInfoRep(ISqlSugarClient client) : base(client)
        {

        }

        public async Task<UserInfo> UserLogin(UserLoginRequest request)
        {
            return await this._client.Queryable<UserInfo>().Where(t => t.MobileNum == request.Account && t.Password == request.Password && t.IsDeleted == false).FirstAsync();
        }
    }
}
