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
    }
}