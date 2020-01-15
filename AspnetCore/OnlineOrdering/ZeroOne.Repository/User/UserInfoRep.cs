using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
using ZeroOne.Entity;
namespace ZeroOne.Repository
{
    public class UserInfoRep : BaseRep<BaseUserSearch, User_Info>, IUserInfoRep
    {
        private ISqlSugarClient _client;
        public UserInfoRep(ISqlSugarClient client) : base(client)
        {
            this._client = client;
        }

        public async Task<User_Info> GetUserByAccount(string account, string pwd)
        {
            return await this._client.Queryable<User_Info>().Where(t => t.MobileNum == account && t.Password == pwd && t.IsDeleted == false).FirstAsync();
        }
    }
}
