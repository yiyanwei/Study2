using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SqlSugar;
using ZeroOne.Entity;

namespace ZeroOne.Repository
{
    public class ProInfoRep : BaseRep<ProInfoSearch, Pro_Info>, IProInfoRep
    {
        private ISqlSugarClient _client;
        public ProInfoRep(ISqlSugarClient client) : base(client)
        {
            this._client = client;
        }

        public async Task<Pro_Info> GetProByName(string name)
        {
            //测试1
            var queryTest1 = this._client.Queryable<Pro_Info>();
            return await queryTest1.Where(x => x.ProName.Contains(name) || x.IsDeleted == true && x.CreateDate > DateTime.Now).FirstAsync();
            // var xx = await queryTest1.FirstAsync();
            // //测试2
            // var queryTest2 = this._client.Queryable<Pro_Info>();
            // queryTest2 = queryTest2.Where(x=>x.ProName.Contains(name));
            // queryTest2 = queryTest2.Where(y=>y.IsDeleted == true);
            // var first = await queryTest2.FirstAsync();
            // //正式返回的数据
            // var query = this._client.Queryable<Pro_Info>();          
            // return await query.Where(t => t.ProName.Contains(name.Trim())).FirstAsync();
        }
    }
}